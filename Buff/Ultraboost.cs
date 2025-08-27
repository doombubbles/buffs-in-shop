using System.Linq;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
namespace BuffsInShop.Buff;

public class Ultraboost : ModBuffInShop
{
    public override string OriginTower => TowerType.EngineerMonkey;
    public override int OriginMidPath => 5;

    public override float BaseCost => UpgradeCost;
    public override bool SubsequentDiscount => true;
    public override string BaseDescription =>
        "Gives a tower a stacking Overclock buff that increases in power each round, up to 10 times.";
    public override KeyCode KeyCode => KeyCode.U;

    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetDescendant<CreateSoundOnAbilityModel>().sound.assetId;

    public override EffectModel? PlacementEffect =>
        OriginTowerModel.GetDescendant<OverclockModel>().initialEffect;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions &&
            TapTowerAbilityBehavior.towerBanList.Contains(tower.towerModel.baseId))
        {
            helperMessage = $"Tower is on the {DisplayName} ban list.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower)
    {
        var ultraboost = OriginTowerModel.GetDescendant<OverclockPermanentModel>().Duplicate();
        ultraboost.mutatorsByStack = new Dictionary<int, OverclockPermanentModel.OverclockPermanentMutator>();

        var stacks = 1;

        if (tower?.GetMutatorById(OverclockPermanentModel.MutatorId)?.mutator
                .Is(out OverclockPermanentModel.OverclockPermanentMutator mutator) == true)
        {
            stacks += mutator.StackCount();
            if (stacks > ultraboost.maxStacks)
            {
                stacks = ultraboost.maxStacks;
            }

            if (!HasBuff(tower))
            {
                stacks--;
            }
        }

        return ultraboost.MutatorByStack(stacks);
    }

    public override void ExtraMutation(TowerModel towerModel)
    {
        var overclock = OriginTowerModel.GetDescendant<OverclockModel>();

        if (towerModel.behaviors
            .FirstOrDefault(model => model.Is<DisplayModel>() && model.name.EndsWith("Overclock"))
            .Is(out var behavior))
        {
            towerModel.RemoveBehavior(behavior);
        }

        towerModel.AddBehavior(new DisplayModel("Overclock", overclock.buffDisplayPath, -1, DisplayCategory.Buff));
    }

    public void HandleBoosting()
    {
        var ultraboost = Sim.model
            .GetTower(TowerType.EngineerMonkey, 0, 5)
            .GetDescendant<OverclockPermanentModel>();

        foreach (var tower in Sim.towerManager.GetTowers().AsIEnumerable())
        {
            if (HasBuff(tower) && tower.GetMutatorById(OverclockPermanentModel.MutatorId)?.mutator
                    .Is(out OverclockPermanentModel.OverclockPermanentMutator mutator) == true &&
                mutator.StackCount() < ultraboost.maxStacks)
            {
                Apply(tower);

                PlacementSideEffects(tower.Position, tower.Transform.Cast<IRootBehavior>());
            }
        }
    }
}