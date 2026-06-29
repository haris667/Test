using System.Threading;
using Cysharp.Threading.Tasks;
using Mirra.Application.Abstractions;
using Mirra.Domain.Models;

namespace Mirra.Application.UseCases
{
    public class GetCurrentTimeUseCase
    {
        private readonly ITimeRepository _repository;

        public GetCurrentTimeUseCase(ITimeRepository repository)
        {
            _repository = repository;
        }

        public UniTask<TimeData> ExecuteAsync(CancellationToken ct = default)
            => _repository.GetCurrentTimeAsync(ct);
    }
}
