using System;
using Mirra.Presentation.MVP.Base;
using UnityEngine;
using UTime = UnityEngine.Time;

namespace Mirra.Presentation.MVP.Time
{
    public class ClockView : ViewBase<ClockPresenter>, IClockView
    {
        [Header("Clock Hands")]
        [SerializeField] private Transform _hourHand;
        [SerializeField] private Transform _minuteHand;
        [SerializeField] private Transform _secondHand;

        [Header("Mouse Tracking")]
        [SerializeField] private float _maxTiltAngle = 12f;
        [SerializeField] private float _followSpeed = 6f;

        public event Action<ClockHandType, float> OnHandDragged;

        private RectTransform _rectTransform;

        protected virtual void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            foreach (var handler in GetComponentsInChildren<TimeLineHandler>())
                handler.OnAngleChanged += (type, angle) => OnHandDragged?.Invoke(type, angle);
        }

        private void LateUpdate()
        {
            float normX = (Input.mousePosition.x - Screen.width  * 0.5f) / (Screen.width  * 0.5f);
            float normY = (Input.mousePosition.y - Screen.height * 0.5f) / (Screen.height * 0.5f);

            Quaternion target = Quaternion.Euler(normY * _maxTiltAngle, -normX * _maxTiltAngle, 0f);

            _rectTransform.localRotation = Quaternion.Lerp(
                _rectTransform.localRotation,
                target,
                UTime.deltaTime * _followSpeed);
        }

        public void SetHourHandAngle(float degrees)
        {
            if (_hourHand != null)
                _hourHand.localEulerAngles = new Vector3(0f, 0f, -degrees);
        }

        public void SetMinuteHandAngle(float degrees)
        {
            if (_minuteHand != null)
                _minuteHand.localEulerAngles = new Vector3(0f, 0f, -degrees);
        }

        public void SetSecondHandAngle(float degrees)
        {
            if (_secondHand != null)
                _secondHand.localEulerAngles = new Vector3(0f, 0f, -degrees);
        }
    }
}
