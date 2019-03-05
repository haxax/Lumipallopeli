using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// If GetComponent is unable to find the Component, tries to find from scene
    /// </summary>
    static public T GetOrFindComponent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() ?? Object.FindObjectOfType<T>();
    }


    /// <summary>
    /// Shuffle list
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, list.Count);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    /// <summary>
    /// Makes sprites to overlap correctly on Y-axis
    /// </summary>
    public static void SetVerticalSpriteOrder(Transform transform, SpriteRenderer rend)
    {
        rend.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
    }


    /// <summary>
    /// Randomise the position within the given radius
    /// </summary>
    public static Vector3 FluctuatePosition(Vector3 position, float radius)
    {
        return position + (Vector3.Normalize(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0)) * radius);
    }


    /// <summary>
    /// Outputs random color hue within wanted saturation
    /// </summary>
    public static Color RandomColor(Vector2 capValues)
    { return RandomColor(capValues, 1); }

    public static Color RandomColor(Vector2 capValues, float alpha)
    {
        List<float> rgbValues = new List<float>();
        rgbValues.Add(capValues.x);
        rgbValues.Add(capValues.y);
        rgbValues.Add(Random.Range(capValues.x, capValues.y));

        Color newColor = new Color();
        int i = Random.Range(0, 3);
        newColor.r = rgbValues[i];
        rgbValues.RemoveAt(i);

        i = Random.Range(0, 2);
        newColor.g = rgbValues[i];
        rgbValues.RemoveAt(i);

        newColor.b = rgbValues[0];
        newColor.a = 1;
        return newColor;
    }


    /// <summary>
    /// Outputs vector with every axis randomised from between capValues
    /// </summary>
    public static Vector3 AllRandomVector(Vector2 capValues)
    {
        return new Vector3(Random.Range(capValues.x, capValues.y), Random.Range(capValues.x, capValues.y), Random.Range(capValues.x, capValues.y));
    }


    /// <summary>
    /// Outputs vector with every axis having the same randomised value from between capValues
    /// </summary>
    public static Vector3 SameRandomVector(Vector2 capValues)
    {
        float rnd = Random.Range(capValues.x, capValues.y);
        return new Vector3(rnd, rnd, rnd);
    }


    /// <summary>
    /// Converts coordinate Char to Byte
    /// </summary>
    /// <param name="c">Coordinate Char</param>
    /// <param name="aValue">Coordinate value for 'a'</param>
    /// <param name="dir">1 if value is ascending, -1 if decending</param>
    public static byte CharToCoordinate(char c, int aValue, int dir)
    {
        char cc = char.ToLower(c);
        if (aValue + (dir * (cc - 'a')) < 0) { return 255; }
        if (aValue + (dir * (cc - 'a')) > 255) { return 255; }
        return (byte)(aValue + (dir * (cc - 'a')));
    }
}

[System.Serializable]
public struct BytePair
{
    //Pair of Bytes, used to handle coordinates

    public byte x;
    public byte y;

    public BytePair(byte X, byte Y)
    {
        x = X;
        y = Y;
    }


    /// <summary>
    /// Table of coordinates generated into one list
    /// </summary>
    public static List<BytePair> CreateBytePairList(int aAmount, int bAmount)
    {
        List<BytePair> pairs = new List<BytePair>();
        for (int i = 0; i < aAmount; i++)
        {
            for (int j = 0; j < bAmount; j++)
            {
                pairs.Add(new BytePair((byte)i, (byte)j));
            }
        }
        return pairs;
    }


    /// <summary>
    /// Converts letter+number coordinate into BytePair
    /// Y-axis = letter, X-axis = number
    /// In case of error, 255 is returned as value
    /// </summary>
    /// <param name="w">Letter+number coordinate</param>
    /// <param name="aValue">Coordinate value for 'a'</param>
    /// <param name="dir">1 if value is ascending, -1 if decending</param>
    public static BytePair StringListToCoordinates(List<string> w, int aValue, int dir)
    {
        //TODO improve to support higher values and spacebar
        BytePair result = new BytePair(255, 255);
        if (char.IsLetter(w[0][0]) && char.IsDigit(w[0][1]))
        {
            result.y = Extensions.CharToCoordinate(w[0][0], aValue, dir);
            result.x = byte.Parse("" + w[0][1]);
        }
        else if (char.IsLetter(w[0][1]) && char.IsDigit(w[0][0]))
        {
            result.y = Extensions.CharToCoordinate(w[0][1], aValue, dir);
            result.x = byte.Parse("" + w[0][0]);
        }
        return result;
    }


    public override string ToString()
    {
        return "x: " + x + " y: " + y;
    }


    public Vector2 ToVector2()
    {
        return new Vector2((float)x, (float)y);
    }
    public Vector2 ToPositionVector2()
    {
        return new Vector2((float)x + 0.5f, (float)y + 0.5f);
    }


    public static bool operator !=(BytePair a, BytePair b)
    {
        if (a.x != b.x) { return true; }
        else if (a.y != b.y) { return true; }
        return false;
    }
    public static bool operator ==(BytePair a, BytePair b)
    {
        if (a.x == b.x && a.y == b.y) { return true; }
        return false;
    }
}
