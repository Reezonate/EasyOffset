using UnityEngine.Networking;

namespace EasyOffset {
    internal interface IWebRequestDescriptor<out T> {
        public UnityWebRequest CreateWebRequest();
        public T ParseResponse(UnityWebRequest request);
    }
}