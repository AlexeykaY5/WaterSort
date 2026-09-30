using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
    public event Action RestartClicked;
    public event Action MenuClicked;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() => RestartClicked?.Invoke());
        menuButton.onClick.AddListener(() => MenuClicked?.Invoke());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowLose()
    {
        gameObject.SetActive(true);
        text.text = "Time is up";
        restartButton.gameObject.SetActive(true);
    }

    public void ShowWin()
    {
        gameObject.SetActive(true);
        text.text = "You Win!";
        restartButton.gameObject.SetActive(false);
    }
}
