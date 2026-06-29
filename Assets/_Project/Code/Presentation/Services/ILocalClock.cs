using System;
using Mirra.Domain.Models;

namespace Mirra.Presentation.Services
{
    public interface ILocalClock
    {
        event Action<TimeData> OnTick;
        void Sync(TimeData time);
    }
}
