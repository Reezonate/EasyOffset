using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace EasyOffset;

public static class JsonUtils {
    [NotNull]
    public static T GetValueUnsafe<T>(this JObject jObject, string key) {
        return jObject.GetValue(key, StringComparison.OrdinalIgnoreCase)!.Value<T>();
    }
}