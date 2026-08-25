using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TubeView : MonoBehaviour
{
    public Image[] slots;
    public Color[] colors;
    public int tubeIndex;
    public Board board;

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

    public void Init(int tubeIndex, Board board)
    {
        this.tubeIndex = tubeIndex;
        this.board = board;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
}
