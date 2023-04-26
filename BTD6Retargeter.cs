using System;
using System.IO;
using System.Linq;
using MelonLoader;
using MelonLoader.Utils;

namespace BTD6EpicGamesModCompat;

internal static class Btd6Retargeter
{
    public static void Retarget()
    {
        Plugin.Logger.WriteSpacer();
        Plugin.Logger.Msg("Loading mods from " + MelonEnvironment.ModsDirectory + "...");
        Plugin.Logger.Msg(ConsoleColor.Magenta, "------------------------------");

        var modAssemblies = Directory.GetFiles(MelonEnvironment.ModsDirectory).Select(modFile =>
        {
            if (!Path.HasExtension(modFile) || !Path.GetExtension(modFile).Equals(".dll")) return null!;
            return MelonAssembly.LoadMelonAssembly(modFile, false);
        }).Where(melon => melon is not null).OrderBy(melon =>
            MelonUtils.PullAttributeFromAssembly<MelonPriorityAttribute>(melon.Assembly)?.Priority ?? 0).ToArray()!;

        Plugin.Logger.WriteSpacer();
        Plugin.Logger.Msg("Retargeting mods...");
        Plugin.Logger.Msg(ConsoleColor.Magenta, "------------------------------");
        foreach (var melonAssembly in modAssemblies)
        {
            melonAssembly.LoadMelons();
            foreach (var mod in melonAssembly.LoadedMelons)
            {
                if (mod is null)
                    continue;

                if (mod.Games.Length < 1)
                    continue;

                if (!TargetsBTD6(mod) || TargetsBTD6Epic(mod))
                    continue;


                var targetIndex = BTD6TargetIndex(mod);
                mod.Games[targetIndex] = new MelonGameAttribute("Ninja Kiwi", "BloonsTD6-Epic");
                Plugin.Logger.Msg(
                    $"Retargeted [{mod.Info.Name} v{mod.Info.Version} by {mod.Info.Author}] to BloonsTD6-Epic");
            }

            melonAssembly.UnregisterMelons(null, true);
            melonAssembly.LoadMelons();
        }
    }

    private static bool TargetsBTD6(MelonBase mod)
    {
        return mod.Games.Any(game => game.Universal || IsTargetTo(game, "Ninja Kiwi", "BloonsTD6"));
    }

    private static bool TargetsBTD6Epic(MelonBase mod)
    {
        return mod.Games.Any(game => game.Universal || IsTargetTo(game, "Ninja Kiwi", "BloonsTD6-Epic"));
    }

    private static int BTD6TargetIndex(MelonBase mod)
    {
        return Array.FindIndex(mod.Games, game => IsTargetTo(game, "Ninja Kiwi", "BloonsTD6"));
    }

    private static bool IsTargetTo(MelonGameAttribute game, string dev, string name)
    {
        return game.Developer.Equals(dev) && game.Name.Equals(name);
    }
}