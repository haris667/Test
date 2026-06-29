using Mirra.Application.Configs;
using UnityEngine;

namespace Mirra.Infrastructure.Configs
{
    [CreateAssetMenu(fileName = "TimeDisplayConfig", menuName = "Mirra/Configs/TimeDisplay")]
    public class TimeDisplayConfigSO : ScriptableObject, ITimeDisplayConfig
    {
        [SerializeField] private string _displayFormat = "HH:mm:ss";

        [SerializeField] private string[] _parseFormats =
        {
            "HH:mm:ss",
            "H:mm:ss",
            "HH:mm",
            "H:mm",
        };

        public string DisplayFormat => _displayFormat;
        public string[] ParseFormats => _parseFormats;
    }
}
