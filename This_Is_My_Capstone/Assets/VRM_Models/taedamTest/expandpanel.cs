using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class expandpanel : MonoBehaviour
{
    private RectTransform panelRectTransform;
    private bool isExpanded = false;
    private Vector2 expandedSize = new Vector2(200f, 200f);
    private Vector2 collapsedSize = new Vector2(100f, 100f);
    void Start()
    {
        panelRectTransform = GetComponent<RectTransform>();
        SetPanelSize(isExpanded);
    }

    public void TogglePanel()
    {
        isExpanded = !isExpanded;
        SetPanelSize(isExpanded);
    }

    private void SetPanelSize(bool expand)
    {
        panelRectTransform.sizeDelta = expand ? expandedSize : collapsedSize;
    }
}
