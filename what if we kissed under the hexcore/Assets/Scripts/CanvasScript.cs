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
	private SpriteRenderer JayvikIdleRenderer;
	[SerializeField]
	private SpriteRenderer JayvikKissRenderer;
	[SerializeField]
	private SpriteRenderer HexcoreCalm;
	[SerializeField]
	private SpriteRenderer HexcoreAggitated;
	[SerializeField]
	private SpriteRenderer HeimerdingerFront;
	[SerializeField]
	private SpriteRenderer HeimerdingerBack;
	[SerializeField]
	private SpriteRenderer HeimerdingerAttention;
	[SerializeField]
	private SpriteRenderer HeimerdingerHandsDown;
	[SerializeField]
	private GameObject Win;
	[SerializeField]
	private GameObject Lose;
	[SerializeField]
	private GameObject Win_game;
	[SerializeField]
	private GameObject Menu;
	[SerializeField]
	private GameObject Settings;
	[SerializeField]
	private GameObject Instructions;
	[SerializeField]
	private TextMeshProUGUI Level_text;
	[SerializeField]
	private AudioSource HeimerginderAudio;
	[SerializeField]
	private AudioSource KissingAudio;
	[SerializeField]
	private AudioSource HexcoreAudio;

	private float maxPoints = 100f;
	private float currentPoints;
	private float targetTimeTalking;
	private float targetTimeAttention = 0.5f;
	private float targetTimeTurned;
	private float targetTimeHandsDown;
	private float[] targetTimeTalkingArray;
	private float[] targetTimeTurnedArray;
	private int level = 0;
	private bool kissing = false;
	private bool turned = false;
	private LevelText level_text_script;

	void Start()
	{
		targetTimeTalkingArray = new float[3];
		targetTimeTalkingArray[0] = UnityEngine.Random.Range(3f, 4f);
		targetTimeTalkingArray[1] = UnityEngine.Random.Range(3f, 3.5f);
		targetTimeTalkingArray[2] = UnityEngine.Random.Range(2f, 3f);
		targetTimeTalking = targetTimeTalkingArray[level];

		targetTimeTurnedArray = new float[3];
		targetTimeTurnedArray[0] = UnityEngine.Random.Range(3f, 4f);
		targetTimeTurnedArray[1] = UnityEngine.Random.Range(2.5f, 3f);
		targetTimeTurnedArray[2] = UnityEngine.Random.Range(2f, 2.5f);
		targetTimeTurned = targetTimeTurnedArray[level];
		targetTimeHandsDown = 0.3f;
		currentPoints = 0.0f;

		level_text_script = Level_text.GetComponent<LevelText>();
		level_text_script.Show();

		if (PlayerPrefs.HasKey("volume"))
		{
			AudioListener.volume = PlayerPrefs.GetFloat("volume");
		}		
	}

	void FixedUpdate()
    {	
		if (!Win.activeInHierarchy && !Lose.activeInHierarchy && !Menu.activeInHierarchy && !Win_game.activeInHierarchy)
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
				if (!turned)
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
		if (currentPoints >= maxPoints && !Win.activeInHierarchy)
		{
			if (level < 2)
				WinLevel();
			else
				WinGame();
		}

		if (kissing && !turned && !Lose.activeInHierarchy)
		{
			LoseGame();
		}

		if (Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonUp(1))
		{
			Menu.SetActive(!Menu.activeInHierarchy);
			UIHandler.instance.ToggleUIVisibility(!Menu.activeInHierarchy);
		}
	}

	public void Continue()
	{
		Win.SetActive(false);
		HeimerginderAudio.Play();
		UIHandler.instance.ToggleUIVisibility(true);
		level_text_script.SetText("Level " + (level + 1));
		level_text_script.Show();
	}

	public void Restart()
	{
		Lose.SetActive(false);
		ToggleWinGameVisibility(false);
		UIHandler.instance.ToggleUIVisibility(true);
		level = 0;
		level_text_script.SetText("Level " + (level + 1));
		level_text_script.Show();
		Reset();
	}

	public void ToggleSettingsVisibility(bool toggle)
	{
		Settings.SetActive(toggle);
		if (toggle == true)
			StopAllSounds();
		else
			ResetSounds();
	}

	public void ToggleInstructionsVisibility(bool toggle)
	{
		Instructions.SetActive(toggle);
		if (toggle == true)
			StopAllSounds();
		else
			ResetSounds();
	}

	public void ToggleWinGameVisibility(bool toggle)
	{
		Win_game.SetActive(toggle);
		if (toggle == true)
			StopAllSounds();
		else
			ResetSounds();
	}

	void TalkingTimerEnded()
	{
		turned = true;
		HeimerginderAudio.Stop();
		HexcoreAudio.Play();
		HeimerdingerFront.enabled = false;
		HeimerdingerAttention.enabled = true;
		HexcoreCalm.enabled = false;
		HexcoreAggitated.enabled = true;
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
		HeimerdingerAttention.enabled = false;
		HeimerdingerBack.enabled = true;
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
		HeimerdingerBack.enabled = false;
		HeimerdingerHandsDown.enabled = true;
		HandsDownTimerStarted();
	}

	void HandsDownTimerStarted()
	{
		targetTimeHandsDown -= Time.deltaTime;
		if (targetTimeHandsDown <= 0.0f)
		{
			turned = false;
			HeimerdingerHandsDown.enabled = false;
			HeimerdingerFront.enabled = true;
			HexcoreAggitated.enabled = false;
			HexcoreCalm.enabled = true;
			HeimerginderAudio.Play();
			HexcoreAudio.Stop();
			ResetTimers();
		}
	}

	void ResetTimers()
	{
		targetTimeTalking = targetTimeTalkingArray[level];
		targetTimeTurned = targetTimeTurnedArray[level];
		targetTimeAttention = 0.5f;
		targetTimeHandsDown = 0.3f;
	}

	void ResetSprites()
	{
		HeimerdingerBack.enabled = false;
		HeimerdingerHandsDown.enabled = false;
		HeimerdingerFront.enabled = true;
		HexcoreAggitated.enabled = false;
		HexcoreCalm.enabled = true;
	}

	void ResetSounds()
	{
		if (turned)
			HexcoreAudio.Play();
		else
		{
			HexcoreAudio.Stop();
			HeimerginderAudio.Play();
		}
	}

	void Reset()
	{
		currentPoints = 0;
		UIHandler.instance.SetHealthValue(0/100);
		ResetTimers();
		ResetSprites();
		kissing = false;
		turned = false;
		ResetSounds();
	}

	void StartKissing()
	{
		kissing = true;
		KissingAudio.Play();
		JayvikIdleRenderer.enabled = false;
		JayvikKissRenderer.enabled = true;
	}

	void EndKissing()
	{
		KissingAudio.Stop();
		kissing = false;
		JayvikIdleRenderer.enabled = true;
		JayvikKissRenderer.enabled = false;
	}

	void WinLevel()
	{
		Win.SetActive(true);
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
		Lose.SetActive(true);
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
		HeimerginderAudio.Stop();
		KissingAudio.Stop();
		HexcoreAudio.Stop();
	}
}
