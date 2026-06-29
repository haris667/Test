using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mirra.Application.UseCases
{
    public interface IUseCase<in TInput, TOutput>
    {
        UniTask<TOutput> ExecuteAsync(TInput input, CancellationToken ct = default);
    }

    public interface IUseCase<in TInput>
    {
        UniTask ExecuteAsync(TInput input, CancellationToken ct = default);
    }
}
