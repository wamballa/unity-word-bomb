using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PixelChanger : MonoBehaviour
{
    [System.Serializable]
    public class HexColor2
    {
        public string h1;
        public string h2;

        public HexColor2()
        {
            h1 = '#' + h1;
            h2 = '#' + h2;
        }
    }

    [SerializeField] HexColor2[] hexColors;

    private Texture2D texture;
    private Sprite sprite;

    private void OnEnable()
    {
        //if (SceneManager.GetActiveScene().buildIndex == 0)
            print("ENABLED " + SceneManager.GetActiveScene().name);
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        if (sprite == null) print("ERROR: no sprite found");
        texture = sprite.texture;

        ChangePixelColors(GetHex());

        ResizeSpriteToScreen();

    }

    HexColor2 GetHex()
    {
        int max = (hexColors.Length);
        int rand = Random.Range(0, max);
        return hexColors[rand];
    }

    void ResizeSpriteToScreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector2 localScale = new Vector2(worldScreenWidth / width, worldScreenHeight / height);

        transform.localScale = localScale;

    }

    void ChangePixelColors(HexColor2 hex)
    {
        // Sprite to create is 1 x 2 (width x height)
        //print("Hex " + hex.h1 + " , " + hex.h2);
        Color color1;
        Color color2;
        ColorUtility.TryParseHtmlString(hex.h1, out color1);
        ColorUtility.TryParseHtmlString(hex.h2, out color2);

        texture.SetPixel(0, 0, color1);
        texture.SetPixel(0,1, color2);

        texture.Apply();
        sprite = Sprite.Create(texture,
            new Rect(0, 0, 1, 2),
            new Vector2(0.5f, 0.5f));
        }
}