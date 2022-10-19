using UnityEngine;
using UnityEngine.Rendering;

namespace EasyOffset {
    public static class VFXUtils {
        #region ReleaseBuffer

        public static void ReleaseBuffer(ref ComputeBuffer buffer) {
            buffer.Release();
            buffer = null;
        }

        public static void ReleaseBuffer(ref CommandBuffer buffer) {
            buffer.Release();
            buffer = null;
        }

        public static void ReleaseBuffer(ref RenderTexture buffer) {
            buffer.Release();
            buffer = null;
        }

        #endregion

        #region DispatchCompute

        public static void DispatchComputeX64(
            this CommandBuffer commandBuffer,
            ComputeShader computeShader,
            int kernelId,
            int xCount
        ) {
            commandBuffer.DispatchCompute(computeShader,
                kernelId,
                GetGroupCount(xCount, 64),
                1,
                1
            );
        }

        public static void DispatchComputeXY8(
            this CommandBuffer commandBuffer,
            ComputeShader computeShader,
            int kernelId,
            int xCount,
            int yCount
        ) {
            commandBuffer.DispatchCompute(computeShader,
                kernelId,
                GetGroupCount(xCount, 8),
                GetGroupCount(yCount, 8),
                1
            );
        }

        public static void DispatchComputeXYZ4(
            this CommandBuffer commandBuffer,
            ComputeShader computeShader,
            int kernelId,
            int xCount,
            int yCount,
            int zCount
        ) {
            commandBuffer.DispatchCompute(computeShader,
                kernelId,
                GetGroupCount(xCount, 4),
                GetGroupCount(yCount, 4),
                GetGroupCount(zCount, 4)
            );
        }

        #endregion

        #region GetGroupCount

        public static int GetGroupCount(int targetCount, int kernelCount) {
            return Mathf.CeilToInt((float) targetCount / kernelCount);
        }

        #endregion
    }
}