using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle;
using TMPro;


[AddComponentMenu("Radial Menu Framework/RMF Core Script")]
public class RMF_RadialMenu : MonoBehaviour {

    [Header("Lerp Settings")]
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float lerpDuration = 0.5f;

    [HideInInspector]
    public RectTransform rt;
    //public RectTransform baseCircleRT;
    //public Image selectionFollowerImage;

    [Tooltip("Adjusts the radial menu for use with a gamepad or joystick. You might need to edit this script if you're not using the default horizontal and vertical input axes.")]
    public bool useGamepad = false;

    [Tooltip("With lazy selection, you only have to point your mouse (or joystick) in the direction of an element to select it, rather than be moused over the element entirely.")]
    public bool useLazySelection = true;


    [Tooltip("If set to true, a pointer with a graphic of your choosing will aim in the direction of your mouse. You will need to specify the container for the selection follower.")]
    public bool useSelectionFollower = true;

    [Tooltip("If using the selection follower, this must point to the rect transform of the selection follower's container.")]
    public RectTransform selectionFollowerContainer;

    [Tooltip("This is the text object that will display the labels of the radial elements when they are being hovered over. If you don't want a label, leave this blank.")]
    public Text textLabel;

    [Tooltip("This is the list of radial menu elements. This is order-dependent. The first element in the list will be the first element created, and so on.")]
    public List<RMF_RadialMenuElement> elements = new List<RMF_RadialMenuElement>();


    [Tooltip("Controls the total angle offset for all elements. For example, if set to 45, all elements will be shifted +45 degrees. Good values are generally 45, 90, or 180")]
    public float globalOffset = 0f;


    [HideInInspector]
    public float currentAngle = 0f; //Our current angle from the center of the radial menu.


    [HideInInspector]
    public int index = 0; //The current index of the element we're pointing at.

    private int elementCount;

    private float angleOffset; //The base offset. For example, if there are 4 elements, then our offset is 360/4 = 90

    private int previousActiveIndex = 0; //Used to determine which buttons to unhighlight in lazy selection.

    private PointerEventData pointer;

    public enum LerpStyle { Circular, Arc }


    void Awake() {

        pointer = new PointerEventData(EventSystem.current);

        rt = GetComponent<RectTransform>();

        if (rt == null)
            Debug.LogError("Radial Menu: Rect Transform for radial menu " + gameObject.name + " could not be found. Please ensure this is an object parented to a canvas.");

        if (useSelectionFollower && selectionFollowerContainer == null)
            Debug.LogError("Radial Menu: Selection follower container is unassigned on " + gameObject.name + ", which has the selection follower enabled.");

        elementCount = elements.Count;

        angleOffset = (360f / (float)elementCount);

        //Loop through and set up the elements.
        for (int i = 0; i < elementCount; i++) {
            if (elements[i] == null) {
                Debug.LogError("Radial Menu: element " + i.ToString() + " in the radial menu " + gameObject.name + " is null!");
                continue;
            }
            elements[i].parentRM = this;

            elements[i].setAllAngles((angleOffset * i) + globalOffset, angleOffset);

            elements[i].assignedIndex = i;

        }

    }


    void Start() {


        if (useGamepad) {
            EventSystem.current.SetSelectedGameObject(gameObject, null); //We'll make this the active object when we start it. Comment this line to set it manually from another script.
            if (useSelectionFollower && selectionFollowerContainer != null)
                selectionFollowerContainer.rotation = Quaternion.Euler(0, 0, -globalOffset); //Point the selection follower at the first element.
        }

    }

