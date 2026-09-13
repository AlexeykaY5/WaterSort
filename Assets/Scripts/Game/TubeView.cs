using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TubeView : MonoBehaviour
{

    [SerializeField] private Image[] slots;
    [SerializeField] private Color[] colors;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Button button;

    private int tubeIndex;

    public event Action<int> Clicked;


    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }

    public void Render(Tube tube)
    {
        List<WaterColor> contents = tube.GetContents();

        for(int i = 0; i < slots.Length; i++)
        {
            int level = slots.Length - 1 - i;
            if(contents.Count > level)
            {
                slots[i].color = colors[(int)contents[level]];
            }
            else
            {
                slots[i].color = Color.clear;
            }
        }
    }

    private void OnClick()
    {
        Clicked?.Invoke(tubeIndex);
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

    public void Init(int tubeIndex)
    {
        this.tubeIndex = tubeIndex;
    }
}