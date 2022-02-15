using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyOffset;

public static class AsyncUtils {
    public static void ReInvokeWithDelay(this MonoBehaviour monoBehaviour, [CanBeNull] ref Coroutine coroutine, Action action, float delaySeconds) {
        monoBehaviour.RestartCoroutine(ref coroutine, InvokeWithDelay(action, delaySeconds));
    }

    private static void RestartCoroutine(this MonoBehaviour monoBehaviour, [CanBeNull] ref Coroutine coroutine, IEnumerator asyncTask) {
        if (coroutine != null) monoBehaviour.StopCoroutine(coroutine);
        coroutine = monoBehaviour.StartCoroutine(asyncTask);
    }

    private static IEnumerator InvokeWithDelay(Action action, float delaySeconds) {
        yield return new WaitForSeconds(delaySeconds);
        action?.Invoke();
    }
}