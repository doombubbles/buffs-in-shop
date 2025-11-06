using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Upgrades;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppNinjaKiwi.Common.ResourceUtils;

namespace BuffsInShop.Buff;

public class ParagonOverclock : ModBuffInShopParagon
{
    public override string OriginTower => TowerType.EngineerMonkey;

    public override string BaseDescription =>
        "Overclocks a tower and other towers near it. Can be used on Paragons.";

    public override bool ParagonAllowed => true;
    public override bool AffectsSubTowers => false;

    public override AudioClipReference PlacementSound => new(VanillaAudioClips.ActivatedOverclock);
    public override EffectModel? PlacementEffect =>
        OriginTowerModel.GetDescendant<OverclockModel>().initialEffect;

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        base.ModifyBaseTowerModel(towerModel);
        towerModel.range = 24;
    }

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

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetDescendant<OverclockModel>().Duplicate().Mutator;

    public override bool ExtraMutation(TowerModel towerModel)
    {
        var overclock = OriginTowerModel.GetDescendant<OverclockModel>();

        towerModel.AddBehavior(new RangeSupportModel("Overclock", true, overclock.rateModifier,
            overclock.villageRangeModifier, overclock.mutatorId, null,
            false, overclock.buffLocsName, overclock.buffIconName)
        {
            appliesToOwningTower = false,
            showBuffIcon = true,
            isCustomRadius = true,
            customRadius = overclock.paragonZoneRange
        });

        return true;
    }

    /// <summary>
    /// Make this support zone work as an Overclock if it has that ID
    /// </summary>
    [HarmonyPatch(typeof(RangeSupport.MutatorTower), nameof(RangeSupport.MutatorTower.Mutate))]
    internal static class RangeSupport_Mutate
    {
        [HarmonyPrefix]
        internal static bool Prefix(RangeSupport.MutatorTower __instance, Model baseModel, Model model,
            ref bool __result)
        {
            if (__instance.id != "Overclock" || ModHelper.HasMod("AbilityChoice")) return true;

            var overclock = Game.instance.model.GetTower(TowerType.EngineerMonkey, 0, 4, 0)
                .GetDescendant<OverclockModel>()
                .Duplicate();

            overclock.rateModifier = __instance.multiplier;
            overclock.villageRangeModifier = __instance.additive;
            overclock.buffIconName = __instance.buffIndicator.iconName;
            overclock.buffLocsName = __instance.buffIndicator.buffName;

            var fakeMutator = new OverclockModel.OverclockMutator(overclock)
            {
                resultCache = __instance.resultCache,
                limiters = __instance.limiters,
                mutated = __instance.mutated
            };

            __result = fakeMutator.Mutate(baseModel, model);
            return false;
        }
    }

}