using System.Threading;
using _Sculpture.Runtime.Content;
using _Sculpture.Runtime.Content.Predefined.Services;
using _Sculpture.Runtime.UI.Services;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VoxelArt.Runtime;

namespace _VRSculpture.Runtime.UI
{
    internal sealed class PredefinedModelView : ModelView
    {
        [Space]
        [SerializeField] private TMP_Text _nameText;

        [Space]
        [SerializeField] private Button _loadButton;

        private IPredefinedContentService _predefinedContentService;
        private IUIService _uiService;

        [Inject]
        private void Construct(IPredefinedContentService predefinedContentService, IUIService uiService)
        {
            _predefinedContentService = predefinedContentService;
            _uiService = uiService;
        }

        public override void Initialize(ModelDescriptor descriptor)
        {
            base.Initialize(descriptor);

            _nameText.text = descriptor.Name;
            _loadButton.onClick.AddListener(LoadModel);
        }

        public override void Uninitialize()
        {
            base.Uninitialize();
            _loadButton.onClick.RemoveListener(LoadModel);
        }

        private void LoadModel() => LoadModelAsync(ActiveCancellationToken).Forget();

        private async UniTask LoadModelAsync(CancellationToken cancellationToken)
        {
            if (!VoxelArtSystems.Components.TryGetModelObject(_ => true, out ModelObject modelObject))
            {
                return;
            }

            _uiService.Open(HelperPageIdentifier.MainLoader);

            try
            {
                ModelGetResult modelGetResult = await _predefinedContentService.GetModelAsync(Descriptor, cancellationToken);

                if (!modelGetResult.IsSuccessful)
                {
                    return;
                }

                ModelApplySettings settings = ModelApplySettings.FromModelObjectSettings();
                settings.ReleasePreviousModel = true;

                await VoxelArtSystems.Build.ApplyModelAsync(modelObject, modelGetResult.Model, settings, cancellationToken);
            }
            finally
            {
                _uiService.Close(HelperPageIdentifier.MainLoader);
            }
        }
    }
}