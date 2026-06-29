using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mirra.Application.Abstractions;
using Mirra.Application.Configs;
using Mirra.Domain.Models;
using Mirra.Infrastructure.REST.Dto;
using UnityEngine;

namespace Mirra.Infrastructure.REST
{
    public class TimeRestRepository : ITimeRepository, IDisposable
    {
        private readonly HttpClient _http;
        private readonly ITimeApiConfig _config;

        public TimeRestRepository(ITimeApiConfig config)
        {
            _config = config;

            // Unity Editor runs on Mono. ServicePointManager is the correct hook
            // for bypassing SSL validation in Mono's HTTP stack.
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = (_, _, _, _) => true;

            _http = new HttpClient();
        }

        public async UniTask<TimeData> GetCurrentTimeAsync(CancellationToken ct = default)
        {
            using var response = await _http.GetAsync(_config.ApiUrl, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonUtility.FromJson<WorldTimeApiResponse>(json);
            return new TimeData(ParseUtc(dto));
        }

        public UniTask SetTimeAsync(TimeData time, CancellationToken ct = default)
            => UniTask.CompletedTask;

        public void Dispose() => _http.Dispose();

        private static DateTime ParseUtc(WorldTimeApiResponse dto)
        {
            if (dto.unixtime != 0)
                return DateTimeOffset.FromUnixTimeSeconds(dto.unixtime).UtcDateTime;

            return DateTime.SpecifyKind(
                DateTime.Parse(dto.dateTime, System.Globalization.CultureInfo.InvariantCulture),
                DateTimeKind.Utc);
        }
    }
}
