using System;
using eggsgd.UiFramework.Examples.Utils;
using eggsgd.UiFramework.Window;
using TMPro;
using UnityEngine;

namespace eggsgd.UiFramework.Examples.ScreenControllers
{
    public class ShowConfirmationPopupSignal : ASignal<ConfirmationPopupProperties>
    {
    }

    [Serializable]
    public class ConfirmationPopupController : AWindowController<ConfirmationPopupProperties>
    {
        [SerializeField] public TextMeshProUGUI titleLabel;
        [SerializeField] public TextMeshProUGUI messageLabel;
        [SerializeField] public TextMeshProUGUI confirmButtonLabel;
        [SerializeField] public TextMeshProUGUI cancelButtonLabel;
        [SerializeField] public GameObject cancelButtonObject;

        protected override void OnPropertiesSet()
        {
            titleLabel.text = Properties.Title;
            messageLabel.text = Properties.Message;
            confirmButtonLabel.text = Properties.ConfirmButtonText;
            cancelButtonLabel.text = Properties.CancelButtonText;

            cancelButtonObject.SetActive(Properties.CancelAction != null);
        }

        public void UI_Confirm()
        {
            UI_Close();
            if (Properties.ConfirmAction != null)
            {
                Properties.ConfirmAction();
            }
        }

        public void UI_Cancel()
        {
            UI_Close();
            if (Properties.CancelAction != null)
            {
                Properties.CancelAction();
            }
        }
    }

    [Serializable]
    public class ConfirmationPopupProperties : WindowProperties
    {
        public readonly Action CancelAction;
        public readonly string CancelButtonText;
        public readonly Action ConfirmAction;
        public readonly string ConfirmButtonText;
        public readonly string Message;
        public readonly string Title;

        public ConfirmationPopupProperties(string title, string message,
                                           string confirmButtonText = "Confirm", Action confirmAction = null,
                                           string cancelButtonText = "Cancel", Action cancelAction = null)
        {
            Title = title;
            Message = message;
            ConfirmButtonText = confirmButtonText;
            CancelButtonText = cancelButtonText;
            CancelAction = cancelAction;
            ConfirmAction = confirmAction;
        }
    }
}