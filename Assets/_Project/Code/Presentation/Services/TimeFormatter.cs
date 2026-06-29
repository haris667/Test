using System;
using System.Globalization;
using Mirra.Application.Configs;
using Mirra.Domain.Models;

namespace Mirra.Presentation.Services
{
    public class TimeFormatter : ITimeFormatter
    {
        private readonly ITimeDisplayConfig _config;

        public TimeFormatter(ITimeDisplayConfig config)
        {
            _config = config;
        }

        public string Format(TimeData time)
            => time.UtcTime.ToLocalTime().ToString(_config.DisplayFormat, CultureInfo.InvariantCulture);

        public bool TryParse(string input, out TimeData result)
        {
            if (DateTime.TryParseExact(
                    input?.Trim(),
                    _config.ParseFormats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var local))
            {
                var today = DateTime.Today;
                var combined = new DateTime(
                    today.Year, today.Month, today.Day,
                    local.Hour, local.Minute, local.Second,
                    DateTimeKind.Local);

                result = new TimeData(combined.ToUniversalTime());
                return true;
            }

            result = default;
            return false;
        }
    }
}
