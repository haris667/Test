using System.Threading;
using VContainer.Unity;

namespace Mirra.Presentation.MVP.Base
{
    public abstract class PresenterBase<TView> : IPresenter, IStartable
        where TView : IView
    {
        protected readonly TView View;
        protected CancellationToken CancellationToken { get; private set; }

        private CancellationTokenSource _cts;
        private CancellationTokenSource _linkedCts;
        private bool _disposed;

        protected PresenterBase(TView view)
        {
            View = view;
        }

        void IStartable.Start()
        {
            _cts = new CancellationTokenSource();
            _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, View.LifetimeToken);
            CancellationToken = _linkedCts.Token;
            OnInitialize();
        }

        protected abstract void OnInitialize();

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _cts?.Cancel();
            _cts?.Dispose();
            _linkedCts?.Dispose();
            OnDispose();
        }

        protected virtual void OnDispose() { }
    }
}
