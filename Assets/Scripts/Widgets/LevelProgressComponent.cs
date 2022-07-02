using eggsgd.UiFramework.Examples.Extras;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace eggsgd.UiFramework.Examples.Widgets
{
    public class LevelProgressComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelCounter;
        [SerializeField] private TextMeshProUGUI levelName;
        [SerializeField] private Image[] stars;
        [SerializeField] private Color starOn = Color.yellow;
        [SerializeField] private Color starOff = Color.black;

        public void SetData(PlayerDataEntry entry, int levelNumber)
        {
            levelCounter.text = "Level " + (levelNumber + 1);
            levelName.text = entry.LevelName;
            for (var i = 0; i < stars.Length; i++)
            {
                stars[i].color = i + 1 <= entry.Stars
                    ? starOn
                    : starOff;
            }
        }
    }
}