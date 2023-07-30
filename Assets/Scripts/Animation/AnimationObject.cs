using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class AnimationObject : MonoBehaviour
{
	private SimpleAnimation animationComponent;
    private SpriteRenderer spriteRenderer;

    private Coroutine coroutine;
    private string currentClipName;

    private AnimationPlayer animationPlayer;

    private Transform parentTransform;

    private IObjectPool<AnimationObject> pool;

    private void Awake()
    {
        animationComponent = GetComponent<SimpleAnimation>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPool(IObjectPool<AnimationObject> pool, AnimationPlayer animationPlayer, Transform parent)
    {
        this.pool = pool;
        this.animationPlayer = animationPlayer;
        parentTransform = parent;
    }

    public void PlayAnimation(string clipName, float clipDuration, Vector3 scale, Color color, float speedModifier = 1f, int sortingOrder = 15)
    {
        if (animationComponent.GetState(clipName) == null)
        {
            animationComponent.AddState(animationPlayer.Animations[clipName], clipName);
        }

        currentClipName = clipName;

        transform.localScale = scale;

        spriteRenderer.sortingOrder = sortingOrder;
        spriteRenderer.color = color;
        animationComponent[clipName].speed = speedModifier;
        animationComponent.Play(clipName);
        coroutine = StartCoroutine(ReturnToPoolCoroutine(clipName, clipDuration));
    }

    public void PlayAndMoveAnimation(string clipName, Vector2 endPostion, float clipDuration, Vector3 scale, Color color, float speedModifier = 1f, int sortingOrder = 15)
    {
        if (animationComponent.GetState(clipName) == null)
        {
            animationComponent.AddState(animationPlayer.Animations[clipName], clipName);
        }

        currentClipName = clipName;

        transform.localScale = scale;

        spriteRenderer.sortingOrder = sortingOrder;
        spriteRenderer.color = color;
        animationComponent[clipName].speed = speedModifier;
        animationComponent.Play(clipName);
        coroutine = StartCoroutine(MoveAndReturnToPoolCoroutine(clipName, endPostion, clipDuration));
    }

    public void PlayAndMoveAnimation(string clipName, Func<Transform, (Vector2, Quaternion, bool)> moveFunction, float clipDuration,
        Vector3 scale, Color color, float speedModifier = 1f, int sortingOrder = 15)
    {
        if (animationComponent.GetState(clipName) == null)
        {
            animationComponent.AddState(animationPlayer.Animations[clipName], clipName);
        }

        currentClipName = clipName;

        transform.localScale = scale;

        spriteRenderer.sortingOrder = sortingOrder;
        spriteRenderer.color = color;
        animationComponent[clipName].speed = speedModifier;
        animationComponent.Play(clipName);
        coroutine = StartCoroutine(MoveAndReturnToPoolCoroutine(clipName, moveFunction, clipDuration));
    }

    public void StopAnimation()
    {
        if (coroutine != null)
        {
            spriteRenderer.color = Color.white;
            animationComponent[currentClipName].speed = 1f;
            animationComponent.Stop(currentClipName);
            currentClipName = string.Empty;
            transform.SetParent(parentTransform);
            StopCoroutine(coroutine);
            pool.Release(this);
        }
        else
        {
            spriteRenderer.color = Color.white;
            animationComponent[currentClipName].speed = 1f;
            animationComponent.Stop(currentClipName);
            currentClipName = string.Empty;
            transform.SetParent(parentTransform);
            pool.Release(this);
        }
    }

    private IEnumerator ReturnToPoolCoroutine(string clipName, float clipDuration)
    {
        yield return new WaitForSeconds(clipDuration);
        currentClipName = string.Empty;
        spriteRenderer.color = Color.white;
        animationComponent[clipName].speed = 1f;
        animationComponent.Stop(clipName);
        transform.SetParent(parentTransform);
        pool.Release(this);
    }

    private IEnumerator MoveAndReturnToPoolCoroutine(string clipName, Vector2 endPostion, float clipDuration)
    {
        var dir = ((Vector3)endPostion - transform.position).normalized;
        var speed = ((Vector3)endPostion - transform.position).magnitude / clipDuration;

        for (float t = 0; t < clipDuration; t += Time.deltaTime)
        {
            transform.position += speed * Time.deltaTime * dir;

            yield return null;
        }

        currentClipName = string.Empty;
        spriteRenderer.color = Color.white;
        animationComponent[clipName].speed = 1f;
        animationComponent.Stop(clipName);
        transform.SetParent(parentTransform);
        pool.Release(this);
    }
    private IEnumerator MoveAndReturnToPoolCoroutine(string clipName, Func<Transform, (Vector2, Quaternion, bool)> moveFunction, float clipDuration)
    {
        float currentDuration = 0;

        while (true)
        {
            currentDuration += Time.deltaTime;
            if (currentDuration > clipDuration) { break; }

            var (position, rotation, shouldStop) = moveFunction(transform);

            if (shouldStop) { break; }

            transform.SetPositionAndRotation(position, rotation);

            yield return null;
        }

        currentClipName = string.Empty;
        spriteRenderer.color = Color.white;
        animationComponent[clipName].speed = 1f;
        animationComponent.Stop(clipName);
        transform.SetParent(parentTransform);
        pool.Release(this);
    }
}
