using System.Threading;
using UnityEngine;

namespace Mirra.Presentation.MVP.Base
{
    public abstract class ViewBase<TPresenter> : MonoBehaviour, IView
        where TPresenter : IPresenter
    {
        public CancellationToken LifetimeToken => destroyCancellationToken;

        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);
    }
}
