using System.Linq;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using UnityEngine;

namespace BuffsInShop.Buff;

public class MonkeyCity : ModBuffInShop
{
    public override string OriginTower => TowerType.MonkeyVillage;
    public override int OriginBotPath => 4;

    public override float BaseCost => GetInstance<MonkeyTown>().BaseCost;
    public override string BaseDescription => "Gives a tower 20% increased cash generation.";
    public override KeyCode KeyCode => KeyCode.Y;
    public override bool SubsequentDiscount => true;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && !defaultMutators.First()
                .Mutate(tower.rootModel, tower.rootModel.Clone().Cast<TowerModel>()))
        {
            helperMessage = "Must have cash generation effects.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetBehavior<MonkeyCityIncomeSupportModel>().CreateMutator();
}