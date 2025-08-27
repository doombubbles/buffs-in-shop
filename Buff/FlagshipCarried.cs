using System.Collections.Generic;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace BuffsInShop.Buff;

public class FlagshipCarried : ModBuffInShop
{
    public override string OriginTower => TowerType.MonkeyBuccaneer;
    public override int OriginTopPath => 5;

    public override float BaseCost => GetInstance<JungleDrums>().BaseCost;
    public override string BaseDescription => "Gives a water based monkey or Monkey Ace 18% increased attack speed.";
    public override KeyCode KeyCode => KeyCode.F;
    public override bool SubsequentDiscount => true;

    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions &&
            !(tower.towerModel.baseId == TowerType.MonkeyAce ||
              tower.GetAreaTowerIsPlacedOn()?.areaModel?.type is AreaType.water or AreaType.waterMermonkey))
        {
            helperMessage = "Must be a water based monkey or Monkey Ace.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        yield return OriginTowerModel.GetBehavior<FlagshipAttackSpeedIncreaseModel>().CreateMutator();
    }
}