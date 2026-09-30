using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class GameHudView : MonoBehaviour
{
    public event Action MenuClicked;

    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button menuButton;

    private int shownSeconds = -1;

    private void Awake()
    {
        menuButton.onClick.AddListener(() => MenuClicked?.Invoke());
    }

    public void SetDifficulty(string text)
    {
        difficultyText.text = text;
    }

    public void SetTime(float seconds)
    {
        int wholeSeconds = Mathf.CeilToInt(seconds);

        if (wholeSeconds == shownSeconds)
        {
            return;
        }

        shownSeconds = wholeSeconds;
        timerText.text = TimeFormat.ToMinutesSeconds(seconds);
    }
}
