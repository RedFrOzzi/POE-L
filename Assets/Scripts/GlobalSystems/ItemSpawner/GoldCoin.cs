using UnityEngine;
using UnityEngine.Pool;

public class GoldCoin : MonoBehaviour
{
	private int goldAmount;
    private IObjectPool<GoldCoin> pool;

    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float startingSpeed = 10f;
    private float speed;
    private Vector2 initialPosition;
    private float delta;
    private bool animArePlaying;

	public void Set(int amount, Vector2 position)
    {
        goldAmount = amount;
        transform.position = position;
        speed = startingSpeed;
        initialPosition = position;
        delta = 0;
        animArePlaying = true;
    }

    public int GetGoldAmount()
    {
        return goldAmount;
    }

    public void SetPool(IObjectPool<GoldCoin> pool)
    {
        this.pool = pool;

    }

    public void ReleaseFromPool()
    {
        pool.Release(this);
    }

    private void FixedUpdate()
    {
        if (animArePlaying == false) { return; }

        delta += Time.fixedDeltaTime * speed;
        transform.position = new Vector3(initialPosition.x + delta, initialPosition.y + curve.Evaluate(delta), 0);

        if (delta > 0.32f)
        {
            animArePlaying = false;
        }
    }
}
