using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppNinjaKiwi.Common.ResourceUtils;

namespace BuffsInShop.Buff;

public class BankHack : ModBuffInShop
{
    public override string OriginTower => TowerType.Benjamin;
    public override int OriginTopPath => 8;
    public override bool Hero => true;

    public override string BaseDescription => "Gives a Monkey Bank 12% increased income generation.";

    public override float BaseCost => 2500;
    public override bool SubsequentDiscount => true;
    public override bool AllowInChimps => false;

    public override AudioClipReference PlacementSound => new(VanillaAudioClips.ActivatedBioHackNoVO);
    public override EffectModel PlacementEffect => new("", new("8a3a3ac08ba475e49a36389cea1356e4"), 1, 1);

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!BuffsInShopMod.BypassTowerRestrictions && !tower.towerModel.HasBehavior<BankModel>())
        {
            helperMessage = "Must be a Monkey Bank.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override BehaviorMutator GetMutator(Tower? tower) =>
        OriginTowerModel.GetBehavior<BananaCashIncreaseSupportModel>().CreateMutator();

}