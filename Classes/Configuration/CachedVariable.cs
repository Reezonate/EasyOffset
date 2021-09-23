using System;

namespace EasyOffset.Configuration {
    public class CachedVariable<T> {
        private Func<T> _actualValueGetter;
        private bool _isNull = true;
        private T _value;

        public CachedVariable(Func<T> actualValueGetter) {
            _actualValueGetter = actualValueGetter;
        }

        public T Value {
            get {
                if (!_isNull) return _value;

                _value = _actualValueGetter.Invoke();
                _actualValueGetter = null;
                _isNull = false;
                return _value;
            }
            set => _value = value;
        }
    }
}