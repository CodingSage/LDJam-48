using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public TMP_Text tooltipText;
    public RectTransform background;

    private static Tooltip instance;

    private void Awake()
    {
        instance = this;
        HideTooltip();
    }

    private void Update()
    {
        Vector2 localPos;
        Vector2 alteredMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y - 4f);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), alteredMousePos, null, out localPos);
        transform.localPosition = localPos;
    }

    private void ShowTooltip(string text)
    {
        gameObject.SetActive(true);
        tooltipText.text = text;
        float padding = 0.4f;
        background.sizeDelta = new Vector2(tooltipText.preferredWidth + padding * 2, tooltipText.preferredHeight + padding * 2);
        tooltipText.rectTransform.sizeDelta = new Vector2(tooltipText.preferredWidth, tooltipText.preferredHeight);
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void Show(string text)
    {
        instance.ShowTooltip(text);
    }

    public static void Hide()
    {
        instance.HideTooltip();
    }
}
