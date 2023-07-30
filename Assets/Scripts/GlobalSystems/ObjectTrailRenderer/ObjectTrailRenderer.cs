using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectTrailRenderer : MonoBehaviour
{
	public static ObjectTrailRenderer Instance;

    [SerializeField] private TrailObject trailObjectPrefab;
    [SerializeField] private TrailRepeater trailRepeater;

    private ObjectPool<TrailObject> pool;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        pool = new(CreateAnimationObgect, OnTakeObjectFromPool, OnReturnObjectToPool);
    }

    public TrailRepeater PlayTrail(SpriteRenderer spriteRenderer, Transform objectTransform, Vector3 scale)
    {
        var TR = Instantiate(trailRepeater);
        TR.SetRepeater(spriteRenderer, objectTransform, pool, scale);
        return TR;
    }

    public TrailRepeater PlayTrail(SpriteRenderer spriteRenderer, Transform objectTransform, Vector3 scale, float forTime, bool shouldShrink)
    {
        var TR = Instantiate(trailRepeater);
        TR.SetRepeater(spriteRenderer, objectTransform, pool, scale, forTime, shouldShrink);
        return TR;
    }

    public void StopTrail(TrailRepeater trailRepeater)
    {
        trailRepeater.StopRepeating();
    }

    private TrailObject CreateAnimationObgect()
    {
        var TO = Instantiate(trailObjectPrefab, transform);
        TO.SetPool(pool, transform);
        return TO;
    }

    private void OnTakeObjectFromPool(TrailObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnReturnObjectToPool(TrailObject obj)
    {
        obj.gameObject.SetActive(false);
    }
}
