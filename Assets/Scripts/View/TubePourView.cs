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
    [SerializeField] private float tiltSpeed = 2f;

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

        yield return Fly(homePosition, pourPosition);
        yield return Tilt(1f - tubeView.ShownFill);

        tubeView.ReleaseFill();
        target.tubeView.ReleaseFill();
        while (tubeView.IsFillMoving || target.tubeView.IsFillMoving)
        {
            animator.SetFloat("Pour", 1f - tubeView.ShownFill);
            yield return null;
        }

        yield return Tilt(0f);
        yield return Fly(pourPosition, homePosition);

        tubeCanvas.overrideSorting = false;
        IsPouring = false;
    }

    private IEnumerator Fly(Vector3 fromPosition, Vector3 toPosition)
    {
        float time = 0f;

        while (time < moveTime)
        {
            time += Time.deltaTime;
            tube.position = Vector3.Lerp(fromPosition, toPosition, time / moveTime);
            yield return null;
        }

        tube.position = toPosition;
    }

    private IEnumerator Tilt(float toPour)
    {
        float pour = animator.GetFloat("Pour");

        while (pour != toPour)
        {
            pour = Mathf.MoveTowards(pour, toPour, tiltSpeed * Time.deltaTime);
            animator.SetFloat("Pour", pour);
            yield return null;
        }
    }
}