using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using UnityEngine;

namespace BuffsInShop.Buff;

public class MonkeyIntelligenceBureau : ModBuffInShop
{
    public override string OriginTower => TowerType.MonkeyVillage;
    public override int OriginMidPath => 3;

    public override string BaseDescription =>
        "Grants special Bloon popping knowledge to a Tower, allowing it to pop all Bloon types.";
    public override KeyCode KeyCode => KeyCode.I;
    public override bool SubsequentDiscount => true;

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetDescendant<DamageTypeSupportModel>().CreateMutator();
}