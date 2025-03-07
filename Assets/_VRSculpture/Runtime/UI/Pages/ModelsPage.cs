using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using _Sculpture.Runtime.UI;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using VoxelArt.Runtime;
using VoxelArt.Runtime.Saving;

namespace _VRSculpture.Runtime.UI.Pages
{
    internal sealed class ModelsPage : BasicPage
    {
        [Space]
        [SerializeField] private ModelView _viewPrefab;
        [SerializeField] private Transform _viewsParent;

        [Space]
        [SerializeField] private Button _createButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        private readonly List<ModelView> _views = new();

        public override void Open(IOpenArguments arguments)
        {
            base.Open(arguments);

            List<ModelDataJson> models = LoadModels();
            RecreateViews(models);

            _createButton.onClick.AddListener(CreateModel);
        }

        public override void Close(ICloseArguments arguments)
        {
            base.Close(arguments);
            _createButton.onClick.RemoveListener(CreateModel);
        }

        private void CreateModel()
        {
            List<ModelDataJson> models = LoadModels();

            ModelDataJson data = new ModelDataJson
            {
                Name = $"Model {models.Count}",
                Path = Path.Combine(Application.persistentDataPath, $"Model_{models.Count}.voxelart")
            };

            _ = PerformAsync(CreateModelAsync, data, OpenCancellationToken);
        }

        private void LoadModel(ModelDataJson data) => _ = PerformAsync(LoadModelAsync, data, OpenCancellationToken);

        private void SaveModel(ModelDataJson data) => _ = PerformAsync(SaveModelAsync, data, OpenCancellationToken);

        private async Task CreateModelAsync(ModelDataJson data, CancellationToken cancellationToken)
        {
            if (!VoxelArtSystems.Components.TryGetModelObject(_ => true, out ModelObject modelObject) ||
                modelObject.Model is null)
            {
                return;
            }

            ModelWriteResult result = await VoxelArtSystems.Saving.WriteAsync(modelObject.Model, data.Path, Serialization.RawVoxelArt, cancellationToken);

            if (result.IsSuccessful)
            {
                List<ModelDataJson> models = LoadModels();

                models.Add(data);
                RecreateViews(models);

                SaveModels(models);
            }
        }

        private async Task LoadModelAsync(ModelDataJson data, CancellationToken cancellationToken)
        {
            if (!VoxelArtSystems.Components.TryGetModelObject(_ => true, out ModelObject modelObject))
            {
                return;
            }

            ModelReadResult result = await VoxelArtSystems.Saving.ReadAsync(data.Path, Serialization.RawVoxelArt, cancellationToken);

            if (!result.IsSuccessful)
            {
                return;
            }

            ModelApplySettings settings = ModelApplySettings.FromModelObjectSettings();
            settings.ReleasePreviousModel = true;

            await VoxelArtSystems.Build.ApplyModelAsync(modelObject, result.Model, settings, cancellationToken);
        }

        private async Task SaveModelAsync(ModelDataJson data, CancellationToken cancellationToken)
        {
            if (!VoxelArtSystems.Components.TryGetModelObject(_ => true, out ModelObject modelObject) ||
                modelObject.Model is null)
            {
                return;
            }

            await VoxelArtSystems.Saving.WriteAsync(modelObject.Model, data.Path, Serialization.RawVoxelArt, cancellationToken);
        }

        private async Task PerformAsync<T>(Func<T, CancellationToken, Task> callback, T arg, CancellationToken cancellationToken)
        {
            _canvasGroup.interactable = false;

            try
            {
                await callback(arg, cancellationToken);
            }
            finally
            {
                _canvasGroup.interactable = true;
            }
        }

        private void RecreateViews(List<ModelDataJson> models)
        {
            foreach (ModelView view in _views)
            {
                Destroy(view.gameObject);
            }

            _views.Clear();

            foreach (ModelDataJson model in models)
            {
                ModelView view = Instantiate(_viewPrefab, _viewsParent);
                view.Initialize(model, LoadModel, SaveModel);

                _views.Add(view);
            }
        }

        private List<ModelDataJson> LoadModels()
        {
            string path = $"{Application.persistentDataPath}/Models.json";

            if (!File.Exists(path))
            {
                return new List<ModelDataJson>();
            }

            try
            {
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<List<ModelDataJson>>(json);
            }
            catch (Exception)
            {
                return new List<ModelDataJson>();
            }
        }

        private void SaveModels(List<ModelDataJson> models)
        {
            string path = $"{Application.persistentDataPath}/Models.json";

            try
            {
                string json = JsonConvert.SerializeObject(models, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception)
            {
                // Ignored.
            }
        }
    }
}
