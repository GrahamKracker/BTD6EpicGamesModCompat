using System;

using BTD6EpicGamesModCompat;

using MelonLoader;

[assembly: HarmonyDontPatchAll]
[assembly: MelonInfo(typeof(Plugin), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6-Epic")]

namespace BTD6EpicGamesModCompat;

public sealed class Plugin : MelonPlugin {
    public static MelonLogger.Instance Logger { get; private set; } = null!;

    public override void OnPreInitialization() {
        Logger = LoggerInstance;
    }

    public override void OnPreModsLoaded() {
        Btd6Retargeter.Retarget();
    }
}