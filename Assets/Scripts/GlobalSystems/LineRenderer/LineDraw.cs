using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class LineDraw : MonoBehaviour
{
    public static LineDraw Instance;

    [SerializeField] private LineObject linePrefab;
    private ObjectPool<LineObject> pool;

    [SerializeField] private Gradient shotgunPelletLine;
    public static Gradient ShotgunPelletLine
    { get
        {
            return Instance.shotgunPelletLine;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        pool = new(CreateLineObject, OnTakeObjectFromPool, OnReturnObjectToPool);
    }

    public void DrawLine(Vector2 start, Vector2 end, float width, Gradient colorGradient, float time)
    {
        var LO = pool.Get();
        LO.DrawLine(start, end, width, colorGradient, time);
    }

    private LineObject CreateLineObject()
    {
        var LO = Instantiate(linePrefab, transform);
        LO.SetPool(pool);
        return LO;
    }

    private void OnTakeObjectFromPool(LineObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnReturnObjectToPool(LineObject obj)
    {
        obj.gameObject.SetActive(false);
    }
}
