using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI.Extensions;


public class RadialMenuSwipeHandler : MonoBehaviour
{
    public RMF_RadialMenu radialMenu;
    public GameObject linePrefab;

    private List<RMF_RadialMenuElement> selectedElements = new();
    private List<UILineRenderer> lines = new();
    private bool isSwiping = false;

    //void Start()
    //{
    //    GameObject lineObj = Instantiate(linePrefab, transform);
    //    var lr = lineObj.GetComponent<UILineRenderer>();
    //    lr.positionCount = 2;
    //    lr.SetPosition(0, new Vector3(0, 0, 0));
    //    lr.SetPosition(1, new Vector3(5, 5, 0));
    //}


    void Update()
    {
#if UNITY_EDITOR
    // For mouse input testing in editor
    if (Input.GetMouseButtonDown(0))
    {
        TrySelectAtPosition(Input.mousePosition);
        isSwiping = true;
    }
    else if (Input.GetMouseButton(0) && isSwiping)
    {
        TrySelectAtPosition(Input.mousePosition);
    }
    else if (Input.GetMouseButtonUp(0))
    {
        ResetSelection();
    }
#else
        // Mobile touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                TrySelectAtPosition(touchPos);
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Moved && isSwiping)
            {
                TrySelectAtPosition(touchPos);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                ResetSelection();
            }
        }
#endif
    }


    void TrySelectAtPosition(Vector2 screenPos)
    {
        foreach (var element in radialMenu.elements)
        {

            Button button = element.GetComponentInChildren<Button>();
            if (button != null)
            {
                RectTransform rt = button.GetComponent<RectTransform>();
                if (RectTransformUtility.RectangleContainsScreenPoint(rt, screenPos))
                {
                    if (!selectedElements.Contains(element))
                    {
                        selectedElements.Add(element);
                        HighlightElement(element);
                        DrawConnectionLine();
                    }
                    break;
                }
            }


            //RectTransform rt = element.GetComponentInChildren<Button>().GetComponent<RectTransform>();

            //if (RectTransformUtility.RectangleContainsScreenPoint(rt, screenPos))
            //{
            //    if (!selectedElements.Contains(element))
            //    {
            //        selectedElements.Add(element);
            //        HighlightElement(element);
            //        DrawConnectionLine();
            //    }
            //    break;
            //}
        }
    }

    void HighlightElement(RMF_RadialMenuElement element)
    {
        //var img = element.GetComponent<Image>();
        var img = element.GetComponentInChildren<Image>();
        if (img != null)
            img.color = Color.cyan; // highlight
    }

    void DrawConnectionLine()
    {
        if (selectedElements.Count < 2) return;

        Vector2 start = WorldToCanvasPosition(selectedElements[^2].GetComponent<RectTransform>());
        Vector2 end = WorldToCanvasPosition(selectedElements[^1].GetComponent<RectTransform>());

        GameObject lineObj = Instantiate(linePrefab, transform);
        var uiLine = lineObj.GetComponent<UILineRenderer>();

        uiLine.Points = new Vector2[] { start, end };
        lines.Add(uiLine);
    }

    Vector2 WorldToCanvasPosition(RectTransform rt)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, rt.position);
        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out Vector2 localPoint);
        return localPoint;
    }


    //void DrawConnectionLine()
    //{
    //    if (selectedElements.Count < 2) return;

    //    Vector3 start = selectedElements[selectedElements.Count - 2].GetComponent<RectTransform>().position;
    //    Vector3 end = selectedElements[selectedElements.Count - 1].GetComponent<RectTransform>().position;

    //    GameObject lineObj = Instantiate(linePrefab, transform);
    //    LineRenderer lr = lineObj.GetComponent<LineRenderer>();
    //    lr.positionCount = 2;
    //    lr.SetPositions(new Vector3[] { start, end });
    //    lines.Add(lr);
    //}

    private void ResetSelection()
    {
        foreach (var element in selectedElements)
        {
            var image = element.GetComponentInChildren<Image>();
            if (image != null)
            {
                image.color = Color.white; // Reset to white
            }
        }

        selectedElements.Clear();

        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }
        lines.Clear();
    }

}
