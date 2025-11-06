using System.Collections.Generic;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using UnityEngine;

namespace BuffsInShop.Buff;

public class PrimaryExpertise : ModBuffInShop<PrimaryMentoring>
{
    public override string OriginTower => TowerType.MonkeyVillage;
    public override int OriginTopPath => 5;

    public override float BaseCost => UpgradeCost / 5;
    public override string BaseDescription =>
        "Gives a tower with Primary Mentoring more popping power, tier 2 upgrades for free, and further reduced ability cooldowns.";
    public override KeyCode KeyCode => KeyCode.X;
    public override bool SubsequentDiscount => true;

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower) =>
        GetInstance<PrimaryMentoring>().GetMutators(OriginTowerModel);

    public override bool ExtraMutation(TowerModel towerModel)
    {
        var model = OriginTowerModel.GetBehavior<FreeUpgradeSupportModel>().Duplicate(Name);

        model.upgrade *= -1;
        model.appliesToOwningTower = true;

        towerModel.AddBehavior(model);

        return true;
    }
}