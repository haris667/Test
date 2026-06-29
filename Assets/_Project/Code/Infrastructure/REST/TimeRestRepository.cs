using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mirra.Application.Abstractions;
using Mirra.Application.Configs;
using Mirra.Domain.Models;
using Mirra.Infrastructure.REST.Dto;
using UnityEngine;
using UnityEngine.Networking;

namespace Mirra.Infrastructure.REST
{
    public class TimeRestRepository : ITimeRepository
    {
        private readonly ITimeApiConfig _config;

        public TimeRestRepository(ITimeApiConfig config)
        {
            _config = config;
        }

        public async UniTask<TimeData> GetCurrentTimeAsync(CancellationToken ct = default)
        {
            using var request = UnityWebRequest.Get(_config.ApiUrl);
            await request.SendWebRequest().WithCancellation(ct);

            if (request.result != UnityWebRequest.Result.Success)
                throw new Exception($"HTTP error: {request.error}");

            var dto = JsonUtility.FromJson<WorldTimeApiResponse>(request.downloadHandler.text);
            return new TimeData(ParseUtc(dto));
        }

        public UniTask SetTimeAsync(TimeData time, CancellationToken ct = default)
            => UniTask.CompletedTask;

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
