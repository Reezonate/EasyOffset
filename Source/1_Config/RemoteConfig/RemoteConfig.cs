using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace EasyOffset;

internal class RemoteConfig : MonoBehaviour, IWebRequestHandler<RemoteConfig.RemoteConfigData> {
    #region Values

    public static bool IsReady;

    public static UserGuideConfigData UserGuideConfig;
    
    public static string DiscordInvite = "https://discord.gg/HRdvMD2R8r";
    public static string DonationsURL = "https://ko-fi.com/reezonate";

    #endregion

    #region Initialize

    public void Awake() {
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
        DiscordInvite = result.DiscordInvite;
        DonationsURL = result.DonationsURL;
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
        public string DiscordInvite;
        public string DonationsURL;
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