namespace EasyOffset {
    internal interface IWebRequestHandler<in T> {
        public void OnRequestStarted();
        public void OnRequestFinished(T result);
        public void OnRequestFailed(string reason);
        
        public void OnRequestProgress(float uploadProgress, float downloadProgress, float overallProgress);
    }
}