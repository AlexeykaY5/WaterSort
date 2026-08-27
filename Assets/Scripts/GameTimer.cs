using UnityEngine;
using TMPro;
using System;


public class GameTimer : MonoBehaviour
{

    public event Action TimeUp;
    public TextMeshProUGUI timerText;
    public bool isTimePassed = false;
    private float timeLeft;
    private bool isRunning = false;

    public void StartTimer(int time)
    {
        timeLeft = time;
        isRunning = true;
        isTimePassed = false;
        UpdateTimerText();
    }

    public void AddBonusTime(int bonusTime)
    {
        timeLeft += bonusTime;
        UpdateTimerText();
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void UpdateTimerText()
    {
        int totalSeconds = Mathf.CeilToInt(timeLeft);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        timerText.text = $"{minutes:00}:{seconds:00}";

    }

    void Update()
    {
        if (!isRunning)
        {
            return;
        }

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            timeLeft = 0f;
            isRunning = false;
            isTimePassed = true;
            TimeUp?.Invoke();
        }

        UpdateTimerText();
    }
}
