using Mirra.Application.Abstractions;
using Mirra.Application.Configs;
using Mirra.Application.UseCases;
using Mirra.Infrastructure.Assets;
using Mirra.Infrastructure.Configs;
using Mirra.Infrastructure.Scene;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Mirra.Bootstrap
{
    public class ProjectLifetimeScope : LifetimeScope
    {
        private AddressableAssetProvider _provider;

        protected override void Configure(IContainerBuilder builder)
        {
            _provider = new AddressableAssetProvider();

            var bootstrapConfig = _provider.LoadSync<GameBootstrapConfigSO>(AssetAddresses.Configs.GameBootstrap);

            builder.RegisterInstance(_provider).As<IAssetProvider>();
            builder.RegisterInstance(bootstrapConfig).As<IGameBootstrapConfig>();

            builder.Register<SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();
            builder.Register<LoadSceneUseCase>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameBootstrapper>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _provider?.Dispose();
        }
    }
}
