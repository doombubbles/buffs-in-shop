using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace BuffsInShop.Buff;

public class Overclock : ModBuffInShop
{
    public override string OriginTower => TowerType.EngineerMonkey;
    public override int OriginMidPath => 4;

    public override bool IsValidOrigin(TowerModel current) => current.HasDescendant<OverclockModel>();

    public override float BaseCost => UpgradeCost;
    public override string BaseDescription =>
        "Supercharges a tower's attack speed, or other effects if it doesn't attack.";
    public override KeyCode KeyCode => KeyCode.O;

    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetDescendant<CreateSoundOnAbilityModel>().sound.assetId;
    public override EffectModel? PlacementEffect =>
        OriginTowerModel.GetDescendant<OverclockModel>().initialEffect;

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetDescendant<OverclockModel>().Duplicate().Mutator;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions &&
            TapTowerAbilityBehavior.towerBanList.Contains(tower.towerModel.baseId))
        {
            helperMessage = $"Tower is on the {DisplayName} ban list.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override void ExtraMutation(TowerModel towerModel)
    {
        var overclock = OriginTowerModel.GetDescendant<OverclockModel>();

        if (!towerModel.behaviors
                .Any(model => model.Is<DisplayModel>() && model.name.EndsWith(Name)))
        {
            towerModel.AddBehavior(new DisplayModel(Name, overclock.buffDisplayPath, -1, DisplayCategory.Buff));
        }
    }
}