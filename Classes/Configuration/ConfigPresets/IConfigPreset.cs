using Newtonsoft.Json.Linq;
using UnityEngine;

namespace EasyOffset.Configuration {
    public interface IConfigPreset {
        public JObject Serialize();
        public string ConfigVersion { get; }
        public long UnixTimestamp { get; }
        public ControllerType ControllerType { get; }

        public Vector3 RightHandPivotPosition { get; }
        public Vector3 RightHandSaberDirection { get; }
        public float RightHandZOffset { get; }

        public Vector3 LeftHandPivotPosition { get; }
        public Vector3 LeftHandSaberDirection { get; }
        public float LeftHandZOffset { get; }
    }
}