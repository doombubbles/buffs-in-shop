using System.Collections.Generic;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.GeraldoItems;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace BuffsInShop.Buff;

public class SeeInvisibilityPotion : ModBuffInShopGeraldo
{
    public override string ItemLocsId => "See invisibility potion";

    public override float BaseCost => GeraldoItem.cost * 10;

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        yield return new SeeInvisibilityPotionBehaviorModel.SeeInvisibilityPotionMutator(
            GeraldoItem.GetDescendant<SeeInvisibilityPotionBehaviorModel>(),
            SeeInvisibilityPotionBehaviorModel.mutatorIdV3, 2).ApplyBuffIcon<SeeInvisibilityPotionIcon>();
        yield return new SeeInvisibilityPotionBehaviorModel.SeeInvisibilityPotionEffectMutator(
            GeraldoItem.GetDescendant<SeeInvisibilityPotionBehaviorModel>(),
            SeeInvisibilityPotionBehaviorModel.effectMutatorId).ApplyBuffIcon<SeeInvisibilityPotionIcon>();
    }

    public class SeeInvisibilityPotionIcon : ModBuffIcon;
}