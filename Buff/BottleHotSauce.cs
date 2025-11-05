using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.GeraldoItems;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace BuffsInShop.Buff;

public class BottleHotSauce : ModBuffInShopGeraldo
{
    public override string ItemLocsId => "Gerrys fire hot sauce";

    public override EffectModel PlacementEffect
    {
        get
        {
            var effect = GeraldoItem.GetDescendant<BottleHotSauceBehaviorModel>().effectAtTowerId;
            return new EffectModel("", effect, 1, 1);
        }
    }

    public override bool AffectsSubTowers => false;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!GeraldoItem.GetDescendant<BottleHotSauceBehaviorModel>().IsTowerModelValid(tower.towerModel))
        {
            helperMessage = "Invalid tower";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower) =>
        new BottleHotSauceBehaviorModel.BottleHotSauceMutator(GeraldoItem.GetDescendant<BottleHotSauceBehaviorModel>());

    public override void Apply(Tower tower, float purchaseCost = -1, bool sideEffects = false)
    {
        base.Apply(tower, purchaseCost, sideEffects);

        if (sideEffects)
        {
            var creature = Sim.towerManager.CreateTower(GameModel.GetTowerWithName("HotSauceCreatureTowerV2"),
                tower.Position, tower.owner, tower.GetAreaTowerIsPlacedOn().Id, tower.Id);
            creature.SetTargetType(tower.TargetType);
        }
    }
}