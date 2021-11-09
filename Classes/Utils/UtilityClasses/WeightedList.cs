using System.Collections.Generic;

namespace EasyOffset {
    public class WeightedList<T> {
        #region Constructor

        private readonly int _maximalCapacity;
        public readonly List<Entry<T>> Entries = new();

        public WeightedList(int maximalCapacity) {
            _maximalCapacity = maximalCapacity;
        }

        #endregion

        #region Interface

        public void Clear() {
            Entries.Clear();
        }

        public void Add(T position, float weight) {
            Entries.Add(new Entry<T>(position, weight));
            if (Entries.Count > _maximalCapacity) Entries.RemoveAt(0);
        }

        #endregion

        #region Entry

        public readonly struct Entry<T2> {
            public readonly T2 Value;
            public readonly float Weight;

            public Entry(
                T2 value,
                float weight
            ) {
                Value = value;
                Weight = weight;
            }
        }

        #endregion
    }
}