using System;
using Mirra.Presentation.MVP.Base;

namespace Mirra.Presentation.Components
{
    public class StandardBaseButtonView : BaseButtonView<StandardButtonPresenter>, IStandardButtonView
    {
        public event Action OnClicked;

        public string Text
        {
            get => Label != null ? Label.text : string.Empty;
            set { if (Label != null) Label.SetText(value); }
        }

        protected override void Awake()
        {
            base.Awake();
            
            if (Button != null) Button.onClick.AddListener(() => OnClicked?.Invoke());
        }
    }
}
