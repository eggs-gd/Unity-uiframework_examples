using System;
using System.Collections.Generic;
using eggsgd.UiFramework.Examples.Extras;
using eggsgd.UiFramework.Examples.Widgets;
using eggsgd.UiFramework.Window;
using UnityEngine;

namespace eggsgd.UiFramework.Examples.ScreenControllers
{
    /// <summary>
    ///     This is the Properties class for this specific window.
    ///     It carries the payload which will be used to fill up this
    ///     window upon opening.
    /// </summary>
    [Serializable]
    public class PlayerWindowProperties : WindowProperties
    {
        public readonly List<PlayerDataEntry> PlayerData;

        public PlayerWindowProperties(List<PlayerDataEntry> data) => PlayerData = data;
    }

    public class PlayerWindowController : AWindowController<PlayerWindowProperties>
    {
        [SerializeField]
        private LevelProgressComponent templateLevelEntry;

        private readonly List<LevelProgressComponent> _currentLevels = new();

        /// <summary>
        ///     Here I'm listening to a global signal that is fired by the ScriptableObject
        ///     itself as a way of exemplifying how you could do this in your codebase.
        ///     I could optionally carry the ScriptableObject itself, store a reference to it
        ///     and do the same process via direct event hooks.
        /// </summary>
        protected override void AddListeners()
        {
            Signals.SignalBus.Get<PlayerDataUpdatedSignal>().AddListener(OnDataUpdated);
        }

        protected override void RemoveListeners()
        {
            Signals.SignalBus.Get<PlayerDataUpdatedSignal>().RemoveListener(OnDataUpdated);
        }

        protected override void OnPropertiesSet()
        {
            OnDataUpdated(Properties.PlayerData);
        }

        private void OnDataUpdated(List<PlayerDataEntry> data)
        {
            VerifyElementCount(data.Count);
            RefreshElementData(data);
        }

        private void VerifyElementCount(int levelCount)
        {
            if (_currentLevels.Count == levelCount)
            {
                return;
            }

            if (_currentLevels.Count < levelCount)
            {
                while (_currentLevels.Count < levelCount)
                {
                    var newLevel = Instantiate(templateLevelEntry,
                        templateLevelEntry.transform.parent,
                        false); // Never forget to pass worldPositionStays as false for UI!
                    newLevel.gameObject.SetActive(true);
                    _currentLevels.Add(newLevel);
                }
            }
            else
            {
                while (_currentLevels.Count > levelCount)
                {
                    var levelToRemove = _currentLevels[_currentLevels.Count - 1];
                    _currentLevels.Remove(levelToRemove);
                    Destroy(levelToRemove.gameObject);
                }
            }
        }

        private void RefreshElementData(List<PlayerDataEntry> playerLevelProgress)
        {
            for (var i = 0; i < _currentLevels.Count; i++)
            {
                _currentLevels[i].SetData(playerLevelProgress[i], i);
            }
        }
    }
}