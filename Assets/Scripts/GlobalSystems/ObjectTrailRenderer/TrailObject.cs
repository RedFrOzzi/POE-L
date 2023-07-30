using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class TrailObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private IObjectPool<TrailObject> pool;

    private float currentAlpha = 0f;
    private Vector2 localScale;

    private const float colorAndScaleSpeedMultipler = 3f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPool(IObjectPool<TrailObject> pool, Transform parent)
    {
        this.pool = pool;
        transform.SetParent(parent);
    }

    public void PlayTrail(SpriteRenderer spriteRenderer, Vector2 position, Quaternion rotation, Vector3 scale, bool isFliped, bool shouldShrink = true)
    {
        this.spriteRenderer.sprite = spriteRenderer.sprite;
        this.spriteRenderer.flipX = isFliped;
        transform.SetPositionAndRotation(position, rotation);
        transform.localScale = scale;
        StartCoroutine(TrailCoroutine(shouldShrink));
    }

    private IEnumerator TrailCoroutine(bool shouldShrink)
    {
        var _initialScale = transform.localScale;

        for (float t = 0f; t < 1f; t += Time.deltaTime * colorAndScaleSpeedMultipler)
        {
            currentAlpha = Mathf.Lerp(1, 0, t);

            Color color = new(1, 1, 1, currentAlpha);
            spriteRenderer.color = color;

            var currentScaleX = Mathf.Lerp(_initialScale.x, 0, t);
            var currentScaleY = Mathf.Lerp(_initialScale.y, 0, t);

            if (shouldShrink)
            {
                localScale.x = currentScaleX;
                localScale.y = currentScaleY;

                spriteRenderer.transform.localScale = localScale;
            }

            yield return null;
        }

        pool.Release(this);
    }
}
