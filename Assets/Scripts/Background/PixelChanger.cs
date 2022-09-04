using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PixelChanger : MonoBehaviour
{
    public Texture2D texture;// = new Texture2D(1, 2); // load your texture here
    public Sprite sprite;


    public string hex1 = "440D0F";
    public string hex2 = "AF9BB6";

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        if (sprite == null) print("ERROR: no sprite found");
        texture = sprite.texture;
        ChangePixelColors();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangePixelColors()
    {
        Color color1;
        Color color2;
        ColorUtility.TryParseHtmlString(hex1, out color1);
        ColorUtility.TryParseHtmlString(hex2, out color2);
        texture.SetPixel(1, 1, color1);
        texture.SetPixel(1, 2, color2);
        texture.Apply();
        sprite = Sprite.Create(texture,
            new Rect(0, 0, 1, 2),
            new Vector2(0.5f, 0.5f));
}

}