using UnityEngine;

namespace EasyOffset {
    public static class MathUtils {
        public static Matrix4x4 MatrixSum(Matrix4x4 a, Matrix4x4 b) {
            return new Matrix4x4 {
                m00 = a.m00 + b.m00,
                m01 = a.m01 + b.m01,
                m02 = a.m02 + b.m02,
                m03 = a.m03 + b.m03,

                m10 = a.m10 + b.m10,
                m11 = a.m11 + b.m11,
                m12 = a.m12 + b.m12,
                m13 = a.m13 + b.m13,

                m20 = a.m20 + b.m20,
                m21 = a.m21 + b.m21,
                m22 = a.m22 + b.m22,
                m23 = a.m23 + b.m23,

                m30 = a.m30 + b.m30,
                m31 = a.m31 + b.m31,
                m32 = a.m32 + b.m32,
                m33 = a.m33 + b.m33
            };
        }

        public static Matrix4x4 GetOuterProduct(Vector4 a, Vector4 b) {
            return new Matrix4x4(
                new Vector4(
                    a[0] * b[0],
                    a[1] * b[0],
                    a[2] * b[0],
                    a[3] * b[0]
                ),
                new Vector4(
                    a[0] * b[1],
                    a[1] * b[1],
                    a[2] * b[1],
                    a[3] * b[1]
                ),
                new Vector4(
                    a[0] * b[2],
                    a[1] * b[2],
                    a[2] * b[2],
                    a[3] * b[2]
                ),
                new Vector4(
                    a[0] * b[3],
                    a[1] * b[3],
                    a[2] * b[3],
                    a[3] * b[3]
                )
            );
        }
    }
}