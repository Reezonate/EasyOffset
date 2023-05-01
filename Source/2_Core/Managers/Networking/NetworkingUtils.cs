using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace EasyOffset {
    internal static class NetworkingUtils {
        #region ProcessRequestCoroutine

        public static IEnumerator ProcessRequestCoroutine<T>(
            IWebRequestDescriptor<T> requestDescriptor,
            IWebRequestHandler<T> requestHandler,
            int retries = 1,
            int timeoutSeconds = 0
        ) {
            for (var i = 1; i <= retries; i++) {
                requestHandler.OnRequestStarted();

                var request = requestDescriptor.CreateWebRequest();
                if (timeoutSeconds > 0) request.timeout = timeoutSeconds;

                Plugin.Log.Debug($"Request[{request.GetHashCode()}]: {request.url}");
                yield return AwaitRequestWithProgress(request, requestHandler);
                Plugin.Log.Debug($"Response[{request.GetHashCode()}]: {request.error ?? request.responseCode.ToString()}");

                if (request.isHttpError || request.isNetworkError) {
                    requestHandler.OnRequestFailed($"HTTP/Network error: {request.error} {request.downloadHandler?.text}");
                    continue; //retry
                }

                try {
                    requestHandler.OnRequestFinished(requestDescriptor.ParseResponse(request));
                    break; //no retry
                } catch (Exception e) {
                    Plugin.Log.Debug($"Response[{request.GetHashCode()}] Exception: {e}");
                    requestHandler.OnRequestFailed($"Internal error: {e.Message}");
                    break; //no retry
                }
            }
        }

        private static IEnumerator AwaitRequestWithProgress<T>(UnityWebRequest request, IWebRequestHandler<T> requestHandler) {
            var asyncOperation = request.SendWebRequest();

            var overallProgress = 0f;
            requestHandler.OnRequestProgress(0, 0, 0);

            bool WaitUntilPredicate() => request.isDone || !overallProgress.Equals(asyncOperation.progress);

            do {
                yield return new WaitUntil(WaitUntilPredicate);
                if (overallProgress.Equals(asyncOperation.progress)) continue;
                overallProgress = asyncOperation.progress;
                requestHandler.OnRequestProgress(request.uploadProgress, request.downloadProgress, overallProgress);
            } while (!request.isDone);
        }

        #endregion
    }
}