using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Tooltip : MonoBehaviour
{
    private static Tooltip instance;

    [SerializeField] private TooltipPanel tooltipPanel;

    private static Coroutine delayCoroutine;

    private void Awake()
    {
        instance = this;
    }

    public static void Show(string headerText, string contentText, int headerFontSize, int contentFontSize)
    {
        delayCoroutine = UtilityDelayFunctions.RunWithDelayRealTime(() =>
        {
            instance.tooltipPanel.gameObject.SetActive(true);
            instance.tooltipPanel.SetText(headerText, contentText, headerFontSize, contentFontSize);
        }
        , 0.5f);
    }

    public static void Hide()
    {
        UtilityDelayFunctions.CancelDelay(delayCoroutine);

        instance.tooltipPanel.OnHideTooltip();
    }
}