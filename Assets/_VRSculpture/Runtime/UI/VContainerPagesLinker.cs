using _Sculpture.Runtime.UI;
using _Sculpture.Runtime.UI.Services;
using VContainer;

namespace _VRSculpture.Runtime.UI
{
    internal sealed class VContainerPagesLinker : PagesLinker
    {
        protected override IUIService UIService => _uiService;

        private IUIService _uiService;

        [Inject]
        private void Construct(IUIService uiService)
        {
            _uiService = uiService;
        }
    }
}