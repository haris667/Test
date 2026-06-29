using System;
using Mirra.Presentation.Components;
using Mirra.Presentation.MVP.Base;

using TMPro;
using UnityEngine;

namespace Mirra.Presentation.MVP.Time
{
    public class TimeView : ViewBase<TimePresenter>, ITimeView
    {
        [SerializeField] private TMP_Text _timeLabel;
        [SerializeField] private TMP_InputField _hoursInputField;
        [SerializeField] private TMP_InputField _minutesInputField;
        [SerializeField] private StandardBaseButtonView _refreshButton;
        [SerializeField] private StandardBaseButtonView _applyButton;

        public event Action OnRefreshRequested;
        public event Action<int, int> OnTimeChangeRequested;

        private void Awake()
        {
            if (_refreshButton != null) _refreshButton.OnClicked += () => OnRefreshRequested?.Invoke();
            if (_applyButton != null)   _applyButton.OnClicked   += OnApplyClicked;
        }

        private void OnApplyClicked()
        {
            int hours   = int.TryParse(_hoursInputField.text, out var h) ? h : 0;
            int minutes = int.TryParse(_minutesInputField.text, out var m) ? m : 0;
            OnTimeChangeRequested?.Invoke(hours, minutes);
        }

        public void DisplayTime(string formattedTime) => _timeLabel.SetText(formattedTime);
        public void ShowLoading() => _timeLabel.SetText("Loading...");
        public void ShowError(string message) => _timeLabel.SetText($"Error: {message}");
    }
}
