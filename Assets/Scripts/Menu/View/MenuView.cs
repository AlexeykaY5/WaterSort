using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    [SerializeField] private MenuController controller;
    [SerializeField] private LevelCardView cardPrefab;
    [SerializeField] private RectTransform cardsContainer;
    [SerializeField] private SnapScroll snapScroll;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button playButton;

    private void Awake()
    {
        nextButton.onClick.AddListener(() => snapScroll.SnapTo(snapScroll.CurrentIndex + 1));
        previousButton.onClick.AddListener(() => snapScroll.SnapTo(snapScroll.CurrentIndex - 1));
        playButton.onClick.AddListener(() => controller.Play(snapScroll.CurrentIndex));
    }

    private void Start()
    {
        for(int i = 0; i < controller.LevelCount; i++)
        {
            CreateCard(controller.GetLevelName(i), controller.GetRecord(i));
        }
    }

    private void CreateCard(string title, float? bestTime)
    {
        LevelCardView card = Instantiate(cardPrefab, cardsContainer);
        card.Show(title, bestTime);
    }
}