using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using UnityEngine;

namespace BuffsInShop.Buff;

public class MonkeyTown : ModBuffInShop
{
    public override string OriginTower => TowerType.MonkeyVillage;
    public override int OriginBotPath => 3;

    public override string BaseDescription => "Makes a monkey get 50% extra cash per Bloon pop.";
    public override KeyCode KeyCode => KeyCode.W;
    public override bool SubsequentDiscount => true;

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetBehavior<AddBehaviorToTowerSupportModel>("CashUp").CreateMutator();
}