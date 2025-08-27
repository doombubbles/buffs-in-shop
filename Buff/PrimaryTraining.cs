using System.Collections.Generic;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using UnityEngine;

namespace BuffsInShop.Buff;

public class PrimaryTraining : ModBuffInShop
{
    public override string OriginTower => TowerType.MonkeyVillage;
    public override int OriginTopPath => 3;

    public override string BaseDescription => "Gives a Primary monkey more range, pierce and projectile speed.";
    public override KeyCode KeyCode => KeyCode.N;
    public override bool SubsequentDiscount => true;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && !tower.towerModel.CheckSet(TowerSet.Primary, false))
        {
            helperMessage = "Must be a Primary Monkey.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower) => GetMutators(OriginTowerModel);

    public IEnumerable<BehaviorMutator> GetMutators(TowerModel towerModel)
    {
        yield return towerModel.GetBehavior<PierceSupportModel>(Name).CreateMutator();

        yield return towerModel.GetBehavior<ProjectileSpeedSupportModel>(Name).CreateMutator();

        yield return towerModel.GetBehavior<RangeSupportModel>(Name).CreateMutator();
    }
}