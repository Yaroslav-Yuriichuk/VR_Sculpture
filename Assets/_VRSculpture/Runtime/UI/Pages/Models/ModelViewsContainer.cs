using System.Collections.Generic;
using _Sculpture.Runtime.Content;
using _Sculpture.Runtime.UI;
using UnityEngine;

namespace _VRSculpture.Runtime.UI.Pages.Models
{
    internal class ModelViewsContainer : MonoBehaviourPageElement
    {
        [SerializeField] private ModelView _viewPrefab;
        [SerializeField] private Transform _viewsParent;

        private readonly Dictionary<string, ModelView> _views = new();

        protected void AddView(ModelDescriptor descriptor)
        {
            if (_views.ContainsKey(descriptor.Id))
            {
                return;
            }

            ModelView view = Instantiate(_viewPrefab, _viewsParent);
            view.Initialize(descriptor);

            _views.Add(descriptor.Id, view);
        }

        protected void RemoveView(ModelDescriptor descriptor)
        {
            if (!_views.TryGetValue(descriptor.Id, out ModelView view))
            {
                return;
            }

            view.Uninitialize();
            Destroy(view.gameObject);

            _views.Remove(descriptor.Id);
        }

        protected void ClearViews()
        {
            foreach (ModelView view in _views.Values)
            {
                view.Uninitialize();
                Destroy(view.gameObject);
            }

            _views.Clear();
        }
    }
}