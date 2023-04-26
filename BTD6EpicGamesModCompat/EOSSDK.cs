﻿using System.IO;
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace BTD6EpicGamesModCompat;

internal static class EOSSDK
{
    // Paths for EOSSDK
    private const string EOSSDKName = "EOSSDK-Win64-Shipping.dll";
    private const string BTD6PluginsFolder = @"BloonsTD6_Data\Plugins\x86_64\";
    private const string BackupFolder = BTD6PluginsFolder + @"\backup\";
    private const string EOSSDKPath = BTD6PluginsFolder + EOSSDKName;
    private const string EOSSDKBackupPath = BackupFolder + EOSSDKName;

    // Remove EOSSDK to not crash melonloader immediately
    public static void Remove()
    {
        Directory.CreateDirectory(BackupFolder);
        if (File.Exists(EOSSDKPath))
            File.Move(EOSSDKPath, EOSSDKBackupPath);

        Plugin.Logger.Msg("Removed EOSSDK");
    }

    // Restore EOSSDK for if the player wants to go unmodded
    public static void Restore()
    {
        if (File.Exists(EOSSDKBackupPath))
            File.Move(EOSSDKBackupPath, EOSSDKPath);

        Plugin.Logger.Msg("Restored EOSSDK");
    }
}