using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.GeraldoItems;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;

namespace BuffsInShop;

public abstract class ModBuffInShopGeraldo : ModBuffInShop
{
    public override string? OriginTower => TowerType.Geraldo;
    public override bool Hero => true;

    public virtual string GeraldoItemId => Name;

    public GeraldoItemModel GeraldoItem => GameModel.GetGeraldoItemWithName(GeraldoItemId);


    public override AudioClipReference PlacementSound =>
        GeraldoItem.GetDescendant<CreateSoundOnGeraldoItemModel>().soundId;

    public override EffectModel PlacementEffect
    {
        get
        {
            var effect = GeraldoItem.GetDescendant<CreateEffectOnGeraldoItemModel>();
            return new EffectModel("", effect.effectId, 1, effect.lifespan);
        }
    }

    public abstract string ItemLocsId { get; }

    public override string DisplayName => $"[{ItemLocsId} name]";
    public override string BaseDescription => $"[{ItemLocsId} description]";

    public override float BaseCost => GeraldoItem.cost * 5;
}