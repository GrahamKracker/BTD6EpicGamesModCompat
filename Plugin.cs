using System;
using BTD6EpicGamesModCompat;
using MelonLoader;

[assembly: HarmonyDontPatchAll]
[assembly: MelonInfo(typeof(Plugin), "BTD6 Epic Games Mod Compat", "1.1.0", "GrahamKracker")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6-Epic")]

namespace BTD6EpicGamesModCompat;

public sealed class Plugin : MelonPlugin
{
    public static MelonLogger.Instance Logger { get; private set; } = null!;

    public override void OnPreInitialization()
    {
        Logger = LoggerInstance;

        EOSSDK.Remove();

        AppDomain.CurrentDomain.ProcessExit += (_, _) => EOSSDK.Restore();
        AppDomain.CurrentDomain.UnhandledException += (_, _) => EOSSDK.Restore();
    }

    public override void OnInitializeMelon()
    {
        HarmonyInstance.PatchAll();
    }

    public override void OnApplicationQuit()
    {
        EOSSDK.Restore();
    }

    public override void OnPreModsLoaded()
    {
        Btd6Retargeter.Retarget();
    }
}