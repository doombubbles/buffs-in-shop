using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Audio;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;

namespace BuffsInShop.Buff;

public class RallyingRoar : ModBuffInShop
{
    public override string OriginTower => TowerType.PatFusty;
    public override int OriginTopPath => 13;
    public override bool Hero => true;

    public override bool IsValidOrigin(TowerModel current) =>
        current.HasBehavior<AbilityModel>(Name, out var abilityModel) && abilityModel.HasDescendant<SoundModel>();

    public override string BaseDescription => "Gives a tower +3 damage.";

    public override float BaseCost => 15000;
    public override bool SubsequentDiscount => true;

    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetBehavior<AbilityModel>(Name).GetBehavior<CreateSoundOnAbilityModel>().sound.assetId;

    public override EffectModel? PlacementEffect =>
        OriginTowerModel.GetBehavior<AbilityModel>(Name).GetDescendant<EffectModel>();

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetDescendant<ActivateTowerDamageSupportZoneModel>().CreateMutator();
}