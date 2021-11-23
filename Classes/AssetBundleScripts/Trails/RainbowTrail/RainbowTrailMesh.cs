using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class RainbowTrailMesh : AbstractTrailMesh<RainbowTrailNode> {
        public RainbowTrailMesh(
            int nodesCount,
            int horizontalResolution,
            int verticalResolution
        ) : base(
            nodesCount,
            horizontalResolution,
            verticalResolution
        ) { }

        protected override void GetVertexData(RainbowTrailNode trailNode, float horizontalRatio,
            out Vector3 vertex,
            out Vector2 texcoord1,
            out Vector2 texcoord2
        ) {
            vertex = trailNode.LerpHorizontal(horizontalRatio);
            texcoord1 = Vector2.zero;
            texcoord2 = Vector2.zero;
        }
    }
}