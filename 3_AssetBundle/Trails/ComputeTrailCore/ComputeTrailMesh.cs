using System;
using UnityEngine;

namespace EasyOffset {
    public class ComputeTrailMesh {
        #region CreateNewMesh

        public static Mesh CreateNewMesh(int horizontalResolution, int verticalResolution) {
            return new ComputeTrailMesh(horizontalResolution, verticalResolution).CreateMesh();
        }

        #endregion

        #region Constructor

        private const int MaxVerticesPerMesh = 65536;

        private readonly int _horizontalResolution;
        private readonly int _verticalResolution;
        private readonly int _columnsCount;
        private readonly int _rowsCount;
        private readonly int _verticesCount;

        private ComputeTrailMesh(int horizontalResolution, int verticalResolution) {
            _horizontalResolution = horizontalResolution;
            _verticalResolution = verticalResolution;
            _columnsCount = horizontalResolution + 1;
            _rowsCount = verticalResolution + 1;
            _verticesCount = _columnsCount * _rowsCount;
            if (_verticesCount > MaxVerticesPerMesh) throw new Exception("Too many vertices!");
        }

        #endregion

        #region CreateMesh

        private Mesh CreateMesh() {
            CreateVertexDataArrays(out var uv0, out var uv1);

            return new Mesh {
                vertices = new Vector3[_verticesCount],
                triangles = CreateTrianglesArray(),
                uv = uv0,
                uv2 = uv1,
                bounds = new Bounds(Vector3.zero, Vector3.one * 50)
            };
        }

        #endregion

        #region CreateVertexDataArrays

        private void CreateVertexDataArrays(out Vector2[] uv0, out Vector2[] uv1) {
            uv0 = new Vector2[_verticesCount];
            uv1 = new Vector2[_verticesCount];

            var vertexIndex = 0;
            for (var rowIndex = 0; rowIndex < _rowsCount; rowIndex++) {
                var verticalRatio = (float) rowIndex / _verticalResolution;
                for (var columnIndex = 0; columnIndex < _columnsCount; columnIndex++) {
                    var horizontalRatio = (float) columnIndex / _horizontalResolution;
                    uv0[vertexIndex] = new Vector2(horizontalRatio, verticalRatio);
                    uv1[vertexIndex] = new Vector2(rowIndex, 0);
                    vertexIndex += 1;
                }
            }
        }

        #endregion

        #region CreateTrianglesArray

        private int[] CreateTrianglesArray() {
            var quadCount = _horizontalResolution * _verticalResolution;
            var trianglesArray = new int[quadCount * 6];

            for (var rowIndex = 0; rowIndex < _verticalResolution; rowIndex++) {
                for (var columnIndex = 0; columnIndex < _horizontalResolution; columnIndex++) {
                    var quadIndex = rowIndex * _horizontalResolution + columnIndex;

                    var topLeftVertexIndex = GetVertexIndex(columnIndex, rowIndex);
                    var topRightVertexIndex = GetVertexIndex(columnIndex + 1, rowIndex);
                    var bottomLeftVertexIndex = GetVertexIndex(columnIndex, rowIndex + 1);
                    var bottomRightVertexIndex = GetVertexIndex(columnIndex + 1, rowIndex + 1);

                    var leftTriangleIndex = quadIndex * 6;
                    trianglesArray[leftTriangleIndex + 0] = bottomRightVertexIndex;
                    trianglesArray[leftTriangleIndex + 1] = bottomLeftVertexIndex;
                    trianglesArray[leftTriangleIndex + 2] = topLeftVertexIndex;

                    var rightTriangleIndex = leftTriangleIndex + 3;
                    trianglesArray[rightTriangleIndex + 0] = topLeftVertexIndex;
                    trianglesArray[rightTriangleIndex + 1] = topRightVertexIndex;
                    trianglesArray[rightTriangleIndex + 2] = bottomRightVertexIndex;
                }
            }

            return trianglesArray;
        }

        private int GetVertexIndex(int columnIndex, int rowIndex) {
            return rowIndex * _columnsCount + columnIndex;
        }

        #endregion
    }
}