using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using ArtilleryCommandDamageMutator =
    Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors.ArtilleryCommand.ArtilleryCommandDamageMutator;

namespace BuffsInShop.Buff;

public class ArtilleryCommand : ModBuffInShop
{
    public override string OriginTower => TowerType.StrikerJones;
    public override int OriginTopPath => 19;
    public override bool Hero => true;

    public override bool IsValidOrigin(TowerModel current) => current.HasDescendant<ArtilleryCommandModel>();

    public override string BaseDescription => "Gives a Bomb Shooter or Mortary Monkey double damage and pierce.";

    public override float BaseCost => 40000;
    public override bool SubsequentDiscount => true;

    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetBehavior<AbilityModel>().GetBehavior<CreateSoundOnAbilityModel>().sound.assetId;

    public override EffectModel? PlacementEffect =>
        OriginTowerModel.GetDescendant<ArtilleryCommandModel>().otherTowerEffectModel;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && tower.towerModel.baseId != TowerType.BombShooter &&
            tower.towerModel.baseId != TowerType.MortarMonkey)
        {
            helperMessage = "Must be a Bomb Shooter or Mortar Monkey.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower) =>
        new ArtilleryCommandDamageMutator().ApplyBuffIcon<ArtilleryCommandIcon>();

    public class ArtilleryCommandIcon : ModBuffIcon;
}