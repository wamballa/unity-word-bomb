using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordDisplay : MonoBehaviour {

	//public Text text;

	public TMP_Text text;

	public float fallSpeed = 4f;
	public float yPos;
	bool isOffScreen;

    private void Start()
    {
        //print("START");

    }
    public void SetWord (string word)
	{
		text.text = word;
	}

    public void RemoveLetter()
	{
		text.text = text.text.Remove(0, 1);
		text.color = Color.red;
	}

	public void RemoveWord ()
	{
		Destroy(gameObject);
	}

	private void Update()
	{
		yPos = transform.position.y;

		transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);

		if( transform.position.y < -4f)
        {
			//print("Word off screen "+text.text);
			isOffScreen = true;
			//if (isOffScreen) Debug.Log(">>>>>>>");
        }

	}

    public bool IsOffScreen()
    {
		return isOffScreen;
    }

}
