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
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private BoardView boardView;

    private bool waitingToShow;

    private void Awake()
    {
        restartButton.onClick.AddListener(() => RestartClicked?.Invoke());
        menuButton.onClick.AddListener(() => MenuClicked?.Invoke());
    }

    private void Update()
    {
        if(waitingToShow && !boardView.IsAnyPouring())
        {
            waitingToShow = false;
            SetVisible(true);
        }
    }

    public void Hide()
    {
        waitingToShow = false;
        SetVisible(false);
    }

    public void ShowLose()
    {
        text.text = "Time is up";
        restartButton.gameObject.SetActive(true);
        waitingToShow = true;
    }

    public void ShowWin()
    {
        text.text = "You Win!";
        text.color = Color.green;
        restartButton.gameObject.SetActive(false);
        waitingToShow = true;
    }
    
    private void SetVisible(bool visible)
    {
        if (visible)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            canvasGroup.alpha = 0f;
        }

        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }

}
