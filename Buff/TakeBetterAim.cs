using System.Linq;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace BuffsInShop.Buff;

public class TakeBetterAim : ModBuffInShop
{
    public override string OriginTower => TowerType.Desperado;
    public override int OriginMidPath => 4;

    public override bool IsValidOrigin(TowerModel current) => current.HasDescendant<TakeAimModel>();

    public override string BaseDescription =>
        "Improves the Take Aim buff on a tower to further improve range and accuracy, and allow damaging Black/White/Purple Bloons.";
    public override float BaseCost => UpgradeCost / 4;
    public override KeyCode KeyCode => KeyCode.Q;

    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetDescendant<CreateSoundOnAbilityModel>().sound.assetId;
    public override EffectModel? PlacementEffect =>
        OriginTowerModel.GetDescendant<TakeAimModel>().initialEffect;

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetDescendant<TakeAimModel>().Duplicate().Mutator;

    public override void ExtraMutation(TowerModel towerModel)
    {
        var takeAim = OriginTowerModel.GetDescendant<TakeAimModel>();

        if (towerModel.behaviors
            .FirstOrDefault(model => model.Is<DisplayModel>() && model.name.EndsWith(nameof(TakeAim)))
            .Is(out var behavior))
        {
            towerModel.RemoveBehavior(behavior);
        }

        towerModel.AddBehavior(new DisplayModel(nameof(TakeAim), takeAim.buffDisplayPath, -1, DisplayCategory.Buff));
    }
}