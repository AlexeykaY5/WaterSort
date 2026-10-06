using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TubeView : MonoBehaviour
{
    public event Action<int> Clicked;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Button button;
    [SerializeField] private Image liquidImage;
    [SerializeField] private float fillSpeed = 1.5f;

    private int tubeIndex;
    private Material material;
    private Texture2D layers;
    private float shownFill;
    private float targetFill;
    private List<Color> pendingColors;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
        material = new Material(liquidImage.material);
        liquidImage.material = material;
    }

    private void Update()
    {
        material.SetFloat("_Angle", rectTransform.eulerAngles.z);

        if(shownFill == targetFill)
        {
            return;
        }

        shownFill = Mathf.MoveTowards(shownFill, targetFill, fillSpeed * Time.deltaTime);
        material.SetFloat("_FillAmount", shownFill);

        if(shownFill == targetFill && pendingColors != null)
        {
            WrittenColors(pendingColors);
            pendingColors = null;
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
        Destroy(material);
        Destroy(layers);
    }

    public void Build(int capacity)
    {
        if(layers != null)
        {
            Destroy(layers);
        }

        layers = new Texture2D(1, capacity, TextureFormat.RGBA32, false);
        layers.filterMode = FilterMode.Point;
        layers.wrapMode = TextureWrapMode.Clamp;

        material.SetTexture("_Layers", layers);
        material.SetFloat("_FillAmount", shownFill);
    }

    public void Render(IReadOnlyList<Color> tubeColors)
    {
        targetFill = (float)tubeColors.Count / layers.height;
        if (targetFill >= shownFill)
        {
            WrittenColors(tubeColors);
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

    //public void SetPoured(bool selected)
    //{
    //    if (selected)
    //    {
    //        rectTransform.anchoredPosition = new Vector2(0, 30f);
    //    }
    //    else
    //    {
    //        rectTransform.anchoredPosition = Vector2.zero;
    //    }
    //}

    public void Init(int tubeIndex)
    {
        this.tubeIndex = tubeIndex;
    }

    private void OnClick()
    {
        Clicked?.Invoke(tubeIndex);
    }

    private void WrittenColors(IReadOnlyList<Color> colors)
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