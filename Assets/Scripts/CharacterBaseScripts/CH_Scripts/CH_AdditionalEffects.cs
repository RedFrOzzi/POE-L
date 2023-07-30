using System.Collections;
using UnityEngine;

public class CH_AdditionalEffects : MonoBehaviour
{
    private Rigidbody2D rb;
    private CH_Stats stats;
    private SpriteRenderer spriteRenderer;

    public float KnockbackTime { get; private set; } = 0.2f;
    public float DashTime { get; private set; } = 0.3f;
    public float BlinkTime { get; private set; } = 0.3f;

    private Coroutine controllLossRoutine;
    private Coroutine rootRoutine;
    private Coroutine dashRoutine;
    private Coroutine knockBackRoutine;
    private Coroutine blinkRoutine;

    private float controllLossEndTime;
    private float rootEndTime;
    private float dashEndTime;
    private float knockBackEndTime;
    private float blinkEndTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CH_Stats>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void KnockBack(Transform fromTransform ,float distance)
    {
        if (knockBackEndTime > Time.time) { return; }

        if (knockBackRoutine == null)
        {
            knockBackRoutine = StartCoroutine(KnockBackRoutine(fromTransform, distance));
        }
        else
        {
            StopCoroutine(knockBackRoutine);
            knockBackRoutine = StartCoroutine(KnockBackRoutine(fromTransform, distance));
        }
    }

    public void Blink(Vector2 position)
    {
        if (blinkEndTime > Time.time) { return; }

        if (blinkRoutine == null)
        {
            blinkRoutine = StartCoroutine(BlinkRoutine(position));
        }
        else
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = StartCoroutine(BlinkRoutine(position));
        }
    }

    public void Dash(Transform towardsTransform, float distance)
    {
        if (dashEndTime > Time.time) { return; }

        if (dashRoutine == null)
        {
            dashRoutine = StartCoroutine(DashRoutine(towardsTransform, distance));
        }
        else
        {
            StopCoroutine(dashRoutine);
            dashRoutine = StartCoroutine(DashRoutine(towardsTransform, distance));
        }
    }

    public void Dash(Vector2 towards, float distance)
    {
        if (dashEndTime > Time.time) { return; }

        if (dashRoutine == null)
        {
            dashRoutine = StartCoroutine(DashWithVectorRoutine(towards, distance));
        }
        else
        {
            StopCoroutine(dashRoutine);
            dashRoutine = StartCoroutine(DashWithVectorRoutine(towards, distance));
        }
    }

    public void Root(float duration)
    {
        if (rootEndTime > Time.time) { return; }

        if (rootRoutine == null)
        {
            rootRoutine = StartCoroutine(RootRoutine(duration));
        }
        else
        {
            StopCoroutine(rootRoutine);
            rootRoutine = StartCoroutine(RootRoutine(duration));
        }
    }

    public void FullControllLoss(float duration)
    {
        if (controllLossEndTime > Time.time) { return; }

        if (controllLossRoutine == null)
        {
            controllLossRoutine = StartCoroutine(ControllLossRoutine(duration));
        }
        else
        {
            StopCoroutine(controllLossRoutine);
            controllLossRoutine = StartCoroutine(ControllLossRoutine(duration));
        }
    }

    private IEnumerator BlinkRoutine(Vector2 position)
    {
        stats.SetAbilityToMove(false);
        Color initialColor = spriteRenderer.color;
        spriteRenderer.color = new Color(1, 1, 1, 0);
        transform.position = position;
        yield return new WaitForSeconds(BlinkTime);
        stats.SetAbilityToMove(true);
        spriteRenderer.color = initialColor;
    }

    private IEnumerator KnockBackRoutine(Transform fromTransform, float distance)
    {
        knockBackEndTime = Time.time + KnockbackTime;
        FullControllLoss(KnockbackTime);
        rb.velocity = (gameObject.transform.position - fromTransform.position).normalized * (distance / KnockbackTime);        
        yield return new WaitForSeconds(KnockbackTime);
        rb.velocity = Vector2.zero;
        knockBackRoutine = null;
    }

    private IEnumerator DashRoutine(Transform towardsTransform, float distance)
    {
        dashEndTime = Time.time + DashTime;
        FullControllLoss(DashTime);
        rb.velocity = (towardsTransform.position - gameObject.transform.position).normalized * (distance / DashTime);        
        yield return new WaitForSeconds(DashTime);
        rb.velocity = Vector2.zero;
        dashRoutine = null;
    }
    
    private IEnumerator DashWithVectorRoutine(Vector2 towards, float distance)
    {
        dashEndTime = Time.time + DashTime;
        FullControllLoss(DashTime);
        rb.velocity = towards.normalized * (distance / DashTime);
        yield return new WaitForSeconds(DashTime);
        rb.velocity = Vector2.zero;
        dashRoutine = null;
    }

    private IEnumerator RootRoutine(float duration)
    {
        rootEndTime = Time.time + duration;
        stats.SetAbilityToMove(false);
        yield return new WaitForSeconds(duration);
        stats.SetAbilityToMove(true);
        rootRoutine = null;
    }

    private IEnumerator ControllLossRoutine(float duration)
    {
        controllLossEndTime = Time.time + duration;
        stats.SetAbilityToControll(false);
        yield return new WaitForSeconds(duration);
        stats.SetAbilityToControll(true);
        controllLossRoutine = null;
    }
}
