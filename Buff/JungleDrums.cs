using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using UnityEngine;

namespace BuffsInShop.Buff;

public class JungleDrums : ModBuffInShop
{
    public override string OriginTower => TowerType.MonkeyVillage;
    public override int OriginTopPath => 2;

    public override string BaseDescription => "Gives a tower 18% increased attack speed.";
    public override KeyCode KeyCode => KeyCode.J;
    public override bool SubsequentDiscount => true;

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetBehavior<RateSupportModel>(Name).CreateMutator();
}