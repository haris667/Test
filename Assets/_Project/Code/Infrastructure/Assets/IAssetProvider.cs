using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mirra.Infrastructure.Assets
{
    public interface IAssetProvider : IDisposable
    {
        UniTask<T> LoadAsync<T>(string address, CancellationToken ct = default);

        // For use only in LifetimeScope.Configure() where async is unavailable.
        // If the asset was already preloaded via LoadAsync, returns from cache with no blocking.
        T LoadSync<T>(string address);

        void Release(string address);
    }
}
