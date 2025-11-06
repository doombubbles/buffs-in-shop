using System.Collections.Generic;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;

namespace BuffsInShop.Buff;

public class IceAge : ModBuffInShop
{
    public override string OriginTower => TowerType.Silas;
    public override int OriginTopPath => 19;
    public override bool Hero => true;

    public override string BaseDescription =>
        "Gives a tower that can Freeze Bloons +150% Freeze duration. Ice Monkeys also get +5 range.";

    public override float BaseCost => 4000;
    public override bool SubsequentDiscount => true;

    public override bool CanApplyTo(Tower tower, ref string helperMessage)
    {
        if (!tower.towerModel.HasDescendant<FreezeModel>())
        {
            helperMessage = "Must Freeze Bloons";
            return false;
        }

        return base.CanApplyTo(tower, ref helperMessage);
    }

    public override IEnumerable<BehaviorMutator> GetMutators(Tower? tower)
    {
        yield return OriginTowerModel.GetBehavior<FreezeDurationSupportModel>().CreateMutator();

        if (tower?.towerModel.baseId == TowerType.IceMonkey)
        {
            yield return OriginTowerModel.GetBehavior<RangeSupportModel>().CreateMutator();
        }
    }
}