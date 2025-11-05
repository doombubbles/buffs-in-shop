using Il2CppAssets.Scripts.Models.GeraldoItems;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace BuffsInShop.Buff;

public class Fertilizer : ModBuffInShopGeraldo
{
    public override string ItemLocsId => "Fertilizer";

    public override bool AllowInChimps => false;

    public override float BaseCost => 5000;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions &&
            GeraldoItem.GetDescendant<FertilizerBehaviorModel>().IsTowerModelValid(tower.towerModel))
        {
            helperMessage = "Invalid tower";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower) =>
        new FertilizerBehaviorModel.FertilizerMutator(GeraldoItem.GetDescendant<FertilizerBehaviorModel>());
}