using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SnapScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private HorizontalLayoutGroup layout;
    [SerializeField] private float snapSpeed = 10f;

    private bool isSnapping;
    private float targetX;

    public int CurrentIndex { get; private set; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isSnapping = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int nearest = Mathf.RoundToInt(-scrollRect.content.anchoredPosition.x / Step());
        SnapTo(nearest);
    }

    public void SnapTo(int index)
    {
        int count = scrollRect.content.childCount;
        CurrentIndex = Mathf.Clamp(index, 0, count - 1);
        targetX = -CurrentIndex * Step();
        isSnapping = true;
        scrollRect.StopMovement();
    }

    private void Update()
    {
        if (!isSnapping)
        {
            return;
        }

        Vector2 position = scrollRect.content.anchoredPosition;
        position.x = Mathf.Lerp(position.x, targetX, snapSpeed * Time.deltaTime);

        if (Mathf.Abs(position.x - targetX) < 0.5f)
        {
            position.x = targetX;
            isSnapping = false;
        }

        scrollRect.content.anchoredPosition = position;
    }

    private float Step()
    {
        RectTransform firstCard = (RectTransform)scrollRect.content.GetChild(0);
        return firstCard.rect.width + layout.spacing;
    }
}