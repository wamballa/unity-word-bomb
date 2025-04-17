// updates UI + explosion visuals

using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class WordVisual : MonoBehaviour
{
    public TMP_Text text;
    public GameObject letterPrefab;
    public Transform explosionPoint;
    public MMF_Player explodeFeedback;

    private string fullWord;
    private int lettersHighlightIndex = 0;

    private BoxCollider2D boxCollider; // add this

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>(); // add this
    }

    public void SetText(string word)
    {
        if (text == null)
        {
            Debug.LogWarning("TextMeshPro reference missing on WordVisual.");
            return;
        }
        transform.name = word;
        fullWord = word;
        lettersHighlightIndex = 0;
        text.text = word;
        text.color = Color.white;

        AdjustColliderWidth();
    }

    private void AdjustColliderWidth()
    {
        if (boxCollider != null && text != null)
        {
            text.ForceMeshUpdate();
            float width = text.textBounds.size.x;
            Vector2 size = boxCollider.size;
            size.x = width;
            boxCollider.size = size;

            // Center collider without offset
            boxCollider.offset = Vector2.zero;

        }
    }

    public void RevealNextLetter()
    {
        if (text == null || lettersHighlightIndex >= fullWord.Length) return;

        lettersHighlightIndex++;

        text.text = $"<color=#FF0000>{fullWord.Substring(0, lettersHighlightIndex)}</color>{fullWord.Substring(lettersHighlightIndex)}";
    }

    public void ExplodeToLetters()
    {
        if (text == null || fullWord == null) return;

        text.ForceMeshUpdate();
        TMP_TextInfo textInfo = text.textInfo;

        for (int i = 0; i < fullWord.Length; i++)
        {
            if (i >= textInfo.characterCount) continue;
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            Vector3 letterWorldPos = text.transform.TransformPoint((charInfo.bottomLeft + charInfo.topRight) / 2);

            GameObject letter = Instantiate(letterPrefab, letterWorldPos, Quaternion.identity);

            //GameObject letter = Instantiate(letterPrefab, explosionPoint.position + new Vector3(i * 0.5f, 0, 0), Quaternion.identity);

            TMP_Text letterText = letter.GetComponentInChildren<TMP_Text>();
            if (letterText != null)
            {
                letterText.text = fullWord[i].ToString();
            }
        }

        if (explodeFeedback != null)
        {
            explodeFeedback?.PlayFeedbacks();
        }
    }
}
