using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using JetBrains.Annotations;

namespace EasyOffset;

internal class PresetsBrowserPanel : ReeUIComponentV2 {
    #region Events

    public void Activate(bool allowSave, bool allowLoad) {
        UpdatePresetsBrowserList();
        PresetsBrowserActive = true;
        PresetsBrowserSaveActive = allowSave;
        PresetsBrowserLoadActive = allowLoad;
    }

    public void Deactivate() {
        if (!PresetsBrowserActive) return;
        PresetsBrowserActive = false;
        ModPanelUI.OpenMainPage();
    }

    #endregion

    #region Active

    private bool _presetsBrowserActive;

    [UIValue("pb-active"), UsedImplicitly]
    private bool PresetsBrowserActive {
        get => _presetsBrowserActive;
        set {
            if (_presetsBrowserActive.Equals(value)) return;
            _presetsBrowserActive = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Preset file name

    private string _presetFileName = "NewPreset";

    [UIValue("pb-name-value"), UsedImplicitly]
    private string PresetFileName {
        get => _presetFileName;
        set {
            if (_presetFileName.Equals(value)) return;
            _presetFileName = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("pb-name-on-change"), UsedImplicitly]
    private void PresetFilenameOnChange(string value) {
        for (var i = 0; i < _storedConfigPresets.Count; i++) {
            if (_storedConfigPresets[i].Name != value) continue;
            _presetsBrowserList.TableView.SelectCellWithIdx(i);
            return;
        }

        _presetsBrowserList.TableView.ClearSelection();
    }

    #endregion

    #region List

    private List<StoredConfigPreset> _storedConfigPresets = new();

    [UIComponent("pb-list"), UsedImplicitly]
    private CustomListTableData _presetsBrowserList;

    [UIAction("pb-list-select-cell"), UsedImplicitly]
    private void PresetsBrowserListSelectCell(TableView tableView, int row) {
        if (row >= _storedConfigPresets.Count) return;
        var selectedConfig = _storedConfigPresets[row];
        PresetFileName = selectedConfig.Name;
    }

    [UIAction("pb-refresh-on-click"), UsedImplicitly]
    private void PresetsBrowserRefreshOnClick() {
        UpdatePresetsBrowserList();
    }

    private void UpdatePresetsBrowserList() {
        _presetsBrowserList.Data.Clear();
        _storedConfigPresets = ConfigPresetsStorage.GetAllStoredPresets();

        foreach (var storedConfigPreset in _storedConfigPresets) {
            _presetsBrowserList.Data.Add(new CustomListTableData.CustomCellInfo(
                    PresetUtils.GetPresetCellString(storedConfigPreset)
                )
            );
        }

        _presetsBrowserList.TableView.ReloadData();
        _presetsBrowserList.TableView.ClearSelection();
    }

    #endregion

    #region Cancel button

    [UIAction("pb-cancel-on-click"), UsedImplicitly]
    private void PresetsBrowserCancelOnClick() {
        Deactivate();
    }

    #endregion

    #region Save button

    [UIValue("pb-save-hint"), UsedImplicitly]
    private string _presetsBrowserSaveHint = "Save current preset to file" +
                                             "\n" +
                                             "\n<color=red>This action will overwrite existing files</color>";

    private bool _presetsBrowserSaveActive;

    [UIValue("pb-save-active"), UsedImplicitly]
    private bool PresetsBrowserSaveActive {
        get => _presetsBrowserSaveActive;
        set {
            if (_presetsBrowserSaveActive.Equals(value)) return;
            _presetsBrowserSaveActive = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("pb-save-on-click"), UsedImplicitly]
    private void PresetsBrowserSaveOnClick() {
        if (!ConfigPresetsStorage.SaveCurrentPreset(PresetFileName)) return;
        Deactivate();
    }

    #endregion

    #region Load button

    private bool _presetsBrowserLoadActive;

    [UIValue("pb-load-hint"), UsedImplicitly]
    private string _presetsBrowserLoadHint = "Load preset from file" +
                                             "\n" +
                                             "\n<color=red>Any unsaved changes will be lost</color>";

    [UIValue("pb-load-active"), UsedImplicitly]
    private bool PresetsBrowserLoadActive {
        get => _presetsBrowserLoadActive;
        set {
            if (_presetsBrowserLoadActive.Equals(value)) return;
            _presetsBrowserLoadActive = value;
            NotifyPropertyChanged();
        }
    }

    [UIAction("pb-load-on-click"), UsedImplicitly]
    private void PresetsBrowserLoadOnClick() {
        PluginConfig.CreateUndoStep($"Load preset: {PresetFileName}");
        if (!ConfigPresetsStorage.LoadPreset(PresetFileName)) return;
        Deactivate();
    }

    #endregion
}