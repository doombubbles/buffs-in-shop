using System.Collections.Generic;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using UnityEngine;

namespace BuffsInShop.Buff;

public class FinallyHarmonizing : ModBuffInShop
{
    public override string OriginTower => TowerType.Mermonkey;
    public override int OriginBotPath => 5;

    public override string BaseDescription =>
        "Gives a Magic monkey +3 pierce, or a Hero 15% improved range and cooldowns.";
    public override float BaseCost => 1250;
    public override bool SubsequentDiscount => true;
    public override KeyCode KeyCode => KeyCode.RightBracket;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && !tower.towerModel.CheckSet(TowerSet.Magic, false) &&
            !tower.towerModel.IsHero())
        {
            helperMessage = "Must be a Magic monkey or Hero.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        if (BuffsInShopMod.BypassTowerRestrictions || tower == null || tower.towerModel.IsHero())
        {
            yield return OriginTowerModel.GetDescendant<TowerModel>().GetDescendant<RangeSupportModel>()
                .CreateMutator();
            yield return OriginTowerModel.GetDescendant<TowerModel>().GetDescendant<AbilityCooldownScaleSupportModel>()
                .CreateMutator();
        }

        if (BuffsInShopMod.BypassTowerRestrictions || tower == null || tower.towerModel.CheckSet(TowerSet.Magic, false))
        {
            yield return OriginTowerModel.GetDescendant<TowerModel>().GetDescendant<PierceSupportModel>()
                .CreateMutator();
        }
    }

}