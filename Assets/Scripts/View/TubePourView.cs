using System.Collections;
using UnityEngine;

public class TubePourView : MonoBehaviour
{
    [SerializeField] private TubeView tubeView;
    [SerializeField] private Animator animator;
    [SerializeField] private RectTransform tube;
    [SerializeField] private Canvas tubeCanvas;
    [SerializeField] private Vector2 pourOffset = new Vector2(0.7f, 0.9f);
    [SerializeField] private float moveTime = 1f;

    public bool IsPouring { get; private set; }

    public void Play(TubePourView target)
    {
        float side;
        if (tube.position.x < target.tube.position.x)
        {
            side = -1f;
        }
        else
        {
            side = 1f;
        }

        animator.SetFloat("Side", -side);

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

        float startPour = 1f - tubeView.ShownFill;
        yield return Fly(homePosition, pourPosition, 0f, startPour);

        tubeView.ReleaseFill();
        target.tubeView.ReleaseFill();
        while (tubeView.IsFillMoving || target.tubeView.IsFillMoving)
        {
            animator.SetFloat("Pour", 1f - tubeView.ShownFill);
            yield return null;
        }

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
            float t = time / moveTime;

            tube.position = Vector3.Lerp(fromPosition, toPosition, t);
            animator.SetFloat("Pour", Mathf.Lerp(fromPour, toPour, t));

            yield return null;
        }

        tube.position = toPosition;
        animator.SetFloat("Pour", toPour);
    }
}