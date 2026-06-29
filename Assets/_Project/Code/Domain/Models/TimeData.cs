using System;

namespace Mirra.Domain.Models
{
    public readonly struct TimeData
    {
        public DateTime UtcTime { get; }

        public TimeData(DateTime utcTime)
        {
            UtcTime = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
        }
    }
}
