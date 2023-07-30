using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities_UI_CastBar : MonoBehaviour
{
    [SerializeField] private Image castBarBG;
    private Image castBar;
    private CH_AbilitiesManager abilitiesManager;
    private Coroutine castBarRoutine;

    private void Awake()
    {
        castBar = GetComponent<Image>();
        castBar.fillAmount = 0;
        castBarBG.enabled = false;
        abilitiesManager = GameObject.FindGameObjectWithTag("Player").GetComponent<CH_AbilitiesManager>();
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
    }

    private void ActivateCastBar(float time)
    {
        castBarRoutine = StartCoroutine(CastBarRoutine(time));
    }

    private void DeactivateCastBar()
    {
        StopCoroutine(castBarRoutine);
        castBar.fillAmount = 0;
        castBarBG.enabled = false;
    }

    private IEnumerator CastBarRoutine(float time)
    {
        castBarBG.enabled = true;

        for (float i = 0; i <= 1; i += 1 / time * Time.deltaTime)
        {
            castBar.fillAmount = i;
            yield return null;
        }

        castBar.fillAmount = 0;
        castBarBG.enabled = false;
    }
}
