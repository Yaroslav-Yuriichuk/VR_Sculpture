using System;
using System.Threading;
using _Sculpture.Runtime.Content;
using _Sculpture.Runtime.Content.Services;
using _Sculpture.Runtime.UI.Services;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VoxelArt.Runtime;

namespace _VRSculpture.Runtime.UI
{
    internal sealed class RemoteModelView : ModelView
    {
        [SerializeField] private TMP_Text _nameText;

        [Space]
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _saveButton;

        private IContentService _contentService;
        private IUIService _uiService;

        [Inject]
        private void Construct(IContentService contentService, IUIService uiService)
        {
            _contentService = contentService;
            _uiService = uiService;
        }

        public override void Initialize(ModelDescriptor descriptor)
        {
            base.Initialize(descriptor);

            _nameText.text = descriptor.Name;

            _loadButton.onClick.AddListener(LoadModel);
            _saveButton.onClick.AddListener(SaveModel);
        }

        public override void Uninitialize()
        {
            base.Uninitialize();

            _loadButton.onClick.RemoveListener(LoadModel);
            _saveButton.onClick.RemoveListener(SaveModel);
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
                ModelGetResult modelGetResult = await _contentService.GetModelAsync(Descriptor, cancellationToken);

                if (!modelGetResult.IsSuccessful)
                {
                    return;
                }

                ModelApplySettings settings = ModelApplySettings.FromModelObjectSettings();
                settings.ReleasePreviousModel = true;

                await VoxelArtSystems.Build.ApplyModelAsync(modelObject, modelGetResult.Model, settings,
                    cancellationToken);
            }
            finally
            {
                _uiService.Close(HelperPageIdentifier.MainLoader);
            }
        }

        private void SaveModel() => SaveModelAsync(ActiveCancellationToken).Forget();

        private async UniTask SaveModelAsync(CancellationToken cancellationToken)
        {
            if (!VoxelArtSystems.Components.TryGetModelObject(_ => true, out ModelObject modelObject) ||
                modelObject.Model is null)
            {
                return;
            }

            _uiService.Open(HelperPageIdentifier.MainLoader);

            try
            {
                await _contentService.UpdateModelAsync(Descriptor, modelObject.Model, cancellationToken);
            }
            finally
            {
                _uiService.Close(HelperPageIdentifier.MainLoader);
            }
        }
    }
}