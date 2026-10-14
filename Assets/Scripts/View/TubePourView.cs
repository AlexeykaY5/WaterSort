using System.Collections;
using UnityEngine;

public class TubePourView : MonoBehaviour
{
    [SerializeField] private TubeView tubeView;     // вода этой пробирки
    [SerializeField] private Animator animator;     // наклоняет Tilt по ползункам Side и Pour
    [SerializeField] private RectTransform tube;    // что летает (Tube)
    [SerializeField] private Canvas tubeCanvas;     // Canvas на Tube, чтобы рисоваться поверх
    [SerializeField] private Vector2 pourOffset = new Vector2(0.7f, 0.9f);
    [SerializeField] private float moveTime = 1f;

    public bool IsPouring { get; private set; }

    public void Play(TubePourView target)
    {
        float side;
        if (tube.position.x < target.tube.position.x)
        {
            side = -1f;     // я левее цели: встаю слева
        }
        else
        {
            side = 1f;      // я правее цели: встаю справа
        }

        animator.SetFloat("Side", -side);   // стою слева → наклон вправо (+1), справа → влево (−1)

        Vector2 size = target.tube.rect.size;
        Vector3 localPoint = new Vector3(side * size.x * pourOffset.x, size.y * pourOffset.y, 0f);
        Vector3 pourPosition = target.tube.TransformPoint(localPoint);

        StopAllCoroutines();
        StartCoroutine(PourRoutine(target, tube.position, pourPosition));
    }

    private IEnumerator PourRoutine(TubePourView target, Vector3 homePosition, Vector3 pourPosition)
    {
        IsPouring = true;
        tubeCanvas.overrideSorting = true;
        tubeCanvas.sortingOrder = 1;

        // 1. Подлёт: наклон растёт до угла, при котором вода у горлышка
        float startPour = 1f - tubeView.ShownFill;
        yield return Fly(homePosition, pourPosition, 0f, startPour);

        // 2. Переливание: ползунок идёт вслед за водой
        tubeView.ReleaseFill();
        target.tubeView.ReleaseFill();
        while (tubeView.IsFillMoving || target.tubeView.IsFillMoving)
        {
            animator.SetFloat("Pour", 1f - tubeView.ShownFill);
            yield return null;
        }

        // 3. Возврат: выпрямиться по дороге домой
        float endPour = 1f - tubeView.ShownFill;
        yield return Fly(pourPosition, homePosition, endPour, 0f);

        tubeCanvas.overrideSorting = false;
        IsPouring = false;
    }

    private IEnumerator Fly(Vector3 fromPosition, Vector3 toPosition, float fromPour, float toPour)
    {
        float time = 0f;

        while (time < moveTime)
        {
            time += Time.deltaTime;
            float t = time / moveTime;                                    // доля пути: от 0 до 1

            tube.position = Vector3.Lerp(fromPosition, toPosition, t);
            animator.SetFloat("Pour", Mathf.Lerp(fromPour, toPour, t));   // наклон вместе с полётом

            yield return null;
        }

        tube.position = toPosition;
        animator.SetFloat("Pour", toPour);
    }
}