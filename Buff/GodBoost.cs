using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace BuffsInShop.Buff;

public class GodBoost : ModBuffInShop
{
    protected override int Order => 100;
    public override string BaseDescription => "Applies many other Buffs from the Shop at once.";
    public override string? OriginTower => null;
    public override float BaseCost => 500000;
    public override int PriorityBoost => base.PriorityBoost + 10;
    public override bool SubsequentDiscount => true;
    public override bool AllowInChimps => false;

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower) => [];

    public override void Apply(Tower tower, float purchaseCost = -1, bool sideEffects = false)
    {
        base.Apply(tower, purchaseCost, sideEffects);

        if (!sideEffects) return;

        foreach (var buff in GetContent<ModBuffInShop>().OrderBy(buff => buff.PriorityBoost))
        {
            var _ = "";
            if (buff.IncludeInGodBoost && buff.CanApplyTo(tower, ref _))
            {
                buff.Apply(tower, 0, true);
                if (buff.MaxStacks > 1)
                {
                    for (var i = 1; i < buff.MaxStacks; i++)
                    {
                        buff.Apply(tower, 0, sideEffects);
                    }
                }
            }
        }
    }
}