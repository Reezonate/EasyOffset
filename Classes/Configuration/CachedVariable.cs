using System;

namespace EasyOffset.Configuration {
    public class CachedVariable<T> {
        private Func<T> _actualValueGetter;
        private bool _hasValue;
        private T _value;

        public CachedVariable(Func<T> actualValueGetter) {
            _actualValueGetter = actualValueGetter;
        }

        public T Value {
            get {
                if (_hasValue) return _value;
                _value = _actualValueGetter.Invoke();
                _actualValueGetter = null;
                _hasValue = true;
                return _value;
            }
            set {
                _value = value;
                if (_hasValue) return;
                _actualValueGetter = null;
                _hasValue = true;
            }
        }
    }
}