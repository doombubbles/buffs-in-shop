using System.Collections.Generic;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace BuffsInShop.Buff;

public class NaturesWrath : ModBuffInShop
{
    public override string? OriginTower => TowerType.ObynGreenfoot;
    public override int OriginTopPath => 19;
    public override bool Hero => true;

    public override string BaseDescription =>
        "Gives a Druid increased pierce, range, income, attack speed, tornado size, and Camo Detection.";

    public override float BaseCost => 5000;
    public override bool SubsequentDiscount => true;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && tower.towerModel.baseId != TowerType.Druid)
        {
            helperMessage = "Must be a Druid.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        yield return OriginTowerModel.GetBehavior<PierceSupportModel>().CreateMutator();
        yield return OriginTowerModel.GetBehavior<VisibilitySupportModel>().CreateMutator();
        yield return OriginTowerModel.GetBehavior<MonkeyCityIncomeSupportModel>().CreateMutator();
    }
}