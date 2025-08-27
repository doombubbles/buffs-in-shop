using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Powers.Effects;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace BuffsInShop.Buff;

public class RadarScanner : ModBuffInShop
{
    public override string OriginTower => TowerType.MonkeyVillage;
    public override int OriginMidPath => 2;

    public override string BaseDescription => "Makes a tower able to attack Camo Bloons.";
    public override KeyCode KeyCode => KeyCode.R;
    public override bool SubsequentDiscount => true;

    public override AudioClipReference? PlacementSound =>
        GameModel.GetPowerWithId("CamoTrap").GetDescendant<CreateSoundOnPowerModel>().sound.assetId;

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetBehavior<VisibilitySupportModel>().CreateMutator();
}