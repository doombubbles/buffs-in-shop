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

public class HomelandDefense : ModBuffInShop<CallToArms>
{
    public override TowerModel OriginTowerModel =>
        base.OriginTowerModel.HasDescendant<CallToArmsModel>()
            ? base.OriginTowerModel
            : Game.instance.model.GetTower(OriginTower, OriginTopPath, OriginMidPath, OriginBotPath);

    public override string OriginTower => TowerType.MonkeyVillage;
    public override int OriginMidPath => 5;

    public override float BaseCost => UpgradeCost;
    public override string BaseDescription => "Gives a tower with Call to Arms now +100% attack speed and pops.";
    public override KeyCode KeyCode => KeyCode.H;
    public override bool SubsequentDiscount => true;
    public override int PriorityBoost => base.PriorityBoost + 1;

    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetDescendant<CreateSoundOnAbilityModel>().sound.assetId;

    public override EffectModel? PlacementEffect =>
        OriginTowerModel.GetDescendant<CreateEffectOnAbilityModel>().effectModel;

    public override BehaviorMutator GetMutator(Tower? tower)
    {
        var callToArms = OriginTowerModel.GetDescendant<CallToArmsModel>().Duplicate();
        var buff = callToArms.Mutator.buffIndicator;
        callToArms._mutator = null;
        var mutator = callToArms.Mutator;
        mutator.buffIndicator = buff;
        return mutator;
    }
}