using System;
using System.IO;
using System.Linq;
using MelonLoader;
using MelonLoader.Utils;

namespace BTD6EpicGamesModCompat;

internal static class Btd6Retargeter
{
    private static MelonAssembly GetAssembly(string fileName)
    {
        try
        {
            return MelonAssembly.LoadMelonAssembly(fileName, false);
        }
        catch (Exception e)
        {
            Plugin.Logger.Error(e);
            return null;
        }
    }

    private static int GetPriority(MelonAssembly assembly)
    {
        try
        {
            return MelonUtils.PullAttributeFromAssembly<MelonPriorityAttribute>(assembly.Assembly)?.Priority ?? 0;
        }
        catch (Exception e)
        {
            Plugin.Logger.Error(e);
            return 0;
        }
    }

    public static void Retarget()
    {
        Plugin.Logger.WriteSpacer();
        Plugin.Logger.Msg("Loading mods from " + MelonEnvironment.ModsDirectory + "...");
        Plugin.Logger.Msg(ConsoleColor.Magenta, "------------------------------");

        Plugin.Logger.WriteSpacer();
        Plugin.Logger.Msg("Retargeting mods...");
        Plugin.Logger.Msg(ConsoleColor.Magenta, "------------------------------");

        var assemblies = from modFile in Directory.GetFiles(MelonEnvironment.ModsDirectory)
            select !Path.HasExtension(modFile) || !Path.GetExtension(modFile).Equals(".dll")
                ? null
                : GetAssembly(modFile)
            into melon
            where melon != null
            orderby GetPriority(melon)
            select melon;

        foreach (var assembly in assemblies)
        {
            try
            {
                assembly.LoadMelons();

                foreach (var melon in assembly.LoadedMelons.Where(melon => melon is not null))
                {
                    var alreadyHasEpicGamesInAttribute = false;
                    var steamIndex = -1;

                    for (int i = 0; i < melon.Games.Length; i++)
                    {
                        var game = melon.Games[i];
                        if (game.Developer.Equals("Ninja Kiwi") && game.Name.Contains("BloonsTD6"))
                        {
                            alreadyHasEpicGamesInAttribute |= game.Name.Contains("-Epic");
                            if (!alreadyHasEpicGamesInAttribute)
                            {
                                steamIndex = i;
                                break;
                            }
                        }
                    }

                    if (!alreadyHasEpicGamesInAttribute && steamIndex != -1)
                    {
                        melon.Games[steamIndex] = new MelonGameAttribute("Ninja Kiwi", "BloonsTD6-Epic");
                        Plugin.Logger.Msg(
                            $"Retargeted [{melon.Info.Name} v{melon.Info.Version} by {melon.Info.Author}] to BloonsTD6-Epic");
                    }
                }

                assembly.UnregisterMelons(silent: true);
                assembly.LoadMelons();
            }
            catch (Exception e)
            {
                Plugin.Logger.Error(e);
            }
        }
    }
}