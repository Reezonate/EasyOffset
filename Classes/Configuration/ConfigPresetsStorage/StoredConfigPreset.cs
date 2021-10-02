using System;
using System.IO;

namespace EasyOffset.Configuration {
    public class StoredConfigPreset : IComparable<StoredConfigPreset> {
        public readonly string AbsoluteFilePath;
        public readonly bool LoadFailed;
        public readonly IConfigPreset ConfigPreset;

        public string Name => Path.GetFileName(AbsoluteFilePath).Replace(".json", "");

        public StoredConfigPreset(
            string absoluteFilePath,
            bool loadFailed,
            IConfigPreset configPreset
        ) {
            AbsoluteFilePath = absoluteFilePath;
            LoadFailed = loadFailed;
            ConfigPreset = configPreset;
        }

        public int CompareTo(StoredConfigPreset other) {
            if (LoadFailed) return -1;
            return other.LoadFailed ? 1 : ConfigPreset.UnixTimestamp.CompareTo(other.ConfigPreset.UnixTimestamp);
        }
    }
}