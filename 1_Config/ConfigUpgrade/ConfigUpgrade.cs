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
            if (!File.Exists(ConfigFilePath)) return;

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
        var currentVersion = source.GetValueUnsafe<string>("ConfigVersion");
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
        var controllerTypeName = source.GetValueUnsafe<string>("DisplayControllerType");

        var leftSaberPivotPosition = ParseVectorV1_0(source, "LeftHandPivot");
        var leftSaberDirection = ParseVectorV1_0(source, "LeftHandSaberDirection");
        var leftSaberRotationEuler = TransformUtils.EulerFromDirection(leftSaberDirection);
        var leftSaberZOffset = source.GetValueUnsafe<float>("LeftHandZOffset");

        var rightSaberPivotPosition = ParseVectorV1_0(source, "RightHandPivot");
        var rightSaberDirection = ParseVectorV1_0(source, "RightHandSaberDirection");
        var rightSaberRotationEuler = TransformUtils.EulerFromDirection(rightSaberDirection);
        var rightSaberZOffset = source.GetValueUnsafe<float>("RightHandZOffset");

        //New Values
        const string newVersion = "2.0";
        source["ConfigVersion"] = newVersion;
        source["ControllerType"] = controllerTypeName;

        source["LeftSaberPivotPosition"] = SerializeVector3(leftSaberPivotPosition / V2PositionUnitScale);
        source["LeftSaberRotationEuler"] = SerializeVector3(leftSaberRotationEuler);
        source["LeftSaberZOffset"] = leftSaberZOffset / V2PositionUnitScale;
        source["LeftSaberHasReference"] = false;
        source["LeftSaberReference"] = SerializeQuaternion(Quaternion.identity);

        source["RightSaberPivotPosition"] = SerializeVector3(rightSaberPivotPosition / V2PositionUnitScale);
        source["RightSaberRotationEuler"] = SerializeVector3(rightSaberRotationEuler);
        source["RightSaberZOffset"] = rightSaberZOffset / V2PositionUnitScale;
        source["RightSaberHasReference"] = false;
        source["RightSaberReference"] = SerializeQuaternion(Quaternion.identity);

        //Cleanup
        source.Remove("DisplayControllerType");
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

    private static JToken SerializeQuaternion(Quaternion quaternion) {
        return new JObject {
            ["x"] = quaternion.x,
            ["y"] = quaternion.y,
            ["z"] = quaternion.z,
            ["w"] = quaternion.w,
        };
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
            jObject.GetValueUnsafe<float>($"{key}X"),
            jObject.GetValueUnsafe<float>($"{key}Y"),
            jObject.GetValueUnsafe<float>($"{key}Z")
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