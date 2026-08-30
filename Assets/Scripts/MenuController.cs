using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public interface IStartDragHandler { };
    public interface IDragHandler { };
    public interface IEndDragHandler { };
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
    private int difficulty = 0;

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

    private void Update()
    {
        offset = Mathf.Lerp(offset, target, smoothSpeed * Time.deltaTime);
        SetPosition();
    }
}