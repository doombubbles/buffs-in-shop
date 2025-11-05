using Il2CppAssets.Scripts.Models.GeraldoItems;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace BuffsInShop.Buff;

public class SharpeningStone : ModBuffInShopGeraldo
{
    public override string ItemLocsId => "Sharpening stone";

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions &&
            !GeraldoItem.GetDescendant<SharpeningStoneBehaviorModel>().IsTowerModelValid(tower.towerModel))
        {
            helperMessage = "Tower does not have Sharp damage type";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower) => new SharpeningStoneBehaviorModel.SharpeningStoneMutator(
        GeraldoItem.GetDescendant<SharpeningStoneBehaviorModel>(), 1,
        SharpeningStoneBehaviorModel.greaterMutatorId);
}