    // Update is called once per frame
    void Update() {

        //If your gamepad uses different horizontal and vertical joystick inputs, change them here!
        //==============================================================================================
        bool joystickMoved = Input.GetAxis("Horizontal") != 0.0 || Input.GetAxis("Vertical") != 0.0;
        //==============================================================================================


        float rawAngle;
        
        if (!useGamepad)
            rawAngle = Mathf.Atan2(Input.mousePosition.y - rt.position.y, Input.mousePosition.x - rt.position.x) * Mathf.Rad2Deg;
        else
            rawAngle = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * Mathf.Rad2Deg;

        //If no gamepad, update the angle always. Otherwise, only update it if we've moved the joystick.
        if (!useGamepad)
            currentAngle = normalizeAngle(-rawAngle + 90 - globalOffset + (angleOffset / 2f));
        else if (joystickMoved)
            currentAngle = normalizeAngle(-rawAngle + 90 - globalOffset + (angleOffset / 2f));

        //Handles lazy selection. Checks the current angle, matches it to the index of an element, and then highlights that element.
        if (angleOffset != 0 && useLazySelection) {

            //Current element index we're pointing at.
            index = (int)(currentAngle / angleOffset);

            if (elements[index] != null) {

                //Select it.
                selectButton(index);

                //If we click or press a "submit" button (Button on joystick, enter, or spacebar), then we'll execut the OnClick() function for the button.
                if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit")) {

                    ExecuteEvents.Execute(elements[index].button.gameObject, pointer, ExecuteEvents.submitHandler);


                }
            }

        }

