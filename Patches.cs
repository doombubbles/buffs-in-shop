using System.Linq;
using System.Reflection;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Helpers;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Profile;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Mods;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Simulation.Input;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Utils;
using Il2CppSystem.Collections.Generic;

namespace BuffsInShop;

/// <summary>
/// Hijack a rate mutator for storing mod buff purchase info
/// </summary>
[HarmonyPatch(typeof(RateSupportModel.RateSupportMutator), nameof(RateSupportModel.RateSupportMutator.Mutate))]
internal static class RateSupportMutator_Mutate
{
    [HarmonyPrefix]
    internal static bool Prefix(RateSupportModel.RateSupportMutator __instance, Model model, ref bool __result)
    {
        if (!ModBuffInShop.Cache.TryGetValue(__instance.id, out var buff) ||
            !model.Is(out TowerModel tower)) return true;

        __result = true;

        buff.ExtraMutation(tower);

        return false;

    }
}

/// <summary>
/// Load towers late after the mutations have been restored
/// </summary>
[HarmonyPatch(typeof(MapSaveLoader), nameof(MapSaveLoader.LoadMapSaveData))]
internal static class MapSaveLoader_LoadMapSaveData
{
    [HarmonyPostfix]
    internal static void Postfix(Simulation sim, MapSaveDataModel mapData)
    {
        if (mapData == null) return;
        foreach (var saveData in mapData.placedTowers)
        {
            if (saveData == null) continue;
            var tower = sim.towerManager.GetTowerById(saveData.IdLastSave);

            BuffsInShopMod.OnLateTowerLoaded(tower, saveData);
        }
    }
}

/// <summary>
/// Personal free upgrade support
/// </summary>
[HarmonyPatch(typeof(TowerManager), nameof(TowerManager.GetFreeUpgrade))]
internal static class TowerManager_GetFreeUpgrade
{
    [HarmonyPostfix]
    internal static void Postfix(Tower tower, int tier, ref bool __result) =>
        __result |= tower.towerModel.GetBehaviors<FreeUpgradeSupportModel>()
            .Any(model => model.upgrade < 0 && tier <= -model.upgrade);
}

/// <summary>
/// Personal discount zone
/// </summary>
[HarmonyPatch(typeof(TowerManager), nameof(TowerManager.GetZoneDiscount))]
internal static class TowerManager_GetZoneDiscount
{
    [HarmonyPostfix]
    internal static void Postfix(TowerModel towerModel, Dictionary<string, List<DiscountZone>> __result)
    {
        foreach (var discount in towerModel.GetBehaviors<DiscountZoneModModel>())
        {
            var discountZoneModel = ModelSerializer.DeserializeModel<DiscountZoneModel>(discount.specificScriptId);

            __result.TryAdd(discountZoneModel.stackName, new List<DiscountZone>());

            __result[discountZoneModel.stackName].Add(new DiscountZone
            {
                discountZoneModel = discountZoneModel
            });
        }
    }
}

/// <summary>
/// Handle RequireBuffOriginUsable
/// </summary>
[HarmonyPatch(typeof(Simulation), nameof(Simulation.StockStandardTowerInventory))]
internal static class Simulation_StockStandardTowerInventory
{
    [HarmonyPostfix]
    internal static void Postfix(TowerInventory ti)
    {
        if (!BuffsInShopMod.RequireBuffOriginUsable) return;

        foreach (var buff in ModContent.GetContent<ModBuffInShop>().Where(buff => buff.IsBlocked(ti)))
        {
            ti.towerMaxes[buff.Id] = 0;
        }
    }
}

/// <summary>
/// Handle buff SubsequentDiscounts
/// </summary>
[HarmonyPatch(typeof(TowerInventory))]
internal static class TowerInventory_TowerChanged
{
    private static System.Collections.Generic.IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(TowerInventory), nameof(TowerInventory.DestroyedTower));
        yield return AccessTools.Method(typeof(TowerInventory), nameof(TowerInventory.CreatedTower));
        yield return AccessTools.Method(typeof(TowerInventory), nameof(TowerInventory.UpdatedTower));
    }

    [HarmonyPostfix]
    internal static void Postfix()
    {
        TaskScheduler.ScheduleTask(ModBuffInShop.UpdateDiscounts);
    }
}

[HarmonyPatch(typeof(TowerModel), nameof(TowerModel.GetPrimaryWeaponThrowMarkerHeight))]
internal static class TowerModel_GetPrimaryWeaponThrowMarkerHeight
{
    [HarmonyPrefix]
    internal static bool Prefix(TowerModel __instance, ref float __result)
    {
        if (!ModBuffInShop.Cache.ContainsKey(__instance.baseId)) return true;

        __result = 0;
        return false;
    }
}