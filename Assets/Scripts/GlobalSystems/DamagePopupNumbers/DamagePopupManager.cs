using UnityEngine;
using UnityEngine.Pool;

public class DamagePopupManager : MonoBehaviour
{
	public static DamagePopupManager Instance;

    [SerializeField] private DamagePopupObject damagePopupPrefab;
    [SerializeField] private Transform parentTransform;

    private ObjectPool<DamagePopupObject> pool;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        pool = new(CreateDamagePopupObject, OnTakeObjectFromPool, OnReturnObjectToPool);
    }

    public void ShowDamage(Vector2 position, float damage, bool isCritical)
    {
        var obj = pool.Get();
        obj.ShowDamage(position, damage, isCritical);
    }


    private DamagePopupObject CreateDamagePopupObject()
    {
        var AO = Instantiate(damagePopupPrefab, transform);
        AO.SetPool(pool, parentTransform);
        return AO;
    }

    private void OnTakeObjectFromPool(DamagePopupObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnReturnObjectToPool(DamagePopupObject obj)
    {
        obj.gameObject.SetActive(false);
    }
}
