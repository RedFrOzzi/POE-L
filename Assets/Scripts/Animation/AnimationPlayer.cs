using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AnimationPlayer : MonoBehaviour
{
    public static AnimationPlayer Instance;

    public static Dictionary<AnimationSortingOrder, int> SortingLayer;

    [SerializeField] private AnimationObject animationObjectPrefab;

    [SerializeField] private int behindPlayerSortingLayer = 5;
    [SerializeField] private int OverPlayerSortingLayer = 15;

    private ObjectPool<AnimationObject> pool;

    public Dictionary<string, AnimationClip> Animations { get; private set; }
    public Dictionary<string, float> AnimationsDurations { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        pool = new(CreateAnimationObgect, OnTakeObjectFromPool, OnReturnObjectToPool);

        SortingLayer = new();
        SortingLayer.Add(AnimationSortingOrder.BehindPlayer, behindPlayerSortingLayer);
        SortingLayer.Add(AnimationSortingOrder.OverPlayer, OverPlayerSortingLayer);
    }

    private void Start()
    {
        Animations = new (GameDatabasesManager.Instance.AnimationsDatabase.Animations);
        AnimationsDurations = new(GameDatabasesManager.Instance.AnimationsDatabase.AnimationDurations);
    }

    public void Play(string clipName, Vector2 position, Quaternion rotation, AnimationSortingOrder sortingOrder = AnimationSortingOrder.OverPlayer)
    {
        var animObj = pool.Get();
        animObj.transform.SetPositionAndRotation(position, rotation);
        animObj.PlayAnimation(clipName, AnimationsDurations[clipName], Vector3.one, Color.white, 1f, SortingLayer[sortingOrder]);
    }

    public void Play(string clipName, Vector2 position, Quaternion rotation, Vector3 scale, AnimationSortingOrder sortingOrder = AnimationSortingOrder.OverPlayer)
    {
        var animObj = pool.Get();
        animObj.transform.SetPositionAndRotation(position, rotation);
        animObj.PlayAnimation(clipName, AnimationsDurations[clipName], scale, Color.white, 1f, SortingLayer[sortingOrder]);
    }

    public void Play(string clipName, Vector2 position, Quaternion rotation, Vector3 scale, Color color, AnimationSortingOrder sortingOrder = AnimationSortingOrder.OverPlayer)
    {
        var animObj = pool.Get();
        animObj.transform.SetPositionAndRotation(position, rotation);
        animObj.PlayAnimation(clipName, AnimationsDurations[clipName], scale, color, 1f, SortingLayer[sortingOrder]);
    }

    public void Play(string clipName, Vector2 position, Quaternion rotation, Vector3 scale, float speedModifier, AnimationSortingOrder sortingOrder = AnimationSortingOrder.OverPlayer)
    {
        var animObj = pool.Get();
        animObj.transform.SetPositionAndRotation(position, rotation);
        animObj.PlayAnimation(clipName, AnimationsDurations[clipName] * (1 / speedModifier), scale, Color.white, speedModifier, SortingLayer[sortingOrder]);
    }

    public void Play(string clipName, Vector2 position, Quaternion rotation, Vector3 scale, Color color, float speedModifier, AnimationSortingOrder sortingOrder = AnimationSortingOrder.OverPlayer)
    {
        var animObj = pool.Get();
        animObj.transform.SetPositionAndRotation(position, rotation);
        animObj.PlayAnimation(clipName, AnimationsDurations[clipName] * (1 / speedModifier), scale, color, speedModifier, SortingLayer[sortingOrder]);
    }

    public void PlayForDuration(string clipName, Vector2 position, Quaternion rotation, Vector3 scale, Color color, float duration, float speedModifier = 1f, AnimationSortingOrder sortingOrder = AnimationSortingOrder.OverPlayer)
    {
        var animObj = pool.Get();
        animObj.transform.SetPositionAndRotation(position, rotation);
        animObj.PlayAnimation(clipName, duration, scale, color, speedModifier, SortingLayer[sortingOrder]);
    }

    public AnimationObject PlayAndFollowForDuration(string clipName, Transform parentToFollow, Quaternion rotation, Vector3 scale, Color color, float duration, float speedModifier = 1f, AnimationSortingOrder sortingOrder = AnimationSortingOrder.OverPlayer)
    {
        var animObj = pool.Get();
        animObj.transform.SetPositionAndRotation(parentToFollow.position, rotation);
        animObj.transform.SetParent(parentToFollow);
        animObj.PlayAnimation(clipName, duration, scale, color, speedModifier, SortingLayer[sortingOrder]);

        return animObj;
    }

    public AnimationObject PlayAndMoveAnimation(string clipName, Vector2 startPosition, Vector2 endPostion, float timeToTravel, Vector3 scale, Color color, AnimationSortingOrder sortingOrder = AnimationSortingOrder.OverPlayer)
    {
        var animObj = pool.Get();
        animObj.transform.position = startPosition;
        animObj.PlayAndMoveAnimation(clipName, endPostion, timeToTravel, scale, color, 1, SortingLayer[sortingOrder]);

        return animObj;
    }

    public AnimationObject PlayAndMoveAnimation(string clipName, Vector2 startPosition, Func<Transform, (Vector2 nextPosition, Quaternion nextRotation, bool shouldStop)> moveFunction,
        float timeToTravel, Vector3 scale, Color color, AnimationSortingOrder sortingOrder = AnimationSortingOrder.OverPlayer)
    {
        var animObj = pool.Get();
        animObj.transform.position = startPosition;
        animObj.PlayAndMoveAnimation(clipName, moveFunction, timeToTravel, scale, color, 1, SortingLayer[sortingOrder]);

        return animObj;
    }

    public void StopAnimation(AnimationObject animationObject)
    {
        animationObject.StopAnimation();
    }

    private AnimationObject CreateAnimationObgect()
    {
        var AO = Instantiate(animationObjectPrefab, transform);
        AO.SetPool(pool, this, transform);
        return AO;
    }

    private void OnTakeObjectFromPool(AnimationObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnReturnObjectToPool(AnimationObject obj)
    {
        obj.gameObject.SetActive(false);
    }
}

public enum AnimationSortingOrder { OverPlayer, BehindPlayer}