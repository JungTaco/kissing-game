using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
	private TMP_Text text;
	private float targetTimeLevelText;
	private bool fadingIn = false;
	private bool fadingOut = false;

	void Awake()
    {
        text = GetComponent<TMP_Text>();
        text.text = "Level 1";
		targetTimeLevelText = 1f;
		text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
	}

    // Update is called once per frame
    void Update()
    {
		if (fadingIn)
		{
			targetTimeLevelText -= Time.deltaTime;
			if (targetTimeLevelText <= 0)
			{
				FadeIn();
			}
		}
		else if (fadingOut)
		{
			targetTimeLevelText -= Time.deltaTime;
			if (targetTimeLevelText <= 0)
			{
				FadeOut();
			}
		}
	}

    public void Show()
    {
		fadingIn = true;
		ResetTimer();
	}

	public void SetText(string level_text)
	{
		text.text = level_text;
	}

	private void FadeIn()
	{
		if (text.color.a < 1.0f)
		{
			text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + Time.deltaTime);
		}
		else
		{
			fadingIn = false;
			ResetTimer();
			fadingOut = true;
		}
	}

	private void FadeOut()
	{
		if (text.color.a > 0)
		{
			text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime);
		}
		else
		{
			fadingOut = false;
		}
	}

	private void ResetTimer()
	{
		targetTimeLevelText = 1f;
	}
}
