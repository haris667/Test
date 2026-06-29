using System.Threading;
using Cysharp.Threading.Tasks;
using Mirra.Application.Configs;
using Mirra.Application.UseCases;
using Mirra.Domain.Models;
using Mirra.Infrastructure.Assets;
using Mirra.Infrastructure.Configs;
using VContainer.Unity;

namespace Mirra.Bootstrap
{
    public class GameBootstrapper : IAsyncStartable
    {
        private readonly LoadSceneUseCase _loadScene;
        private readonly IGameBootstrapConfig _config;
        private readonly IAssetProvider _assetProvider;

        public GameBootstrapper(
            LoadSceneUseCase loadScene,
            IGameBootstrapConfig config,
            IAssetProvider assetProvider)
        {
            _loadScene = loadScene;
            _config = config;
            _assetProvider = assetProvider;
        }

        public async UniTask StartAsync(CancellationToken ct)
        {
            await LoadInitialSceneAsync(ct);
        }

        private async UniTask LoadInitialSceneAsync(CancellationToken ct)
        {
            var command = new LoadSceneCommand(
                sceneName: _config.InitialSceneName,
                delay: _config.TransitionDelay,
                mode: SceneLoadMode.Single);

            await _loadScene.ExecuteAsync(command, ct);
        }
    }
}
