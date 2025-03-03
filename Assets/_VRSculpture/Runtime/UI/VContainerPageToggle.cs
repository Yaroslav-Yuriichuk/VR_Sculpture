using _Sculpture.Runtime.UI;
using VContainer;

namespace _VRSculpture.Runtime.UI
{
    internal sealed class VContainerPageToggle : PageToggle
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