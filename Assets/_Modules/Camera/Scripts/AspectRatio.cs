using System;
using UnityEngine;

public static class AspectRatio
{
    public static Vector2Int GetAspectRatio(int x, int y)
    {
        float f = (float) x / (float) y;
        int i = 0;
        while (true)
        {
            i++;
            if (Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
                break;
        }

        return new Vector2Int((int) Math.Round(f * i, 2), i);
    }

    public static bool GetAspectRatio(int x, int y, out Vector2Int ratio)
    {
        ratio = Vector2Int.one;
        float f = (float) x / (float) y;

        if (f >= 0.42 && f < 0.44f)
        {
            ratio = new Vector2Int(9, 21);
        }
        else if(f >= 0.44 && f < 0.46f)
        {
            ratio = new Vector2Int(9, 20);
        }
        else if(f >= 0.46 && f < 0.5f)
        {
            ratio = new Vector2Int(18, 39);
        }
        else if(f >= 0.5 && f < 0.56f)
        {
            ratio = new Vector2Int(9, 18);
        }
        else if(f >= 0.56 && f < 0.6f)
        {
            ratio = new Vector2Int(9, 16);
        }
        else if(f >= 0.6 && f < 0.7f)
        {
            ratio = new Vector2Int(2, 3);
        }
        else if(f >= 0.7)
        {
            ratio = new Vector2Int(3, 4);
        }
        else
        {
            return false;
        }
        return true;
    }
    
    public static Vector2Int GetAspectRatio(Vector2 xy)
    {
        float f = xy.x / xy.y;
        int i = 0;
        while (true)
        {
            i++;
            if (Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
                break;
        }

        return new Vector2Int((int) Math.Round(f * i, 2), i);
    }

    public static Vector2Int GetAspectRatio(int x, int y, bool debug)
    {
        float f = (float) x / (float) y;
        int i = 0;
        while (true)
        {
            i++;
            if (Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
                break;
        }

        if (debug)
            Debug.Log("Aspect ratio is " + f * i + ":" + i + " (Resolution: " + x + "x" + y + ")");
        return new Vector2Int((int) Math.Round(f * i, 2), i);
    }

    public static Vector2Int GetAspectRatio(Vector2 xy, bool debug)
    {
        float f = xy.x / xy.y;
        int i = 0;
        while (true)
        {
            i++;
            if (Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
                break;
        }

        if (debug)
            Debug.Log("Aspect ratio is " + f * i + ":" + i + " (Resolution: " + xy.x + "x" + xy.y + ")");
        return new Vector2Int((int) Math.Round(f * i, 2), i);
    }
}