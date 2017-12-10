using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PathHelper
{
    public static float Heuristic(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}