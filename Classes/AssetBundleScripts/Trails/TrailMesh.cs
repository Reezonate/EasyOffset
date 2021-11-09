using UnityEngine;

namespace EasyOffset.AssetBundleScripts {
    public class TrailMesh {
        #region Properties

        private int MaxEdgesCount => _resolution + 1;
        private int MaxVerticesCount => MaxEdgesCount * 2;
        private int MaxTrianglesCount => _resolution * 2;
        private int MaxTrianglesArraySize => MaxTrianglesCount * 3;

        #endregion

        #region Arrays

        private readonly TrailSpline _leftCurve;
        private readonly TrailSpline _rightCurve;

        private readonly Vector3[] _leftBuffer;
        private readonly Vector3[] _rightBuffer;

        private readonly Vector3[] _vertices;
        private readonly Vector2[] _uv;
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

            _leftCurve = new TrailSpline(lifetime);
            _rightCurve = new TrailSpline(lifetime);

            _leftBuffer = new Vector3[MaxEdgesCount];
            _rightBuffer = new Vector3[MaxEdgesCount];

            _vertices = new Vector3[MaxVerticesCount];
            _uv = new Vector2[MaxVerticesCount];
            _triangles = new int[MaxTrianglesArraySize];

            FillUvArray();
            FillTrianglesArray();
        }

        #endregion

        #region Update

        private Vector3 _previousPosition = Vector3.zero;

        public void Update(
            Vector3 position,
            Vector3 normal
        ) {
            if (position == _previousPosition) return;

            var cutTangent = (position - _previousPosition).normalized;
            var cutNormal = Quaternion.AngleAxis(90f, normal) * cutTangent;
            var widthOffset = cutNormal * _halfWidth;

            var curveReady = _leftCurve.Add(position - widthOffset);
            _rightCurve.Add(position + widthOffset);

            if (curveReady) {
                UpdateVertices();
                UpdateMesh();
            }

            _previousPosition = position;
        }

        private void UpdateVertices() {
            _leftCurve.FillArray(_leftBuffer);
            _rightCurve.FillArray(_rightBuffer);

            for (var edgeIndex = 0; edgeIndex < MaxEdgesCount; edgeIndex++) {
                var indexOffset = edgeIndex * 2;

                var leftPosition = _leftBuffer[edgeIndex];
                var rightPosition = _rightBuffer[edgeIndex];

                _vertices[0 + indexOffset] = leftPosition;
                _vertices[1 + indexOffset] = rightPosition;
            }
        }

        private void UpdateMesh() {
            Mesh.vertices = _vertices;
            Mesh.uv = _uv;
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