﻿using System;
using System.Collections.Generic;
using eggsgd.Signals;
using eggsgd.UiFramework.Examples.Widgets;
using eggsgd.UiFramework.Panel;
using UnityEngine;

namespace eggsgd.UiFramework.Examples.ScreenControllers
{
    [Serializable]
    public class NavigationPanelEntry
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private string buttonText = "";
        [SerializeField] private string targetScreen = "";

        public Sprite Sprite => sprite;

        public string ButtonText => buttonText;

        public string TargetScreen => targetScreen;
    }

    public class NavigateToWindowSignal : ASignal<string>
    {
    }

    public class NavigationPanelController : APanelController
    {
        [SerializeField]
        private List<NavigationPanelEntry> navigationTargets = new();

        [SerializeField]
        private NavigationPanelButton templateButton;

        private readonly List<NavigationPanelButton> _currentButtons = new();

        // I usually always place AddListeners and RemoveListeners together
        // to reduce the chances of adding a listener and not removing it.
        protected override void AddListeners()
        {
            Signals.SignalBus.Get<NavigateToWindowSignal>().AddListener(OnExternalNavigation);
        }

        protected override void RemoveListeners()
        {
            Signals.SignalBus.Get<NavigateToWindowSignal>().RemoveListener(OnExternalNavigation);
        }

        /// <summary>
        ///     This is called whenever this screen is opened
        ///     be it for the first time or coming from the history/queue
        /// </summary>
        protected override void OnPropertiesSet()
        {
            ClearEntries();
            foreach (var target in navigationTargets)
            {
                var newBtn = Instantiate(templateButton);
                // When using UI, never forget to pass the parameter
                // worldPositionStays as FALSE, otherwise your RectTransform
                // won't layout properly after reparenting.
                // This is the cause for the most common head-scratching issues
                // when starting to deal with Unity UI: adding objects via the editor
                // working fine but objects instanced via code having broken sizes/positions
                newBtn.transform.SetParent(templateButton.transform.parent, false);
                newBtn.SetData(target);
                newBtn.gameObject.SetActive(true);
                newBtn.ButtonClicked += OnNavigationButtonClicked;
                _currentButtons.Add(newBtn);
            }

            // The first button is selected by default
            OnNavigationButtonClicked(_currentButtons[0]);
        }

        private void OnNavigationButtonClicked(NavigationPanelButton currentlyClickedButton)
        {
            Signals.SignalBus.Get<NavigateToWindowSignal>().Dispatch(currentlyClickedButton.Target);
            foreach (var button in _currentButtons)
            {
                button.SetCurrentNavigationTarget(currentlyClickedButton);
            }
        }

        private void OnExternalNavigation(string screenId)
        {
            foreach (var button in _currentButtons)
            {
                button.SetCurrentNavigationTarget(screenId);
            }
        }

        private void ClearEntries()
        {
            foreach (var button in _currentButtons)
            {
                button.ButtonClicked -= OnNavigationButtonClicked;
                Destroy(button.gameObject);
            }
        }
    }
}