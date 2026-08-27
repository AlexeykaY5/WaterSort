using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TubeView : MonoBehaviour
{
    public Image[] slots;
    public Color[] colors;
    private int tubeIndex;
    private Board board;
    public RectTransform rectTransform;

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
        board.OnTubeClicked(tubeIndex);
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

    public void Init(int tubeIndex, Board board)
    {
        this.tubeIndex = tubeIndex;
        this.board = board;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
}