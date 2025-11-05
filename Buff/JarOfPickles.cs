using Il2CppAssets.Scripts.Models.GeraldoItems;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace BuffsInShop.Buff;

public class JarOfPickles : ModBuffInShopGeraldo
{
    public override string ItemLocsId => "Jar of Pickles";

    public override BehaviorMutator GetMutator(Tower? tower) =>
        new JarOfPicklesBehaviorModel.JarOfPicklesMutator(GeraldoItem.GetDescendant<JarOfPicklesBehaviorModel>(),
            JarOfPicklesBehaviorModel.mutatorIdV3, 2);
}