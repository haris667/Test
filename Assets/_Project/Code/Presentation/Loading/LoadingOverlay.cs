using Cysharp.Threading.Tasks;
using LitMotion;
using Mirra.Domain.Models;
using Mirra.Presentation.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Mirra.Presentation.Loading
{
    public class LoadingOverlay : MonoBehaviour, IStartable
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _timePanel;

        [Header("Animation")]
        [SerializeField] private float _fadeDuration = 0.8f;
        [SerializeField] private Ease _overlayEase = Ease.InQuad;
        [SerializeField] private Ease _panelEase = Ease.OutCubic;
        [SerializeField] private float _panelOffset = 200f;

        private ILocalClock _clock;

        [Inject]
        public void Construct(ILocalClock clock) => _clock = clock;

        void IStartable.Start()
        {
            if (_timePanel != null)
            {
                _timePanel.offsetMin = new Vector2(-_panelOffset, -_panelOffset);
                _timePanel.offsetMax = new Vector2( _panelOffset,  _panelOffset);
            }

            _clock.OnTick += OnFirstTick;
        }

        private void OnFirstTick(TimeData _)
        {
            _clock.OnTick -= OnFirstTick;
            FadeOutAsync().Forget();
        }

        private async UniTaskVoid FadeOutAsync()
        {
            if (this == null || _canvasGroup == null) return;

            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            LMotion.Create(1f, 0f, _fadeDuration)
                .WithEase(_overlayEase)
                .Bind(a => _canvasGroup.alpha = a)
                .AddTo(this);

            if (_timePanel != null)
            {
                LMotion.Create(new Vector2(-_panelOffset, -_panelOffset), Vector2.zero, _fadeDuration)
                    .WithEase(_panelEase)
                    .Bind(v => _timePanel.offsetMin = v)
                    .AddTo(this);

                LMotion.Create(new Vector2(_panelOffset, _panelOffset), Vector2.zero, _fadeDuration)
                    .WithEase(_panelEase)
                    .Bind(v => _timePanel.offsetMax = v)
                    .AddTo(this);
            }

            await UniTask.Delay(
                Mathf.RoundToInt(_fadeDuration * 1000),
                cancellationToken: destroyCancellationToken);

            gameObject.SetActive(false);
        }
    }
}
