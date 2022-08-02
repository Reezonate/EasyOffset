using System;

namespace EasyOffset;

internal class DirectModeVariable {
    #region Constructor

    private readonly Hand _hand;
    private readonly SliderValueType _sliderValueType;
    private float _value;

    public DirectModeVariable(SliderValueType sliderValueType, Hand hand, float value = default) {
        _sliderValueType = sliderValueType;
        _hand = hand;
        _value = value;
    }

    #endregion

    #region Events

    public event Action<SliderValueType, Hand> SmoothChangeStartedEvent;
    public event Action<SliderValueType, Hand> SmoothChangeFinishedEvent;
    public event Action<SliderValueType, float> ChangedFromUIEvent;
    public event Action<float> ChangedFromCodeEvent;

    #endregion

    #region Logic

    private bool _applyOnChange = true;
    private bool _pressed;
    private bool _changed;

    public void NotifyChangeStarted() {
        _changed = false;
        _pressed = true;

        SmoothChangeStartedEvent?.Invoke(_sliderValueType, _hand);

        if (_sliderValueType == SliderValueType.ZOffset) {
            _applyOnChange = true;
        } else {
            _applyOnChange = _hand switch {
                Hand.Left => ReeInputManager.TriggerState != ReeTriggerState.LeftPressed,
                Hand.Right => ReeInputManager.TriggerState != ReeTriggerState.RightPressed,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public void NotifyChangeFinished() {
        _applyOnChange = true;
        _pressed = false;
        
        SmoothChangeFinishedEvent?.Invoke(_sliderValueType, _hand);
        if (_changed) ChangedFromUIEvent?.Invoke(_sliderValueType, _value);
    }

    public void SetValueFromUI(float value) {
        if (_value.Equals(value)) return;
        _value = value;
        _changed = true;

        if (!_pressed) {
            SmoothChangeStartedEvent?.Invoke(_sliderValueType, _hand);
            SmoothChangeFinishedEvent?.Invoke(_sliderValueType, _hand);
        }
        
        if (_applyOnChange) ChangedFromUIEvent?.Invoke(_sliderValueType, _value);
    }

    public void SetValueFromCode(float value) {
        if (_value.Equals(value)) return;
        _value = value;
        ChangedFromCodeEvent?.Invoke(value);
    }

    public float GetValue() => _value;

    #endregion
}