using System.Threading;
using Cysharp.Threading.Tasks;
using Mirra.Domain.Models;

namespace Mirra.Application.Abstractions
{
    public interface ITimeRepository
    {
        UniTask<TimeData> GetCurrentTimeAsync(CancellationToken ct = default);
        UniTask SetTimeAsync(TimeData time, CancellationToken ct = default);
    }
}
