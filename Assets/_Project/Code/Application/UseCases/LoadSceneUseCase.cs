using System.Threading;
using Cysharp.Threading.Tasks;
using Mirra.Application.Abstractions;
using Mirra.Domain.Models;

namespace Mirra.Application.UseCases
{
    public class LoadSceneUseCase
    {
        private readonly ISceneLoader _sceneLoader;

        public LoadSceneUseCase(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public UniTask ExecuteAsync(LoadSceneCommand command, CancellationToken ct = default)
            => _sceneLoader.LoadAsync(command, ct);
    }
}
