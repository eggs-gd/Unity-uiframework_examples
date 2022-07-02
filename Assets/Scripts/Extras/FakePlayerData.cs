using System;
using System.Collections.Generic;
using eggsgd.Signals;
using UnityEngine;

namespace eggsgd.UiFramework.Examples.Extras
{
    public class PlayerDataUpdatedSignal : ASignal<List<PlayerDataEntry>>
    {
    }

    [Serializable]
    public class PlayerDataEntry
    {
        public string LevelName;

        [Range(0, 3)]
        public int Stars;
    }

    [CreateAssetMenu(fileName = "PlayerData", menuName = "eGGs.gd UI/Examples/Fake Player Data")]
    public class FakePlayerData : ScriptableObject
    {
        [SerializeField]
        private List<PlayerDataEntry> levelProgress;

        public List<PlayerDataEntry> LevelProgress => levelProgress;

        /// <summary>
        ///     This is called by the Unity Editor in MonoBehaviours and
        ///     ScriptableObjects whenever a value is changed in the inspector.
        ///     Here I'm using it to propagate the changes for the example,
        ///     but in practice, you could implement the same kind of behaviour
        ///     by having an observable variable passed into the screen via its
        ///     properties, or other forms of data-binding data to the controller.
        /// </summary>
        private void OnValidate()
        {
            Signals.SignalBus.Get<PlayerDataUpdatedSignal>().Dispatch(levelProgress);
        }
    }
}