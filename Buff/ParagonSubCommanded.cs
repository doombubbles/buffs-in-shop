using System.Collections.Generic;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppNinjaKiwi.Common.ResourceUtils;

namespace BuffsInShop.Buff;

public class ParagonSubCommanded : ModBuffInShopParagon
{
    public override string OriginTower => TowerType.MonkeySub;

    public override string BaseDescription =>
        "Gives a Hero x6 damage, x3 pierce, -23% attack speed, and 5x XP. " +
        "Gives a Monkey Sub x7 damage and x3 pierce. " +
        "Gives a Paragon 10% reduced ability cooldowns.";

    public override float BaseCost => UpgradeCost / 4;
    public override bool SubsequentDiscount => true;
    public override bool ParagonAllowed => true;

    public override AudioClipReference PlacementSound => new(VanillaAudioClips.ModeSubmerge);

    public override EffectModel PlacementEffect =>
        new("", new PrefabReference("5236c152f24c52f47bbb47e0538fc7f3"), 1, 1);

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (tower.towerModel.baseId != TowerType.MonkeySub && !tower.towerModel.IsHero() && !tower.towerModel.isParagon)
        {
            helperMessage = "Must be a Hero, Monkey Sub, or Paragon.";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        var submerge = OriginTowerModel.GetBehavior<SubmergeModel>();
        var filters = new Il2CppReferenceArray<TowerFilterModel>(0);

        if (tower?.towerModel.isParagon == true)
        {
            yield return new AbilityCooldownScaleSupport.MutatorTower(new AbilityCooldownScaleSupport
            {
                abilityCooldownScaleSupportModel = new AbilityCooldownScaleSupportModel(
                    "SubmergeAbilityCooldownScaleParagon", true, submerge.abilityCooldownSpeedScaleParagon, false,
                    false, filters, submerge.buffLocsName, submerge.buffIconName, false,
                    submerge.supportMutatorPriority)
            }, submerge.abilityCooldownSpeedScaleParagon, "SubmergeAbilityCooldownScaleParagon");
        }
        else
        {
            yield return new MonkeySubParagonSupport.MutatorTower(new MonkeySubParagonSupport
            {
                monkeySubParagonSupportModel = OriginTowerModel.GetDescendant<MonkeySubParagonSupportModel>(),
            });
        }
    }
}