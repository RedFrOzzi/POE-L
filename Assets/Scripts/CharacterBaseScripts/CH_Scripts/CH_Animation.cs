using UnityEngine;
using Database;
using System.Collections.Generic;

public class CH_Animation : MonoBehaviour
{
    [SerializeField] private string idleClipName;
    [SerializeField] private string walkClipName;
    [SerializeField] private string attackClipName;
    [SerializeField] private string deathClipName;
    [SerializeField] private string emptyClipName = "Empty";

    [Space(10)]
    [SerializeField] float slowAnimationMS;
    [SerializeField] float normalAnimationMS;
    [SerializeField] float fastAnimationMS;

    private AnimationClip idleClip;
    private AnimationClip walkClip;
    private AnimationClip attackClip;
    private AnimationClip deathClip;
    private AnimationClip emptyClip;

    public float IdleClipLengh { get; private set; }
    public float WalkClipLengh { get; private set; }
    public float AttackClipLengh { get; private set; }
    public float DeathClipLengh { get; private set; }
    public float EmptyClipLengh { get; private set; }

    private SimpleAnimation animationComponent;
    private CH_Stats stats;

    private List<SimpleAnimation.State> states;
    
    private void Awake()
    {
        animationComponent = GetComponent<SimpleAnimation>();
        stats = GetComponent<CH_Stats>();
        states = new();
    }

    private void Start()
    {
        stats.OnStatsChange += OnStatsChange;
    }

    private void OnDestroy()
    {
        stats.OnStatsChange -= OnStatsChange;
    }

    public void AnimationsSetUp()
    {
        var dataBase = GameDatabasesManager.Instance.AnimationsDatabase.Animations;

        if (idleClipName != string.Empty)
        {
            idleClip = dataBase[idleClipName];
            animationComponent.AddState(idleClip, idleClipName);
            IdleClipLengh = dataBase[idleClipName].length;
        }

        if (walkClipName != string.Empty)
        {
            walkClip = dataBase[walkClipName];
            animationComponent.AddState(walkClip, walkClipName);
            WalkClipLengh = dataBase[walkClipName].length;
        }

        if (attackClipName != string.Empty)
        {
            attackClip = dataBase[attackClipName];
            animationComponent.AddState(attackClip, attackClipName);
            AttackClipLengh = dataBase[attackClipName].length;
        }

        if (deathClipName != string.Empty)
        {
            deathClip = dataBase[deathClipName];
            animationComponent.AddState(deathClip, deathClipName);
            DeathClipLengh = dataBase[deathClipName].length;
        }

        if (emptyClipName != string.Empty)
        {
            emptyClip = dataBase[emptyClipName];
            animationComponent.AddState(emptyClip, emptyClipName);
            EmptyClipLengh = dataBase[emptyClipName].length;
        }

        foreach (var state in animationComponent.GetStates())
        {
            states.Add(state);
        }
    }

    public void PlayIdle()
    {
        animationComponent.Play(idleClipName);
    }

    public void StopIdle()
    {
        animationComponent.Stop(idleClipName);
    }

    public void PlayWalk()
    {
        animationComponent.Play(walkClipName);
    }

    public void StopWalk()
    {
        animationComponent.Stop(walkClipName);
    }

    public void PlayAttack(float clipLengh)
    {
        animationComponent[attackClipName].speed = 1f;
        animationComponent[attackClipName].speed = animationComponent[attackClipName].length * (1 / clipLengh);
        animationComponent.Play(attackClipName);
    }

    public void StopAttack()
    {
        animationComponent.Stop(attackClipName);
    }

    public void PlayDeath()
    {
        animationComponent.Play(deathClipName);
    }

    public void StopDeath()
    {
        animationComponent.Stop(deathClipName);
    }

    public void PlayEmpty()
    {
        animationComponent.Play(emptyClipName);
    }

    public void StopEmpty()
    {
        animationComponent.Stop(emptyClipName);
    }

    public void Stop()
    {
        animationComponent.Stop();
    }

    public void Freeze(bool value)
    {
        if (value)
        {
            foreach (var t in states)
            {
                if (t.enabled)
                {
                    t.speed = 0f;
                }
            }
        }
        else
        {
            foreach (var t in states)
            {
                if (t.enabled)
                {
                    t.speed = 1f;
                }
            }
        }
    }

    private void OnStatsChange()
    {
        if (stats.CurrentMovementSpeed > fastAnimationMS)
        {
            animationComponent[walkClipName].speed = 2f;
            return;
        }

        if (stats.CurrentMovementSpeed > normalAnimationMS)
        {
            animationComponent[walkClipName].speed = 1f;
            return;
        }

        if (stats.CurrentMovementSpeed > slowAnimationMS)
        {
            animationComponent[walkClipName].speed = 0.5f;
            return;
        }
    }
}