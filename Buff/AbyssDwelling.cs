using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using UnityEngine;

namespace BuffsInShop.Buff;

public class AbyssDwelling : ModBuffInShop
{
    public override string OriginTower => TowerType.Mermonkey;
    public override int OriginTopPath => 3;

    public override string BaseDescription => "Gives a tower 10% increased pierce.";
    public override float BaseCost => 800;
    public override bool SubsequentDiscount => true;

    public override KeyCode KeyCode => KeyCode.LeftBracket;

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetBehavior<PiercePercentageSupportModel>().CreateMutator();
}