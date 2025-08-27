using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using UnityEngine;

namespace BuffsInShop.Buff;

public class MonkeyVillager : ModBuffInShop
{
    public override string OriginTower => TowerType.MonkeyVillage;
    public override float BaseCost => TowerCost / 2;
    public override string BaseDescription => "Gives a monkey 10% increased range.";
    public override KeyCode KeyCode => KeyCode.V;
    public override bool SubsequentDiscount => true;

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetBehavior<RangeSupportModel>().CreateMutator();

}