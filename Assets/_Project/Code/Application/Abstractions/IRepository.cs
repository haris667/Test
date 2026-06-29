using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mirra.Application.Abstractions
{
    public interface IRepository<T>
    {
        UniTask<T> GetAsync(string id, CancellationToken ct = default);
        UniTask<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
        UniTask SaveAsync(T entity, CancellationToken ct = default);
        UniTask DeleteAsync(string id, CancellationToken ct = default);
    }
}
