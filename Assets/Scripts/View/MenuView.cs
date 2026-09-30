using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public event Action<int> PlayClicked;

    [SerializeField] private DifficultyCardView cardPrefab;
    [SerializeField] private RectTransform cardsContainer;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Canvas canvas;

    private float step = 1000f;
    private float offset;
    private float target = 0f;
    private float smoothSpeed = 10f;
    private readonly List<DifficultyCardView> cards = new List<DifficultyCardView>();

    private bool isDragging;
    private float dragStartOffset;

    private void Awake()
    {
        nextButton.onClick.AddListener(NextCard);
        previousButton.onClick.AddListener(PreviousCard);

        playButton.onClick.AddListener(Play);
    }

    private void Update()
    {
        if (!isDragging)
        {
            offset = Mathf.Lerp(offset, target, smoothSpeed * Time.deltaTime);
        }
        SetPosition();
    }

    public void AddCard(string title, float? bestTime)
    {
        DifficultyCardView card = Instantiate(cardPrefab, cardsContainer);

        card.Show(title, bestTime);
        cards.Add(card);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        dragStartOffset = offset;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float delta = (eventData.position.x - eventData.pressPosition.x) / canvas.scaleFactor;
        offset = dragStartOffset - delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        target = Mathf.Round(offset / step) * step;
    }

    private float PositiveMod(float value, float length)
    {
        float mod = value % length;
        if (mod < 0)
        {
            mod += length;
        }
        return mod;
    }

    private void SetPosition()
    {
        float total = cards.Count * step;

        for(int i = 0; i < cards.Count; i++)
        {
            float raw = i * step - offset;
            float halfTotal = total / 2f;
            float wrapped = PositiveMod(raw + halfTotal, total) - halfTotal;
            cards[i].SetPositionX(wrapped);
        }
    }

    private void NextCard() => target += step;

    private void PreviousCard() => target -= step;

    private void Play()
    {
        int index = Mathf.RoundToInt(target / step);

        index = (int)PositiveMod(index, cards.Count);

        PlayClicked?.Invoke(index);
    }
}