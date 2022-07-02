using eggsgd.Signals;
using eggsgd.UiFramework.Examples.Extras;
using eggsgd.UiFramework.Examples.ScreenControllers;
using UnityEngine;

namespace eggsgd.UiFramework.Examples
{
    public class UIDemoController : MonoBehaviour
    {
        [SerializeField] private UISettings defaultUISettings;
        [SerializeField] private FakePlayerData fakePlayerData;
        [SerializeField] private Camera cam;
        [SerializeField] private Transform transformToFollow;

        private UIFrame _uiFrame;

        private void Awake()
        {
            _uiFrame = defaultUISettings.CreateUIInstance();
            SignalBus.Get<StartDemoSignal>().AddListener(OnStartDemo);
            SignalBus.Get<NavigateToWindowSignal>().AddListener(OnNavigateToWindow);
            SignalBus.Get<ShowConfirmationPopupSignal>().AddListener(OnShowConfirmationPopup);
        }

        private void Start()
        {
            _uiFrame.OpenWindow(ScreenIds.StartGameWindow);
        }

        private void OnDestroy()
        {
            SignalBus.Get<StartDemoSignal>().RemoveListener(OnStartDemo);
            SignalBus.Get<NavigateToWindowSignal>().RemoveListener(OnNavigateToWindow);
            SignalBus.Get<ShowConfirmationPopupSignal>().RemoveListener(OnShowConfirmationPopup);
        }

        private void OnStartDemo()
        {
            // The navigation panel will automatically navigate
            // to the first screen upon opening
            _uiFrame.ShowPanel(ScreenIds.NavigationPanel);
            _uiFrame.ShowPanel(ScreenIds.ToastPanel);
        }

        private void OnNavigateToWindow(string windowId)
        {
            // You usually don't have to do this as the system takes care of everything
            // automatically, but since we're dealing with navigation and the Window layer
            // has a history stack, this way we can make sure we're not just adding
            // entries to the stack indefinitely
            _uiFrame.CloseCurrentWindow();

            switch (windowId)
            {
                case ScreenIds.PlayerWindow:
                    _uiFrame.OpenWindow(windowId, new PlayerWindowProperties(fakePlayerData.LevelProgress));
                    break;
                case ScreenIds.CameraProjectionWindow:
                    transformToFollow.parent.gameObject.SetActive(true);
                    _uiFrame.OpenWindow(windowId, new CameraProjectionWindowProperties(cam, transformToFollow));
                    break;
                default:
                    _uiFrame.OpenWindow(windowId);
                    break;
            }
        }

        private void OnShowConfirmationPopup(ConfirmationPopupProperties popupPayload)
        {
            _uiFrame.OpenWindow(ScreenIds.ConfirmationPopup, popupPayload);
        }
    }
}