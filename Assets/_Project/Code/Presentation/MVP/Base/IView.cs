using System.Threading;

namespace Mirra.Presentation.MVP.Base
{
    public interface IView
    {
        CancellationToken LifetimeToken { get; }
        void Show();
        void Hide();
    }
}
