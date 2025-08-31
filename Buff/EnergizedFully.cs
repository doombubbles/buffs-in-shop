using System.Collections.Generic;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Map;
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

public class EnergizedFully : ModBuffInShop<Energized>
{
    public override string OriginTower => TowerType.MonkeySub;
    public override int OriginTopPath => 5;

    public override bool IsValidOrigin(TowerModel current) => current.HasBehavior<SubmergeModel>();

    public override float BaseCost => UpgradeCost / 2 - GetInstance<Energized>().BaseCost;
    public override string BaseDescription =>
        "Makes an Energized tower have 50% reduced cooldowns if its in water, and/or earn XP 50% faster if its a Hero.";
    public override KeyCode KeyCode => KeyCode.Z;
    public override bool SubsequentDiscount => true;

    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetBehavior<SubmergeModel>().submergeSound.assetId;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!tower.towerModel.IsHero() &&
            tower.GetAreaTowerIsPlacedOn().areaModel.type is not (AreaType.water or AreaType.waterMermonkey))
        {
            helperMessage = "Must be a Hero or placed in water";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        var submerge = OriginTowerModel.GetBehavior<SubmergeModel>();

        var filters = new Il2CppReferenceArray<TowerFilterModel>(0);

        if (BuffsInShopMod.BypassTowerRestrictions || tower == null ||
            tower.GetAreaTowerIsPlacedOn().areaModel.type is AreaType.water or AreaType.waterMermonkey)
        {
            yield return new AbilityCooldownScaleSupport.MutatorTower(new AbilityCooldownScaleSupport
            {
                abilityCooldownScaleSupportModel = new AbilityCooldownScaleSupportModel(
                    "SubmergeAbilityCooldownScale", true, submerge.abilityCooldownSpeedScale, true, false,
                    filters, submerge.buffLocsName, submerge.buffIconName, false, submerge.supportMutatorPriority)
            }, submerge.abilityCooldownSpeedScale, "SubmergeAbilityCooldownScale");
        }

        if (BuffsInShopMod.BypassTowerRestrictions || tower == null || tower.towerModel.IsHero())
        {
            yield return new HeroXpScaleSupport.MutatorTower(new HeroXpScaleSupport
            {
                heroXpScaleSupportModel = new HeroXpScaleSupportModel("SubmergeHeroXpScale", true, submerge.heroXpScale,
                    filters, submerge.buffLocsName, submerge.buffIconName)
            });
        }
    }
}