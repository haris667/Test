#if UNITY_EDITOR
using Mirra.Infrastructure.Assets;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Mirra.Infrastructure.Boot
{
    public static class BootSceneEnforcer
    {
        private const string BootSceneFileName = "Boot";

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnforceBootScene()
        {
            if (SceneManager.GetActiveScene().name == BootSceneFileName)
                return;

            Addressables.LoadSceneAsync(AssetAddresses.Scenes.Boot, LoadSceneMode.Single)
                .WaitForCompletion();
        }
    }
}
#endif
