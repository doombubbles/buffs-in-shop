using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;

namespace BuffsInShop;

public class BuffInShopMutator(ModBuffInShop modBuffInShop) : ModMutator<BuffInShopMutator.Data>
{
    public record struct Data(float Cost, int Stacks);

    public override string Name => modBuffInShop.Name;

    public override bool CantBeAbsorbed => true;

    /// <summary>
    /// Using OnTowerSaved and OnTowerLoaded instead for backwards compatibility
    /// </summary>
    public override bool Saved => false;

    public override bool Mutate(Model baseModel, Model model, Data data)
    {
        return modBuffInShop.ExtraMutation(model.Cast<TowerModel>());
    }
}