using UnityEngine;

public class DebugOverlay : MonoBehaviour
{
    private const float MinAutoplayInterval = 0.05f;
    private const float MaxAutoplayInterval = 1.5f;

    [SerializeField] private bool enableOverlay = true;
    [SerializeField] private bool overlayVisible = false;
    [SerializeField] private bool autoplayEnabled = false;
    [SerializeField] private float autoplayInterval = 0.18f;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private WordGameplayManager wordGameplayManager;
    [SerializeField] private ObjectSpawner objectSpawner;

    private GUIStyle panelStyle;
    private GUIStyle titleStyle;
    private GUIStyle valueStyle;
    private float nextAutoplayTime;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateRuntimeOverlay()
    {
        var existing = FindFirstObjectByType<DebugOverlay>();
        if (existing != null) return;

        var overlayObject = new GameObject("DebugOverlay");
        DontDestroyOnLoad(overlayObject);
        overlayObject.AddComponent<DebugOverlay>();
    }

    private void Awake()
    {
        RefreshReferences();
    }

    private void Update()
    {
        if (!enableOverlay) return;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            overlayVisible = !overlayVisible;
        }

        if (autoplayEnabled)
        {
            TryAutoplay();
        }
    }

    private void OnGUI()
    {
        if (!enableOverlay) return;

        EnsureStyles();

        if (GUI.Button(new Rect(12f, 12f, 88f, 28f), overlayVisible ? "Debug -" : "Debug +"))
        {
            overlayVisible = !overlayVisible;
        }

        if (!overlayVisible) return;

        GUI.Box(new Rect(12f, 48f, 320f, 258f), GUIContent.none, panelStyle);
        GUILayout.BeginArea(new Rect(24f, 58f, 296f, 238f));

        GUILayout.Label("Debug Overlay", titleStyle);
        DrawValue("Score", gameManager ? gameManager.GetScore().ToString() : "n/a");
        DrawValue("Danger", gameManager ? $"{gameManager.GetPercentageFilled():0.0}% / {gameManager.GetDangerThresholdPercent():0.0}%" : "n/a");
        DrawValue("Radial Set", gameManager ? gameManager.GetCurrentRadialLetterSet() : "n/a");
        DrawValue("Word Length", gameManager ? gameManager.GetWordDifficultyLevel().ToString() : "n/a");
        DrawValue("Words / Numbers", wordGameplayManager ? $"{wordGameplayManager.WordCount} / {wordGameplayManager.NumberCount}" : "n/a");
        DrawValue("Spawning", objectSpawner ? $"W:{objectSpawner.canSpawnWord} N:{objectSpawner.canSpawnNumber}" : "n/a");
        DrawValue("Active / Auto", wordGameplayManager ? $"{wordGameplayManager.HasActiveWord} / {autoplayEnabled}" : $"n/a / {autoplayEnabled}");

        var target = GetTargetWord();
        DrawValue("Target", FormatTarget(target));

        GUILayout.Space(6f);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(autoplayEnabled ? "Autoplay On" : "Autoplay Off", GUILayout.Height(26f)))
        {
            autoplayEnabled = !autoplayEnabled;
            nextAutoplayTime = Time.time;
        }
        if (GUILayout.Button("Clear Target", GUILayout.Height(26f)))
        {
            wordGameplayManager?.ClearDebugTarget();
            nextAutoplayTime = Time.time + autoplayInterval;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Slower", GUILayout.Height(24f)))
        {
            autoplayInterval = Mathf.Min(MaxAutoplayInterval, autoplayInterval + 0.05f);
        }
        GUILayout.Label($"{autoplayInterval:0.00}s", valueStyle, GUILayout.Width(64f));
        if (GUILayout.Button("Faster", GUILayout.Height(24f)))
        {
            autoplayInterval = Mathf.Max(MinAutoplayInterval, autoplayInterval - 0.05f);
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    private void TryAutoplay()
    {
        if (Time.time < nextAutoplayTime) return;

        RefreshReferences();

        if (wordGameplayManager != null && wordGameplayManager.TryGetAutoplayTarget(out var target))
        {
            char nextLetter = target.GetNextLetter();
            if (nextLetter != '\0')
            {
                InputRouter.RouteKey(nextLetter.ToString());
            }
        }

        nextAutoplayTime = Time.time + autoplayInterval;
    }

    private FallingWord GetTargetWord()
    {
        if (wordGameplayManager == null) return null;
        return wordGameplayManager.TryGetAutoplayTarget(out var target) ? target : null;
    }

    private string FormatTarget(FallingWord target)
    {
        if (target == null) return "none";

        string word = target.GetCurrentWord();
        char nextLetter = target.GetNextLetter();
        return $"{word} [{target.GetTypedIndex()}] next:{(nextLetter == '\0' ? "-" : nextLetter.ToString())}";
    }

    private void RefreshReferences()
    {
        if (gameManager == null) gameManager = FindFirstObjectByType<GameManager>();
        if (wordGameplayManager == null) wordGameplayManager = FindFirstObjectByType<WordGameplayManager>();
        if (objectSpawner == null) objectSpawner = FindFirstObjectByType<ObjectSpawner>();
    }

    private void DrawValue(string label, string value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, valueStyle, GUILayout.Width(106f));
        GUILayout.Label(value, valueStyle);
        GUILayout.EndHorizontal();
    }

    private void EnsureStyles()
    {
        if (panelStyle != null) return;

        Texture2D background = new Texture2D(1, 1);
        background.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.72f));
        background.Apply();

        panelStyle = new GUIStyle(GUI.skin.box);
        panelStyle.normal.background = background;
        panelStyle.padding = new RectOffset(10, 10, 10, 10);

        titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.white;

        valueStyle = new GUIStyle(GUI.skin.label);
        valueStyle.normal.textColor = Color.white;
    }
}
