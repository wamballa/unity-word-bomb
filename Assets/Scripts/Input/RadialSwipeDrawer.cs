using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Collections.Generic;
using TMPro;

public class RadialSwipeDrawer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public UILineRenderer lineRenderer;
    public RMF_RadialMenu radialMenu;
    public Color highlightColor = Color.cyan;
    public Color defaultColor = Color.white;

    private RectTransform canvasRect;
    private List<Vector2> points = new();
    private List<RMF_RadialMenuElement> selectedElements = new();
    private bool isDragging = false;

    void Start()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        lineRenderer.Points = new Vector2[0];
        lineRenderer.color = highlightColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        points.Clear();
        selectedElements.Clear();
        isDragging = true;
        TrySelect(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            TrySelect(eventData.position);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetSelection();
        isDragging = false;
    }

    void TrySelect(Vector2 screenPos)
    {
        foreach (var element in radialMenu.elements)
        {
            Button button = element.GetComponentInChildren<Button>();
            if (button == null) continue;

            RectTransform rt = button.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rt, screenPos))
            {
                if (!selectedElements.Contains(element))
                {
                    selectedElements.Add(element);
                    HighlightElement(element);

                    // --- Get letter or number from TMP text ---
                    TMP_Text tmp = element.GetComponentInChildren<TMPro.TMP_Text>();
                    if (tmp != null && !string.IsNullOrEmpty(tmp.text))
                    {
                        char c = tmp.text[0];
                        if (char.IsLetter(c))
                            InputRouter.Receiver?.TypeLetter(c);
                        else if (char.IsDigit(c))
                            InputRouter.Receiver?.TypeNumber((int)char.GetNumericValue(c));

                        // --- Line drawing from TMP center ---
                        RectTransform textRT = tmp.GetComponent<RectTransform>();
                        Vector2 worldCenter = textRT.position;
                        Vector2 localPoint;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, worldCenter, null, out localPoint);
                        points.Add(localPoint);
                        RefreshLine();
                    }
                }
                break;
            }
        }
    }


    void HighlightElement(RMF_RadialMenuElement element)
    {
        var img = element.GetComponentInChildren<Image>();
        if (img != null)
            img.color = highlightColor;
    }

    void ResetSelection()
    {
        foreach (var element in selectedElements)
        {
            var img = element.GetComponentInChildren<Image>();
            if (img != null)
                img.color = defaultColor;
        }
        selectedElements.Clear();
        points.Clear();
        RefreshLine();
    }

    void RefreshLine()
    {
        lineRenderer.Points = points.ToArray();
        lineRenderer.SetAllDirty();
    }
}
