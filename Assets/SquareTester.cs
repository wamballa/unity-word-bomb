using MoreMountains.Feedbacks;
using UnityEngine;

public class SquareTester : MonoBehaviour
{

    public MMFeedbacks moveFeedback;
    public MMFeedbacks landingFeedback;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (moveFeedback != null)
        {
            moveFeedback.Events.OnComplete.AddListener(OnMoveFeedbackComplete);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (moveFeedback != null)
            {
                moveFeedback.PlayFeedbacks();
            }
        }
    }

    void OnMoveFeedbackComplete()
    {
        if (landingFeedback != null)
        {
            Debug.Log("LANDING");
            landingFeedback.PlayFeedbacks();
        }
    }

}
