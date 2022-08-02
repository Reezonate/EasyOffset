using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ModestTree;
using UnityEngine;

namespace EasyOffset;

internal partial class PluginConfig {
    #region Logic

    private const int MaxUndoStepsCount = 30;

    public static event Action<bool, string> UndoAvailableChangedEvent;
    public static event Action<bool, string> RedoAvailableChangedEvent;

    private static readonly List<ConfigSnapshot> UndoSteps = new();
    private static readonly List<ConfigSnapshot> RedoSteps = new();

    public static void CreateUndoStep(
        string description,
        [CanBeNull] Action onUndoAction = null,
        [CanBeNull] Action onRedoAction = null
    ) {
        var snapshot = GenerateSnapshot(description, onUndoAction, onRedoAction);
        UndoSteps.Add(snapshot);
        RedoSteps.Clear();
        if (UndoSteps.Count > MaxUndoStepsCount) UndoSteps.RemoveAt(0);
        NotifyUndoListChanged();
        NotifyRedoListChanged();
    }

    public static void UndoLastChange() {
        if (UndoSteps.IsEmpty()) return;

        var lastUndoIndex = UndoSteps.Count - 1;
        var undoSnapshot = UndoSteps[lastUndoIndex];
        UndoSteps.RemoveAt(lastUndoIndex);

        var redoSnapshot = GenerateSnapshot(undoSnapshot.Description, undoSnapshot.OnUndoAction, undoSnapshot.OnRedoAction);
        RedoSteps.Add(redoSnapshot);

        ApplySnapshot(undoSnapshot);
        undoSnapshot.OnUndoAction?.Invoke();

        NotifyUndoListChanged();
        NotifyRedoListChanged();
    }

    public static void RedoLastChange() {
        if (RedoSteps.IsEmpty()) return;

        var lastRedoIndex = RedoSteps.Count - 1;
        var redoSnapshot = RedoSteps[lastRedoIndex];
        RedoSteps.RemoveAt(lastRedoIndex);

        var undoSnapshot = GenerateSnapshot(redoSnapshot.Description, redoSnapshot.OnUndoAction, redoSnapshot.OnRedoAction);
        UndoSteps.Add(undoSnapshot);

        ApplySnapshot(redoSnapshot);
        redoSnapshot.OnRedoAction?.Invoke();

        NotifyUndoListChanged();
        NotifyRedoListChanged();
    }

    private static void NotifyUndoListChanged() {
        if (UndoSteps.IsEmpty()) {
            UndoAvailableChangedEvent?.Invoke(false, "");
        } else {
            UndoAvailableChangedEvent?.Invoke(true, UndoSteps.Last().Description);
        }
    }

    private static void NotifyRedoListChanged() {
        if (RedoSteps.IsEmpty()) {
            RedoAvailableChangedEvent?.Invoke(false, "");
        } else {
            RedoAvailableChangedEvent?.Invoke(true, RedoSteps.Last().Description);
        }
    }

    #endregion

    #region GenerateSnapshot

    private static ConfigSnapshot GenerateSnapshot(
        string description,
        [CanBeNull] Action onUndoAction,
        [CanBeNull] Action onRedoAction
    ) {
        return new ConfigSnapshot {
            Description = description,

            OnUndoAction = onUndoAction,
            OnRedoAction = onRedoAction,

            LeftPivotPosition = LeftSaberPivotPosition,
            LeftRotation = LeftSaberRotation,
            LeftZOffset = LeftSaberZOffset,
            LeftHasReference = LeftSaberHasReference,
            LeftReferenceRotation = LeftSaberReferenceRotation,

            RightPivotPosition = RightSaberPivotPosition,
            RightRotation = RightSaberRotation,
            RightZOffset = RightSaberZOffset,
            RightHasReference = RightSaberHasReference,
            RightReferenceRotation = RightSaberReferenceRotation,
        };
    }

    #endregion

    #region ApplySnapshot

    private static void ApplySnapshot(ConfigSnapshot snapshot) {
        DisableConfigChangeEvent();

        LeftSaberPivotPosition = snapshot.LeftPivotPosition;
        LeftSaberRotation = snapshot.LeftRotation;
        LeftSaberZOffset = snapshot.LeftZOffset;
        LeftSaberHasReference = snapshot.LeftHasReference;
        LeftSaberReferenceRotation = snapshot.LeftReferenceRotation;

        RightSaberPivotPosition = snapshot.RightPivotPosition;
        RightSaberRotation = snapshot.RightRotation;
        RightSaberZOffset = snapshot.RightZOffset;
        RightSaberHasReference = snapshot.RightHasReference;
        RightSaberReferenceRotation = snapshot.RightReferenceRotation;

        EnableConfigChangeEvent();
        NotifyConfigWasChanged();
    }

    #endregion

    #region ConfigSnapshot struct

    private struct ConfigSnapshot {
        public string Description;

        [CanBeNull] public Action OnUndoAction;
        [CanBeNull] public Action OnRedoAction;

        public Vector3 LeftPivotPosition;
        public Quaternion LeftRotation;
        public float LeftZOffset;
        public bool LeftHasReference;
        public Quaternion LeftReferenceRotation;

        public Vector3 RightPivotPosition;
        public Quaternion RightRotation;
        public float RightZOffset;
        public bool RightHasReference;
        public Quaternion RightReferenceRotation;
    }

    #endregion
}