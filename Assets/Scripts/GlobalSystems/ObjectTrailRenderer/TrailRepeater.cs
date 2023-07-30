using UnityEngine;
using UnityEngine.Pool;

public class TrailRepeater : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform objectTransform;
    private ObjectPool<TrailObject> pool;

    private float destroyTime;
    private bool shouldShrink;
    private Vector3 scale;


    private void FixedUpdate()
    {
        if (destroyTime < Time.time)
        {
            StopRepeating();
        }

        SpawnTrailObject();
    }

    public void SetRepeater(SpriteRenderer spriteRenderer, Transform objectTransform, ObjectPool<TrailObject> pool, Vector3 scale, float forTime = float.MaxValue, bool shouldShrink = true)
    {
        this.spriteRenderer = spriteRenderer;
        this.objectTransform = objectTransform;
        this.pool = pool;
        this.destroyTime = Time.time + forTime;
        this.shouldShrink = shouldShrink;
        this.scale = scale;
    }

    private void SpawnTrailObject()
    {
        if (spriteRenderer == null)
        {
            StopRepeating();
            return;
        }

        var trailObj = pool.Get();
        trailObj.PlayTrail(spriteRenderer, objectTransform.position, objectTransform.rotation, scale, spriteRenderer.flipX, shouldShrink);
    }

    public void StopRepeating()
    {
        Destroy(gameObject);
    }
}
