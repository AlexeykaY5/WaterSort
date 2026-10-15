using System;
using System.Collections.Generic;

public class Tube : ITubeState
{
    private readonly int capacity;
    private List<string> contents = new List<string>();

    public bool IsEmpty => contents.Count == 0;

    public IReadOnlyList<string> Contents => contents;

    public int Capacity => capacity;

    private bool IsFull => contents.Count >= capacity;

    private int FreeSpace => capacity - contents.Count;

    public Tube(int capacity)
    {
        this.capacity = capacity;
    }

    public void Fill(List<string> colors)
    {
        contents.Clear();
        contents.AddRange(colors);
    }

    public void Clear()
    {
        contents.Clear();
    }
    
    public string TopColor()
    {
        if (IsEmpty)
        {
            return null;
        }
        else
        {
            return contents[contents.Count - 1];
        }
    }

    public int PourInto(Tube other)
    {
        if (!CanPourInto(other))
        {
            return 0;
        }

        int result =  Math.Min(TopGroupSize(), other.FreeSpace);
        string topColor = TopColor();

        for(int i = 0; i < result; i++)
        {
            contents.RemoveAt(contents.Count - 1);
            other.contents.Add(topColor);
        }
        return result;
    }

    public bool IsSingleColor()
    {
        if (!IsFull)
        {
            return false;
        }

        foreach(string content in contents)
        {
            if (content != contents[0])
            {
                return false;
            }
        }
        return true;
    }

    private bool CanPourInto(Tube other)
    {
        if (IsEmpty)
        {
            return false;
        }
        else if (other.IsFull)
        {
            return false;
        }
        else if (this == other)
        {
            return false;
        }
        return true;
    }

    private int TopGroupSize()
    {
        if (IsEmpty)
        {
            return 0;
        }
        int count = 1;
        string top = contents[contents.Count - 1];

        for (int i = contents.Count - 2; i >= 0; i--)
        {
            if (contents[i] == top)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        return count;
    }
}
