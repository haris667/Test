using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mirra.Presentation.MVP.Base
{
    [RequireComponent(typeof(Button))]
    public abstract class BaseButtonView<TPresenter> : ViewBase<TPresenter>,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler
        where TPresenter : class, IPresenter
    {
        [Header("References")]
        [SerializeField] protected TMP_Text Label;
        [SerializeField] private Image _glowImage;

        [Header("Hover")]
        [SerializeField] private float _hoverScale = 1.05f;
        [SerializeField] private float _hoverDuration = 0.2f;
        [SerializeField] private Ease _hoverEase = Ease.OutBack;

        [Header("Press")]
        [SerializeField] private float _pressScale = 0.95f;
        [SerializeField] private float _pressDuration = 0.1f;
        [SerializeField] private Ease _pressEase = Ease.OutQuad;

        [Header("Glow")]
        [SerializeField] private float _glowIdleAlpha = 0.3f;
        [SerializeField] private float _glowHoverAlpha = 1f;
        [SerializeField] private float _glowDuration = 0.25f;
        [SerializeField] private Ease _glowEase = Ease.OutQuad;

        private Vector3 _originalScale;
        private MotionHandle _scaleHandle;
        private MotionHandle _glowHandle;

        protected Button Button { get; private set; }

        protected virtual void Awake()
        {
            Button = GetComponent<Button>();
            _originalScale = transform.localScale;

            if (_glowImage != null)
                SetGlowAlpha(_glowIdleAlpha);
        }

        public virtual void OnPointerEnter(PointerEventData _)
        {
            AnimateScale(_hoverScale, _hoverDuration, _hoverEase);
            AnimateGlow(_glowHoverAlpha);
        }

        public virtual void OnPointerExit(PointerEventData _)
        {
            AnimateScale(1f, _hoverDuration, _hoverEase);
            AnimateGlow(_glowIdleAlpha);
        }

        public virtual void OnPointerDown(PointerEventData _) =>
            AnimateScale(_pressScale, _pressDuration, _pressEase);

        public virtual void OnPointerUp(PointerEventData _) =>
            AnimateScale(_hoverScale, _pressDuration, _pressEase);

        protected virtual void AnimateScale(float multiplier, float duration, Ease ease)
        {
            CancelHandle(ref _scaleHandle);
            _scaleHandle = LMotion.Create(transform.localScale, _originalScale * multiplier, duration)
                .WithEase(ease)
                .BindToLocalScale(transform)
                .AddTo(this);
        }

        protected virtual void AnimateGlow(float targetAlpha)
        {
            if (_glowImage == null) return;
            CancelHandle(ref _glowHandle);
            _glowHandle = LMotion.Create(_glowImage.color.a, targetAlpha, _glowDuration)
                .WithEase(_glowEase)
                .Bind(SetGlowAlpha)
                .AddTo(this);
        }

        private void SetGlowAlpha(float alpha)
        {
            if (_glowImage == null) return;
            var c = _glowImage.color;
            c.a = alpha;
            _glowImage.color = c;
        }

        private static void CancelHandle(ref MotionHandle h)
        {
            if (h.IsActive()) h.Cancel();
        }

        protected virtual void OnDestroy()
        {
            CancelHandle(ref _scaleHandle);
            CancelHandle(ref _glowHandle);
        }
    }
}
