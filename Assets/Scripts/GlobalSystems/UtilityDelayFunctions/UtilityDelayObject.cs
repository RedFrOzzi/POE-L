using System;
using System.Collections;
using UnityEngine;

public class UtilityDelayObject : MonoBehaviour
{
    public Coroutine RunWithDelay(Action action, float delay)
    {
        return StartCoroutine(RunWithDelayCoroutine(action, delay));
    }

    public Coroutine RunWithDelay<T>(Action<T> action, float delay, T t)
    {
        return StartCoroutine(RunWithDelayCoroutine<T>(action, delay, t));
    }

    public Coroutine RunWithDelay<T1, T2>(Action<T1, T2> action, float delay, T1 t1, T2 t2)
    {
        return StartCoroutine(RunWithDelayCoroutine<T1, T2>(action, delay, t1, t2));
    }

    public Coroutine RunWithDelay<T1, T2, T3>(Action<T1, T2, T3> action, float delay, T1 t1, T2 t2, T3 t3)
    {
        return StartCoroutine(RunWithDelayCoroutine<T1, T2, T3>(action, delay, t1, t2, t3));
    }

    public Coroutine RunWithDelay(Action<MonoBehaviour> action, MonoBehaviour obj, float delay)
    {
        return StartCoroutine(RunWithDelayCoroutine(action, obj, delay));
    }

    public Coroutine RunWithDelayRealTime(Action action, float delay)
    {
        return StartCoroutine(RunWithDelayRealTimeCoroutine(action, delay));
    }

    public Coroutine RunMultipleTimes(Action action, int numberOfTimes, float betweenDelay)
    {
        return StartCoroutine(RunMultipleTimesCoroutine(action, numberOfTimes, betweenDelay));
    }

    public Coroutine RunMultipleTimes<T1>(Action<T1> action, int numberOfTimes, float betweenDelay, T1 t1)
    {
        return StartCoroutine(RunMultipleTimesCoroutine(action, numberOfTimes, betweenDelay, t1));
    }

    public Coroutine RunMultipleTimes<T1, T2>(Action<T1, T2> action, int numberOfTimes, float betweenDelay, T1 t1, T2 t2)
    {
        return StartCoroutine(RunMultipleTimesCoroutine(action, numberOfTimes, betweenDelay, t1, t2));
    }

    public Coroutine RunMultipleTimes<T1, T2, T3>(Action<T1, T2, T3> action, int numberOfTimes, float betweenDelay, T1 t1, T2 t2, T3 t3)
    {
        return StartCoroutine(RunMultipleTimesCoroutine(action, numberOfTimes, betweenDelay, t1, t2, t3));
    }

    public void CancelDelay(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
    }

    private IEnumerator RunWithDelayCoroutine(Action<MonoBehaviour> action, MonoBehaviour obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null)
        {
            action?.Invoke(obj);
        }
    }

    private IEnumerator RunWithDelayCoroutine(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        action?.Invoke();
    }

    private IEnumerator RunWithDelayCoroutine<T>(Action<T> action, float delay, T t)
    {
        T value = t;

        yield return new WaitForSeconds(delay);

        if (t == null)
        {
            yield break;
        }

        action?.Invoke(value);
    }

    private IEnumerator RunWithDelayCoroutine<T1, T2>(Action<T1, T2> action, float delay, T1 t1, T2 t2)
    {
        T1 value1 = t1;
        T2 value2 = t2;

        yield return new WaitForSeconds(delay);

        if (t1 == null || t2 == null)
        {
            yield break;
        }

        action?.Invoke(value1, value2);
    }

    private IEnumerator RunWithDelayCoroutine<T1, T2, T3>(Action<T1, T2, T3> action, float delay, T1 t1, T2 t2, T3 t3)
    {
        T1 value1 = t1;
        T2 value2 = t2;
        T3 value3 = t3;

        yield return new WaitForSeconds(delay);

        if (t1 == null || t2 == null || t3 == null)
        {
            yield break;
        }

        action?.Invoke(value1, value2, value3);
    }

    private IEnumerator RunWithDelayRealTimeCoroutine(Action action, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        action?.Invoke();
    }

    private IEnumerator RunMultipleTimesCoroutine(Action action, int numberOfTimes, float betweenDelay)
    {
        for (int i = 0; i < numberOfTimes; i++)
        {
            action?.Invoke();

            yield return new WaitForSeconds(betweenDelay);
        }
    }

    private IEnumerator RunMultipleTimesCoroutine<T1>(Action<T1> action, int numberOfTimes, float betweenDelay, T1 t1)
    {
        T1 value1 = t1;

        for (int i = 0; i < numberOfTimes; i++)
        {
            if (t1 == null)
            {
                yield break;
            }

            action?.Invoke(t1);

            yield return new WaitForSeconds(betweenDelay);
        }
    }

    private IEnumerator RunMultipleTimesCoroutine<T1, T2>(Action<T1, T2> action, int numberOfTimes, float betweenDelay, T1 t1, T2 t2)
    {
        T1 value1 = t1;
        T2 value2 = t2;

        for (int i = 0; i < numberOfTimes; i++)
        {
            if (t1 == null || t2 == null)
            {
                yield break;
            }

            action?.Invoke(t1, t2);

            yield return new WaitForSeconds(betweenDelay);
        }
    }

    private IEnumerator RunMultipleTimesCoroutine<T1, T2, T3>(Action<T1, T2, T3> action, int numberOfTimes, float betweenDelay, T1 t1, T2 t2, T3 t3)
    {
        T1 value1 = t1;
        T2 value2 = t2;
        T3 value3 = t3;

        for (int i = 0; i < numberOfTimes; i++)
        {
            if (t1 == null || t2 == null || t3 == null)
            {
                yield break;
            }

            action?.Invoke(t1, t2, t3);

            yield return new WaitForSeconds(betweenDelay);
        }
    }
}
