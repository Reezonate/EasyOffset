namespace EasyOffset.AssetBundleScripts {
    public interface ITrailNode {
        ITrailNode Plus(ITrailNode other);
        ITrailNode Minus(ITrailNode other);
        ITrailNode Div(float number);
        ITrailNode Times(float number);
    }
}