using UnityEngine;
using TMPro;
using System;


public class GameTimer : MonoBehaviour
{

    public event Action TimeUp;
    public TextMeshProUGUI timerText;
    public bool isTimePassed = false;
    public float TimeLeft { get; private set; }
    private bool isRunning = false;

    public void StartTimer(int time)
    {
        TimeLeft = time;
        isRunning = true;
        isTimePassed = false;
        UpdateTimerText();
    }

    public void AddBonusTime(int bonusTime)
    {
        TimeLeft += bonusTime;
        UpdateTimerText();
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void UpdateTimerText()
    {
        timerText.text = TimeFormat.ToMinutesSeconds(TimeLeft);
    }

    void Update()
    {
        if (!isRunning)
        {
            return;
        }

        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0)
        {
            TimeLeft = 0f;
            isRunning = false;
            isTimePassed = true;
            TimeUp?.Invoke();
        }

        UpdateTimerText();
    }
}
