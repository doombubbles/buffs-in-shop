using System.Collections.Generic;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace BuffsInShop.Buff;

public class SuperHeatedUp : ModBuffInShop<HeatedUp>
{
    public override string OriginTower => TowerType.Gwendolin;
    public override int OriginTopPath => 17;
    public override bool Hero => true;

    public override string BaseDescription => "Gives a tower with Heated Up +1 damage and +2 damage to Lead Bloons.";

    public override float BaseCost => 6000;
    public override bool SubsequentDiscount => true;

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        var proj = OriginTowerModel.GetDescendant<BonusProjectileAfterIntervalModel>().projectileModel;

        yield return proj.GetBehavior<PierceUpTowersModel>().CreateMutator().ApplyBuffIcon<SuperHeatedUpIcon>();
        yield return proj.GetBehavior<HeatItUpDamageBuffModel>().CreateMutator().ApplyBuffIcon<SuperHeatedUpIcon>();
        yield return proj.GetBehavior<DamageUpTowersModel>().CreateMutator().ApplyBuffIcon<SuperHeatedUpIcon>();
        yield return proj.GetBehavior<DamageUpTagTowersModel>().CreateMutator().ApplyBuffIcon<SuperHeatedUpIcon>();
    }

    public class SuperHeatedUpIcon : ModBuffIcon;
}