using Cysharp.Threading.Tasks;
using Mirra.Application.Abstractions;
using Mirra.Application.Configs;
using Mirra.Application.UseCases;
using Mirra.Infrastructure.Assets;
using Mirra.Infrastructure.Configs;
using Mirra.Infrastructure.Scene;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VContainer.Unity;

namespace Mirra.Bootstrap
{
    public class ProjectLifetimeScope : LifetimeScope
    {
        private AddressableAssetProvider _provider;
        private GameBootstrapConfigSO _bootstrapConfig;
        private AsyncOperationHandle<GameBootstrapConfigSO> _bootstrapHandle;

        protected override void Awake()
        {
            // Skip base.Awake() — synchronous Build() calls WaitForCompletion, forbidden on WebGL.
            LoadAndBuildAsync().Forget();
        }

        private async UniTaskVoid LoadAndBuildAsync()
        {
            _provider = new AddressableAssetProvider();

            _bootstrapHandle = Addressables.LoadAssetAsync<GameBootstrapConfigSO>(AssetAddresses.Configs.GameBootstrap);
            _bootstrapConfig = await _bootstrapHandle.Task.AsUniTask();

            if (this == null) return;

            Build();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_provider).As<IAssetProvider>();
            builder.RegisterInstance(_bootstrapConfig).As<IGameBootstrapConfig>();

            builder.Register<SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();
            builder.Register<LoadSceneUseCase>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameBootstrapper>();
        }

        protected override void OnDestroy()
        {
            if (_bootstrapHandle.IsValid()) Addressables.Release(_bootstrapHandle);
            base.OnDestroy();
            _provider?.Dispose();
        }
    }
}
