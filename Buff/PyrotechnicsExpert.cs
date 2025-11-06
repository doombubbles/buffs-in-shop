using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.TowerFilters;

namespace BuffsInShop.Buff;

public class PyrotechnicsExpert : ModBuffInShop
{
    public override string OriginTower => TowerType.Gwendolin;
    public override int OriginTopPath => 19;
    public override bool Hero => true;

    public override string BaseDescription =>
        "Gives a Ring of Fire Tack Shooter, Signal Flare Mortar, or Dragon's Breath Wizard 20% increased projectile size and 25% increased attack speed.";

    public override float BaseCost => 1300;
    public override bool SubsequentDiscount => true;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && !new PyrotechnicsSupportFilter().FilterTower(tower))
        {
            helperMessage = "Must be Ring of Fire Tack Shooter, Signal Flare Mortar, or Dragon's Breath Wizard";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower)
    {
        return new PyrotechnicsSupport.PyrotechnicsSupportMutator(true, "PyrotechnicsSupport", .2f,
            0, OriginTowerModel.GetDescendant<PyrotechnicsSupportModel>().GetBuffIndicatorModel());
    }

}