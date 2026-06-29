using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mirra.Domain.Models;
using VContainer.Unity;

namespace Mirra.Presentation.Services
{
    public class LocalClock : ILocalClock, IStartable, IDisposable
    {
        public event Action<TimeData> OnTick;

        private DateTime _syncedUtc = DateTime.UtcNow;
        private DateTime _syncPoint = DateTime.UtcNow;
        private CancellationTokenSource _cts;

        public void Sync(TimeData time)
        {
            _syncedUtc = time.UtcTime;
            _syncPoint = DateTime.UtcNow;
            OnTick?.Invoke(GetCurrent());
        }

        void IStartable.Start()
        {
            _cts = new CancellationTokenSource();
            TickLoopAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid TickLoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await UniTask.Delay(1000, cancellationToken: ct);
                OnTick?.Invoke(GetCurrent());
            }
        }

        private TimeData GetCurrent()
        {
            var elapsed = DateTime.UtcNow - _syncPoint;
            return new TimeData(_syncedUtc + elapsed);
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
