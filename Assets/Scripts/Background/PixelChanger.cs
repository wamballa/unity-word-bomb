using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HexColor2 
{
    public string h1;
    public string h2;
    public HexColor2(string _hex1, string _hex2)
    {
        h1 = _hex1;
        h2 = _hex2;
    }
    //public float this[int index] { get;set; }
}

public class PixelChanger : MonoBehaviour
{
    [SerializeField] HexColor2[] hexColors;

    public Texture2D texture;// = new Texture2D(1, 2); // load your texture here
    public Sprite sprite;

    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        if (sprite == null) print("ERROR: no sprite found");
        texture = sprite.texture;

        print("sprite width " + sprite.rect.width);
        print("sprite height " + sprite.rect.height);

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
        //print("Hex 1 = " + hex.h1);
        //print("Hex 2 = " + hex.h2);
        Color color1;
        Color color2;
        ColorUtility.TryParseHtmlString(hex.h1, out color1);
        ColorUtility.TryParseHtmlString(hex.h2, out color2);

        //print("Colour 1 = " + color1);
        //print("Colour 2 = " + color2);

        //print("Reverse = " + ColorUtility.ToHtmlStringRGB(color2));

        texture.SetPixel(0, 0, color1);
        texture.SetPixel(0,1, color2);

        texture.Apply();
        sprite = Sprite.Create(texture,
            new Rect(0, 0, 1, 2),
            new Vector2(0.5f, 0.5f));
}


}