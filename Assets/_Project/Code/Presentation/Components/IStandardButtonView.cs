using System;
using Mirra.Presentation.MVP.Base;

namespace Mirra.Presentation.Components
{
    public interface IStandardButtonView : IView
    {
        event Action OnClicked;
    }
}
