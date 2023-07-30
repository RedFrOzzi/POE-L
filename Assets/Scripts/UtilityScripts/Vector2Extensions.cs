using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2 GetRandomPointInCircle(this Vector2 center, float radius)
    {
        return center - (Vector2)(Quaternion.Euler(0, 0, Random.Range(0, 360f)) * Vector2.up * radius);
    }

    public static Vector3 GetRandomPointInCircle(this Vector3 center, float radius)
    {
        return center - (Quaternion.Euler(0, 0, Random.Range(0, 360f)) * Vector3.up * radius);
    }
}
