using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper.Api.Helpers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Mods;
using Il2CppAssets.Scripts.Models.Towers.Mutators;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace BuffsInShop.Buff;

public class SunTemple : ModBuffInShop
{
    public override string OriginTower => TowerType.SuperMonkey;
    public override int OriginTopPath => 4;

    public override string DisplayName => "Sun Temple's Blessing";

    public override float BaseCost => 50000;
    public override string BaseDescription => "Gives a tower the support of the Sun Temple.";
    public override KeyCode KeyCode => KeyCode.T;
    public override bool SubsequentDiscount => true;

    public override AudioClipReference? PlacementSound => GameModel.GetTower(TowerType.SuperMonkey, 3, 0, 0)
        .GetBehavior<CreateSoundOnAttachedModel>().sound.assetId;

    public override EffectModel? PlacementEffect => GameModel.GetTower(TowerType.SunAvatarMini)
        .GetBehavior<CreateEffectOnPlaceModel>().effectModel;

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower) => GetMutators(OriginTowerModel);

    public static IEnumerable<BehaviorMutator> GetMutators(TowerModel towerModel)
    {
        var sacrifices = towerModel
            .GetBehaviors<TempleTowerMutatorGroupModel>()
            .Where(model => model.towerSet == TowerSet.Support)
            .SelectMany(model => model.mutators.OfIl2CppType<AddBehaviorToTowerMutatorModel>())
            .SelectMany(model => model.behaviors)
            .ToArray();

        foreach (var rateSupportModel in sacrifices.OfIl2CppType<RateSupportModel>())
        {
            yield return rateSupportModel.CreateMutator();
        }

        foreach (var rangeSupportModel in sacrifices.OfIl2CppType<RangeSupportModel>())
        {
            yield return rangeSupportModel.CreateMutator();
        }

        foreach (var rangeSupportModel in sacrifices.OfIl2CppType<RangeSupportModel>())
        {
            yield return rangeSupportModel.CreateMutator();
        }

        foreach (var pierceSupportModel in sacrifices.OfIl2CppType<PierceSupportModel>())
        {
            yield return pierceSupportModel.CreateMutator();
        }

        foreach (var damageSupportModel in sacrifices.OfIl2CppType<DamageSupportModel>())
        {
            yield return damageSupportModel.CreateMutator();
        }
    }

    public override void ExtraMutation(TowerModel towerModel)
    {
        ExtraMutation(towerModel, OriginTowerModel);
    }

    public static void ExtraMutation(TowerModel towerModel, TowerModel monkey)
    {
        var discount = monkey
            .GetBehaviors<TempleTowerMutatorGroupModel>()
            .Where(model => model.towerSet == TowerSet.Support)
            .SelectMany(model => model.mutators.OfIl2CppType<AddBehaviorToTowerMutatorModel>())
            .SelectMany(model => model.behaviors)
            .OfIl2CppType<DiscountZoneModel>()
            .Last();

        towerModel.AddBehavior(new DiscountZoneModModel(discount.stackName, discount.discountMultiplier,
            ModelSerializer.SerializeModel(discount)));
    }
}