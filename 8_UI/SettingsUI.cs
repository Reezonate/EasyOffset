using System;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using JetBrains.Annotations;

namespace EasyOffset {
    public class SettingsUI : NotifiableSingleton<SettingsUI> {
        #region Global Settings

        [UIValue("enabled-value")]
        [UsedImplicitly]
        private bool EnabledValue {
            get => PluginConfig.Enabled;
            set => PluginConfig.Enabled = value;
        }

        [UIValue("hide-controllers-value")]
        [UsedImplicitly]
        private bool HideControllersValue {
            get => PluginConfig.HideControllers;
            set => PluginConfig.HideControllers = value;
        }

        [UIValue("hide-coordinates-value")]
        [UsedImplicitly]
        private bool HideCoordinatesValue {
            get => PluginConfig.HideCoordinates;
            set => PluginConfig.HideCoordinates = value;
        }

        #endregion

        #region Config Migration

        #region ZOffset slider

        [UIValue("zo-hint")] [UsedImplicitly]
        private string _zOffsetSliderHint = "Pivot point offset along saber axis (cm)\n0 - top of the hilt, 17 - bottom of the hilt";

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

        #region Import

        #region Import from settings

        [UIAction("import-from-settings-on-click")]
        [UsedImplicitly]
        private void ImportFromSettingsOnClick() {
            var result = ConfigMigration.ImportFromSettings();
            SetImportStatus(result);
        }

        #endregion

        #region Import from tailor

        [UIAction("import-from-tailor-on-click")]
        [UsedImplicitly]
        private void ImportFromTailorOnClick() {
            var result = ConfigMigration.ImportFromSaberTailor();
            SetImportStatus(result);
        }

        #endregion

        #region Import status

        private const string SuccessfulImportText = "<color=green>Imported</color>";
        private const string DevicelessImportFailText = "<color=red>Import failed!</color> Unknown VR device";
        private const string ParseImportFailText = "<color=red>Import failed!</color> SaberTailor.json file read error";

        private void SetImportStatus(ConfigImportResult configImportResult) {
            SetStatusText(configImportResult switch {
                ConfigImportResult.Success => SuccessfulImportText,
                ConfigImportResult.DevicelessFail => DevicelessImportFailText,
                ConfigImportResult.ParseFail => ParseImportFailText,
                _ => throw new ArgumentOutOfRangeException()
            });
        }

        #endregion

        #endregion

        #region Export

        #region Export to settings

        [UIAction("export-to-settings-on-click")]
        [UsedImplicitly]
        private void ExportToSettingsOnClick() {
            var result = ConfigMigration.ExportToSettings();
            SetExportStatus(result);
        }

        #endregion

        #region Export to tailor

        [UIAction("export-to-tailor-on-click")]
        [UsedImplicitly]
        private void ExportToTailorOnClick() {
            var result = ConfigMigration.ExportToSaberTailor();
            SetExportStatus(result);
        }

        #endregion

        #region Export status

        private const string SuccessfulExportText = "<color=green>Exported</color>";
        private const string DevicelessExportFailText = "<color=red>Export failed!</color> Unknown VR device";
        private const string WriteExportFailText = "<color=red>Export failed!</color> file write error";

        private void SetExportStatus(ConfigExportResult configExportResult) {
            SetStatusText(configExportResult switch {
                ConfigExportResult.Success => SuccessfulExportText,
                ConfigExportResult.DevicelessFail => DevicelessExportFailText,
                ConfigExportResult.WriteFail => WriteExportFailText,
                _ => throw new ArgumentOutOfRangeException()
            });
        }

        #endregion

        #endregion

        #endregion

        #region Status text

        private string _statusText = "";

        [UIValue("status-text")]
        [UsedImplicitly]
        private string StatusText {
            get => _statusText;
            set {
                _statusText = value;
                NotifyPropertyChanged();
            }
        }

        private readonly DelayedAction _statusTextResetAction = new();

        private void SetStatusText(string value) {
            StatusText = value;
            _statusTextResetAction.InvokeLater(3000, ResetStatusText);
        }

        private void ResetStatusText() {
            StatusText = "";
        }

        #endregion
    }
}