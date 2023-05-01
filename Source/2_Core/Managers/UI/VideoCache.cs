using System.Collections;
using System.Collections.Generic;
using System.IO;
using IPA.Utilities;
using UnityEngine.Networking;

namespace EasyOffset;

internal static class VideoCache {
    #region Cache

    private static readonly string CacheDirectory = Path.Combine(UnityGame.UserDataPath, "EasyOffset", "Cache");

    private static readonly Dictionary<string, string> Cache = new();

    private static string StoreInCache(string key, byte[] data) {
        CreateCacheDirectoryIfNeeded();
        var absolutePath = Path.Combine(CacheDirectory, $"{key}.mp4");
        File.WriteAllBytes(absolutePath, data);
        Cache[key] = absolutePath;
        return absolutePath;
    }

    private static void CreateCacheDirectoryIfNeeded() {
        if (Directory.Exists(CacheDirectory)) return;
        Directory.CreateDirectory(CacheDirectory);
    }

    #endregion

    #region GetVideoCoroutine

    public static IEnumerator GetVideoCoroutine(string key, string url, IWebRequestHandler<string> handler) {
        if (Cache.ContainsKey(key)) {
            handler.OnRequestFinished(Cache[key]);
            yield break;
        }

        var request = new VideoRequestDescriptor(key, url);
        yield return NetworkingUtils.ProcessRequestCoroutine(request, handler, 1, 300);
    }

    #endregion

    #region VideoRequestDescriptor

    private class VideoRequestDescriptor : IWebRequestDescriptor<string> {
        private readonly string _key;
        private readonly string _url;

        public VideoRequestDescriptor(string key, string url) {
            _key = key;
            _url = url;
        }

        public UnityWebRequest CreateWebRequest() {
            return UnityWebRequest.Get(_url);
        }

        public string ParseResponse(UnityWebRequest request) {
            return StoreInCache(_key, request.downloadHandler.data);
        }
    }

    #endregion
}