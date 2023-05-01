using System;
using UnityEngine;

namespace EasyOffset;

[Serializable]
internal struct ConfigFileQuaternion {
    public float x;
    public float y;
    public float z;
    public float w;

    public static ConfigFileQuaternion FromUnityQuaternion(Quaternion unityQuaternion) {
        return new ConfigFileQuaternion {
            x = unityQuaternion.x,
            y = unityQuaternion.y,
            z = unityQuaternion.z,
            w = unityQuaternion.w
        };
    }

    public Quaternion ToUnityQuaternion() {
        return new Quaternion(x, y, z, w);
    }
}