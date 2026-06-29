using Cysharp.Threading.Tasks;
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
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VContainer.Unity;

namespace Mirra.Bootstrap
{
    public class TimeSceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private TimeView _timeView;
        [SerializeField] private ClockView _clockView;
        [SerializeField] private LoadingOverlay _loadingOverlay;

        private TimeApiConfigSO _timeApiConfig;
        private TimeDisplayConfigSO _timeDisplayConfig;

        private AsyncOperationHandle<TimeApiConfigSO> _timeApiHandle;
        private AsyncOperationHandle<TimeDisplayConfigSO> _timeDisplayHandle;

        protected override void Awake()
        {
            // Skip base.Awake() — it triggers synchronous Build().
            // We load configs first, then call Build() manually.
            LoadAndBuildAsync().Forget();
        }

        private async UniTaskVoid LoadAndBuildAsync()
        {
            // Load directly from Addressables — Parent is not available yet
            // because parent is resolved inside Build(), which we haven't called.
            // Use .Task to avoid conflict with Addressables' own void WithCancellation() instance method.
            _timeApiHandle     = Addressables.LoadAssetAsync<TimeApiConfigSO>(AssetAddresses.Configs.TimeApi);
            _timeDisplayHandle = Addressables.LoadAssetAsync<TimeDisplayConfigSO>(AssetAddresses.Configs.TimeDisplay);

            (_timeApiConfig, _timeDisplayConfig) = await UniTask.WhenAll(
                _timeApiHandle.Task.AsUniTask(),
                _timeDisplayHandle.Task.AsUniTask());

            if (this == null) return;

            Build();
        }

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
            builder.RegisterInstance(_timeApiConfig).As<ITimeApiConfig>();
            builder.RegisterInstance(_timeDisplayConfig).As<ITimeDisplayConfig>();
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

        protected override void OnDestroy()
        {
            if (_timeApiHandle.IsValid())     Addressables.Release(_timeApiHandle);
            if (_timeDisplayHandle.IsValid()) Addressables.Release(_timeDisplayHandle);
            base.OnDestroy();
        }
    }
}
