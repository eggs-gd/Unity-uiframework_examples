﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using eggsgd.UiFramework.Examples.Extras;
using eggsgd.UiFramework.Panel;
using UnityEngine;

namespace eggsgd.UiFramework.Examples.ScreenControllers
{
    /// <summary>
    ///     Yes, this panel is there, all the time, just waiting for its moment to shine
    /// </summary>
    public class ToastPanelController : APanelController
    {
        [SerializeField] private RectTransform toastRect;
        [SerializeField] private float toastDuration = 0.5f;
        [SerializeField] private float toastPause = 2f;
        [SerializeField] private Ease toastEase = Ease.Linear;

        private bool _isToasting;

        /// <summary>
        ///     We're making this respond to the same signal as the PlayerWindow does.
        ///     This allows us to have the toast if it's present, but there's no issues if
        ///     it's not present for any reason.
        /// </summary>
        protected override void AddListeners()
        {
            Signals.SignalBus.Get<PlayerDataUpdatedSignal>().AddListener(OnDataUpdated);
        }

        protected override void RemoveListeners()
        {
            Signals.SignalBus.Get<PlayerDataUpdatedSignal>().RemoveListener(OnDataUpdated);
        }

        private void OnDataUpdated(List<PlayerDataEntry> data)
        {
            if (_isToasting)
            {
                return;
            }

            // HACK: more info below
            StartCoroutine(YieldForDOTween());
        }

        /// <summary>
        ///     TIL: DOTween uses AddComponent internally, which means
        ///     it can't be called OnValidate.
        ///     Since I've used that to avoid having a custom inspector
        ///     just for the FakePlayerData, I'll allow myself this
        ///     hack, otherwise I'll be stuck writing custom inspectors
        ///     instead of writing documentation :D
        ///     It just emits a harmless warning and wouldn't happen
        ///     in a real-life situation as OnValidate is only called
        ///     in the Editor.
        /// </summary>
        private IEnumerator YieldForDOTween()
        {
            yield return null;

            _isToasting = true;
            var seq = DOTween.Sequence();
            seq.Append(toastRect.DOAnchorPosY(0f, toastDuration).SetEase(toastEase));
            seq.AppendInterval(toastPause);
            seq.Append(toastRect.DOAnchorPosY(toastRect.rect.height, toastDuration).SetEase(toastEase));
            seq.OnComplete(() => _isToasting = false);

            seq.Play();
        }
    }
}