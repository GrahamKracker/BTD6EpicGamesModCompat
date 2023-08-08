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

        Plugin.Logger.WriteSpacer();
        Plugin.Logger.Msg("Retargeting mods...");
        Plugin.Logger.Msg(ConsoleColor.Magenta, "------------------------------");

        IOrderedEnumerable<MelonAssembly> assemblies =
            from modFile in Directory.GetFiles(MelonEnvironment.ModsDirectory)
            select !Path.HasExtension(modFile) || !Path.GetExtension(modFile).Equals(".dll")
                ? null
                : MelonAssembly.LoadMelonAssembly(modFile, false)
            into melon
            where melon != null
            orderby MelonUtils.PullAttributeFromAssembly<MelonPriorityAttribute>(melon.Assembly)?.Priority ?? 0
            select melon;

        foreach (var assembly in assemblies)
        {
            try
            {
                assembly.LoadMelons();
                
                foreach (var melon in assembly.LoadedMelons.Where(melon => melon is not null))
                {
                    var alreadyHasEpicGamesAttribute = false;
                    var steamIndex = -1;

                    for (int i = 0; i < melon.Games.Length; i++)
                    {
                        var game = melon.Games[i];
                        if (game.Developer.Equals("Ninja Kiwi") && game.Name.Contains("BloonsTD6"))
                        {
                            alreadyHasEpicGamesAttribute |= game.Name.Contains("-Epic");
                            if (!alreadyHasEpicGamesAttribute)
                            {
                                steamIndex = i;
                                break;
                            }
                        }
                    }

                    if (!alreadyHasEpicGamesAttribute && steamIndex != -1)
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