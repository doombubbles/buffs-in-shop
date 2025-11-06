using System;
using System.Linq;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.SimulationBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.SimulationBehaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;

namespace BuffsInShop.Buff;

public class BloodSacrifice : ModBuffInShop
{
    public override string OriginTower => TowerType.Adora;
    public override int OriginTopPath => 19;

    public override bool Hero => true;

    public override bool IsValidOrigin(TowerModel current) => current.HasBehavior<AbilityModel>(Name, out _);

    public override string BaseDescription =>
        "Gives Adora or a Sun Avatar/Temple/God 10% increased range and 10% reduced reload time, but puts you $500 in debt. " +
        $"Can be purchased up to {MaxStacks} times per tower, doubling the debt each time.";

    public override float BaseCost => 1000;

    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetBehavior<AbilityModel>(Name).GetBehavior<CreateSoundOnAbilityModel>().sound.assetId;

    public override EffectModel? PlacementEffect =>
        OriginTowerModel.GetBehavior<AbilityModel>(Name).GetBehavior<BloodSacrificeModel>().effectAtSacrificeModel;

    public override int MaxStacks => 5;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (Sim.GetBehaviors().ToArray().OfIl2CppType<ImfLoanCollection>()
            .Any(loan => loan.imfLoanCollectionModel.name.Contains(Name + tower.Id)))
        {
            helperMessage = "Still in debt from last purchase";
            return false;
        }

        if (!BuffsInShopMod.BypassTowerRestrictions && tower.towerModel.baseId != TowerType.Adora &&
            !(tower.towerModel.baseId == TowerType.SuperMonkey && tower.towerModel.tiers[0] >= 3))
        {
            helperMessage = "Must be Adora or a Sun Avatar/Temple/God";
            return false;
        }

        if (tower.GetMutatorById("BloodSacrificeBuff")?.mutator?
                .Is(out BloodSacrificeModel.BloodSacrificeMutator bloodSacrificeMutator) == true
            && bloodSacrificeMutator.bonusCount >= 40)
        {
            helperMessage = "Sacrifice Level is at maximum";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower)
    {
        var sacrifice = OriginTowerModel.GetDescendant<BloodSacrificeModel>();

        var bonusCount = 0;

        if (tower?.GetMutatorById("BloodSacrificeBuff")?.mutator
                ?.Is(out BloodSacrificeModel.BloodSacrificeMutator bloodSacrificeMutator) == true)
        {
            bonusCount = bloodSacrificeMutator.bonusCount + 10;
        }

        return new BloodSacrificeModel.BloodSacrificeMutator(sacrifice, bonusCount, OriginTowerModel.name);
    }

    public override void Apply(Tower tower, float purchaseCost = -1, bool sideEffects = false)
    {
        base.Apply(tower, purchaseCost, sideEffects);

        if (sideEffects)
        {
            var stacks = GetStackCount(tower);
            var debt = 250 * Math.Pow(2, stacks);

            TaskScheduler.ScheduleTask(() => Sim.AddBehavior<ImfLoanCollection>(new ImfLoanCollectionModel(
                Name + tower.Id, .5f, (float) debt)));
        }
    }
}