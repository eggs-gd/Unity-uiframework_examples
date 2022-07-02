using eggsgd.Signals;
using eggsgd.UiFramework.Window;
using UnityEngine;
using UnityEngine.UI;

namespace eggsgd.UiFramework.Examples.ScreenControllers
{
    public class PopupExampleWindowController : AWindowController
    {
        [SerializeField]
        private Image exampleImage;

        private int _currentPopupExample;
        private Color _originalColor;

        /// <summary>
        ///     You can use all of Unity's regular functions, as Screens
        ///     are all MonoBehaviours, but don't forget that many of them
        ///     have important operations being called in their base methods
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _originalColor = exampleImage.color;
        }

        public void UI_ShowPopup()
        {
            SignalBus.Get<ShowConfirmationPopupSignal>().Dispatch(GetPopupData());
        }

        private ConfirmationPopupProperties GetPopupData()
        {
            ConfirmationPopupProperties testProps = null;

            switch (_currentPopupExample)
            {
                case 0:
                    testProps = new ConfirmationPopupProperties("Uh-oh!",
                        "You were curious and clicked the button! Try a few more times.",
                        "Got it!");
                    break;
                case 1:
                    testProps = new ConfirmationPopupProperties("Question:",
                        "What is your favourite color?",
                        "Blue", OnBlueSelected,
                        "Red", OnRedSelected);
                    break;
                case 2:
                    testProps = new ConfirmationPopupProperties("Pretty cool huh?",
                        "Let's return our buddy to its original color.",
                        "Fine.", OnRevertColors);
                    break;
                case 3:
                    testProps = new ConfirmationPopupProperties("YOU DIED",
                        "The Dark Souls of Pop-Ups", "Respawn");
                    break;
            }

            _currentPopupExample++;
            if (_currentPopupExample > 3)
            {
                _currentPopupExample = 0;
            }

            return testProps;
        }

        private void OnRevertColors()
        {
            exampleImage.color = _originalColor;
        }

        private void OnRedSelected()
        {
            exampleImage.color = Color.red;
        }

        private void OnBlueSelected()
        {
            exampleImage.color = Color.blue;
        }
    }
}