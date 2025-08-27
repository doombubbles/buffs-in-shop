using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using UnityEngine;

namespace BuffsInShop.Buff;

public class SubCommanded : ModBuffInShop
{
    public override string OriginTower => TowerType.MonkeySub;
    public override int OriginBotPath => 5;

    public override float BaseCost => 5000;
    public override string BaseDescription => "Gives a Monkey Sub extra pierce and damage.";
    public override KeyCode KeyCode => KeyCode.D;
    public override bool SubsequentDiscount => true;

    public override AudioClipReference? PlacementSound =>
        OriginTowerModel.GetBehavior<CreateSoundOnTowerPlaceModel>().sound1.assetId;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && tower.towerModel.baseId != TowerType.MonkeySub)
        {
            helperMessage = "Must be a Monkey Sub";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetBehavior<SubCommanderSupportModel>().CreateMutator();
}