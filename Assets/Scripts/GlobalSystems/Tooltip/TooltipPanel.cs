using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

//[ExecuteInEditMode()]
public class TooltipPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI content;

    [SerializeField, Space(10)] private LayoutElement layoutElement;
    [SerializeField] private int characterWrapLimit;

    private RectTransform rectTransform;
    private Image imagePanel;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        imagePanel = GetComponent<Image>();
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;

        var pivotX = mousePos.x / Screen.width;
        var pivotY = mousePos.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = mousePos;
    }


    public void SetText(string headerText, string contentText = "", int headerFontSize = 18, int contentFontSize = 12)
    {
        imagePanel.DOFade(1, 0.5f);

        if (string.IsNullOrEmpty(headerText))
            header.gameObject.SetActive(false);
        else
        {
            header.gameObject.SetActive(true);
            header.text = headerText;
            header.fontSize = headerFontSize;
        }

        if (string.IsNullOrEmpty(contentText))
            content.gameObject.SetActive(false);
        else
        {
            content.gameObject.SetActive(true);
            content.text = contentText;
            content.fontSize = contentFontSize;
        }

        layoutElement.enabled = Mathf.Max(header.preferredWidth, content.preferredWidth) >= layoutElement.preferredWidth;
    }

    public void OnHideTooltip()
    {
        gameObject.SetActive(false);
    }
}