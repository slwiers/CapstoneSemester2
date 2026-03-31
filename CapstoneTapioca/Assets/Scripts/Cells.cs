using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public struct Cells
{
    public enum Type
    {
        Invalid,
        Empty,
        Mine,
        Number,
    }

    public Vector3Int position;
    public Type type;
    public int number;
    public bool revealed;
    public bool flagged;
    public bool exploded;
}
