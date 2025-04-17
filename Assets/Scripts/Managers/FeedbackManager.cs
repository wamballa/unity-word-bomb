using MoreMountains.Feedbacks;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public bool logToConsole = true;
    public static FeedbackManager Instance { get; private set; }

    public MMF_Player playCameraShakeFeedback;

    public MMF_Player scoreMovesFeedback;
    public MMF_Player scoreLandingFeedback;

    public MMF_Player groundMovesFeedback;
    public MMF_Player groundLandingFeedback;

    public MMF_Player pauseMovesFeedback;
    public MMF_Player pauseLandingFeedback;

    public MMF_Player keyboardMovesFeedback;
    public MMF_Player keyboardLandingFeedback;

    public void PlayCameraShake() => playCameraShakeFeedback?.PlayFeedbacks();
    // public void PlayScoreMove() => scoreMovesFeedback?.PlayFeedbacks();

    private void Awake()
    {
        if (Instance == null)
        {
            Log("Awake: Create FeedbackManager");
            Instance = this;
        }

        else
        {
            Log("Awake: Destroy FeedbackManager");
            Destroy(gameObject);
        }
            
    }


    public void PlayKeyboardMove()
    {
        if (keyboardMovesFeedback == null || keyboardLandingFeedback == null) return;

        // Clear any previous listeners to avoid duplicates
        keyboardMovesFeedback.Events.OnComplete.RemoveAllListeners();

        // Add the landing trigger
        keyboardMovesFeedback.Events.OnComplete.AddListener(() =>
        {
            keyboardLandingFeedback.PlayFeedbacks();
        });

        keyboardMovesFeedback.PlayFeedbacks();
    }

    public void PlayPauseMove()
    {
        if (pauseMovesFeedback == null || pauseLandingFeedback == null) return;

        // Clear any previous listeners to avoid duplicates
        pauseMovesFeedback.Events.OnComplete.RemoveAllListeners();

        // Add the landing trigger
        pauseMovesFeedback.Events.OnComplete.AddListener(() =>
        {
            pauseLandingFeedback.PlayFeedbacks();
        });

        pauseMovesFeedback.PlayFeedbacks();
    }

    public void PlayGroundMove()
    {
        if (groundMovesFeedback == null || groundLandingFeedback == null) return;

        // Clear any previous listeners to avoid duplicates
        groundMovesFeedback.Events.OnComplete.RemoveAllListeners();

        // Add the landing trigger
        groundMovesFeedback.Events.OnComplete.AddListener(() =>
        {
            groundLandingFeedback.PlayFeedbacks();
        });

        groundMovesFeedback.PlayFeedbacks();
    }

    public void PlayScoreMove()
    {
        if (scoreMovesFeedback == null || scoreLandingFeedback == null) return;

        // Clear any previous listeners to avoid duplicates
        scoreMovesFeedback.Events.OnComplete.RemoveAllListeners();

        // Add the landing trigger
        scoreMovesFeedback.Events.OnComplete.AddListener(() =>
        {
            scoreLandingFeedback.PlayFeedbacks();
        });

        scoreMovesFeedback.PlayFeedbacks();
    }

    void Log(object message)
    {
        if (logToConsole)
            Debug.Log("[FeedbackManager] " + message);
    }

    void LogError(object message)
    {
        if (logToConsole)
            Debug.LogError("[FeedbackManager] " + message);
    }


}
