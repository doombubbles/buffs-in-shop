using System.Collections.Generic;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace BuffsInShop.Buff;

public class EliteSniping : ModBuffInShop
{
    public override string OriginTower => TowerType.SniperMonkey;
    public override int OriginMidPath => 5;

    public override float BaseCost => 2000;
    public override string BaseDescription => "Gives a Sniper Monkey Elite targeting prio and 33% more attack speed.";
    public override KeyCode KeyCode => KeyCode.L;
    public override bool SubsequentDiscount => true;


    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && tower.towerModel.baseId != TowerType.SniperMonkey)
        {
            helperMessage = "Must be a Sniper Monkey.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        yield return OriginTowerModel.GetBehavior<RateSupportModel>().CreateMutator();
        yield return OriginTowerModel.GetBehavior<TargetSupplierSupportModel>().CreateMutator();
    }
}