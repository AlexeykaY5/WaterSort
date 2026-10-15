using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TubeView : MonoBehaviour
{
    public event Action<int> Clicked;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform tilt;
    [SerializeField] private Button button;
    [SerializeField] private Image liquidImage;
    [SerializeField] private TubePourView pour;
    [SerializeField] private float fillSpeed = 0.7f;

    private int tubeIndex;
    private Material material;
    private Texture2D layers;
    private float shownFill;
    private float targetFill;
    private List<Color> pendingColors;
    private bool holdFill;

    public TubePourView Pour => pour;
    public float ShownFill => shownFill;
    public bool IsFillMoving => shownFill != targetFill;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
        material = new Material(liquidImage.material);
        liquidImage.material = material;
    }

    private void Update()
    {
        material.SetFloat("_Angle", tilt.eulerAngles.z);

        if (holdFill || shownFill == targetFill)
        {
            return;
        }

        shownFill = Mathf.MoveTowards(shownFill, targetFill, fillSpeed * Time.deltaTime);
        material.SetFloat("_FillAmount", shownFill);

        if (shownFill == targetFill && pendingColors != null)
        {
            WriteColors(pendingColors);
            pendingColors = null;
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
        Destroy(material);
        Destroy(layers);
    }

    public void ResetFill()
    {
        shownFill = 0f;
        targetFill = 0f;
        holdFill = false;
        pendingColors = null;
        material.SetFloat("_FillAmount", 0f);
    }

    public void Init(int tubeIndex)
    {
        this.tubeIndex = tubeIndex;
    }

    public void Build(int capacity)
    {
        if (layers != null)
        {
            Destroy(layers);
        }

        layers = new Texture2D(1, capacity, TextureFormat.RGBA32, false);
        layers.filterMode = FilterMode.Point;
        layers.wrapMode = TextureWrapMode.Clamp;

        material.SetTexture("_Layers", layers);
        material.SetFloat("_FillAmount", shownFill);
    }

    public void Render(IReadOnlyList<Color> tubeColors, bool waitForPour)
    {
        targetFill = (float)tubeColors.Count / layers.height;
        holdFill = waitForPour;

        if (targetFill >= shownFill)
        {
            WriteColors(tubeColors);
            pendingColors = null;
        }
        else
        {
            pendingColors = new List<Color>(tubeColors);
        }
    }


    public void SetSelected(bool selected)
    {
        if (selected)
        {
            rectTransform.anchoredPosition = new Vector2(0, 30f);
        }
        else
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public void ReleaseFill()
    {
        holdFill = false;
    }

    private void OnClick()
    {
        Clicked?.Invoke(tubeIndex);
    }

    private void WriteColors(IReadOnlyList<Color> colors)
    {
        for (int i = 0; i < layers.height; i++)
        {
            if (i < colors.Count)
            {
                layers.SetPixel(0, i, colors[i]);
            }
            else
            {
                layers.SetPixel(0, i, Color.clear);
            }
        }
        layers.Apply();
    }
}