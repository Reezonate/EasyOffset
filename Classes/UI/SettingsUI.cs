using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using EasyOffset.Configuration;
using JetBrains.Annotations;

namespace EasyOffset.UI {
    public class SettingsUI : NotifiableSingleton<SettingsUI> {
        #region Global Settings

        [UIValue("enabled-value")]
        [UsedImplicitly]
        private bool EnabledValue {
            get => PluginConfig.Enabled;
            set => PluginConfig.Enabled = value;
        }

        #endregion

        #region Migration

        #region ZOffset slider

        [UIValue("zo-min")] [UsedImplicitly] private float _zOffsetSliderMin = -15f;

        [UIValue("zo-max")] [UsedImplicitly] private float _zOffsetSliderMax = 25f;

        [UIValue("zo-increment")] [UsedImplicitly]
        private float _zOffsetSliderIncrement = 1f;


        [UIValue("zo-value")]
        [UsedImplicitly]
        private float ZOffsetSliderValue {
            get => ConfigMigration.ZOffset * 100f;
            set => ConfigMigration.ZOffset = value / 100f;
        }

        #endregion

        #region Import from settings

        [UIAction("import-from-settings-on-click")]
        [UsedImplicitly]
        private void ImportFromSettingsOnClick() {
            var result = ConfigMigration.ImportFromSettings();
            UpdateResultText(result);
        }

        #endregion

        #region Import from tailor

        [UIAction("import-from-tailor-on-click")]
        [UsedImplicitly]
        private void ImportFromTailorOnClick() {
            var result = ConfigMigration.ImportFromSaberTailor();
            UpdateResultText(result);
        }

        #endregion

        #region Result text

        private const string SuccessfulImportText = "<color=green>Imported</color>";
        private const string DevicelessFailText = "<color=red>Import failed!</color>\nUnknown VR device";
        private const string ParseFailText = "<color=red>Import failed!</color>\nSaberTailor.json file read error";

        private void UpdateResultText(MigrationResult migrationResult) {
            SetMigrationResultText(migrationResult switch {
                MigrationResult.Success => SuccessfulImportText,
                MigrationResult.DevicelessFail => DevicelessFailText,
                MigrationResult.ParseFail => ParseFailText,
                _ => throw new ArgumentOutOfRangeException()
            });
        }

        private string _migrationResultText = "";

        [UIValue("migration-result-text")]
        [UsedImplicitly]
        private string MigrationResultText {
            get => _migrationResultText;
            set {
                _migrationResultText = value;
                NotifyPropertyChanged();
            }
        }

        private readonly DelayedAction _migrationResultTextResetAction = new();

        private void SetMigrationResultText(string value) {
            MigrationResultText = value;
            _migrationResultTextResetAction.InvokeLater(3000, ResetMigrationResultText);
        }

        private void ResetMigrationResultText() {
            MigrationResultText = "";
        }

        #endregion

        #endregion
    }
}