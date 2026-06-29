using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Mirra.Infrastructure.Assets
{
    public class AddressableAssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _handles = new();

        public async UniTask<T> LoadAsync<T>(string address, CancellationToken ct = default)
        {
            if (_handles.TryGetValue(address, out var cached))
                return (T)cached.Result;

            var handle = Addressables.LoadAssetAsync<T>(address);
            _handles[address] = handle;
            return await handle.WithCancellation(ct);
        }

        public T LoadSync<T>(string address)
        {
            if (_handles.TryGetValue(address, out var cached))
                return (T)cached.Result;

            var handle = Addressables.LoadAssetAsync<T>(address);
            var result = handle.WaitForCompletion();
            _handles[address] = handle;

            if (result == null)
                Debug.LogError($"[AssetProvider] Asset not found at address '{address}'");

            return result;
        }

        public void Release(string address)
        {
            if (!_handles.Remove(address, out var handle)) return;
            if (handle.IsValid()) Addressables.Release(handle);
        }

        public void Dispose()
        {
            foreach (var handle in _handles.Values)
                if (handle.IsValid()) Addressables.Release(handle);
            _handles.Clear();
        }
    }
}
