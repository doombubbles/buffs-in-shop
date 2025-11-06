using System.Collections.Generic;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;

namespace BuffsInShop.Buff;

public class SacrificialTotem : ModBuffInShop
{
    public override string OriginTower => TowerType.Ezili;
    public override int OriginTopPath => 6;

    public override bool Hero => true;

    public override float BaseCost => 3500;
    public override bool SubsequentDiscount => true;

    private static TowerModel Totem => GameModel.GetTower(TowerType.SacrificialTotem);

    public override string BaseDescription =>
        "Gives a tower +1 pierce, +20% range, +18% attack speed, +25 projectile speed, and Camo Detection. " +
        "Wizard Monkeys gain an additional +1 pierce and 6% attack speed.";

    public override AudioClipReference PlacementSound => new(VanillaAudioClips.ActivatedSacrificialTotem);

    public override EffectModel? PlacementEffect => Totem.GetBehavior<CreateEffectOnPlaceModel>().effectModel;

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        yield return Totem.GetBehavior<RangeSupportModel>().CreateMutator();
        yield return Totem.GetBehavior<RateSupportModel>().CreateMutator();
        yield return Totem.GetBehavior<VisibilitySupportModel>().CreateMutator();
        yield return Totem.GetBehavior<PierceSupportModel>().CreateMutator();
        yield return Totem.GetBehavior<ProjectileSpeedSupportModel>().CreateMutator();

        if (BuffsInShopMod.BypassTowerRestrictions || tower?.towerModel.baseId == TowerType.WizardMonkey)
        {
            yield return Totem.GetBehavior<RateSupportModel>("Wizard").CreateMutator();
            yield return Totem.GetBehavior<PierceSupportModel>("Wizard").CreateMutator();
        }
    }
}