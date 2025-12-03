using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace BuffsInShop.Buff;

public class NaturesClarity : ModBuffInShop
{
    public override string OriginTower => TowerType.ObynGreenfoot;
    public override int OriginTopPath => 11;
    public override bool Hero => true;

    public override string DisplayName => "Nature's Clarity";

    public override string BaseDescription => "Give a Magic monkey +5 range and +2 pierce.";

    public override float BaseCost => 900;
    public override bool SubsequentDiscount => true;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && !tower.towerModel.CheckSet(TowerSet.Magic, false))
        {
            helperMessage = "Must be a Magic monkey.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        yield return OriginTowerModel.GetBehaviors<RangeSupportModel>().Last().CreateMutator();
        yield return OriginTowerModel.GetBehaviors<PierceSupportModel>().Last().CreateMutator();
    }
}