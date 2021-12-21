using UnityEngine;

namespace EasyOffset {
    public class TrailMesh {
        #region Data

        private readonly Vector3[] _vertices;

        private readonly int _horizontalResolution;
        private readonly int _columnsCount;
        private readonly int _rowsCount;
        private readonly int _vertexCount;

        private readonly TrailSpline _trailSpline;
        private readonly TrailNode[] _nodesBuffer;

        public readonly Mesh Mesh;

        #endregion

        #region Constructor

        public TrailMesh(int nodesCount, int horizontalResolution, int verticalResolution) {
            _horizontalResolution = horizontalResolution;
            _columnsCount = horizontalResolution + 1;
            _rowsCount = verticalResolution + 1;
            _vertexCount = _columnsCount * _rowsCount;

            _vertices = new Vector3[_vertexCount];

            _trailSpline = new TrailSpline(nodesCount);
            _nodesBuffer = new TrailNode[_rowsCount];

            Mesh = new Mesh {
                vertices = _vertices,
                triangles = CreateTrianglesArray(horizontalResolution, verticalResolution),
                uv = CreateUvArray(),
                bounds = new Bounds(Vector3.zero, Vector3.one * 50)
            };
        }

        #endregion

        #region AddNode

        public void AddNode(TrailNode trailNode) {
            if (!_trailSpline.Add(trailNode)) return;

            _trailSpline.FillArray(_nodesBuffer);

            for (var rowIndex = 0; rowIndex < _rowsCount; rowIndex++) {
                var node = _nodesBuffer[rowIndex];

                for (var columnIndex = 0; columnIndex < _columnsCount; columnIndex++) {
                    var horizontalRatio = (float) columnIndex / _horizontalResolution;
                    var vertexIndex = GetVertexIndex(columnIndex, rowIndex);
                    _vertices[vertexIndex] = node.LerpHorizontal(horizontalRatio);
                }
            }

            Mesh.vertices = _vertices;
        }

        #endregion

        #region Utils

        private int GetVertexIndex(int columnIndex, int rowIndex) {
            return rowIndex * _columnsCount + columnIndex;
        }

        #endregion

        #region CreateArrays

        private Vector2[] CreateUvArray() {
            var tmp = new Vector2[_vertexCount];

            for (var rowIndex = 0; rowIndex < _rowsCount; rowIndex++) {
                var verticalRatio = (float) rowIndex / (_rowsCount - 1);
                for (var columnIndex = 0; columnIndex < _columnsCount; columnIndex++) {
                    var horizontalRatio = (float) columnIndex / (_columnsCount - 1);
                    var vertexIndex = GetVertexIndex(columnIndex, rowIndex);
                    tmp[vertexIndex] = new Vector2(horizontalRatio, verticalRatio);
                }
            }

            return tmp;
        }

        private int[] CreateTrianglesArray(int horizontalResolution, int verticalResolution) {
            var quadCount = horizontalResolution * verticalResolution;
            var tmp = new int[quadCount * 6];

            for (var i = 0; i < verticalResolution; i++) {
                for (var j = 0; j < horizontalResolution; j++) {
                    var quadIndex = i * horizontalResolution + j;

                    var topLeftVertexIndex = GetVertexIndex(j, i);
                    var topRightVertexIndex = GetVertexIndex(j + 1, i);
                    var bottomLeftVertexIndex = GetVertexIndex(j, i + 1);
                    var bottomRightVertexIndex = GetVertexIndex(j + 1, i + 1);

                    var leftTriangleIndex = quadIndex * 6;
                    tmp[leftTriangleIndex + 0] = topLeftVertexIndex;
                    tmp[leftTriangleIndex + 1] = bottomLeftVertexIndex;
                    tmp[leftTriangleIndex + 2] = bottomRightVertexIndex;

                    var rightTriangleIndex = leftTriangleIndex + 3;
                    tmp[rightTriangleIndex + 0] = bottomRightVertexIndex;
                    tmp[rightTriangleIndex + 1] = topRightVertexIndex;
                    tmp[rightTriangleIndex + 2] = topLeftVertexIndex;
                }
            }

            return tmp;
        }

        #endregion
    }
}