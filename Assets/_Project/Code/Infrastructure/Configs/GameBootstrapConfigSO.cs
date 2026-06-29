using System;
using Mirra.Application.Configs;
using UnityEngine;

namespace Mirra.Infrastructure.Configs
{
    [CreateAssetMenu(fileName = "GameBootstrapConfig", menuName = "Mirra/Configs/GameBootstrap")]
    public class GameBootstrapConfigSO : ScriptableObject, IGameBootstrapConfig
    {
        [Tooltip("Addressable address of the scene to load after Boot (e.g. 'TimeScene')")]
        [SerializeField] private string _initialSceneAddress = "TimeScene";

        [SerializeField] private float _transitionDelaySeconds = 2f;

        public string InitialSceneName => _initialSceneAddress;
        public TimeSpan TransitionDelay => TimeSpan.FromSeconds(_transitionDelaySeconds);
    }
}
