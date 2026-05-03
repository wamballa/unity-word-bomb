using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Word Bomb/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Fall Speeds")]
    [Min(0f)] [SerializeField] private float wordFallSpeed = 0.4f;
    [Min(0f)] [SerializeField] private float letterFallSpeed = 0.6f;
    [Min(0f)] [SerializeField] private float numberFallSpeed = 4.6f;

    [Header("Spawn Delays")]
    [Min(0f)] [SerializeField] private float wordFallDelay = 3.2f;
    [Min(0f)] [SerializeField] private float letterFallDelay = 5f;
    [Min(0f)] [SerializeField] private float numberFallDelay = 3.1f;

    [Header("Difficulty")]
    [Min(0f)] [SerializeField] private float wordDelayDecrement = 0.2f;
    [Min(0f)] [SerializeField] private float letterDelayDecrement = 0.1f;
    [Min(0f)] [SerializeField] private float wordSpeedIncrement = 0f;
    [Min(0f)] [SerializeField] private float difficultyDuration = 30f;
    [Min(1)] [SerializeField] private int startingWordLength = 3;
    [Min(1)] [SerializeField] private int maxWordLength = 7;

    [Header("Danger")]
    [Range(0f, 100f)] [SerializeField] private float dangerThresholdPercent = 80f;

    [Header("Radial Letters")]
    [SerializeField] private List<string> radialLetterSets = new()
    {
        "abeort",
        "aefnos",
        "aeglsx",
        "eflort",
        "ehirsu",
        "einrst",
        "acelot",
        "adoprt",
        "acirst"
    };
    [Min(0)] [SerializeField] private int startingRadialSetIndex = 0;

    public float WordFallSpeed => wordFallSpeed;
    public float LetterFallSpeed => letterFallSpeed;
    public float NumberFallSpeed => numberFallSpeed;
    public float WordFallDelay => wordFallDelay;
    public float LetterFallDelay => letterFallDelay;
    public float NumberFallDelay => numberFallDelay;
    public float WordDelayDecrement => wordDelayDecrement;
    public float LetterDelayDecrement => letterDelayDecrement;
    public float WordSpeedIncrement => wordSpeedIncrement;
    public float DifficultyDuration => difficultyDuration;
    public int StartingWordLength => startingWordLength;
    public int MaxWordLength => maxWordLength;
    public float DangerThresholdPercent => dangerThresholdPercent;
    public IReadOnlyList<string> RadialLetterSets => radialLetterSets;
    public int StartingRadialSetIndex => startingRadialSetIndex;
}
