using System;
using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Data.Global;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Mutators;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Mutators;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons.Behaviors;

namespace BuffsInShop;

public static class Extensions
{
    public static BuffIconSprite GetIcon(this BuffIndicatorModel buffIndicator) =>
        GameData.Instance.buffIconSprites.buffIconSprites
            .FirstOrDefault(sprite => sprite.buffId == buffIndicator.iconName);

    public static RateSupportModel.RateSupportMutator CreateMutator(this RateSupportModel rateSupport) =>
        new(rateSupport.isUnique, rateSupport.mutatorId, rateSupport.multiplier, rateSupport.priority,
            rateSupport.GetBuffIndicatorModel(), false, rateSupport.buffDisplayModel);

    public static PierceSupport.MutatorTower CreateMutator(this PierceSupportModel pierceSupport) => new(
        new PierceSupport
        {
            pierceSupportModel = pierceSupport
        });

    public static ProjectileSpeedSupport.ProjectileSpeedSupportMutator CreateMutator(
        this ProjectileSpeedSupportModel projSpeedSupport) =>
        new(projSpeedSupport.multiplier, projSpeedSupport.isUnique, projSpeedSupport.mutatorId, 0,
            projSpeedSupport.GetBuffIndicatorModel());

    public static RangeSupport.MutatorTower CreateMutator(this RangeSupportModel rangeSupport) =>
        new(rangeSupport.isUnique, rangeSupport.mutatorId, rangeSupport.additive,
            rangeSupport.multiplier, rangeSupport.GetBuffIndicatorModel());

    public static AbilityCooldownScaleSupport.MutatorTower CreateMutator(
        this AbilityCooldownScaleSupportModel abilityCooldownScaleSupport) =>
        new(new AbilityCooldownScaleSupport
        {
            abilityCooldownScaleSupportModel = abilityCooldownScaleSupport
        }, abilityCooldownScaleSupport.abilityCooldownSpeedScale, abilityCooldownScaleSupport.name);

    public static VisibilitySupport.MutatorTower CreateMutator(
        this VisibilitySupportModel visibilitySupport) => new(new VisibilitySupport
    {
        visibilitySupportModel = visibilitySupport
    });

    public static AddBehaviorToTowerSupport.MutatorTower CreateMutator(
        this AddBehaviorToTowerSupportModel addBehaviorToTowerSupport) => new(addBehaviorToTowerSupport.behaviors,
        addBehaviorToTowerSupport.mutationId, addBehaviorToTowerSupport.GetBuffIndicatorModel());

    public static MonkeyCityIncomeSupport.MutatorTower CreateMutator(
        this MonkeyCityIncomeSupportModel monkeyCityIncomeSupport) => new(
        monkeyCityIncomeSupport.GetBuffIndicatorModel(), monkeyCityIncomeSupport.incomeModifier,
        monkeyCityIncomeSupport.uniqueMutatorId);

    public static TargetSupplierSupportModel.MutatorTower CreateMutator(
        this TargetSupplierSupportModel targetSupplierSupport) => new(targetSupplierSupport);


    public static FlagshipAttackSpeedIncrease.Mutator CreateMutator(
        this FlagshipAttackSpeedIncreaseModel flagshipAttackSpeedIncreaseModel) =>
        new(flagshipAttackSpeedIncreaseModel);

    public static SubCommanderSupport.MutatorTower CreateMutator(this SubCommanderSupportModel subCommanderSupport) =>
        new(new SubCommanderSupport
        {
            subCommandSupportModel = subCommanderSupport
        });

    public static BonusCashZone.Mutator CreateMutator(this BonusCashZoneModel bonusCashZone) =>
        new(true, bonusCashZone.stackName, bonusCashZone.multiplier)
        {
        };

    public static DamageSupport.MutatorTower CreateMutator(this DamageSupportModel damageSupport) =>
        new(damageSupport.increase, damageSupport.isUnique, damageSupport.mutatorId,
            damageSupport.GetBuffIndicatorModel());

    public static PiercePercentageSupport.MutatorTower CreateMutator(
        this PiercePercentageSupportModel piercePercentageSupport) => new(new PiercePercentageSupport
    {
        piercePercentageSupportModel = piercePercentageSupport
    });

    public static RateSupportModel.RateSupportMutator CreateMutator(
        this ActivateRateSupportZoneModel activateRateSupportZoneModel) => new(activateRateSupportZoneModel.isUnique,
        activateRateSupportZoneModel.mutatorId, activateRateSupportZoneModel.rateModifier, 0,
        activateRateSupportZoneModel.GetBuffIndicatorModel());

    public static DamageTypeSupport.MutatorTower CreateMutator(
        this DamageTypeSupportModel damageTypeSupportModel) => new(damageTypeSupportModel.mutatorId,
        damageTypeSupportModel.isUnique, damageTypeSupportModel.GetBuffIndicatorModel(),
        damageTypeSupportModel.immuneBloonProperties);



    public static int Compare<T, TBase>(this (T, TBase) elements, Func<IEnumerable<T>, IEnumerable<T>> orderBy,
        Func<TBase, int>? fallback = null) where T : TBase
    {
        var (self, unknown) = elements;

        if (unknown is T other && !Equals(self, unknown))
        {
            var a = orderBy([self, other]).First();
            var b = orderBy([other, self]).First();

            if (Equals(self, a) && Equals(self, b)) return -1;

            if (Equals(unknown, a) && Equals(unknown, b)) return 1;
        }

        return fallback?.Invoke(unknown) ?? 0;
    }

    public static int Compare<T, TBase>(this T self, TBase unknown, Func<IEnumerable<T>, IEnumerable<T>> orderBy,
        Func<TBase, int>? fallback = null) where T : IComparable<TBase>, TBase
    {
        return (self, unknown).Compare(orderBy, fallback);
    }

    public static int Compare<T, TBase>(this T self, TBase unknown, Func<TBase, int>? fallback,
        Func<IEnumerable<T>, IEnumerable<T>> orderBy) where T : IComparable<TBase>, TBase
    {
        return (self, unknown).Compare(orderBy, fallback);
    }
}