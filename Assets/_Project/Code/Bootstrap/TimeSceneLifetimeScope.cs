using Mirra.Application.Abstractions;
using Mirra.Application.Configs;
using Mirra.Application.UseCases;
using Mirra.Infrastructure.Assets;
using Mirra.Infrastructure.Configs;
using Mirra.Infrastructure.REST;
using Mirra.Presentation.Loading;
using Mirra.Presentation.MVP.Time;
using Mirra.Presentation.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Mirra.Bootstrap
{
    public class TimeSceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private TimeView _timeView;
        [SerializeField] private ClockView _clockView;
        [SerializeField] private LoadingOverlay _loadingOverlay;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigs(builder);
            RegisterRepositories(builder);
            RegisterUseCases(builder);
            RegisterServices(builder);
            RegisterViews(builder);
            RegisterPresenters(builder);
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {
            var provider = Parent.Container.Resolve<IAssetProvider>();

            builder.RegisterInstance(provider.LoadSync<TimeApiConfigSO>(AssetAddresses.Configs.TimeApi))
                   .As<ITimeApiConfig>();

            builder.RegisterInstance(provider.LoadSync<TimeDisplayConfigSO>(AssetAddresses.Configs.TimeDisplay))
                   .As<ITimeDisplayConfig>();
        }

        private static void RegisterRepositories(IContainerBuilder builder)
        {
            builder.Register<TimeRestRepository>(Lifetime.Singleton).As<ITimeRepository>();
        }

        private static void RegisterUseCases(IContainerBuilder builder)
        {
            builder.Register<GetCurrentTimeUseCase>(Lifetime.Scoped);
            builder.Register<SetTimeUseCase>(Lifetime.Scoped);
        }

        private static void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<TimeFormatter>(Lifetime.Singleton).As<ITimeFormatter>();
            builder.Register<LocalClock>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterViews(IContainerBuilder builder)
        {
            builder.RegisterInstance(_timeView).As<ITimeView>();
            builder.RegisterInstance(_clockView).As<IClockView>();
            builder.RegisterInstance(_loadingOverlay).AsImplementedInterfaces();
            builder.RegisterBuildCallback(c => c.Inject(_loadingOverlay));
        }

        private static void RegisterPresenters(IContainerBuilder builder)
        {
            builder.Register<TimePresenter>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<ClockPresenter>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
        }
    }
}
