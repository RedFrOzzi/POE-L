using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float time = 0;
    private bool timerIsActive = false;

    private float countdown = 0;
    private bool countdownIsActive = false;

    public event Action OnCountdownEnd;

    public void SetTimerActive(bool isActive)
    {
        timerIsActive = isActive;
    }

    public void SetCountdouwnActive(float timeInSeconds)
    {
        countdown = timeInSeconds;
        countdownIsActive = true;
    }

    public void UpdateTime()
    {
        if (timerIsActive)
        {
            time += Time.deltaTime;
        }

        if (countdownIsActive)
        {
            countdown -= Time.deltaTime;

            if (countdown <= 0f)
            {
                OnCountdownEnd?.Invoke();
                countdown = 0f;
                countdownIsActive = false;
            }
        }
    }

    public float GetCurrentTimeInMinutes() => time / 60;
    public float GetCurrentTimeInSeconds() => time;
    public string GetCurrentTimeInString() => $"{Mathf.FloorToInt(time / 60)}m : {Mathf.FloorToInt(time % 60)}s";

    public float GetCountdownInMinutes() => countdown / 60;
    public float GetCountdownInSeconds() => countdown;
    public string GetCountdownInString() => $"{Mathf.FloorToInt(countdown / 60)}m : {Mathf.FloorToInt(countdown % 60)}s";
}
