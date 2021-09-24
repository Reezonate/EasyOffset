using UnityEngine;

namespace EasyOffset.SyncedWithUnity {
    public class TrailMesh {
        #region Properties

        private int MaxEdgesCount => _resolution + 1;
        private int MaxVerticesCount => MaxEdgesCount * 2;
        private int MaxTrianglesCount => _resolution * 2;
        private int MaxTrianglesArraySize => MaxTrianglesCount * 3;

        #endregion

        #region Arrays

        private readonly TrailSpline _pathCurve;
        private readonly TrailSpline _offsetsCurve;
        private readonly Vector3[] _pathBuffer;
        private readonly Vector3[] _offsetBuffer;

        private readonly Vector3[] _vertices;
        private readonly Vector2[] _uv;
        private readonly Color[] _colors;
        private readonly int[] _triangles;

        #endregion

        #region Constructor

        private readonly int _resolution;
        private readonly float _halfWidth;
        public readonly Mesh Mesh;

        public TrailMesh(
            int lifetime,
            int resolution,
            float width
        ) {
            _halfWidth = width / 2;
            _resolution = resolution;
            Mesh = new Mesh();

            _pathCurve = new TrailSpline(lifetime);
            _offsetsCurve = new TrailSpline(lifetime);
            _pathBuffer = new Vector3[MaxEdgesCount];
            _offsetBuffer = new Vector3[MaxEdgesCount];

            _vertices = new Vector3[MaxVerticesCount];
            _uv = new Vector2[MaxVerticesCount];
            _colors = new Color[MaxVerticesCount];
            _triangles = new int[MaxTrianglesArraySize];

            FillUvArray();
            FillTrianglesArray();
        }

        #endregion

        #region Update

        private Vector3 _previousTipPosition = Vector3.zero;

        public void Update(
            Vector3 position,
            Vector3 normal
        ) {
            if (position == _previousTipPosition) return;

            var cutTangent = position - _previousTipPosition;
            var cutNormal = Quaternion.LookRotation(cutTangent, normal) * Vector3.right;
            var widthOffset = cutNormal * _halfWidth;
            var curveReady = _pathCurve.Add(position);

            _offsetsCurve.Add(widthOffset);

            if (curveReady) {
                UpdateVertices();
                UpdateMesh();
            }

            _previousTipPosition = position;
        }

        private void UpdateVertices() {
            _pathCurve.FillArray(_pathBuffer);
            _offsetsCurve.FillArray(_offsetBuffer);

            for (var edgeIndex = 0; edgeIndex < MaxEdgesCount; edgeIndex++) {
                var indexOffset = edgeIndex * 2;

                var trailPosition = _pathBuffer[edgeIndex];
                var widthOffset = _offsetBuffer[edgeIndex];
                var cutNormal = widthOffset.normalized;

                var vertexColor = new Color(
                    cutNormal.x,
                    cutNormal.y,
                    cutNormal.z,
                    1f
                );

                _colors[0 + indexOffset] = vertexColor;
                _colors[1 + indexOffset] = vertexColor;

                _vertices[0 + indexOffset] = trailPosition + widthOffset;
                _vertices[1 + indexOffset] = trailPosition - widthOffset;
            }
        }

        private void UpdateMesh() {
            Mesh.vertices = _vertices;
            Mesh.uv = _uv;
            Mesh.colors = _colors;
            Mesh.triangles = _triangles;
            Mesh.bounds = new Bounds(new Vector3(0f, 0f, 3f), new Vector3(5f, 5f, 5f));
        }

        #endregion

        #region FillImmutableArrays

        private void FillUvArray() {
            for (var edgeIndex = 0; edgeIndex < MaxEdgesCount; edgeIndex++) {
                var ratio = (float) edgeIndex / (MaxEdgesCount - 1);
                var offset = edgeIndex * 2;
                _uv[offset] = new Vector2(0f, ratio);
                _uv[offset + 1] = new Vector2(1f, ratio);
            }
        }

        private void FillTrianglesArray() {
            for (var quadIndex = 0; quadIndex < _resolution; quadIndex++) {
                var trianglesOffset = quadIndex * 6;
                var verticesOffset = quadIndex * 2;
                _triangles[0 + trianglesOffset] = 0 + verticesOffset;
                _triangles[1 + trianglesOffset] = 2 + verticesOffset;
                _triangles[2 + trianglesOffset] = 3 + verticesOffset;
                _triangles[3 + trianglesOffset] = 3 + verticesOffset;
                _triangles[4 + trianglesOffset] = 1 + verticesOffset;
                _triangles[5 + trianglesOffset] = 0 + verticesOffset;
            }
        }

        #endregion
    }
}