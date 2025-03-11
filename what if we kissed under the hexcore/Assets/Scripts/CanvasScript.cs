using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer jayvikIdleRenderer;
	[SerializeField]
	private SpriteRenderer jayvikKissRenderer;
	[SerializeField]
	private SpriteRenderer hexcoreCalm;
	[SerializeField]
	private SpriteRenderer hexcoreAggitated;
	[SerializeField]
	private SpriteRenderer heimerdingerFront;
	[SerializeField]
	private SpriteRenderer heimerdingerBack;
	[SerializeField]
	private SpriteRenderer heimerdingerAttention;
	[SerializeField]
	private SpriteRenderer heimerdingerHandsDown;
	[SerializeField]
	private GameObject win;
	[SerializeField]
	private GameObject lose;
	[SerializeField]
	private GameObject win_game;
	[SerializeField]
	private GameObject menu;
	[SerializeField]
	private GameObject settings;
	[SerializeField]
	private GameObject instructions;
	[SerializeField]
	private TextMeshProUGUI level_text;
	[SerializeField]
	private AudioSource heimerginderAudio;
	[SerializeField]
	private AudioSource kissingAudio;
	[SerializeField]
	private AudioSource hexcoreAudio;

	private float maxPoints = 100f;
	private float currentPoints;
	private float targetTimeTalking;
	private float targetTimeAttention = 0.5f;
	private float targetTimeTurned;
	private float targetTimeHandsDown;
	private float[] targetTimeTalkingArray;
	private float[] targetTimeTurnedAwayArray;
	private int level = 0;
	private bool kissing = false;
	private bool turnedAway = false;
	private LevelText level_text_script;

	void Start()
	{
		//sets timers for each of the 3 levels
		targetTimeTalkingArray = new float[3];
		targetTimeTalkingArray[0] = UnityEngine.Random.Range(3f, 4f);
		targetTimeTalkingArray[1] = UnityEngine.Random.Range(3f, 3.5f);
		targetTimeTalkingArray[2] = UnityEngine.Random.Range(2f, 3f);
		targetTimeTalking = targetTimeTalkingArray[level];

		targetTimeTurnedAwayArray = new float[3];
		targetTimeTurnedAwayArray[0] = UnityEngine.Random.Range(3f, 4f);
		targetTimeTurnedAwayArray[1] = UnityEngine.Random.Range(2.5f, 3f);
		targetTimeTurnedAwayArray[2] = UnityEngine.Random.Range(2f, 2.5f);
		targetTimeTurned = targetTimeTurnedAwayArray[level];
		targetTimeHandsDown = 0.3f;

		currentPoints = 0.0f;
		level_text_script = level_text.GetComponent<LevelText>();
		level_text_script.Show();

		if (PlayerPrefs.HasKey("volume"))
		{
			AudioListener.volume = PlayerPrefs.GetFloat("volume");
		}		
	}

	void FixedUpdate()
    {	
		if (!win.activeInHierarchy && !lose.activeInHierarchy && !menu.activeInHierarchy && !win_game.activeInHierarchy)
		{
			if (Input.GetMouseButton(0))
			{
				if (!kissing)
					StartKissing();
				SetPoints();
			}
			else
			{
				EndKissing();
			}

			targetTimeTalking -= Time.deltaTime;
			if (targetTimeTalking <= 0.0f)
			{
				if (!turnedAway)
				{
					TalkingTimerEnded();
				}
				AttentionTimerStarted();
				TurnedTimerStarted();
			}
		}
	}

	private void Update()
	{
		if (currentPoints >= maxPoints && !win.activeInHierarchy)
		{
			if (level < 2)
				WinLevel();
			else
				WinGame();
		}

		if (kissing && !turnedAway && !lose.activeInHierarchy)
		{
			LoseGame();
		}

		if (Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonUp(1))
		{
			menu.SetActive(!menu.activeInHierarchy);
			UIHandler.instance.ToggleUIVisibility(!menu.activeInHierarchy);
		}
	}

	public void Continue()
	{
		win.SetActive(false);
		heimerginderAudio.Play();
		UIHandler.instance.ToggleUIVisibility(true);
		level_text_script.SetText("Level " + (level + 1));
		level_text_script.Show();
	}

	public void Restart()
	{
		lose.SetActive(false);
		ToggleWinGameVisibility(false);
		UIHandler.instance.ToggleUIVisibility(true);
		level = 0;
		level_text_script.SetText("Level " + (level + 1));
		level_text_script.Show();
		Reset();
	}

	public void ToggleSettingsVisibility(bool toggle)
	{
		settings.SetActive(toggle);
		if (toggle == true)
			StopAllSounds();
		else
			ResetSounds();
	}

	public void ToggleInstructionsVisibility(bool toggle)
	{
		instructions.SetActive(toggle);
		if (toggle == true)
			StopAllSounds();
		else
			ResetSounds();
	}

	public void ToggleWinGameVisibility(bool toggle)
	{
		win_game.SetActive(toggle);
		if (toggle == true)
			StopAllSounds();
		else
			ResetSounds();
	}

	void TalkingTimerEnded()
	{
		turnedAway = true;
		heimerginderAudio.Stop();
		hexcoreAudio.Play();
		heimerdingerFront.enabled = false;
		heimerdingerAttention.enabled = true;
		hexcoreCalm.enabled = false;
		hexcoreAggitated.enabled = true;
	}

	void AttentionTimerStarted()
	{
		targetTimeAttention -= Time.deltaTime;
		if (targetTimeAttention <= 0.0f)
		{
			AttentionTimerEnded();
		}
	}

	void AttentionTimerEnded()
	{
		heimerdingerAttention.enabled = false;
		heimerdingerBack.enabled = true;
	}

	void TurnedTimerStarted()
	{
		targetTimeTurned -= Time.deltaTime;
		if (targetTimeTurned <= 0.0f)
		{
			TurnedTimerEnded();
		}
	}

	void TurnedTimerEnded()
	{
		heimerdingerBack.enabled = false;
		heimerdingerHandsDown.enabled = true;
		HandsDownTimerStarted();
	}

	void HandsDownTimerStarted()
	{
		targetTimeHandsDown -= Time.deltaTime;
		if (targetTimeHandsDown <= 0.0f)
		{
			turnedAway = false;
			heimerdingerHandsDown.enabled = false;
			heimerdingerFront.enabled = true;
			hexcoreAggitated.enabled = false;
			hexcoreCalm.enabled = true;
			heimerginderAudio.Play();
			hexcoreAudio.Stop();
			ResetTimers();
		}
	}

	void ResetTimers()
	{
		targetTimeTalking = targetTimeTalkingArray[level];
		targetTimeTurned = targetTimeTurnedAwayArray[level];
		targetTimeAttention = 0.5f;
		targetTimeHandsDown = 0.3f;
	}

	void ResetSprites()
	{
		heimerdingerBack.enabled = false;
		heimerdingerHandsDown.enabled = false;
		heimerdingerFront.enabled = true;
		hexcoreAggitated.enabled = false;
		hexcoreCalm.enabled = true;
	}

	void ResetSounds()
	{
		if (turnedAway)
			hexcoreAudio.Play();
		else
		{
			hexcoreAudio.Stop();
			heimerginderAudio.Play();
		}
	}

	void Reset()
	{
		currentPoints = 0;
		UIHandler.instance.SetHealthValue(0/100);
		ResetTimers();
		ResetSprites();
		kissing = false;
		turnedAway = false;
		ResetSounds();
	}

	void StartKissing()
	{
		kissing = true;
		kissingAudio.Play();
		jayvikIdleRenderer.enabled = false;
		jayvikKissRenderer.enabled = true;
	}

	void EndKissing()
	{
		kissingAudio.Stop();
		kissing = false;
		jayvikIdleRenderer.enabled = true;
		jayvikKissRenderer.enabled = false;
	}

	void WinLevel()
	{
		win.SetActive(true);
		if (level < 2)
		{
			level++;
		}
		Reset();
		StopAllSounds();
		UIHandler.instance.ToggleUIVisibility(false);
	}

	void WinGame()
	{
		ToggleWinGameVisibility(true);
		UIHandler.instance.ToggleUIVisibility(false);
	}

	void LoseGame()
	{
		lose.SetActive(true);
		level = 0;
		Reset();
		StopAllSounds();
		UIHandler.instance.ToggleUIVisibility(false);
	}

	void SetPoints()
	{
		if (currentPoints < maxPoints)
		{
			currentPoints += 0.5f;
		}
		UIHandler.instance.SetHealthValue(currentPoints / maxPoints);
	}

	void StopAllSounds()
	{
		heimerginderAudio.Stop();
		kissingAudio.Stop();
		hexcoreAudio.Stop();
	}
}
