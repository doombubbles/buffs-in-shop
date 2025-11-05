using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace BuffsInShop.Buff;

public class EtienneRange : ModBuffInShop
{
    public override string? OriginTower => TowerType.Etienne;
    public override int OriginTopPath => 19;
    public override bool Hero => true;

    public override string BaseDescription => "Give a tower 20% increased range.";

    public override float BaseCost => 900;
    public override bool SubsequentDiscount => true;

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetBehavior<RangeSupportModel>().CreateMutator();
}