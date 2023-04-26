using System;
using System.IO;
using BTD6EpicGamesModCompat;
using MelonLoader;
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming


[assembly: MelonInfo(typeof(Plugin), "BTD6 Epic Games Mod Compat", "1.0.9", "GrahamKracker")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6-Epic")]
namespace BTD6EpicGamesModCompat;

public sealed class Plugin : MelonPlugin {
    private const string EOSBypasserModPath = "Mods/BTD6EOSBypasser.dll";
    private const string EOSBypasserResourcePath = "BTD6EOSBypasser.dll";
    public static MelonLogger.Instance Logger { get; private set; } = null!;

    // Runs before crash caused by EOSSDK thankfully
    public override void OnPreInitialization() {
        Logger = LoggerInstance;

        // Avoid crash
        EOSSDK.Remove();

        // Not fully tested to work for all close cases
        AppDomain.CurrentDomain.ProcessExit += (_, _) => EOSSDK.Restore();
        AppDomain.CurrentDomain.UnhandledException += (_, _) => EOSSDK.Restore();
    }

    public override void OnApplicationQuit() {
        EOSSDK.Restore();
    }

    public override void OnPreModsLoaded() {
        // Regenerate BTD6EOSBypasser mod
        File.WriteAllBytes(EOSBypasserModPath, Resources.GetResource(EOSBypasserResourcePath)!);

        // Retarget Mods
        Btd6Retargeter.Retarget();
    }
}
