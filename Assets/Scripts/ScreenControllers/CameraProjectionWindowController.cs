using System;
using System.Collections.Generic;
using eggsgd.UiFramework.Examples.Widgets;
using eggsgd.UiFramework.Window;
using UnityEngine;

namespace eggsgd.UiFramework.Examples.ScreenControllers
{
    [Serializable]
    public class CameraProjectionWindowProperties : WindowProperties
    {
        public readonly Transform TransformToFollow;
        public readonly Camera WorldCamera;

        public CameraProjectionWindowProperties(Camera worldCamera, Transform toFollow)
        {
            WorldCamera = worldCamera;
            TransformToFollow = toFollow;
        }
    }

    public class CameraProjectionWindowController : AWindowController<CameraProjectionWindowProperties>
    {
        [SerializeField]
        private UIFollowComponent followTemplate;

        private readonly List<UIFollowComponent> _allElements = new();

        private void LateUpdate()
        {
            foreach (var t in _allElements)
            {
                t.UpdatePosition(Properties.WorldCamera);
            }
        }

        protected override void OnPropertiesSet()
        {
            CreateNewLabel(Properties.TransformToFollow, "Look at me!", null);
        }

        protected override void WhileHiding()
        {
            foreach (var element in _allElements)
            {
                Destroy(element.gameObject);
            }

            _allElements.Clear();
            // This is the kind of thing you *COULD* do, but you usually wouldn't
            // want to - in theory this is UI code, so it shouldn't control external things.
            // This is an example of "with great power comes great responsibility":
            // the UI Framework enforces very few rules, but the rest is up to you.
            Properties.TransformToFollow.parent.gameObject.SetActive(false);
        }

        private void CreateNewLabel(Transform target, string label, Sprite icon)
        {
            var followComponent = Instantiate(followTemplate, followTemplate.transform.parent, false);
            followComponent.LabelDestroyed += OnLabelDestroyed;
            followComponent.gameObject.SetActive(true);
            followComponent.SetFollow(target);
            followComponent.SetText(label);

            if (icon != null)
            {
                followComponent.SetIcon(icon);
            }

            _allElements.Add(followComponent);
        }

        private void OnLabelDestroyed(UIFollowComponent destroyedLabel)
        {
            _allElements.Remove(destroyedLabel);
        }
    }
}