using Mirra.Application.Configs;
using UnityEngine;

namespace Mirra.Infrastructure.Configs
{
    [CreateAssetMenu(fileName = "TimeApiConfig", menuName = "Mirra/Configs/TimeApi")]
    public class TimeApiConfigSO : ScriptableObject, ITimeApiConfig
    {
        [SerializeField] private string _apiUrl = "https://worldtimeapi.org/api/timezone/UTC";

        public string ApiUrl => _apiUrl;
    }
}
