using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace BuffsInShop.Buff;

public class TakeAim : ModBuffInShop
{
    public override string OriginTower => TowerType.Desperado;
    public override int OriginMidPath => 3;

    public override bool IsValidOrigin(TowerModel current) => current.HasDescendant<TakeAimModel>();

    public override string BaseDescription =>
        "Gives a tower improved range, accuracy, and Camo detection.";
    public override KeyCode KeyCode => KeyCode.K;

    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetDescendant<CreateSoundOnAbilityModel>().sound.assetId;
    public override EffectModel? PlacementEffect =>
        OriginTowerModel.GetDescendant<TakeAimModel>().initialEffect;

    public override bool AffectsSubTowers => false;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions &&
            TapTowerAbilityBehavior.towerBanList.Contains(tower.towerModel.baseId))
        {
            helperMessage = $"Tower is on the {DisplayName} ban list.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }


    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetDescendant<TakeAimModel>().Duplicate().Mutator;

    public override void ExtraMutation(TowerModel towerModel)
    {
        var takeAim = OriginTowerModel.GetDescendant<TakeAimModel>();

        if (!towerModel.behaviors.Any(model => model.Is<DisplayModel>() && model.name.EndsWith(Name)))
        {
            towerModel.AddBehavior(new DisplayModel(Name, takeAim.buffDisplayPath, -1, DisplayCategory.Buff));
        }
    }
}