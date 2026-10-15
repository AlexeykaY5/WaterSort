using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuView : MonoBehaviour
{
    public event Action<int> PlayClicked;

    [SerializeField] private DifficultyCardView cardPrefab;
    [SerializeField] private RectTransform cardsContainer;
    [SerializeField] private SnapScroll snapScroll;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button playButton;

    private ConfigDatabase configs;
    private RecordStorage recordStorage;

    [Inject]
    public void Construct(ConfigDatabase configs, RecordStorage recordStorage)
    {
        this.configs = configs;
        this.recordStorage = recordStorage;
    }

    private void Awake()
    {
        nextButton.onClick.AddListener(() => snapScroll.SnapTo(snapScroll.CurrentIndex + 1));
        previousButton.onClick.AddListener(() => snapScroll.SnapTo(snapScroll.CurrentIndex - 1));
        playButton.onClick.AddListener(() => PlayClicked?.Invoke(snapScroll.CurrentIndex));
    }

    private void Start()
    {
        foreach (DifficultySettings difficulty in configs.Difficulties)
        {
            CreateCard(difficulty.DifficultyName, recordStorage.Load(difficulty.Id));
        }
    }

    private void CreateCard(string title, float? bestTime)
    {
        DifficultyCardView card = Instantiate(cardPrefab, cardsContainer);
        card.Show(title, bestTime);
    }
}