using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EasyOffset;

public class PepegaSingletonFix<T> : MonoBehaviour, INotifyPropertyChanged where T : MonoBehaviour {
    protected static T _instance;
    protected static object _lock = new();
    protected static bool _applicationIsQuitting;

    public static T instance {
        get {
            if (_applicationIsQuitting) {
                Debug.LogWarning((object)("[Singleton] Instance '" + (object)typeof(T) + "' already destroyed on application quit. Won't create again - returning null."));
                return default(T);
            }

            lock (_lock) {
                if (_instance != null) return _instance;

                _instance = (T)FindObjectOfType(typeof(T));
                if (FindObjectsOfType(typeof(T)).Length > 1) {
                    Debug.LogError("[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopenning the scene might fix it.");
                    return _instance;
                }

                if (_instance != null) return _instance;

                var target = new GameObject();
                _instance = target.AddComponent<T>();
                target.name = typeof(T).ToString();
                DontDestroyOnLoad((Object)target);

                return _instance;
            }
        }
    }

    public static void TouchInstance() {
        var num = instance == null ? 1 : 0;
    }

    public static bool IsSingletonAvailable => !_applicationIsQuitting && _instance != null;

    public virtual void OnEnable() => DontDestroyOnLoad((Object)this);

    protected virtual void OnDestroy() => _applicationIsQuitting = true;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
        try {
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null) return;
            propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        } catch (Exception ex) {
            Plugin.Log?.Error("Error Invoking PropertyChanged: " + ex.Message);
            Plugin.Log?.Error(ex);
        }
    }
}