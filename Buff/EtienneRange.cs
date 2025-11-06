using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;

namespace BuffsInShop.Buff;

public class EtienneRange : ModBuffInShop
{
    public override string OriginTower => TowerType.Etienne;
    public override int OriginTopPath => 19;
    public override bool Hero => true;

    public override string BaseDescription => "Give a tower 20% increased range.";

    public override float BaseCost => 900;
    public override bool SubsequentDiscount => true;

    public override AudioClipReference PlacementSound => new(VanillaAudioClips.ActivatedUcavNoVO);
    public override EffectModel PlacementEffect => new("", new("b99b2ce7a50d15547a91abe3cc058e4f"), 1, 1);

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetBehavior<RangeSupportModel>().CreateMutator();
}