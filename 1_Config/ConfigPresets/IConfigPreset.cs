using Newtonsoft.Json.Linq;
using UnityEngine;

namespace EasyOffset {
    public interface IConfigPreset {
        public JObject Serialize();
        public string ConfigVersion { get; }
        public long UnixTimestamp { get; }
        public ControllerType ControllerType { get; }

        public Vector3 LeftSaberPivotPosition { get; }
        public Quaternion LeftSaberRotation { get; }
        public float LeftSaberZOffset { get; }

        public Vector3 RightSaberPivotPosition { get; }
        public Quaternion RightSaberRotation { get; }
        public float RightSaberZOffset { get; }
    }
}