using TMPro;
using UnityEngine;

public class DifficultyCardView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI recordText;

    public void Show(string title, float? bestTime)
    {
        titleText.text = title;

        if (bestTime.HasValue)
        {
            recordText.text = TimeFormat.ToMinutesSeconds(bestTime.Value);
        }
        else
        {
            recordText.text = " - ";
        }
    }
}
