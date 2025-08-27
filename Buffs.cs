using BTD_Mod_Helper.Api.Towers;
namespace BuffsInShop;

public class Buffs : ModTowerSet
{
    public override bool AllowInRestrictedModes => true;

    protected override int Order => 100;
}