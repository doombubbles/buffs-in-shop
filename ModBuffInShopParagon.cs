using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Upgrades;
using Il2CppAssets.Scripts.Unity;

namespace BuffsInShop;

public abstract class ModBuffInShopParagon : ModBuffInShop
{
    public override int OriginTopPath => 5;
    public override int OriginMidPath => 5;
    public override int OriginBotPath => 5;

    public override UpgradeModel? OriginUpgradeModel =>
        Game.instance.model.GetParagonUpgrade(OriginTower);
    public override TowerModel OriginTowerModel =>
        Game.instance.model.GetParagonTower(OriginTower);

}