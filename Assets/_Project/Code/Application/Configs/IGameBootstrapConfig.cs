using System;

namespace Mirra.Application.Configs
{
    public interface IGameBootstrapConfig
    {
        string InitialSceneName { get; }
        TimeSpan TransitionDelay { get; }
    }
}
