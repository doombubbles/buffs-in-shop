using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace BuffsInShop.Buff;

public class Energized : ModBuffInShop
{
    public override string OriginTower => TowerType.MonkeySub;
    public override int OriginTopPath => 5;

    public override float BaseCost => 4000;
    public override string BaseDescription => "Gives a tower 20% reduced ability cooldowns.";
    public override KeyCode KeyCode => KeyCode.E;
    public override bool SubsequentDiscount => true;

    public override bool IsValidOrigin(TowerModel current) => current.HasBehavior<SubmergeModel>();

    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetBehavior<SubmergeModel>().submergeSound.assetId;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!tower.towerModel.GetAbilities().Any())
        {
            helperMessage = "Must have abilities.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        var submerge = OriginTowerModel.GetBehavior<SubmergeModel>();

        var filters = new Il2CppReferenceArray<TowerFilterModel>(0);

        yield return new AbilityCooldownScaleSupport.MutatorTower(new AbilityCooldownScaleSupport
        {
            abilityCooldownScaleSupportModel = new AbilityCooldownScaleSupportModel(
                "SubmergeAbilityCooldownScaleGlobal", true,
                submerge.abilityCooldownSpeedScaleGlobal, false, false, filters, submerge.buffLocsName,
                "BuffIconSubHalf5xx", false, submerge.supportMutatorPriority)
        }, submerge.abilityCooldownSpeedScaleGlobal, "SubmergeAbilityCooldownScaleGlobal");
    }
}