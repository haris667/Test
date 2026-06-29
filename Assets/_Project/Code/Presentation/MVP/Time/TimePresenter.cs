using System;
using Cysharp.Threading.Tasks;
using Mirra.Application.UseCases;
using Mirra.Domain.Models;
using Mirra.Presentation.MVP.Base;
using Mirra.Presentation.Services;
using UnityEngine;

namespace Mirra.Presentation.MVP.Time
{
    public class TimePresenter : PresenterBase<ITimeView>
    {
        private readonly GetCurrentTimeUseCase _getTime;
        private readonly SetTimeUseCase _setTime;
        private readonly ITimeFormatter _formatter;
        private readonly ILocalClock _clock;

        private TimeData? _lastKnownTime;

        public TimePresenter(
            ITimeView view,
            GetCurrentTimeUseCase getTime,
            SetTimeUseCase setTime,
            ITimeFormatter formatter,
            ILocalClock clock) : base(view)
        {
            _getTime = getTime;
            _setTime = setTime;
            _formatter = formatter;
            _clock = clock;
        }

        protected override void OnInitialize()
        {
            View.OnRefreshRequested += HandleRefresh;
            View.OnTimeChangeRequested += HandleTimeChange;
            _clock.OnTick += OnClockTick;
            FetchTimeAsync().Forget();
        }

        private void OnClockTick(TimeData time)
        {
            _lastKnownTime = time;
            View.DisplayTime(_formatter.Format(time));
        }

        private void HandleRefresh() => FetchTimeAsync().Forget();

        private void HandleTimeChange(int hours, int minutes)
        {
            hours = Mathf.Clamp(hours, 0, 23);
            minutes = Mathf.Clamp(minutes, 0, 59);

            var baseUtc = _lastKnownTime?.UtcTime ?? DateTime.UtcNow;
            var local = baseUtc.ToLocalTime();

            var adjusted = new DateTime(
                local.Year, local.Month, local.Day,
                hours, minutes, local.Second,
                DateTimeKind.Local);

            ApplyTimeAsync(new TimeData(adjusted.ToUniversalTime())).Forget();
        }

        private async UniTaskVoid FetchTimeAsync()
        {
            View.ShowLoading();
            try
            {
                var time = await _getTime.ExecuteAsync(CancellationToken);
                _lastKnownTime = time;
                _clock.Sync(time);
            }
            catch (OperationCanceledException) { }
            catch (Exception e) { View.ShowError(e.Message); }
        }

        private async UniTaskVoid ApplyTimeAsync(TimeData time)
        {
            View.ShowLoading();
            try
            {
                await _setTime.ExecuteAsync(time, CancellationToken);
                _lastKnownTime = time;
                _clock.Sync(time);
            }
            catch (OperationCanceledException) { }
            catch (Exception e) { View.ShowError(e.Message); }
        }

        protected override void OnDispose()
        {
            View.OnRefreshRequested -= HandleRefresh;
            View.OnTimeChangeRequested -= HandleTimeChange;
            _clock.OnTick -= OnClockTick;
        }
    }
}
