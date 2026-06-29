using System.Threading;
using Cysharp.Threading.Tasks;
using Mirra.Domain.Models;

namespace Mirra.Application.Abstractions
{
    public interface ISceneLoader
    {
        // New call cancels any in-flight load automatically.
        UniTask LoadAsync(LoadSceneCommand command, CancellationToken ct = default);
    }
}
