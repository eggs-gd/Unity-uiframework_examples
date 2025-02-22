﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace eggsgd.UiFramework.Examples.Widgets
{
	/// <summary>
	///     Important: this only works if the template UI element
	///     is anchored to the bottom left corner. It also considers
	///     the RectTransform that contains it is stretched to fit
	///     the screen.
	/// </summary>
	public class UIFollowComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Image icon;
        [SerializeField] private bool clampAtBorders = true;
        [SerializeField] private bool rotateWhenClamped = true;
        [SerializeField] private RectTransform rotatingElement;

        private Transform _currentFollow;
        private RectTransform _mainCanvasRectTransform;
        private CanvasScaler _parentScaler;
        private RectTransform _rectTransform;

        private void Start()
        {
            _mainCanvasRectTransform = transform.root as RectTransform;
            _rectTransform = transform as RectTransform;
            _parentScaler = _mainCanvasRectTransform.GetComponent<CanvasScaler>();

            if (rotatingElement == null)
            {
                rotatingElement = _rectTransform;
            }
        }

        private void OnDestroy()
        {
            if (LabelDestroyed != null)
            {
                LabelDestroyed(this);
            }
        }

        public event Action<UIFollowComponent> LabelDestroyed;

        /// <summary>
        ///     Calculates the anchored position for a given Worldspace transform for a Screenspace-Camera UI
        /// </summary>
        /// <param name="viewingCamera">The worldspace camera</param>
        /// <param name="followTransform">The transform to be followed</param>
        /// <param name="canvasScaler">The canvas scaler</param>
        /// <param name="followElementRect">The rect of the UI element that will follow the transform</param>
        /// <returns></returns>
        public static Vector2 GetAnchoredPosition(Camera viewingCamera, Transform followTransform,
                                                  CanvasScaler canvasScaler, Rect followElementRect)
        {
            // We need to calculate the object's relative position to the camera make sure the
            // follow element's position doesn't end up getting "inverted" by WorldToViewportPoint when far away
            var relativePosition = viewingCamera.transform.InverseTransformPoint(followTransform.position);
            relativePosition.z = Mathf.Max(relativePosition.z, 1f);
            var viewportPos =
                viewingCamera.WorldToViewportPoint(viewingCamera.transform.TransformPoint(relativePosition));

            return new Vector2(viewportPos.x * canvasScaler.referenceResolution.x - followElementRect.size.x / 2f,
                viewportPos.y * canvasScaler.referenceResolution.y - followElementRect.size.y / 2f);
        }

        /// <summary>
        ///     Clamps the position on the screen for a Screenspace-Camera UI
        /// </summary>
        /// <param name="onScreenPosition">The current on-screen position for an UI element</param>
        /// <param name="followElementRect">The rect that follows the worldspace object</param>
        /// <param name="mainCanvasRectTransform">The rect transform of this UI's main canvas</param>
        /// <returns></returns>
        public static Vector2 GetClampedOnScreenPosition(Vector2 onScreenPosition, Rect followElementRect,
                                                         RectTransform mainCanvasRectTransform) =>
            new Vector2(
                Mathf.Clamp(onScreenPosition.x, 0f, mainCanvasRectTransform.sizeDelta.x - followElementRect.size.x),
                Mathf.Clamp(onScreenPosition.y, 0f, mainCanvasRectTransform.sizeDelta.y - followElementRect.size.y));

        public void SetFollow(Transform toFollow)
        {
            _currentFollow = toFollow;
        }

        public void SetText(string label)
        {
            this.label.text = label;
        }

        public void SetIcon(Sprite icon)
        {
            this.icon.sprite = icon;
        }

        /// <summary>
        ///     Positions element at the center of the screen
        /// </summary>
        protected void PositionAtOrigin()
        {
            var mainSize = _mainCanvasRectTransform.sizeDelta;
            var labelSize = _rectTransform.rect.size;
            _rectTransform.anchoredPosition = new Vector2((mainSize.x - labelSize.x) * 0.5f, mainSize.y * 0.5f);
        }

        public void UpdatePosition(Camera cam)
        {
            if (_currentFollow == null)
            {
                return;
            }

            var onScreenPosition =
                GetAnchoredPosition(cam, _currentFollow.transform, _parentScaler, _rectTransform.rect);
            if (!clampAtBorders)
            {
                _rectTransform.anchoredPosition = onScreenPosition;
                return;
            }

            var clampedPosition =
                GetClampedOnScreenPosition(onScreenPosition, _rectTransform.rect, _mainCanvasRectTransform);
            _rectTransform.anchoredPosition = clampedPosition;

            if (!rotateWhenClamped)
            {
                return;
            }

            if (onScreenPosition != clampedPosition)
            {
                var delta = clampedPosition - onScreenPosition;
                var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
                rotatingElement.localRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            }
            else
            {
                rotatingElement.localRotation = Quaternion.identity;
            }
        }
    }
}