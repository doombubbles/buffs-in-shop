using System.Collections.Generic;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using UnityEngine;

namespace BuffsInShop.Buff;

public class PrimaryMentoring : ModBuffInShop<PrimaryTraining>
{
    public override string OriginTower => TowerType.MonkeyVillage;
    public override int OriginTopPath => 4;

    public override string BaseDescription =>
        "Gives a tower with Primary Training increased range, tier 1 upgrades for free, and reduced ability cooldowns.";
    public override KeyCode KeyCode => KeyCode.M;
    public override bool SubsequentDiscount => true;

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower) => GetMutators(OriginTowerModel);

    public IEnumerable<BehaviorMutator> GetMutators(TowerModel towerModel)
    {
        foreach (var mutator in GetInstance<PrimaryTraining>().GetMutators(towerModel))
        {
            yield return mutator;
        }

        yield return OriginTowerModel.GetBehavior<RangeSupportModel>(Name).CreateMutator();

        yield return OriginTowerModel.GetBehavior<AbilityCooldownScaleSupportModel>().CreateMutator();
    }

    public override void ExtraMutation(TowerModel towerModel)
    {
        var model = OriginTowerModel.GetBehavior<FreeUpgradeSupportModel>().Duplicate(Name);

        model.upgrade *= -1;
        model.appliesToOwningTower = true;

        towerModel.AddBehavior(model);
    }
}