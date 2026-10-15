using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameOverView : MonoBehaviour
{
    public event Action RestartClicked;
    public event Action MenuClicked;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private BoardView boardView;

    private IGameState state;
    private bool isShown;

    [Inject]
    public void Construct(IGameState state)
    {
        this.state = state;
    }

    private void Awake()
    {
        restartButton.onClick.AddListener(() => RestartClicked?.Invoke());
        menuButton.onClick.AddListener(() => MenuClicked?.Invoke());
        SetVisible(false);
    }

    private void Update()
    {
        if (!state.IsOver)
        {
            if (isShown)
            {
                SetVisible(false);
            }
            return;
        }

        if (!isShown && !boardView.IsBusy())
        {
            ShowResult();
            SetVisible(true);
        }
    }

    private void ShowResult()
    {
        if (state.IsWon)
        {
            text.text = "You Win!";
            text.color = Color.green;
            restartButton.gameObject.SetActive(false);
        }
        else
        {
            text.text = "Time is up";
            text.color = Color.red;
            restartButton.gameObject.SetActive(true);
        }
    }

    private void SetVisible(bool visible)
    {
        isShown = visible;

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