using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;

namespace BuffsInShop.Buff;

public class ShinobiTactics : ModBuffInShop
{
    public override string OriginTower => TowerType.NinjaMonkey;
    public override int OriginMidPath => 3;

    public override float BaseCost => UpgradeCost;

    public override string BaseDescription =>
        $"Gives a Ninja Monkey 8% increased pierce and 8% reduced attack cooldown. Stackable up to {MaxStacks} times.";

    public override int MaxStacks => 20;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && tower.towerModel.baseId != TowerType.NinjaMonkey)
        {
            helperMessage = "Must be a Ninja Monkey";
            return false;
        }
        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower)
    {
        var stacks = tower == null ? -1 : GetStackCount(tower);
        var shinobi = OriginTowerModel.GetBehavior<SupportShinobiTacticsModel>();

        return new SupportShinobiTactics.MutatorTower(new SupportShinobiTactics
        {
            supportShinobiTacticsModel = shinobi
        }, shinobi.mutatorId + (stacks + 1));
    }
}