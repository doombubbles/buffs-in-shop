#if DEBUG
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Commands;
using BTD_Mod_Helper.Api.Helpers;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhotoSauce.MagicScaler;
using UnityEngine;
using Math = Il2CppAssets.Scripts.Simulation.SMath.Math;

namespace BuffsInShop;

internal static class Commands
{
    public static string Folder => Path.Combine(ModHelper.ModSourcesDirectory, nameof(BuffsInShop));

    public class BuffsInShopCommand : ModCommand<GenerateCommand>
    {
        public override string Command => "buffsinshop";

        public override string Help => "Generate commands for Buffs In Shop";

        public override bool Execute(ref string resultText) => ExplainSubcommands(out resultText);
    }

    public class BuffIcons : ModCommand<BuffsInShopCommand>
    {
        public override string Command => "icons";

        public override string Help => "Generates properly sized buff icons";

        public override bool Execute(ref string resultText)
        {
            resultText = "Coroutine started";
            MelonCoroutines.Start(Execute());
            return true;
        }

        public static IEnumerator Execute()
        {
            foreach (var buff in GetContent<ModBuffInShop>().Where(shop => shop.mod is BuffsInShopMod))
            {
                var buffIndicator = buff
                    .GetMutators(null)
                    .Select(mutator => mutator.buffIndicator)
                    .Where(buffIndicator => buffIndicator != null && !string.IsNullOrEmpty(buffIndicator.FullName))
                    .OrderByDescending(buffIndicator => buffIndicator.iconName)
                    .First()
                    .GetIcon();

                var getSprite = ResourceLoader.LoadAsync<Sprite>(buffIndicator.icon.AssetGUID);
                yield return getSprite;

                var sprite = getSprite.Result.PadSpriteToScale(.75f).PadSpriteToSquare();

                var path = Path.Combine(Folder, "Resources", buff.Name);

                sprite.TrySaveToPNG(path + "Small.png");

                Task.Run(() =>
                {
                    MagicImageProcessor.ProcessImage(path + "Small.png", path + ".png", new ProcessImageSettings
                    {
                        Anchor = CropAnchor.Center,
                        Width = 256,
                        Height = 256,
                        Interpolation = InterpolationSettings.CatmullRom,
                        Sharpen = true,
                        HybridMode = HybridScaleMode.Off
                    });

                    File.Delete(path + "Small.png");
                });

            }
        }

    }

    public class Readme : ModCommand<BuffsInShopCommand>
    {
        public override string Command => "readme";

        public override string Help => "Generates entries in the README";

        public override bool Execute(ref string resultText)
        {
            var folder = Path.Combine(ModHelper.ModSourcesDirectory, nameof(BuffsInShop));

            var buffs = GetContent<ModBuffInShop>().Where(shop => shop.mod is BuffsInShopMod);

            var readme = Path.Combine(folder, "README.md");

            var lines = File.ReadAllLines(readme).ToList();
            var start = lines.IndexOf("<!--Start-->") + 1;
            var end = lines.LastIndexOf("<!--End-->");

            lines.RemoveRange(start, end - start);

            var text =
                $"""
                 | Icon | Name | Description | Cost | Discount |
                 |:----:|:----:|:-----------:|:----:|:--------:|
                 {buffs.Select(buff =>
                     $"""| <img src="Resources/{buff.Name}.png" width=50> | {buff.DisplayName} | {buff.BaseDescription} | ${buff.BaseCost} | {(buff.SubsequentDiscount ? "Yes" : "No")} |"""
                 ).Join(delimiter: "\n")}
                 """;

            lines.InsertRange(start, text.ReplaceLineEndings().Split(Environment.NewLine));

            File.WriteAllLines(readme, lines);

            return true;
        }
    }

    public class Costs : ModCommand<BuffsInShopCommand>
    {
        public override string Command => "costs";

        public override string Help => "Generates base buff costs from upgrades";

        public override bool Execute(ref string resultText)
        {
            var file = Path.Combine(Folder, "Resources", "costs.json");

            var jobject = new JObject();

            foreach (var buff in GetContent<ModBuffInShop>().Where(shop => shop.mod is BuffsInShopMod))
            {
                jobject[buff.Name] = Math.RoundToNearestInt(buff.BaseCost, 5);
            }

            File.WriteAllText(file, jobject.ToString(Formatting.Indented));

            return true;
        }

    }

    public class All : ModCommand<BuffsInShopCommand>
    {
        public override string Command => "all";

        public override string Help => "Runs all commands";

        public override bool Execute(ref string resultText)
        {
            return GetContent<ModCommand<BuffsInShopCommand>>()
                .Where(command => command != this)
                .All(command =>
                {
                    var _ = "";
                    return command.Execute(ref _);
                });
        }

    }
}
#endif