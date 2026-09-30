using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TubeView : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Button button;
    [SerializeField] private Image slotPrefab;
    [SerializeField] private RectTransform slotsContainer;


    private int tubeIndex;

    private List<Image> slots = new List<Image>();

    public event Action<int> Clicked;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }

    public void Build(int capacity)
    {
        foreach(Image slot in slots)
        {
            Destroy(slot.gameObject);
        }

        slots.Clear();

        for(int i = 0; i < capacity; i++)
        {
            Image slot = Instantiate(slotPrefab, slotsContainer);
            slots.Add(slot);
        }
    }

    public void Render(IReadOnlyList<Color> tubeColors)
    {

        for(int i = 0; i < slots.Count; i++)
        {
            int level = slots.Count - 1 - i;
            if(tubeColors.Count > level)
            {
                slots[i].color = tubeColors[level];
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