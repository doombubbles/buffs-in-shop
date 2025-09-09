using System.Linq;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Audio;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace BuffsInShop.Buff;

public class AcidicMixtureDip : ModBuffInShop
{
    private TowerModel AlchemistPerma => GameModel.GetTower(OriginTower, 5);

    public override string OriginTower => TowerType.Alchemist;
    public override int OriginTopPath => 2;

    public override float BaseCost => UpgradeCost * 10;
    public override string BaseDescription =>
        "Makes a tower able to pop Lead Bloons and deal extra damage to Ceramic and MOAB-class Bloons.";
    public override KeyCode KeyCode => KeyCode.A;
    public override bool SubsequentDiscount => true;

    public override bool AffectsSubTowers => false;

    public override EffectModel? PlacementEffect =>
        AlchemistPerma.GetAttackModel(1).GetDescendant<CreateEffectOnExhaustFractionModel>().effectModel;

    public override AudioClipReference? PlacementSound =>
        AlchemistPerma.GetAttackModel(2).GetDescendants<SoundModel>().AsIEnumerable().Skip(0).First().assetId;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && AlchemistPerma
                .GetDescendant<AddAcidicMixtureToProjectileModel>().ignoreList
                .Contains(tower.towerModel.baseId))
        {
            helperMessage = "Invalid tower for Alchemist buff.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower) =>
        AlchemistPerma.GetDescendant<AddAcidicMixtureToProjectileModel>().Duplicate().Mutator;
}