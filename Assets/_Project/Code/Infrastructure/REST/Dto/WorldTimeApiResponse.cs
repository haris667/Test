using System;

namespace Mirra.Infrastructure.REST.Dto
{
    [Serializable]
    public class WorldTimeApiResponse
    {
        public long unixtime;       // worldtimeapi.org  — http://worldtimeapi.org/api/timezone/UTC
        public string dateTime;     // timeapi.io        — https://timeapi.io/api/time/current/zone?timeZone=UTC
    }
}
