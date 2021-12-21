using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace EasyOffset {
    public static class BsmlUtils {
        private static readonly Assembly BsmlAssembly = typeof(BeatSaberMarkupLanguage.Plugin).Assembly;
        private static readonly Dictionary<string, Type> AllTypes = new();

        static BsmlUtils() {
            foreach (var type in BsmlAssembly.GetTypes()) {
                AllTypes.TryAdd(type.Name, type);
            }
        }

        [CanBeNull]
        public static Type GetType(string shortName) {
            return AllTypes.ContainsKey(shortName) ? AllTypes[shortName] : null;
        }
    }
}