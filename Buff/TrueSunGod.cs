using System.Collections.Generic;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace BuffsInShop.Buff;

public class TrueSunGod : ModBuffInShop<SunTemple>
{
    public override string OriginTower => TowerType.SuperMonkey;
    public override int OriginTopPath => 5;

    public override string DisplayName => "True Sun God's Blessing";

    public override float BaseCost => 50000;
    public override string BaseDescription =>
        "Gives a tower with the the Sun Temple's Blessing the support of the True Sun God.";
    public override KeyCode KeyCode => KeyCode.G;
    public override bool SubsequentDiscount => true;

    public override AudioClipReference? PlacementSound => GameModel.GetTower(TowerType.SuperMonkey, 3, 0, 0)
        .GetBehavior<CreateSoundOnAttachedModel>().sound.assetId;

    public override EffectModel? PlacementEffect => GameModel.GetTower(TowerType.SunAvatarMini)
        .GetBehavior<CreateEffectOnPlaceModel>().effectModel;

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower) => SunTemple.GetMutators(OriginTowerModel);

    public override bool ExtraMutation(TowerModel towerModel) => SunTemple.ExtraMutation(towerModel, OriginTowerModel);
}