using Mirra.Presentation.MVP.Base;

namespace Mirra.Presentation.Components
{
    public class StandardButtonPresenter : PresenterBase<IStandardButtonView>
    {
        public StandardButtonPresenter(IStandardButtonView view) : base(view) { }

        protected override void OnInitialize() { }
    }
}
