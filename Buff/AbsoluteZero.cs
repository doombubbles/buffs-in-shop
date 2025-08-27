using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity;
using UnityEngine;

namespace BuffsInShop.Buff;

public class AbsoluteZero : ModBuffInShop
{
    public override TowerModel OriginTowerModel =>
        base.OriginTowerModel.HasDescendant<ActivateRateSupportZoneModel>()
            ? base.OriginTowerModel
            : Game.instance.model.GetTower(OriginTower, OriginTopPath, OriginMidPath, OriginBotPath);

    public override string OriginTower => TowerType.IceMonkey;
    public override int OriginMidPath => 5;
    public override string BaseDescription => "Gives an Ice Monkey 50% increased attack speed.";
    public override bool SubsequentDiscount => true;
    public override KeyCode KeyCode => KeyCode.Alpha0;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && tower.towerModel.baseId != TowerType.IceMonkey)
        {
            helperMessage = "Must be an Ice Monkey.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetDescendant<ActivateRateSupportZoneModel>().CreateMutator();
}