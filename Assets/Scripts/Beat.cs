using System;
using UnityEngine;

[Serializable]
public class Beat
{
    public string beatData;
    public int beatIndex;
    public Beat(string from, int index)
    {
        beatData = from;
        beatIndex = index;
    }
}
