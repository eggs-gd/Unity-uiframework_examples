using System;
using eggsgd.UiFramework.Examples.ScreenControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace eggsgd.UiFramework.Examples.Widgets
{
    [RequireComponent(typeof(Button))]
    public class NavigationPanelButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI buttonLabel;

        [SerializeField]
        private Image icon;

        private Button _button;

        private NavigationPanelEntry navigationData;

        private Button button
        {
            get
            {
                if (_button == null)
                {
                    _button = GetComponent<Button>();
                }

                return _button;
            }
        }

        public string Target => navigationData.TargetScreen;

        public event Action<NavigationPanelButton> ButtonClicked;

        public void SetData(NavigationPanelEntry target)
        {
            navigationData = target;
            buttonLabel.text = target.ButtonText;
            icon.sprite = target.Sprite;
        }

        public void SetCurrentNavigationTarget(NavigationPanelButton selectedButton)
        {
            button.interactable = selectedButton != this;
        }

        public void SetCurrentNavigationTarget(string screenId)
        {
            if (navigationData != null)
            {
                button.interactable = navigationData.TargetScreen == screenId;
            }
        }

        public void UI_Click()
        {
            if (ButtonClicked != null)
            {
                ButtonClicked(this);
            }
        }
    }
}