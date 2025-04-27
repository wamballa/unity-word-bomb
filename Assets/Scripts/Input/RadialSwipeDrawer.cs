using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Collections.Generic;
using TMPro;
using System;
using System.Security.Cryptography;

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

    public static event Action OnRadialPointerUp;

    private List<char> typedSequence = new();

    public string GetTypedSequence => new string(typedSequence.ToArray());
    private Vector2 currentPointerPos;

    private int snapCounter;

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
        if (!isDragging) return;
        currentPointerPos = eventData.position;
        if (snapCounter < 6) UpdateFloatingLine();
        TrySelect(currentPointerPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetSelection();
        isDragging = false;
        typedSequence.Clear();
        snapCounter = 0;
        OnRadialPointerUp?.Invoke();
    }

    void UpdateFloatingLine()
    {
        if (points.Count == 0) return;

        Vector2 localPointerPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            lineRenderer.rectTransform, currentPointerPos, null, out localPointerPos);

        var dynamicPoints = new List<Vector2>(points);
        dynamicPoints.Add(localPointerPos);
        lineRenderer.Points = dynamicPoints.ToArray();
        lineRenderer.SetAllDirty();
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
                    TMP_Text tmp = element.GetComponentInChildren<TMP_Text>();
                    if (tmp != null && !string.IsNullOrEmpty(tmp.text))
                    {
                        char c = tmp.text[0];
                        typedSequence.Add(c);
                        if (char.IsLetter(c))
                            InputRouter.Receiver?.TypeLetter(c);
                        else if (char.IsDigit(c))
                            InputRouter.Receiver?.TypeNumber((int)char.GetNumericValue(c));

                        // --- Line drawing using local position relative to the lineRenderer's RectTransform ---
                        RectTransform textRT = tmp.GetComponent<RectTransform>();
                        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, textRT.position);

                        Vector2 localPoint;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                            lineRenderer.rectTransform, screenPoint, null, out localPoint
                        );

                        snapCounter++;
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
