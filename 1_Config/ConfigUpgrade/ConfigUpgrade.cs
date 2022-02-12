using System;
using System.IO;
using IPA.Utilities;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace EasyOffset;

internal static class ConfigUpgrade {
    #region Constants

    private static readonly string ConfigFilePath = Path.Combine(UnityGame.UserDataPath, "EasyOffset.json");
    private static readonly string BackupsFolderPath = Path.Combine(UnityGame.UserDataPath, "EasyOffset\\Backups\\");

    #endregion

    #region TryUpgrade

    public static void TryUpgrade() {
        try {
            var jsonObject = JObject.Parse(File.ReadAllText(ConfigFilePath));
            if (!jsonObject.DoUpgradeCycle()) return;

            TrySaveBackup();
            File.WriteAllText(ConfigFilePath, jsonObject.ToString());
        } catch (Exception e) {
            Plugin.Log.Error($"Config upgrade failed: {e.Message}! Saving backup");
            TrySaveBackup();
        }
    }

    private static bool DoUpgradeCycle(this JObject source) {
        var currentVersion = source.GetValue("ConfigVersion", StringComparison.OrdinalIgnoreCase)!.Value<string>();
        var changed = false;

        while (currentVersion != ConfigFileData.CurrentConfigVersion) {
            var newVersion = currentVersion switch {
                "1.0" => source.Upgrade1_0To2_0(),
                _ => throw new Exception($"Unsupported config version ({currentVersion})")
            };

            Plugin.Log.Notice($"Config upgraded! (v{currentVersion} -> v{newVersion})");
            currentVersion = newVersion;
            changed = true;
        }

        return changed;
    }

    #endregion

    #region 1.0 -> 2.0

    private const float V2PositionUnitScale = 0.01f;

    private static string Upgrade1_0To2_0(this JObject source) {
        //Conversion
        var leftSaberPivotPosition = ParseVectorV1_0(source, "LeftHandPivot");
        var leftSaberDirection = ParseVectorV1_0(source, "LeftHandSaberDirection");
        var leftSaberRotationEuler = TransformUtils.EulerFromDirection(leftSaberDirection);
        var leftSaberZOffset = source.GetValue("LeftHandZOffset", StringComparison.OrdinalIgnoreCase)!.Value<float>();

        var rightSaberPivotPosition = ParseVectorV1_0(source, "RightHandPivot");
        var rightSaberDirection = ParseVectorV1_0(source, "RightHandSaberDirection");
        var rightSaberRotationEuler = TransformUtils.EulerFromDirection(rightSaberDirection);
        var rightSaberZOffset = source.GetValue("RightHandZOffset", StringComparison.OrdinalIgnoreCase)!.Value<float>();

        //New Values
        const string newVersion = "2.0";
        source["ConfigVersion"] = newVersion;
        source["LeftSaberPivotPosition"] = SerializeVector3(leftSaberPivotPosition / V2PositionUnitScale);
        source["LeftSaberRotationEuler"] = SerializeVector3(leftSaberRotationEuler);
        source["LeftSaberZOffset"] = leftSaberZOffset / V2PositionUnitScale;
        source["RightSaberPivotPosition"] = SerializeVector3(rightSaberPivotPosition / V2PositionUnitScale);
        source["RightSaberRotationEuler"] = SerializeVector3(rightSaberRotationEuler);
        source["RightSaberZOffset"] = rightSaberZOffset / V2PositionUnitScale;

        //Cleanup
        source.Remove("LeftHandPivotPositionX");
        source.Remove("LeftHandPivotPositionY");
        source.Remove("LeftHandPivotPositionZ");
        source.Remove("LeftHandSaberDirectionX");
        source.Remove("LeftHandSaberDirectionY");
        source.Remove("LeftHandSaberDirectionZ");
        source.Remove("LeftHandZOffset");
        source.Remove("RightHandPivotPositionX");
        source.Remove("RightHandPivotPositionY");
        source.Remove("RightHandPivotPositionZ");
        source.Remove("RightHandSaberDirectionX");
        source.Remove("RightHandSaberDirectionY");
        source.Remove("RightHandSaberDirectionZ");
        source.Remove("RightHandZOffset");

        return newVersion;
    }

    private static JToken SerializeVector3(Vector3 vector) {
        return new JObject {
            ["x"] = vector.x,
            ["y"] = vector.y,
            ["z"] = vector.z,
        };
    }

    private static Vector3 ParseVectorV1_0(JObject jObject, string key) {
        return new Vector3(
            jObject.GetValue($"{key}X", StringComparison.OrdinalIgnoreCase)!.Value<float>(),
            jObject.GetValue($"{key}Y", StringComparison.OrdinalIgnoreCase)!.Value<float>(),
            jObject.GetValue($"{key}Z", StringComparison.OrdinalIgnoreCase)!.Value<float>()
        );
    }

    #endregion

    #region Backup

    private static void TrySaveBackup() {
        try {
            CreateDirectoryIfNecessary(BackupsFolderPath);
            var backupFilePath = Path.Combine(BackupsFolderPath, $"EasyOffset{GetBackupPostfix()}.json");
            File.Copy(ConfigFilePath, backupFilePath, true);
        } catch (Exception e) {
            Plugin.Log.Error($"Config backup failed: {e.Message}");
        }
    }

    private static string GetBackupPostfix() {
        var now = DateTime.Now;
        return $"({now.Hour}-{now.Minute}-{now.Second}_{now.Day}-{now.Month}-{now.Year})";
    }

    #endregion

    #region Utils

    private static void CreateDirectoryIfNecessary(string path) {
        var isDirectoryPresent = Directory.Exists(path);
        if (!isDirectoryPresent) Directory.CreateDirectory(path);
    }

    #endregion
}