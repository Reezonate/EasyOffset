using System;
using BeatSaberMarkupLanguage.FloatingScreen;
using HMUI;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace EasyOffset;

[UsedImplicitly]
public class UserGuideFloatingManager : IInitializable, IDisposable {
    #region Inject

    private readonly UserGuideViewController _userGuideViewController;

    public UserGuideFloatingManager(
        UserGuideViewController userGuideViewController
    ) {
        _userGuideViewController = userGuideViewController;
    }

    #endregion

    #region Initialize

    private FloatingScreen _floatingScreen;
    private Transform _handleTransform;
    private Material _handleMaterial;

    public void Initialize() {
        UIEvents.UserGuideButtonWasPressedEvent += OnUserGuideButtonWasPressed;

        _floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(120, 90), true, new Vector3(0, 1, 1), Quaternion.identity);
        _floatingScreen.SetRootViewController(_userGuideViewController, ViewController.AnimationType.None);
        InitializeHandle(_floatingScreen.Handle);
        ResetPosition();

        _floatingScreen.gameObject.SetActive(false);
    }

    public void Dispose() {
        UIEvents.UserGuideButtonWasPressedEvent -= OnUserGuideButtonWasPressed;
        if (_floatingScreen != null) Object.Destroy(_floatingScreen);
    }

    #endregion

    #region ResetPosition

    private static readonly Vector3 DefaultPosition = new(0, 1, 1);
    private static readonly Quaternion DefaultRotation = Quaternion.identity;
    private const float SpawnDistance = 1.1f;
    private const float SpawnHeight = 0.7f;

    private void ResetPosition() {
        var mainCamera = Camera.main;
        if (mainCamera == null) {
            _floatingScreen.transform.SetPositionAndRotation(DefaultPosition, DefaultRotation);
        } else {
            var transform = mainCamera.transform;
            var cameraPosition = transform.position;
            var offset = Vector3.Normalize(transform.forward with { y = 0 });
            var position = new Vector3(
                cameraPosition.x + offset.x * SpawnDistance,
                SpawnHeight,
                cameraPosition.z + offset.z * SpawnDistance
            );
            var rotation = Quaternion.LookRotation(position - cameraPosition);
            _floatingScreen.transform.SetPositionAndRotation(position, rotation);
        }
    }

    #endregion

    #region Events

    private bool _isActive;

    private void OnUserGuideButtonWasPressed() {
        _isActive = !_isActive;
        _floatingScreen.gameObject.SetActive(_isActive);
        if (_isActive) ResetPosition();
    }

    #endregion

    #region Handle

    private static readonly Color MoveIconDefaultColor = new Color(0.5f, 0.5f, 0.5f, 0.0f);
    private static readonly Color MoveIconHoverColor = new Color(0.3f, 0.3f, 1.0f, 0.8f);

    private void InitializeHandle(GameObject handle) {
        _handleTransform = handle.transform;
        _handleMaterial = BundleLoader.UserGuideHandle.GetComponent<MeshRenderer>().material;

        _handleTransform.localPosition = new Vector3(0, -30, 0);

        handle.GetComponent<MeshFilter>().mesh = BundleLoader.UserGuideHandle.GetComponent<MeshFilter>().mesh;
        handle.GetComponent<MeshRenderer>().material = _handleMaterial;
        handle.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 0.1f);

        handle.AddComponent<SmoothHoverController>().HoverStateChangedEvent += OnHandleHoverStateChanged;
        OnHandleHoverStateChanged(false, 0.0f);
    }

    private void OnHandleHoverStateChanged(bool isHovered, float progress) {
        var scale = 7.0f + 2.0f * progress;
        _handleTransform.localScale = new Vector3(scale, scale, scale);
        _handleMaterial.color = Color.Lerp(MoveIconDefaultColor, MoveIconHoverColor, progress);
    }

    #endregion
}