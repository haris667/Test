using System;
using Mirra.Presentation.MVP.Base;

namespace Mirra.Presentation.MVP.Time
{
    public interface IClockView : IView
    {
        event Action<ClockHandType, float> OnHandDragged;

        void SetHourHandAngle(float degrees);
        void SetMinuteHandAngle(float degrees);
        void SetSecondHandAngle(float degrees);
    }
}
