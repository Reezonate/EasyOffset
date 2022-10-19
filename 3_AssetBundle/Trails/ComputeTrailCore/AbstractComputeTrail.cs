using UnityEngine;
using UnityEngine.Rendering;

namespace EasyOffset {
    internal abstract class AbstractComputeTrail : MonoBehaviour {
        #region Serialized

        [SerializeField] private ComputeShader library;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private int horizontalResolution = 1;
        [SerializeField] private int verticalResolution = 10;
        [SerializeField] private int trailLength = 20;

        protected int Lifetime {
            get => trailLength;
            set {
                if (trailLength.Equals(value)) return;
                trailLength = value;
                HardReset();
            }
        }

        #endregion

        #region KernelsID

        private int _shiftKernelId;
        private int _addNewNodeKernelId;
        private int _recalculateNodesKernelId;

        private void FindKernels() {
            _shiftKernelId = library.FindKernel("shift");
            _addNewNodeKernelId = library.FindKernel("add_new_node");
            _recalculateNodesKernelId = library.FindKernel("recalculate_nodes");
        }

        #endregion

        #region ShaderProperties

        private static readonly int ShiftIndexPropertyId = Shader.PropertyToID("_ShiftIndex");
        private static readonly int TrailsCountPropertyId = Shader.PropertyToID("_TrailsCount");
        private static readonly int NodesPerTrailPropertyId = Shader.PropertyToID("_NodesPerTrail");
        private static readonly int TotalNodesPropertyId = Shader.PropertyToID("_TotalNodes");
        private static readonly int CurvedSegmentsCountPropertyId = Shader.PropertyToID("_CurvedSegmentsCount");

        private static readonly int PositionPropertyId = Shader.PropertyToID("_Position");
        private static readonly int Data0PropertyId = Shader.PropertyToID("_Data0");
        private static readonly int Data1PropertyId = Shader.PropertyToID("_Data1");
        private static readonly int CurvedCountPropertyId = Shader.PropertyToID("_CurvedCount");
        private static readonly int NodesBufferPropertyId = Shader.PropertyToID("_NodesBuffer");
        private static readonly int SplineBufferPropertyId = Shader.PropertyToID("_SplineBuffer");

        private const int TrailsCount = 1;
        private const int NodesPerLinear = 2;
        private const int NodesPerHandles = 2;
        private const int NodesPerCurved = 4;
        private const int SplineNodeStride = sizeof(float) * (3 + 4 + 4); //pos_3 + rot_4 + data0_4

        private int NodesPerTrail => verticalResolution + 1;
        private int TotalNodes => TrailsCount * NodesPerTrail;
        protected int CurvedSegmentsCount => trailLength;
        private int SplineBufferSize => TrailsCount * (NodesPerLinear + NodesPerHandles + NodesPerCurved * CurvedSegmentsCount);

        protected MaterialPropertyBlock MaterialPropertyBlock;

        private void InitializeRendering() {
            MaterialPropertyBlock = new MaterialPropertyBlock();
            MaterialPropertyBlock.SetBuffer(NodesBufferPropertyId, _nodesBuffer);
            meshFilter.mesh = ComputeTrailMesh.CreateNewMesh(horizontalResolution, verticalResolution);
            ApplyMaterial();
        }

        protected void ApplyMaterial() {
            meshRenderer.SetPropertyBlock(MaterialPropertyBlock);
        }

        #endregion

        #region Buffers

        private ComputeBuffer _nodesBuffer;
        private ComputeBuffer _splineBuffer;
        private CommandBuffer _updateCommandBuffer;

        private void InitializeBuffers() {
            _nodesBuffer = new ComputeBuffer(TotalNodes, SplineNodeStride);
            _splineBuffer = new ComputeBuffer(SplineBufferSize, SplineNodeStride);
            _updateCommandBuffer = BuildUpdateCommandBuffer();
        }

        private void ReleaseBuffers() {
            VFXUtils.ReleaseBuffer(ref _nodesBuffer);
            VFXUtils.ReleaseBuffer(ref _splineBuffer);
            VFXUtils.ReleaseBuffer(ref _updateCommandBuffer);
        }

        #endregion

        #region BuildUpdateCommandBuffer

        private CommandBuffer BuildUpdateCommandBuffer() {
            var commandBuffer = new CommandBuffer();

            commandBuffer.SetExecutionFlags(CommandBufferExecutionFlags.AsyncCompute);

            SetParams(commandBuffer);

            for (var curvedSegmentIndex = CurvedSegmentsCount - 1; curvedSegmentIndex > 0; curvedSegmentIndex--) {
                commandBuffer.SetComputeIntParam(library, ShiftIndexPropertyId, curvedSegmentIndex);
                commandBuffer.DispatchComputeX64(library, _shiftKernelId, TrailsCount);
            }

            commandBuffer.DispatchComputeX64(library, _addNewNodeKernelId, TrailsCount);
            commandBuffer.DispatchComputeX64(library, _recalculateNodesKernelId, TotalNodes);

            return commandBuffer;
        }

        private void SetParams(CommandBuffer commandBuffer) {
            //global params
            commandBuffer.SetComputeIntParam(library, TrailsCountPropertyId, TrailsCount);
            commandBuffer.SetComputeIntParam(library, NodesPerTrailPropertyId, NodesPerTrail);
            commandBuffer.SetComputeIntParam(library, TotalNodesPropertyId, TotalNodes);
            commandBuffer.SetComputeIntParam(library, CurvedSegmentsCountPropertyId, CurvedSegmentsCount);

            //shift
            commandBuffer.SetComputeBufferParam(library, _shiftKernelId, SplineBufferPropertyId, _splineBuffer);

            //add_new_node
            commandBuffer.SetComputeBufferParam(library, _addNewNodeKernelId, SplineBufferPropertyId, _splineBuffer);

            //recalculate_nodes
            commandBuffer.SetComputeBufferParam(library, _recalculateNodesKernelId, NodesBufferPropertyId, _nodesBuffer);
            commandBuffer.SetComputeBufferParam(library, _recalculateNodesKernelId, SplineBufferPropertyId, _splineBuffer);
        }

        #endregion

        #region Initialize & Release

        protected bool Initialized { get; private set; }

        private void Initialize() {
            Release();

            FindKernels();
            InitializeBuffers();
            InitializeRendering();
            InitializeSpline();

            Initialized = true;
        }

        private void Release() {
            if (!Initialized) return;
            ReleaseBuffers();
            Initialized = false;
        }

        private void InitializeSpline() {
            _curvedSegmentsAdded = -2;
        }

        #endregion

        #region AddNewNode

        private int _curvedSegmentsAdded;

        protected void AddNewNode(ComputeTrailNode node) {
            if (!Initialized) return;
            SetInputNode(node);
            Graphics.ExecuteCommandBufferAsync(_updateCommandBuffer, ComputeQueueType.Default);
            if (_curvedSegmentsAdded < CurvedSegmentsCount) _curvedSegmentsAdded += 1;
        }

        private void SetInputNode(ComputeTrailNode node) {
            library.SetVector(PositionPropertyId, node.Position);
            library.SetVector(Data0PropertyId, node.Data0);
            library.SetVector(Data1PropertyId, node.Data1);
            library.SetInt(CurvedCountPropertyId, _curvedSegmentsAdded);
        }

        #endregion

        #region Events

        protected void HardReset() {
            Initialize();
        }

        private void OnDestroy() {
            Release();
        }

        #endregion
    }
}