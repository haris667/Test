using System;

namespace Mirra.Domain.Models
{
    public enum SceneLoadMode { Single, Additive }

    public readonly struct LoadSceneCommand
    {
        public string SceneName { get; }
        public TimeSpan Delay { get; }
        public SceneLoadMode Mode { get; }

        public LoadSceneCommand(string sceneName, TimeSpan delay, SceneLoadMode mode)
        {
            SceneName = sceneName;
            Delay = delay;
            Mode = mode;
        }
    }
}
