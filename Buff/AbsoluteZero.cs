using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace BuffsInShop.Buff;

public class AbsoluteZero : ModBuffInShop
{
    public override string OriginTower => TowerType.IceMonkey;
    public override int OriginMidPath => 5;

    public override bool IsValidOrigin(TowerModel current) => current.HasDescendant<ActivateRateSupportZoneModel>();

    public override string BaseDescription => "Gives an Ice Monkey 50% increased attack speed.";
    public override bool SubsequentDiscount => true;
    public override KeyCode KeyCode => KeyCode.Alpha0;


    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetDescendant<CreateSoundOnAbilityModel>().sound.assetId;
    public override EffectModel? PlacementEffect =>
        OriginTowerModel.GetDescendant<TakeAimModel>().initialEffect;


    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && tower.towerModel.baseId != TowerType.IceMonkey)
        {
            helperMessage = "Must be an Ice Monkey.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetDescendant<ActivateRateSupportZoneModel>().CreateMutator();
}