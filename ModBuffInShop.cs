using System;
using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Data;
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using BuffsInShop.Buff;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Profile;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Upgrades;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Simulation.Input;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.RightMenu;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.StoreMenu;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using Il2CppSystem.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace BuffsInShop;

public abstract class ModBuffInShop : ModFakeTower<Buffs>, IModSettings
{
    private static readonly JObject BaseCosts = typeof(ModBuffInShop).Assembly.GetEmbeddedJson("costs.json");

    public static readonly Dictionary<string, ModBuffInShop> Cache = new();

    public static readonly ModSettingCategory Costs = new("Costs");

    public static readonly ModSettingCategory Hotkeys = new("Hotkeys");

    public static readonly ModSettingCategory Always = new("Always Usable");

    public static readonly ModSettingCategory GodBoosting = new("God Boost")
    {
        icon = GetTextureGUID<BuffsInShopMod>(nameof(GodBoost)),
    };


    protected BehaviorMutator[] defaultMutators = null!;
    private ModSettingInt cost = null!;
    private ModSettingHotkey hotkey = null!;
    private ModSettingBool includeInGodBoost = null!;
    private ModSettingBool alwaysUsable = null!;

    public static Simulation Sim => InGame.Bridge.Simulation;

    public static GameModel GameModel => InGame.instance == null ? Game.instance.model : Sim.model;


    public abstract string? OriginTower { get; }
    public abstract string BaseDescription { get; }

    public float UpgradeCost => OriginTower == null ? 0 : OriginUpgradeModel?.cost ?? 0;
    public float TowerCost => OriginTowerModel?.cost ?? 0;

    public virtual float BaseCost => UpgradeCost / 2;
    public virtual KeyCode KeyCode => KeyCode.None;
    public virtual bool SubsequentDiscount => false;
    public virtual int PriorityBoost => 1;
    public virtual bool AllowInChimps => true;
    public virtual int OriginTopPath => 0;
    public virtual int OriginMidPath => 0;
    public virtual int OriginBotPath => 0;
    public virtual int[] OriginTiers => [OriginTopPath, OriginMidPath, OriginBotPath];
    public virtual bool DefaultGodBoost => mod is BuffsInShopMod;
    public virtual bool Hero => false;
    public virtual int MaxStacks => 1;
    public virtual bool ParagonAllowed => false;


    private TowerModel? originTowerModel;
    public virtual TowerModel OriginTowerModel => OriginTower == null
        ? null!
        : originTowerModel ??=
            InGame.instance != null &&
            GameModel.GetTower(OriginTower, OriginTopPath, OriginMidPath, OriginBotPath)?.Is(out var current) == true &&
            IsValidOrigin(current)
                ? current
                : Game.instance.model.GetTower(OriginTower, OriginTopPath, OriginMidPath, OriginBotPath);

    public virtual UpgradeModel? OriginUpgradeModel =>
        OriginTiers.Any(i => i > 0) ? GameModel.GetUpgrade(OriginTowerModel?.appliedUpgrades.Last()) : null;

    public virtual bool IsValidOrigin(TowerModel current) => true;

    /// <summary>
    /// Default is Sharpening Stone apply sound
    /// </summary>
    public override AudioClipReference? PlacementSound => new("4e14271436fb5b445afff1240816d6b9");

    /// <summary>
    /// Default is power placement effect
    /// </summary>
    public override EffectModel? PlacementEffect =>
        new("", new PrefabReference("cc4e51ecd049ee249a5bd51db3612483"), 1, 1);


    public IEnumerable<string> MutatorIds => defaultMutators.Select(mutator => mutator.id);

    public override string? Icon => GetSpriteReferenceOrNull(Name)?.AssetGUID;

    public override SpriteReference IconReference =>
        Icon != null ? new SpriteReference(Icon) : OriginUpgradeModel?.icon ?? OriginTowerModel.icon;

    public override ModSettingHotkey Hotkey => hotkey;
    public override int Cost => SavedBaseCost == null ? (int) BaseCost : cost;
    public override bool DontAddToShop => Cost < 0;
    private int? SavedBaseCost => BaseCosts.Value<int?>(Name);
    public bool IncludeInGodBoost => this is not GodBoost && includeInGodBoost;

    public override bool HighlightTowers => true;

    public virtual bool AffectsSubTowers => true;

    public sealed override string Description =>
        BaseDescription +
        (SubsequentDiscount
            ? "\nSubsequent purchases are less expensive."
            : "");

    public override void Register()
    {
        base.Register();


        defaultMutators = GetMutators(null).ToArray();
        foreach (var defaultMutator in defaultMutators)
        {
            defaultMutator.priority += PriorityBoost;
        }
        originTowerModel = null;

        Cache[Id] = this;
    }

