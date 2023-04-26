using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using BTD6EpicGamesModCompat;
using MelonLoader;
using MelonLoader.Utils;


namespace BTD6EpicGamesModCompat;

internal static class Btd6Retargeter
{
    // Takes all mods that target BloonsTD6 and retarget them to BloonsTD6-Epic if they don't already
    public static void Retarget()
    {
        Plugin.Logger.WriteSpacer();
        Plugin.Logger.Msg("Loading mods from " + MelonEnvironment.ModsDirectory + "...");
        Plugin.Logger.Msg(ConsoleColor.Magenta, "------------------------------");
        var modAssemblies = (from melon in Directory.GetFiles(MelonEnvironment.ModsDirectory).Select(
                delegate(string modFile)
                {
                    if (!Path.HasExtension(modFile) || !Path.GetExtension(modFile).Equals(".dll"))
                    {
                        return null;
                    }

                    return MelonAssembly.LoadMelonAssembly(modFile, false);
                })
            where melon != null
            select melon).OrderBy(delegate(MelonAssembly melon)
        {
            var melonPriorityAttribute =
                MelonUtils.PullAttributeFromAssembly<MelonPriorityAttribute>(melon.Assembly);
            return melonPriorityAttribute?.Priority ?? 0;
        }).ToArray();

        Plugin.Logger.WriteSpacer();
        Plugin.Logger.Msg("Retargeting mods...");
        Plugin.Logger.Msg(ConsoleColor.Magenta, "------------------------------");
        foreach (var melonAssembly in modAssemblies)
        {
            melonAssembly.LoadMelons();
            foreach (var mod in melonAssembly.LoadedMelons)
            {
                // Probably will never happen, but nice to check for
                if (mod is null)
                    continue;

                // If the mod doesn't target a game, skip
                if (mod.Games.Length < 1)
                    continue;

                // If the mod targets the epic version already or doesn't target btd6 to begin with, skip
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

    // Tests if the mod targets BloonsTD6
    private static bool TargetsBTD6(MelonBase mod)
    {
        return mod.Games.Any(game => game.Universal || IsTargetTo(game, "Ninja Kiwi", "BloonsTD6"));
    }

    // Tests if the mod targets BloonsTD6-Epic
    private static bool TargetsBTD6Epic(MelonBase mod)
    {
        return mod.Games.Any(game => game.Universal || IsTargetTo(game, "Ninja Kiwi", "BloonsTD6-Epic"));
    }

    // Finds the index that the mod targets BloonsTD6
    private static int BTD6TargetIndex(MelonBase mod)
    {
        return Array.FindIndex(mod.Games, game => IsTargetTo(game, "Ninja Kiwi", "BloonsTD6"));
    }

    // Determines if the target is the the given dev and game name
    private static bool IsTargetTo(MelonGameAttribute game, string dev, string name)
    {
        return game.Developer.Equals(dev) && game.Name.Equals(name);
    }
}