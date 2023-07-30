using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class LineObject : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private IObjectPool<LineObject> pool;

    public void SetPool(IObjectPool<LineObject> pool)
    {
        this.pool = pool;
    }

    public void DrawLine(Vector2 start, Vector2 end, float width, Gradient colorGradient, float time)
    {
        StartCoroutine(DrawLineRoutine(start, end, width, colorGradient, time));
    }

    private IEnumerator DrawLineRoutine(Vector2 start, Vector2 end, float width, Gradient colorGradient, float time)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.colorGradient = colorGradient;

        yield return new WaitForSeconds(time);

        pool.Release(this);
    }
}
