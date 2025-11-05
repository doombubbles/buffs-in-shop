using System.Collections.Generic;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;

namespace BuffsInShop.Buff;

public class NavalTactics : ModBuffInShop
{
    public override string? OriginTower => TowerType.AdmiralBrickell;

    public override bool IsValidOrigin(TowerModel current) => current.GetAbility()?.displayName == DisplayName;

    public override int OriginTopPath => 8;
    public override bool Hero => true;

    public override string BaseDescription =>
        "Gives a water based monkey double attack speed, +1 Pierce, Normal damage type, and Camo detection.";
    public override bool SubsequentDiscount => true;

    public override float BaseCost => 25000;

    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetAbility().GetBehavior<CreateSoundOnAbilityModel>().sound.assetId;

    public override EffectModel PlacementEffect
    {
        get
        {
            var effect = OriginTowerModel.GetAbility().GetDescendant<EffectModel>().Duplicate();
            effect.lifespan = 2;
            return effect;
        }
    }

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && tower.GetAreaTowerIsPlacedOn().areaModel.type is not
                (AreaType.water or AreaType.waterMermonkey))
        {
            helperMessage = "Must be a water based monkey.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        var ability = OriginTowerModel.GetAbility();

        yield return ability.GetBehavior<ActivateRateSupportZoneModel>().CreateMutator();
        yield return ability.GetBehavior<ActivatePierceSupportZoneModel>().CreateMutator();
        yield return ability.GetBehavior<ActivateTowerDamageSupportZoneModel>().CreateMutator();
        yield return ability.GetBehavior<ActivateVisibilitySupportZoneModel>().CreateMutator();
    }
}