        //Updates the selection follower if we're using one.
        if (useSelectionFollower && selectionFollowerContainer != null) {
            if (!useGamepad || joystickMoved)
                selectionFollowerContainer.rotation = Quaternion.Euler(0, 0, rawAngle + 270);
           

        } 

    }


    public void ShuffleElementsRuntime()
    {
        Debug.Log("[RMF_RadialMenu] ShuffleElementsRuntime");

        int count = elements.Count;

        // Randomly decide to preserve 0, 1, or 2 elements
        int preservedCount = Random.Range(0, 3);
        HashSet<int> preservedIndices = new HashSet<int>();
        while (preservedIndices.Count < preservedCount)
        {
            preservedIndices.Add(Random.Range(0, count));
        }

        // Create shuffled list
        List<RMF_RadialMenuElement> shuffled = new List<RMF_RadialMenuElement>(elements);

        // Fisher-Yates Shuffle
        for (int i = 0; i < count; i++)
        {
            if (preservedIndices.Contains(i)) continue;

            int randIndex = i;
            while (randIndex == i || preservedIndices.Contains(randIndex))
            {
                randIndex = Random.Range(0, count);
            }

            (shuffled[i], shuffled[randIndex]) = (shuffled[randIndex], shuffled[i]);
        }

        elements = shuffled;
        RecalculateAnglesWithLerp(preservedIndices);

    }

    private void RecalculateAnglesWithLerp(HashSet<int> preservedIndices)
    {
        elementCount = elements.Count;
        angleOffset = 360f / elementCount;

        for (int i = 0; i < elementCount; i++)
        {
            var element = elements[i];
            element.parentRM = this;
            element.assignedIndex = i;

            float angle = i * angleOffset;
            element.setAllAngles(angle, angleOffset);

            RectTransform rt = element.GetComponent<RectTransform>();
            Vector2 startPos = rt.anchoredPosition;
            Vector2 targetPos = Quaternion.Euler(0, 0, -angle) * (Vector2.up * startPos.magnitude);

            // Choose lerp style
            int prevIndex = elements.IndexOf(element);
            int distance = Mathf.Abs(prevIndex - i);
            if (distance > elementCount / 2) distance = elementCount - distance;

            LerpStyle style = (distance <= 1) ? LerpStyle.Circular : LerpStyle.Arc;

            TMP_Text text = element.GetComponentInChildren<TMP_Text>();

            if (preservedIndices.Contains(i))
            {
                rt.anchoredPosition = targetPos;
                rt.localRotation = Quaternion.Euler(0, 0, -angle);
                if (text != null)
                    text.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                StartCoroutine(LerpToPosition(rt, startPos, targetPos, lerpDuration, angle, style, text));
            }
        }
    }


    private IEnumerator LerpToPosition(RectTransform element, Vector2 start, Vector2 target, float duration, float angle, LerpStyle style, TMP_Text text)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float curvedT = movementCurve.Evaluate(t);

            Vector2 pos;
            switch (style)
            {
                case LerpStyle.Arc:
                    float theta = curvedT * Mathf.PI;
                    Vector2 mid = Vector2.Lerp(start, target, 0.5f) + Vector2.Perpendicular(target - start).normalized * 30f;
                    Vector2 a = Vector2.Lerp(start, mid, curvedT);
                    Vector2 b = Vector2.Lerp(mid, target, curvedT);
                    pos = Vector2.Lerp(a, b, curvedT);
                    break;

                case LerpStyle.Circular:
                default:
                    pos = Vector2.Lerp(start, target, curvedT);
                    break;
            }

            element.anchoredPosition = pos;
            element.localRotation = Quaternion.Euler(0, 0, -angle);
            if (text != null)
                text.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);

            elapsed += Time.deltaTime;
            yield return null;
        }

        element.anchoredPosition = target;
        element.localRotation = Quaternion.Euler(0, 0, -angle);
        if (text != null)
            text.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }




    private IEnumerator AnimateShuffle(float duration = 0.3f)
    {
        elementCount = elements.Count;
        angleOffset = 360f / elementCount;

        Vector3[] startPositions = new Vector3[elementCount];
        Quaternion[] startRotations = new Quaternion[elementCount];
        Vector3[] targetPositions = new Vector3[elementCount];
        Quaternion[] targetRotations = new Quaternion[elementCount];

        for (int i = 0; i < elementCount; i++)
        {
            RectTransform rt = elements[i].GetComponent<RectTransform>();

            startPositions[i] = rt.anchoredPosition;
            startRotations[i] = rt.rotation;

            float angle = i * angleOffset;
            float radius = rt.anchoredPosition.magnitude; // keep current distance

            Vector2 targetPos = new Vector2(
                Mathf.Cos(Mathf.Deg2Rad * angle),
                Mathf.Sin(Mathf.Deg2Rad * angle)
            ) * radius;

            targetPositions[i] = targetPos;
            targetRotations[i] = Quaternion.Euler(0, 0, -angle);
        }

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            for (int i = 0; i < elementCount; i++)
            {
                RectTransform rt = elements[i].GetComponent<RectTransform>();
                TMP_Text text = rt.GetComponentInChildren<TMP_Text>();

                rt.anchoredPosition = Vector3.Lerp(startPositions[i], targetPositions[i], t);
                rt.rotation = Quaternion.Slerp(startRotations[i], targetRotations[i], t);

                if (text != null)
                {
                    float angle = i * angleOffset;
                    text.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
                }
            }

            yield return null;
        }
    }


    // Helper to reapply angles and rotation
    private void RecalculateAngles()
    {
        elementCount = elements.Count;
        angleOffset = 360f / elementCount;

        for (int i = 0; i < elementCount; i++)
        {
            var element = elements[i];
            element.parentRM = this;
            element.assignedIndex = i;

            Image image = element.GetComponentInChildren<Image>();
            if (i==0) image.color = Color.red; else image.color = Color.white;

            float angle = i * angleOffset;

            element.setAllAngles(angle, angleOffset);

            // Rotate the entire element container
            RectTransform rt = element.GetComponent<RectTransform>();
            rt.rotation = Quaternion.Euler(0, 0, -angle);

            TMP_Text text = element.GetComponentInChildren<TMP_Text>();
            if (text != null)
            {
                text.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                Debug.LogError("[RMF_RadialMenu] RecalculateAngles no text found ");
            }
        }
    }

    //Selects the button with the specified index.
    private void selectButton(int i) {

          if (elements[i].active == false) {

            elements[i].highlightThisElement(pointer); //Select this one

            if (previousActiveIndex != i) 
                elements[previousActiveIndex].unHighlightThisElement(pointer); //Deselect the last one.
            

        }

        previousActiveIndex = i;

    }

    //Keeps angles between 0 and 360.
    private float normalizeAngle(float angle) {

        angle = angle % 360f;

        if (angle < 0)
            angle += 360;

        return angle;

    }


}
