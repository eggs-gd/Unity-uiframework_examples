﻿using eggsgd.UiFramework.Examples.Extras;
using eggsgd.UiFramework.Examples.ScreenControllers;
using eggsgd.UiFramework.Examples.Utils;
using UnityEngine;

namespace eggsgd.UiFramework.Examples
{
    public class UIDemoController : MonoBehaviour
    {
        [SerializeField] private UISettings defaultUISettings;
        [SerializeField] private FakePlayerData fakePlayerData;
        [SerializeField] private Camera cam;
        [SerializeField] private Transform transformToFollow;

        private UIFrame uiFrame;

        private void Awake()
        {
            uiFrame = defaultUISettings.CreateUIInstance();
            Signals.Get<StartDemoSignal>().AddListener(OnStartDemo);
            Signals.Get<NavigateToWindowSignal>().AddListener(OnNavigateToWindow);
            Signals.Get<ShowConfirmationPopupSignal>().AddListener(OnShowConfirmationPopup);
        }

        private void Start()
        {
            uiFrame.OpenWindow(ScreenIds.StartGameWindow);
        }

        private void OnDestroy()
        {
            Signals.Get<StartDemoSignal>().RemoveListener(OnStartDemo);
            Signals.Get<NavigateToWindowSignal>().RemoveListener(OnNavigateToWindow);
            Signals.Get<ShowConfirmationPopupSignal>().RemoveListener(OnShowConfirmationPopup);
        }

        private void OnStartDemo()
        {
            // The navigation panel will automatically navigate
            // to the first screen upon opening
            uiFrame.ShowPanel(ScreenIds.NavigationPanel);
            uiFrame.ShowPanel(ScreenIds.ToastPanel);
        }

        private void OnNavigateToWindow(string windowId)
        {
            // You usually don't have to do this as the system takes care of everything
            // automatically, but since we're dealing with navigation and the Window layer
            // has a history stack, this way we can make sure we're not just adding
            // entries to the stack indefinitely
            uiFrame.CloseCurrentWindow();

            switch (windowId)
            {
                case ScreenIds.PlayerWindow:
                    uiFrame.OpenWindow(windowId, new PlayerWindowProperties(fakePlayerData.LevelProgress));
                    break;
                case ScreenIds.CameraProjectionWindow:
                    transformToFollow.parent.gameObject.SetActive(true);
                    uiFrame.OpenWindow(windowId, new CameraProjectionWindowProperties(cam, transformToFollow));
                    break;
                default:
                    uiFrame.OpenWindow(windowId);
                    break;
            }
        }

        private void OnShowConfirmationPopup(ConfirmationPopupProperties popupPayload)
        {
            uiFrame.OpenWindow(ScreenIds.ConfirmationPopup, popupPayload);
        }
    }
}