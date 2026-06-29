using System;
using Mirra.Domain.Models;
using Mirra.Presentation.MVP.Base;
using Mirra.Presentation.Services;
using UnityEngine;

namespace Mirra.Presentation.MVP.Time
{
    public class ClockPresenter : PresenterBase<IClockView>
    {
        private readonly ILocalClock _clock;
        private TimeData _currentTime;

        public ClockPresenter(IClockView view, ILocalClock clock) : base(view)
        {
            _clock = clock;
        }

        protected override void OnInitialize()
        {
            _clock.OnTick += OnClockTick;
            View.OnHandDragged += OnHandDragged;
        }

        private void OnClockTick(TimeData time)
        {
            _currentTime = time;

            var local = time.UtcTime.ToLocalTime();
            float hourAngle   = (local.Hour % 12) * 30f + local.Minute * 0.5f;
            float minuteAngle = local.Minute * 6f + local.Second * 0.1f;
            float secondAngle = local.Second * 6f;

            View.SetHourHandAngle(hourAngle);
            View.SetMinuteHandAngle(minuteAngle);
            View.SetSecondHandAngle(secondAngle);
        }

        private void OnHandDragged(ClockHandType type, float angle)
        {
            var local = _currentTime.UtcTime.ToLocalTime();

            int newHour   = local.Hour;
            int newMinute = local.Minute;

            switch (type)
            {
                case ClockHandType.Hour:
                    int h12 = Mathf.FloorToInt(angle / 30f) % 12;
                    newHour = local.Hour >= 12 ? h12 + 12 : h12;
                    break;

                case ClockHandType.Minute:
                    newMinute = Mathf.FloorToInt(angle / 6f) % 60;
                    break;
            }

            var adjusted = new DateTime(
                local.Year, local.Month, local.Day,
                newHour, newMinute, local.Second,
                DateTimeKind.Local);

            _clock.Sync(new TimeData(adjusted.ToUniversalTime()));
        }

        protected override void OnDispose()
        {
            _clock.OnTick -= OnClockTick;
            View.OnHandDragged -= OnHandDragged;
        }
    }
}
