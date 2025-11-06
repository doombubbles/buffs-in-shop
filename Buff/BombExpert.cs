using System.Collections.Generic;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace BuffsInShop.Buff;

public class BombExpert : ModBuffInShop
{
    public override string OriginTower => TowerType.StrikerJones;
    public override int OriginTopPath => 17;
    public override bool Hero => true;

    public override string BaseDescription =>
        "Gives a tower +5% range and 25% pierce. A Bomb Shooter gets 24% increased attack speed, and a Mortar Monkey gets that as well as 10% increased blast radius.";

    public override float BaseCost => 2000;
    public override bool SubsequentDiscount => true;

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        var mainBuff = OriginTowerModel.GetBehavior<RateSupportExplosiveModel>();
        var bombExport = OriginTowerModel.GetBehavior<RateSupportBombExpertModel>().Duplicate();

        bombExport.buffLocsName = mainBuff.buffLocsName;
        bombExport.buffIconName = mainBuff.buffIconName;

        bombExport.AddChildDependant(new BuffIndicatorModel("", bombExport.buffLocsName, bombExport.buffIconName));

        yield return bombExport.CreateMutator();

        if (tower?.towerModel.baseId == TowerType.BombShooter || tower?.towerModel.baseId == TowerType.MortarMonkey)
        {
            yield return mainBuff.CreateMutator();
        }

        if (tower?.towerModel.baseId == TowerType.MortarMonkey)
        {
            yield return OriginTowerModel.GetBehavior<ProjectileRadiusSupportModel>().CreateMutator();
        }
    }


}