using System;
using LitMotion;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mirra.Presentation.MVP.Time
{
    [RequireComponent(typeof(Image))]
    public class TimeLineHandler : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler,
        IDragHandler
    {
        [SerializeField] private ClockHandType _handType;

        [Header("Sprites")]
        [SerializeField] private Sprite _idleSprite;
        [SerializeField] private Sprite _hoverSprite;

        [Header("Colors")]
        [SerializeField] private Color _idleColor = Color.white;
        [SerializeField] private Color _pressColor = new Color(0.4f, 0.8f, 1f, 1f);
        [SerializeField] private float _colorDuration = 0.15f;

        [Header("Optional overrides")]
        [SerializeField] private RectTransform _clockCenter;  // auto-found if null
        [SerializeField] private Camera _uiCamera;            // null = Screen Space Overlay

        public event Action<ClockHandType, float> OnAngleChanged;

        private Image _image;
        private MotionHandle _colorHandle;
        private bool _isDragging;

        private void Awake()
        {
            _image = GetComponent<Image>();

            if (_clockCenter == null)
            {
                var clock = GetComponentInParent<ClockView>();
                if (clock != null) _clockCenter = clock.GetComponent<RectTransform>();
            }

            if (_idleSprite != null) _image.sprite = _idleSprite;
        }

        public void OnPointerEnter(PointerEventData _)
        {
            if (_isDragging) return;
            if (_hoverSprite != null) _image.sprite = _hoverSprite;
        }

        public void OnPointerExit(PointerEventData _)
        {
            if (_isDragging) return;
            if (_idleSprite != null) _image.sprite = _idleSprite;
        }

        public void OnPointerDown(PointerEventData _)
        {
            _isDragging = true;
            AnimateColor(_pressColor);
        }

        public void OnPointerUp(PointerEventData _)
        {
            _isDragging = false;
            AnimateColor(_idleColor);
            if (_idleSprite != null) _image.sprite = _idleSprite;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_clockCenter == null) return;

            Vector2 screenCenter = RectTransformUtility.WorldToScreenPoint(_uiCamera, _clockCenter.position);
            Vector2 dir = eventData.position - screenCenter;

            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            if (angle < 0f) angle += 360f;

            OnAngleChanged?.Invoke(_handType, angle);
        }

        private void AnimateColor(Color target)
        {
            if (_colorHandle.IsActive()) _colorHandle.Cancel();
            var from = _image.color;
            _colorHandle = LMotion.Create(from, target, _colorDuration)
                .WithEase(Ease.OutQuad)
                .Bind(c => _image.color = c)
                .AddTo(this);
        }

        private void OnDestroy()
        {
            if (_colorHandle.IsActive()) _colorHandle.Cancel();
        }
    }
}
