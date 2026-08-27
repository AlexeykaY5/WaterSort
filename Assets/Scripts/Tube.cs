using System;
using System.Collections.Generic;

public enum WaterColor 
{
    Red, Green, Blue, Yellow, Purple, Black, White, Orange
}

public class Tube
{
    private readonly int capacity = 4;
    private List<WaterColor> contents = new List<WaterColor>();

    public Tube(int capacity)
    {
        this.capacity = capacity;
    }

    public Tube(int capacity, List<WaterColor> initialContents)
    {
        this.capacity = capacity;
        this.contents.AddRange(initialContents);
    }

    public bool IsFull()
    {
        return contents.Count >= capacity;
    }

    public bool IsEmpty()
    {
        return contents.Count == 0;
    }

    public int FreeSpace()
    {
        return capacity - contents.Count;
    }
    
    public WaterColor? TopColor()
    {
        if (IsEmpty())
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
        if (IsEmpty())
        {
            return 0;
        }
        int count = 1;
        WaterColor top = contents[contents.Count - 1];

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
        if (this.IsEmpty())
        {
            return false;
        }else if (other.IsFull())
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

        int result =  Math.Min(this.TopGroupSize(), other.FreeSpace());
        WaterColor topColor = this.TopColor().Value;

        for(int i = 0; i < result; i++)
        {
            this.contents.RemoveAt(this.contents.Count - 1);
            other.contents.Add(topColor);
        }
        return result;
    }

    public bool IsSingleColor()
    {
        if (!IsFull())
        {
            return false;
        }

        foreach(WaterColor content in contents)
        {
            if (content != contents[0])
            {
                return false;
            }
        }
        return true;
    }

    public List<WaterColor> GetContents()
    {
        return new List<WaterColor>(contents);
    }
}
