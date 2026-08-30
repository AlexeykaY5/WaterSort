using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public DifficultySettings[] difficultySettings;
    public Canvas canvas;
    private float touch;
    private bool isDragging;
    public RectTransform[] cards;
    private float step = 1000f;
    private float offset;
    private float target = 0f;
    private float smoothSpeed = 10f;
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI record;
    public Board board;

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        touch = offset;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float razn = (eventData.position.x - eventData.pressPosition.x) / canvas.scaleFactor;
        offset = touch - razn;
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
        float total = cards.Length * step;

        for(int i = 0; i < cards.Length; i++)
        {
            float raw = i * step - offset;
            float halfTotal = total / 2f;
            float wrapped = PositiveMod(raw + halfTotal, total) - halfTotal;
            cards[i].anchoredPosition = new Vector2(wrapped, 0f);
        }
    }

    public void NextCard()
    {
        target += step;
    }

    public void PreviousCard()
    {
        target -= step;
    }

    public void Play()
    {
        int difficulty = Mathf.RoundToInt(target / step);

        difficulty = (int)PositiveMod(difficulty, difficultySettings.Length);

        board.StartGame(difficultySettings[difficulty]);
    }

    private void Update()
    {
        if (!isDragging)
        {
            offset = Mathf.Lerp(offset, target, smoothSpeed * Time.deltaTime);
        }
        SetPosition();
    }
}