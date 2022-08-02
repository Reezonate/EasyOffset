using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset {
    public class SettingsUI : NotifiableSingleton<SettingsUI> {
        #region Initialize

        private void Start() {
            PluginConfig.OnEnabledChange += OnEnabledChanged;
            OnEnabledChanged(PluginConfig.Enabled);
        }

        private void OnEnabledChanged(bool value) {
            UniversalImportInteractable = !value;
        }

        #endregion

        #region Global Settings

        [UIValue("enabled-value"), UsedImplicitly]
        private bool EnabledValue {
            get => PluginConfig.Enabled;
            set => PluginConfig.Enabled = value;
        }

        [UIValue("hide-controllers-value"), UsedImplicitly]
        private bool HideControllersValue {
            get => PluginConfig.HideControllers;
            set => PluginConfig.HideControllers = value;
        }

        #region Minimal warning level

        [UIValue("warnings-choices"), UsedImplicitly]  private List<object> _minimalWarningLevelChoices = WarningLevelUtils.AllNamesObjects.ToList();

        [UIValue("warnings-choice"), UsedImplicitly]  private string _minimalWarningLevelChoice = WarningLevelUtils.TypeToName(PluginConfig.MinimalWarningLevel);

        [UIAction("warnings-on-change"), UsedImplicitly]
        private void MinimalWarningLevelOnChange(string selectedValue) {
            PluginConfig.MinimalWarningLevel = WarningLevelUtils.NameToType(selectedValue);
        }

        #endregion

        #endregion

        #region Config Migration

        #region ZOffset slider

        [UIValue("zo-hint"), UsedImplicitly]  private string _zOffsetSliderHint = "Pivot point offset along saber axis (cm)\n0 - top of the hilt, 17 - bottom of the hilt";

        [UIValue("zo-min"), UsedImplicitly]  private float _zOffsetSliderMin = -15f;

        [UIValue("zo-max"), UsedImplicitly]  private float _zOffsetSliderMax = 25f;

        [UIValue("zo-increment"), UsedImplicitly]  private float _zOffsetSliderIncrement = 0.5f;


        [UIValue("zo-value"), UsedImplicitly]
        private float ZOffsetSliderValue {
            get => ConfigMigration.ZOffset * 100f;
            set => ConfigMigration.ZOffset = value / 100f;
        }

        #endregion

        #region Import

        #region Universal import

        private bool _universalImportInteractable;

        [UIValue("universal-import-interactable"), UsedImplicitly]
        private bool UniversalImportInteractable {
            get => _universalImportInteractable;
            set {
                if (_universalImportInteractable.Equals(value)) return;
                _universalImportInteractable = value;
                NotifyPropertyChanged();
            }
        }

        [UIAction("universal-import-on-click"), UsedImplicitly]
        private void UniversalImportOnClick() {
            var result = ConfigMigration.UniversalImport();
            SetImportStatus(result);
        }

        #endregion

        #region Import from settings

        [UIAction("import-from-settings-on-click"), UsedImplicitly]
        private void ImportFromSettingsOnClick() {
            var result = ConfigMigration.ImportFromSettings();
            SetImportStatus(result);
        }

        #endregion

        #region Import status

        private const string SuccessfulImportText = "<color=green>Imported</color>";
        private const string DevicelessImportFailText = "<color=red>Import failed!</color> Unknown VR device";
        private const string ParseImportFailText = "<color=red>Import failed!</color> SaberTailor.json file read error";
        private const string InternalErrorText = "<color=red>Import failed!</color> Internal error";

        private void SetImportStatus(ConfigImportResult configImportResult) {
            SetStatusText(configImportResult switch {
                ConfigImportResult.Success => SuccessfulImportText,
                ConfigImportResult.DevicelessFail => DevicelessImportFailText,
                ConfigImportResult.ParseFail => ParseImportFailText,
                ConfigImportResult.InternalError => InternalErrorText,
                _ => throw new ArgumentOutOfRangeException()
            });
        }

        #endregion

        #endregion

        #region Export

        #region Export to settings right

        [UIAction("export-to-settings-right-on-click")]
        [UsedImplicitly]
        private void ExportToSettingsRightOnClick() {
            var result = ConfigMigration.ExportToSettings(Hand.Right);
            SetExportStatus(result);
        }

        #endregion

        #region Export to settings left

        [UIAction("export-to-settings-left-on-click")]
        [UsedImplicitly]
        private void ExportToSettingsLeftOnClick() {
            var result = ConfigMigration.ExportToSettings(Hand.Left);
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

        private void SetStatusText(string value) {
            StatusText = value;
            this.ReInvokeWithDelay(ref _statusResetCoroutine, ResetStatusText, 3.0f);
        }

        private Coroutine _statusResetCoroutine;

        private void ResetStatusText() {
            StatusText = "";
        }

        #endregion
    }
}