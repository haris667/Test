using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mirra.Application.Abstractions;
using Mirra.Domain.Models;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Mirra.Infrastructure.Scene
{
    public class SceneLoader : ISceneLoader, IDisposable
    {
        // Cancelled only when a newer LoadAsync supersedes the current one (race guard).
        // NOT cancelled in Dispose — Unity cannot cleanly abort a scene load mid-flight.
        private CancellationTokenSource _raceCts = new();

        public async UniTask LoadAsync(LoadSceneCommand command, CancellationToken ct = default)
        {
            CancelRace();

            // ct controls the delay only — external lifecycle tokens (VContainer scope)
            // should not reach into the actual scene load.
            using var delayLinked = CancellationTokenSource.CreateLinkedTokenSource(_raceCts.Token, ct);

            if (command.Delay > TimeSpan.Zero)
                await UniTask.Delay(command.Delay, cancellationToken: delayLinked.Token);

            var unityMode = command.Mode == SceneLoadMode.Additive
                ? LoadSceneMode.Additive
                : LoadSceneMode.Single;

            await Addressables
                .LoadSceneAsync(command.SceneName, unityMode)
                .ToUniTask(cancellationToken: _raceCts.Token);
        }

        private void CancelRace()
        {
            _raceCts.Cancel();
            _raceCts.Dispose();
            _raceCts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _raceCts?.Dispose();
        }
    }
}