    public override IEnumerable<ModContent> Load()
    {
        var baseCost = SavedBaseCost;

        if (baseCost is null)
        {
            try
            {
                baseCost = (int) BaseCost;
            }
            catch (Exception)
            {
                baseCost = 0;
            }
        }

        mod.ModSettings[Name + "Cost"] = cost = new ModSettingInt(baseCost.Value)
        {
            category = Costs,
            icon = Icon,
            displayName = DisplayName,
            description = $"In Game Cost for {DisplayName}. Set to a negative number to disable the buff."
        };

        mod.ModSettings[Name + "HotKey"] = hotkey = new ModSettingHotkey(KeyCode,
            KeyCode == KeyCode.None ? HotkeyModifier.None : HotkeyModifier.Alt)
        {
            category = Hotkeys,
            icon = Icon,
            displayName = DisplayName,
            description = $"Hot Key for {DisplayName}"
        };

        if (this is not GodBoost)
        {
            mod.ModSettings[Name + "Always"] = alwaysUsable = new ModSettingBool(false)
            {
                category = Always,
                icon = Icon,
                displayName = DisplayName,
                description =
                    $"Makes {DisplayName} always show up in the shop regardless of the Require Buff Origin Usable setting"
            };

            mod.ModSettings[Name + "GodBoosting"] = includeInGodBoost = new ModSettingBool(DefaultGodBoost)
            {
                category = GodBoosting,
                icon = Icon,
                displayName = DisplayName,
                description = $"Whether {DisplayName} should be included within the God Boost buff. " +
                              $"Reminder: Still follows the Bypass Tower Restrictions setting."
            };
        }

        return base.Load();
    }

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        base.ModifyBaseTowerModel(towerModel);
        towerModel.behaviors ??= new Il2CppReferenceArray<Model>(0);
    }

    public override void ModifyTowerModelForMatch(TowerModel towerModel, GameModel gameModel)
    {
        originTowerModel = null;
    }

    public override int CompareTo(ModContent other) => this.Compare(other, base.CompareTo, buffs => buffs
        .OrderBy(buff => buff.Order)
        .ThenBy(buff => buff.Hero)
        .ThenBy(buff => buff.ParagonAllowed)
        .ThenByDescending(buff => buff.OriginTower == TowerType.Alchemist)
        .ThenByDescending(buff => buff.OriginTower == TowerType.Desperado)
        .ThenByDescending(buff => buff.OriginTower == TowerType.EngineerMonkey)
        .ThenByDescending(buff => buff.OriginTower == TowerType.MonkeyVillage)
        .ThenByDescending(buff => buff.OriginTower != TowerType.SuperMonkey)
        .ThenBy(buff => buff.OriginTower != null ? TowerType.towers.IndexOf(buff.OriginTower) : 999)
        .ThenBy(buff => (buff.OriginBotPath, buff.OriginMidPath, buff.OriginTopPath))
    );

    public override bool CanPlaceAt(Vector2 at, Tower? tower, ref string helperMessage)
    {
        if (tower == null)
        {
            helperMessage = "Must be placed on a tower.";
            return false;
        }

        if (!tower.IsSelectable)
        {
            helperMessage = "Can't select that tower";
            return false;
        }

        if (tower.towerModel.HasBehavior<IgnoreAllMutatorsTowerModel>())
        {
            helperMessage = "That tower can't be mutated.";
            return false;
        }

        return CanApplyTo(tower, ref helperMessage);
    }

    public override void OnPlace(Vector2 at, TowerModel towerModelFake, Tower? hoveredTower, float towerCost)
    {
        if (hoveredTower == null) return;

        hoveredTower.worth += towerCost;
        Apply(hoveredTower, towerCost, true);

        UpdateDiscounts();
    }

    public int GetStackCount(Tower? tower) =>
        tower?.GetMutatorById(Id)?.mutator.Is(out RateSupportModel.RateSupportMutator rateSupportMutator) == true
            ? rateSupportMutator.priority
            : 0;

    public bool HasBuff(Tower tower) => tower.GetMutatorById(Id) != null;

    public bool HasBuff(Tower tower, out float towerCost)
    {
        var mutator = tower.GetMutatorById(Id);
        towerCost = mutator?.mutator.Is(out RateSupportModel.RateSupportMutator fakeMutator) == true
            ? fakeMutator.multiplier
            : -1;
        return mutator is not null;
    }

    public virtual BehaviorMutator GetMutator(Tower? tower) => null!;

    public virtual IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        yield return GetMutator(tower);
    }

    public virtual bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (GetStackCount(tower) >= MaxStacks ||
            MaxStacks == 1 && defaultMutators.Length > 0 &&
            defaultMutators.All(defaultMutator =>
                tower.GetMutatorById(defaultMutator.id).Is(out var mutator) &&
                mutator.mutator.priority >= defaultMutator.priority))
        {
            helperMessage = "Tower already buffed.";
            return false;
        }

        if (tower.IsParagonBased() && !ParagonAllowed)
        {
            helperMessage = "Can't buff Paragons.";
            return false;
        }

        if (tower.ParentId.IsValid)
        {
            helperMessage = "Can't apply directly to sub towers";
            return false;
        }

        return true;
    }


    public virtual void Apply(Tower tower, float purchaseCost = -1, bool sideEffects = false)
    {
        var mutators = GetMutators(tower).ToArray();
        foreach (var mutator in mutators)
        {
            mutator.priority += PriorityBoost;
            mutator.cantBeAbsorbed = true;
        }

        var affectsSubTowers = BuffsInShopMod.AlwaysAffectSubTowers || AffectsSubTowers;

        if (purchaseCost > -1)
        {
            var count = GetStackCount(tower);
            tower.RemoveMutatorsById(Id);
            var mutator = new RateSupportModel.RateSupportMutator(true, Id, purchaseCost, count + 1, null)
            {
                cantBeAbsorbed = true
            };
            if (tower.IsParagonBased())
            {
                tower.AddParagonMutator(mutator);
            }
            else
            {
                tower.AddMutator(mutator);
            }
        }

        foreach (var mutatorId in MutatorIds)
        {
            if (affectsSubTowers)
            {
                tower.RemoveMutatorsIncludeSubTowersById(mutatorId);
            }
            else
            {
                tower.RemoveMutatorsById(mutatorId);
            }
        }

        foreach (var mutator in mutators)
        {
            if (affectsSubTowers)
            {
                if (ParagonAllowed && tower.IsParagonBased())
                {
                    tower.AddParagonMutatorIncludeSubTowers(mutator);
                }
                else
                {
                    tower.AddMutatorIncludeSubTowers(mutator);
                }
            }
            else
            {
                if (ParagonAllowed && tower.IsParagonBased())
                {
                    tower.AddParagonMutator(mutator);
                }
                else
                {
                    tower.AddMutator(mutator);
                }
            }
        }
    }

    public virtual bool ExtraMutation(TowerModel towerModel) => false;

    public virtual void OnSaved(Tower tower, TowerSaveDataModel saveData)
    {
    }

    public virtual void OnLoaded(Tower tower, TowerSaveDataModel saveData)
    {
    }

    public virtual bool IsBlocked(TowerInventory ti) =>
        !alwaysUsable &&
        OriginTower != null &&
        (ti.towerMaxes.TryGetValue(OriginTower, out var max) && max == 0 || OriginTiers.Where((tier, path) =>
            tier != 0 && ti.IsPathTierLocked(new Tower {towerModel = OriginTowerModel}, path, tier)).Any());

    internal static void UpdateDiscounts()
    {
        var discounts = InGame.Bridge.Simulation.GetTowerInventory(InGame.Bridge.MyPlayerNumber).towerDiscounts;
        var towers = InGame.Bridge.Simulation.towerManager.GetTowers().ToArray();

        foreach (var buff in GetContent<ModBuffInShop>().Where(buff => buff.SubsequentDiscount))
        {
            var doDiscount = towers.Any(buff.HasBuff);

            var i = discounts.FindIndex(d => d.id == buff.Id);

            if (doDiscount == i >= 0) continue;

            if (doDiscount)
            {
                discounts.Add(new TowerDiscount
                {
                    id = buff.Id,
                    towers = new Il2CppStringArray([buff.Id]),
                    charges = 99999999,
                    multiplier = .5f,
                    subtraction = 0
                });
            }
            else
            {
                discounts.RemoveAt(i);
            }

            var button = ShopMenu.instance.GetTowerButtonFromBaseId(buff.Id);

            if (button != null && button.GetComponentInChildren<ITowerPurchaseButton>().Is(out var purchaseButton))
            {
                purchaseButton.SetDirty();
            }
        }
    }
}

public abstract class ModBuffInShop<T> : ModBuffInShop where T : ModBuffInShop
{
    public override int PriorityBoost => GetInstance<T>().PriorityBoost + 1;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!base.CanApplyTo(tower, ref helperMessage)) return false;

        var buff = GetInstance<T>();

        if (!buff.HasBuff(tower))
        {
            helperMessage = $"Must have the {buff.DisplayName} buff";
            return false;
        }

        return true;
    }
}