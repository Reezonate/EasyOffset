using Newtonsoft.Json;
using UnityEngine.Networking;

namespace EasyOffset;

internal class RemoteConfig : PersistentSingleton<RemoteConfig>, IWebRequestHandler<RemoteConfig.RemoteConfigData> {
    #region Values

    public static bool IsReady;

    public static UserGuideConfigData UserGuideConfig;

    #endregion

    #region Initialize

    public void Initialize() {
        if (IsReady) return;
        StartCoroutine(NetworkingUtils.ProcessRequestCoroutine(new RequestDescriptor(), this));
    }

    #endregion

    #region Request

    private class RequestDescriptor : IWebRequestDescriptor<RemoteConfigData> {
        public UnityWebRequest CreateWebRequest() {
            return UnityWebRequest.Get("https://github.com/Reezonate/EasyOffset/raw/master/media/RemoteConfig.json");
        }

        public RemoteConfigData ParseResponse(UnityWebRequest request) {
            var serializerSettings = new JsonSerializerSettings() {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.DeserializeObject<RemoteConfigData>(request.downloadHandler.text, serializerSettings);
        }
    }

    public void OnRequestStarted() { }

    public void OnRequestFinished(RemoteConfigData result) {
        UserGuideConfig = result.UserGuideConfig;
        IsReady = true;
    }

    public void OnRequestFailed(string reason) {
        Plugin.Log.Error($"RemoteConfig download failed! UserGuide won't be available. Reason: {reason}");
    }

    public void OnRequestProgress(float uploadProgress, float downloadProgress, float overallProgress) { }

    #endregion

    #region Structs

    public struct RemoteConfigData {
        public UserGuideConfigData UserGuideConfig;
    }

    public struct UserGuideConfigData {
        public string GettingStartedVideoURL;
        public string PositionAutoVideoURL;
        public string RotationAutoVideoURL;
        public string SwingBenchmarkVideoURL;
        public string RotationVideoURL;
        public string MoreInfoVideoURL;
        public bool FunnyEnabled;
        public int FunnyType;
    }

    #endregion
}