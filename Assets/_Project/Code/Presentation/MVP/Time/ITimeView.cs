using System;
using Mirra.Presentation.MVP.Base;

namespace Mirra.Presentation.MVP.Time
{
    public interface ITimeView : IView
    {
        event Action OnRefreshRequested;
        event Action<int, int> OnTimeChangeRequested;

        void DisplayTime(string formattedTime);
        void ShowLoading();
        void ShowError(string message);
    }
}
