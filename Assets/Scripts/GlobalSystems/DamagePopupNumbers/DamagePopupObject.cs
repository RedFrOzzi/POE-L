using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class DamagePopupObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private float fontSize;
    [SerializeField] private float fontSizeOnCrit;
    [SerializeField] private Material fontMaterial;
    [SerializeField] private Material fontMaterialOnCrit;

    private ObjectPool<DamagePopupObject> pool;

    public void SetPool(ObjectPool<DamagePopupObject> pool, Transform parent)
    {
        this.pool = pool;
        transform.SetParent(parent);
    }

    public void ShowDamage(Vector2 position, float damage, bool isCritical)
    {
        if (isCritical)
        {
            textMesh.fontSize = fontSizeOnCrit;
            textMesh.fontMaterial = fontMaterialOnCrit;
        }
        else
        {
            textMesh.fontSize = fontSize;
            textMesh.fontMaterial = fontMaterial;

        }

        transform.position = position;
        transform.SetAsLastSibling();
        textMesh.text = Mathf.Round(damage).ToString();
        

        StartCoroutine(PopupCoroutine(position));
    }

    private IEnumerator PopupCoroutine(Vector2 position)
    {
        for (float t = 0; t < 0.5f; t += Time.deltaTime)
        {
            transform.position += new Vector3(0, 1) * Time.deltaTime;

            yield return null;
        }

        pool.Release(this);
    }
}
