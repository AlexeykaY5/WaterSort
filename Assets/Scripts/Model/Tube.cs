using System;
using System.Collections.Generic;

public class Tube
{
    private readonly int capacity = 4;
    private List<int> contents = new List<int>();

    public Tube(int capacity)
    {
        this.capacity = capacity;
    }

    public Tube(int capacity, List<int> initialContents)
    {
        this.capacity = capacity;
        contents.AddRange(initialContents);
    }

    private bool IsFull => contents.Count >= capacity;

    public bool IsEmpty => contents.Count == 0;

    private int FreeSpace => capacity - contents.Count;

    public int? TopColor()
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

    public int TopGroupSize()
    {
        if (IsEmpty)
        {
            return 0;
        }
        int count = 1;
        int top = contents[contents.Count - 1];

        for(int i = contents.Count - 2; i >= 0; i--)
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

    public bool CanPourInto(Tube other)
    {
        if (IsEmpty)
        {
            return false;
        }else if (other.IsFull)
        {
            return false;
        }else if(this == other)
        {
            return false;
        }
        return true;
    }


    public int PourInto(Tube other)
    {
        if (!CanPourInto(other))
        {
            return 0;
        }

        int result =  Math.Min(TopGroupSize(), other.FreeSpace);
        int topColor = TopColor().Value;

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

        foreach(int content in contents)
        {
            if (content != contents[0])
            {
                return false;
            }
        }
        return true;
    }

    public List<int> GetContents()
    {
        return new List<int>(contents);
    }
}
