using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using JetBrains.Annotations;

namespace EasyOffset;

internal partial class ModPanelUI {
    #region Active

    private bool _presetsBrowserActive;

    [UIValue("pb-active")]
    [UsedImplicitly]
    private bool PresetsBrowserActive {
        get => _presetsBrowserActive;
        set {
            _presetsBrowserActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Preset file name

    private string _presetFileName = "NewPreset";

    [UIValue("pb-name-value")]
    [UsedImplicitly]
    private string PresetFileName {
        get => _presetFileName;
        set {
            _presetFileName = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("pb-name-on-change")]
    [UsedImplicitly]
    private void PresetFilenameOnChange(string value) {
        for (var i = 0; i < _storedConfigPresets.Count; i++) {
            if (_storedConfigPresets[i].Name != value) continue;
            _presetsBrowserList.tableView.SelectCellWithIdx(i);
            return;
        }

        _presetsBrowserList.tableView.ClearSelection();
    }

    #endregion

    #region List

    private List<StoredConfigPreset> _storedConfigPresets = new();

    [UIComponent("pb-list")] [UsedImplicitly]
    private CustomListTableData _presetsBrowserList;

    [UIAction("pb-list-select-cell")]
    [UsedImplicitly]
    private void PresetsBrowserListSelectCell(TableView tableView, int row) {
        if (row >= _storedConfigPresets.Count) return;
        var selectedConfig = _storedConfigPresets[row];
        PresetFileName = selectedConfig.Name;
    }

    [UIAction("pb-refresh-on-click")]
    [UsedImplicitly]
    private void PresetsBrowserRefreshOnClick() {
        UpdatePresetsBrowserList();
    }

    private void UpdatePresetsBrowserList() {
        _presetsBrowserList.data.Clear();
        _storedConfigPresets = ConfigPresetsStorage.GetAllStoredPresets();

        foreach (var storedConfigPreset in _storedConfigPresets) {
            _presetsBrowserList.data.Add(new CustomListTableData.CustomCellInfo(
                PresetUtils.GetPresetCellString(storedConfigPreset)
            ));
        }

        _presetsBrowserList.tableView.ReloadData();
        _presetsBrowserList.tableView.ClearSelection();
    }

    #endregion

    #region Cancel button

    [UIAction("pb-cancel-on-click")]
    [UsedImplicitly]
    private void PresetsBrowserCancelOnClick() {
        GoToMainPage();
    }

    #endregion

    #region Save button

    [UIValue("pb-save-hint")] [UsedImplicitly]
    private string _presetsBrowserSaveHint = "Save current preset to file" +
                                             "\n" +
                                             "\n<color=red>This action will overwrite existing files</color>";

    private bool _presetsBrowserSaveActive;

    [UIValue("pb-save-active")]
    [UsedImplicitly]
    private bool PresetsBrowserSaveActive {
        get => _presetsBrowserSaveActive;
        set {
            _presetsBrowserSaveActive = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("pb-save-on-click")]
    [UsedImplicitly]
    private void PresetsBrowserSaveOnClick() {
        if (!ConfigPresetsStorage.SaveCurrentPreset(PresetFileName)) return;
        GoToMainPage();
    }

    #endregion

    #region Load button

    private bool _presetsBrowserLoadActive;

    [UIValue("pb-load-hint")] [UsedImplicitly]
    private string _presetsBrowserLoadHint = "Load preset from file" +
                                             "\n" +
                                             "\n<color=red>Any unsaved changes will be lost</color>";

    [UIValue("pb-load-active")]
    [UsedImplicitly]
    private bool PresetsBrowserLoadActive {
        get => _presetsBrowserLoadActive;
        set {
            _presetsBrowserLoadActive = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("pb-load-on-click")]
    [UsedImplicitly]
    private void PresetsBrowserLoadOnClick() {
        PluginConfig.CreateUndoStep($"Load preset: {PresetFileName}");
        if (!ConfigPresetsStorage.LoadPreset(PresetFileName)) return;
        GoToMainPage();
    }

    #endregion
}