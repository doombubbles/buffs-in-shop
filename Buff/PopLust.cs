using System.Linq;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppSystem.Linq;

namespace BuffsInShop.Buff;

public class PopLust : ModBuffInShop
{
    public override string OriginTower => TowerType.Druid;
    public override int OriginBotPath => 4;

    public override string DisplayName => "Poplust";

    public override string BaseDescription =>
        $"Gives a Druid 15% increased attack speed and pierce. Stackable up to {MaxStacks} times.";

    public override float BaseCost => UpgradeCost;

    public override int MaxStacks => 5;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && tower.towerModel.baseId != TowerType.Druid)
        {
            helperMessage = "Must be a Druid";
            return false;
        }
        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower)
    {
        var stacks = GetStackCount(tower);
        var poplust = OriginTowerModel.GetBehavior<PoplustSupportModel>();
        return new PoplustSupport.PoplustMutator(poplust.mutatorId, poplust, stacks + 1);
    }

    public override void Apply(Tower tower, float purchaseCost = -1, bool sideEffects = false)
    {
        base.Apply(tower, purchaseCost, sideEffects);

        if (sideEffects)
        {
            foreach (var poplustSupport in Sim.factory.GetUncast<PoplustSupport>().ToArray().Take(1))
            {
                poplustSupport.UpdateMutatorForTower(tower, Sim);
            }
        }
    }

    [HarmonyPatch(typeof(PoplustSupport), nameof(PoplustSupport.GetStacks))]
    internal static class PoplustSupport_GetStacks
    {
        [HarmonyPostfix]
        internal static void Postfix(PoplustSupport __instance, Tower towerToCheck, ref int __result)
        {
            __result += GetInstance<PopLust>().GetStackCount(towerToCheck);
            if (__result > __instance.poplustSupportModel.maxStacks)
            {
                __result = __instance.poplustSupportModel.maxStacks;
            }
        }
    }
}