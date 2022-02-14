using System;
using System.Collections;
using UnityEngine;

namespace EasyOffset; 

public static class AsyncUtils {
    public static IEnumerator InvokeWithDelay(Action action, float delaySeconds) {
        yield return new WaitForSeconds(delaySeconds);
        action?.Invoke();
    }
}