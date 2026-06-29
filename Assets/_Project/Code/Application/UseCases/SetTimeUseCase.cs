using System.Threading;
using Cysharp.Threading.Tasks;
using Mirra.Application.Abstractions;
using Mirra.Domain.Models;

namespace Mirra.Application.UseCases
{
    public class SetTimeUseCase
    {
        private readonly ITimeRepository _repository;

        public SetTimeUseCase(ITimeRepository repository)
        {
            _repository = repository;
        }

        public UniTask ExecuteAsync(TimeData time, CancellationToken ct = default)
            => _repository.SetTimeAsync(time, ct);
    }
}
