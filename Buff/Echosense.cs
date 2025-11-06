using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;

namespace BuffsInShop.Buff;

public class Echosense : ModBuffInShop
{
    public override string OriginTower => TowerType.Mermonkey;
    public override int OriginBotPath => 2;

    public override float BaseCost => UpgradeCost;

    public override string BaseDescription =>
        $"Gives a Mermonkey 6% increased range. Stackable up to {MaxStacks} times.";

    public override int MaxStacks => 10;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && tower.towerModel.baseId != TowerType.Mermonkey)
        {
            helperMessage = "Must be a Mermonkey";
            return false;
        }
        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower)
    {
        var stacks = tower == null ? -1 : GetStackCount(tower);
        var model = OriginTowerModel.GetBehavior<SupportStackingRangeModel>();

        return new SupportStackingRange.MutatorTower(new SupportStackingRange
        {
            supportStackingRangeModel = model,
        }, model.mutatorId + (stacks + 1));
    }
}