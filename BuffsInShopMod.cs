using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper.Extensions;
using BuffsInShop;
using BuffsInShop.Buff;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Models.Gameplay.Mods;
using Il2CppAssets.Scripts.Models.Profile;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using MelonLoader;
using Newtonsoft.Json.Linq;

[assembly: MelonInfo(typeof(BuffsInShopMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace BuffsInShop;

public class BuffsInShopMod : BloonsTD6Mod
{
    public static readonly ModSettingBool RequireBuffOriginUsable = new(true)
    {
        description = "Requires the original source of a buff to be usable in a match for it to show up in the shop",
        icon = VanillaSprites.MonkeyVillageIcon,
    };

    public static readonly ModSettingBool BypassTowerRestrictions = new(false)
    {
        description = "Makes it so buffs in shop can be placed on any towers, not just ones they'd naturally apply to",
        icon = VanillaSprites.PrimaryTrainingUpgradeIcon
    };

    public static readonly ModSettingBool AlwaysAffectSubTowers = new(false)
    {
        description =
            "Makes the buffs that don't normally affect sub-towers like Alchemists' and Overclock now affect sub-towers.",
        icon = VanillaSprites.FasterEngineeringUpgradeIcon
    };

    private static readonly Dictionary<ModBuffInShop, float> Clipboard = [];

    public override void OnTitleScreen()
    {
        var chimps = GameData.Instance.mods.FirstOrDefault(model => model.name == "Clicks");
        if (chimps == null) return;

        foreach (var buff in ModContent.GetContent<ModBuffInShop>().Where(buff => !buff.AllowInChimps))
        {
            chimps.AddMutator(new LockTowerModModel("", buff.Id));
        }
    }

    public override void OnSaveSettings(JObject settings)
    {
        foreach (var modBuffInShop in ModContent.GetContent<ModBuffInShop>())
        {
            Game.instance.model.GetTower(modBuffInShop.Id).cost = modBuffInShop.Cost;
            modBuffInShop.AddOrRemoveFromShop();
        }
    }

    public override void OnRoundEnd() => ModContent.GetInstance<Ultraboost>().HandleBoosting();

    public override void OnTowerDestroyed(Tower tower)
    {
        foreach (var buff in ModContent.GetContent<ModBuffInShop>().Where(buff => buff.HasBuff(tower)))
        {
            var inventory = InGame.Bridge.Simulation.GetTowerInventory(tower.owner);
            if (inventory.towerMaxes.TryGetValue(buff.Id, out var max) && max > -1)
            {
                inventory.towerMaxes[buff.Id]++;
            }
        }
    }

    public override void OnTowerSaved(Tower tower, TowerSaveDataModel saveData)
    {
        foreach (var mutator in tower.mutators.ToArray())
        {
            if (ModBuffInShop.Cache.TryGetValue(mutator.mutator.id, out var buff))
            {
                var fakeMutator = mutator.mutator.Cast<RateSupportModel.RateSupportMutator>();
                saveData.metaData[buff.Id] = $"{fakeMutator.multiplier}";
                buff.OnSaved(tower, saveData);
            }
        }
    }

    public static void OnLateTowerLoaded(Tower tower, TowerSaveDataModel saveData)
    {
        foreach (var (id, buff) in ModBuffInShop.Cache)
        {
            if (saveData.metaData.TryGetValue(id, out var value) && float.TryParse(value, out var cost))
            {
                buff.Apply(tower, cost);
            }
        }

        foreach (var mutator in tower.mutators.ToArray())
        {
            if (ModBuffInShop.Cache.TryGetValue(mutator.mutator.id, out var buff))
            {
                buff.OnLoaded(tower, saveData);
            }
        }
    }

    public override object? Call(string operation, params object[] parameters)
    {
        switch (operation)
        {
            case "OnTowerCopied" when parameters.CheckTypes(out Tower tower):
                Clipboard.Clear();
                foreach (var buff in ModContent.GetContent<ModBuffInShop>())
                {
                    if (buff.HasBuff(tower, out var cost))
                    {
                        Clipboard[buff] = cost;
                    }
                }
                break;
            case "OnTowerPasted" when parameters.CheckTypes(out Tower tower):
                foreach (var (buff, cost) in Clipboard)
                {
                    buff.Apply(tower, cost);
                }
                break;
            case "OnClipboardCleared":
                Clipboard.Clear();
                break;
            case "ModifyClipboardCost" when parameters.CheckTypes(out Tower tower):
                return ModContent.GetContent<ModBuffInShop>()
                    .Where(buff => buff.HasBuff(tower))
                    .Sum(buff => InGame.Bridge.Simulation.towerManager
                        .GetTowerCost(
                            InGame.Bridge.Model.GetTowerWithName(buff.Id),
                            tower.Position,
                            InGame.Bridge.Simulation.GetTowerInventory(tower.owner),
                            tower.owner
                        )
                    );
        }

        return null;
    }
}