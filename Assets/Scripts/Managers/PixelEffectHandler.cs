using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PixelEffectHandler : MonoBehaviour
{

    public bool dontDestroyOnLoad = false;


    [System.Serializable]
    public class HexColor
    {
        public Color h1;
        public Color h1_5;
        public Color h2;
    }

    #region VARIABLES
    HexColor[] hexColors3;
    private int colorIndex;

    private Texture2D texture;
    private Sprite sprite;

    private GameObject groundPF;
    private HexColor groundColor;
    bool hasSetGroundColor = false;

    public bool hasGradientBackground = false;

    private Color textColor;

    #endregion

    private void Awake()
    {
        if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Initialise();
    }


    private void Update()
    {
        ChangeGroundColor();
    }


    private void Initialise()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        if (sprite == null) print("ERROR: no sprite found");
        texture = sprite.texture;

        // ChangePixelColors(GetHex());

        //ResizeSpriteToScreen();
    }


    void ChangeGroundColor()
    {
        if (hasSetGroundColor) return;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            groundPF = GameObject.Find("Ground");
            if (groundPF != null)
            {
                SpriteRenderer spriteRenderer = groundPF.GetComponent<SpriteRenderer>();
                spriteRenderer.color = hexColors3[colorIndex].h1;
                hasSetGroundColor = true;
            }
            else
            {
                print("ERROR: cannot find GROUND");
            }
        }
        else hasSetGroundColor = false;
    }


    HexColor GetHex()
    {
        Debug.Log("[PixelEffectHandler] GetHex" + hexColors3.Length);
        int max = (hexColors3.Length);
        int rand = Random.Range(0, max);
        colorIndex = rand;
        //print("Max / Rand " + max+ " / "+rand);
        return hexColors3[rand];
    }


    void ChangePixelColors(HexColor hex)
    {
        if (hasGradientBackground)
        {
            texture.SetPixel(0, 1, hex.h1);
            texture.SetPixel(0, 0, hex.h2);
        }
        else
        {
            texture.SetPixel(0, 1, hex.h1);
            texture.SetPixel(0, 0, hex.h1);
        }
        texture.Apply();
        sprite = Sprite.Create(texture,
            new Rect(0, 0, 1, 2),
            new Vector2(0.5f, 0.5f));
    }

    public void SetTextColorInScene()
    {

    }


    public Color GetColorForText()
    {
        //return hexColors3[colorIndex].h1_5;
        return hexColors3[colorIndex].h2;
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

}