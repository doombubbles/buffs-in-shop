using System.Collections.Generic;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace BuffsInShop.Buff;

public class HeatedUp : ModBuffInShop
{
    public override string OriginTower => TowerType.Gwendolin;
    public override int OriginTopPath => 4;
    public override bool Hero => true;

    public override string BaseDescription => "Gives a tower +1 Pierce and allows it to pop Lead and Frozen Bloons.";

    public override float BaseCost => 1000;
    public override bool SubsequentDiscount => true;

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        var proj = OriginTowerModel.GetDescendant<BonusProjectileAfterIntervalModel>().projectileModel;

        yield return proj.GetBehavior<PierceUpTowersModel>().CreateMutator();
        yield return proj.GetBehavior<HeatItUpDamageBuffModel>().CreateMutator();
    }
}