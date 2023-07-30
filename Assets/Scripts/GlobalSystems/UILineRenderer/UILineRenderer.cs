using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UILineRenderer : Graphic
{
    private List<Vector2> points = new();

    private Color newColor;

    [SerializeField] private float thickness = 5;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points.Count < 2) { return; }

        float angle = 0;

        for (int i = 0; i < points.Count; i++)
        {
            if (i < points.Count - 1)
            {
                angle = GetAngle(points[i], points[i + 1]) + 45f;
            }

            DrawVerticesForPoint(points[i], vh, angle);
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            int index = i * 2;
            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index);
        }
    }

    private void DrawVerticesForPoint(Vector2 point, VertexHelper vh, float angle)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = newColor;
        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(point.x, point.y);

        vh.AddVert(vertex);

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(point.x, point.y);

        vh.AddVert(vertex);
    }

    private float GetAngle(Vector2 origin, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - origin.y, target.x - origin.x) * (180 / Mathf.PI));
    }

    public void ClearPoints()
    {
        points.Clear();
    }

    public void AddPoint(Vector2 position, Color color)
    {
        newColor = color;
        points.Add(transform.InverseTransformPoint(position));
    }
}