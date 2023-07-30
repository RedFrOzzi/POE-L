using System;
using UnityEngine;

public static class UtilityDelayFunctions
{
	private static UtilityDelayObject utilityDelayObject;

    private static bool utilityObjectIsSpawned = false;

    private static void CheckForSpawnUtilityObject()
    {
        if (utilityObjectIsSpawned) { return; }

        var ob = new GameObject("UtilityDelayObject", typeof(UtilityDelayObject));

        utilityDelayObject = ob.GetComponent<UtilityDelayObject>();

        utilityObjectIsSpawned = true;
    }

    public static Coroutine RunWithDelay(Action action, float delay)
    {
        CheckForSpawnUtilityObject();

        return utilityDelayObject.RunWithDelay(action, delay);
    }

    public static Coroutine RunWithDelay<T>(Action<T> action, float delay, T t)
    {
        CheckForSpawnUtilityObject();

        return utilityDelayObject.RunWithDelay<T>(action, delay, t);
    }

    public static Coroutine RunWithDelay<T1, T2>(Action<T1, T2> action, float delay, T1 t1, T2 t2)
    {
        CheckForSpawnUtilityObject();

        return utilityDelayObject.RunWithDelay<T1, T2>(action, delay, t1, t2);
    }

    public static Coroutine RunWithDelay<T1, T2, T3>(Action<T1, T2, T3> action, float delay, T1 t1, T2 t2, T3 t3)
    {
        CheckForSpawnUtilityObject();

        return utilityDelayObject.RunWithDelay<T1, T2, T3>(action, delay, t1, t2, t3);
    }

    public static Coroutine RunWithDelay(Action<MonoBehaviour> action, MonoBehaviour obj, float delay)
    {
        CheckForSpawnUtilityObject();

        return utilityDelayObject.RunWithDelay(action, obj, delay);
    }

    public static Coroutine RunWithDelayRealTime(Action action, float delay)
    {
        CheckForSpawnUtilityObject();

        return utilityDelayObject.RunWithDelayRealTime(action, delay);
    }

    public static Coroutine RunMultipleTimes(Action action, int numberOfTimes, float betweenDelay)
    {
        CheckForSpawnUtilityObject();

        return utilityDelayObject.RunMultipleTimes(action, numberOfTimes, betweenDelay);
    }

    public static Coroutine RunMultipleTimes<T1>(Action<T1> action, int numberOfTimes, float betweenDelay, T1 t1)
    {
        CheckForSpawnUtilityObject();

        return utilityDelayObject.RunMultipleTimes(action, numberOfTimes, betweenDelay, t1);
    }

    public static Coroutine RunMultipleTimes<T1, T2>(Action<T1, T2> action, int numberOfTimes, float betweenDelay, T1 t1, T2 t2)
    {
        CheckForSpawnUtilityObject();

        return utilityDelayObject.RunMultipleTimes(action, numberOfTimes, betweenDelay, t1, t2);
    }

    public static Coroutine RunMultipleTimes<T1, T2, T3>(Action<T1, T2, T3> action, int numberOfTimes, float betweenDelay, T1 t1, T2 t2, T3 t3)
    {
        CheckForSpawnUtilityObject();

        return utilityDelayObject.RunMultipleTimes(action, numberOfTimes, betweenDelay, t1, t2, t3);
    }

    public static void CancelDelay(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            utilityDelayObject.CancelDelay(coroutine);
        }
    }
}
