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

public class StrongerStimulant : ModBuffInShop<BerserkerBrew>
{
    private TowerModel AlchemistPerma => GameModel.GetTower(OriginTower, 5);

    public override string OriginTower => TowerType.Alchemist;
    public override int OriginTopPath => 4;

    public override float BaseCost => UpgradeCost * 5;
    public override string BaseDescription =>
        "Gives a tower with Berserker Brew further increased pierce, range and attack speed.";
    public override KeyCode KeyCode => KeyCode.S;
    public override bool SubsequentDiscount => true;

    public override bool AffectsSubTowers => false;

    public override EffectModel? PlacementEffect =>
        AlchemistPerma.GetAttackModel(2).GetDescendant<CreateEffectOnExhaustFractionModel>().effectModel;

    public override AudioClipReference? PlacementSound =>
        AlchemistPerma.GetAttackModel(2).GetDescendants<SoundModel>().AsIEnumerable().Skip(2).First().assetId;


    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && AlchemistPerma
                .GetDescendant<AddBerserkerBrewToProjectileModel>().ignoreList
                .Contains(tower.towerModel.baseId))
        {
            helperMessage = "Invalid tower for Alchemist buff.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower)
    {
        var brew = AlchemistPerma.GetDescendant<AddBerserkerBrewToProjectileModel>().Duplicate();
        var baseBrew = OriginTowerModel.GetDescendant<AddBerserkerBrewToProjectileModel>();
        brew.mutator = null;

        brew.buffIconName = baseBrew.buffIconName;
        brew.buffLocsName = baseBrew.buffLocsName;

        var mutator = brew.Mutator;
        mutator.buffIndicator = baseBrew.GetBuffIndicatorModel();
        return mutator;
    }
}