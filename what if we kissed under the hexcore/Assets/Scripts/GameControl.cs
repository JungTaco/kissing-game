using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameControl : MonoBehaviour
{
	public static GameControl Instance { get; private set; }
	[SerializeField]
	private ClickableCanvas canvas;
	private SpriteControl spriteControl;
	private SFXControl sfxControl;
	private MenuControl menuControl;
	private TextControl textControl;
	private ProgressBarControl progressBar;
	private bool kissing = false;
	private bool turnedAway = false;
	private float maxPoints = 100f;
	private float currentPoints;
	private int level = 0;
	private float targetTimeTalking;
	private float targetTimeAttention = 0.5f;
	private float targetTimeTurned;
	private float targetTimeHandsDown;
	private float[] targetTimeTalkingArray;
	private float[] targetTimeTurnedAwayArray;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		spriteControl = SpriteControl.Instance;
		sfxControl = SFXControl.Instance;
		menuControl = MenuControl.Instance;
		textControl = TextControl.Instance;
		progressBar = ProgressBarControl.Instance;
		currentPoints = 0.0f;

		//sets timers for each of the 3 levels
		targetTimeTalkingArray = new float[3];
		targetTimeTalkingArray[0] = UnityEngine.Random.Range(3f, 4f);
		targetTimeTalkingArray[1] = UnityEngine.Random.Range(3f, 3.5f);
		targetTimeTalkingArray[2] = UnityEngine.Random.Range(2f, 3f);

		targetTimeTurnedAwayArray = new float[3];
		targetTimeTurnedAwayArray[0] = UnityEngine.Random.Range(3f, 4f);
		targetTimeTurnedAwayArray[1] = UnityEngine.Random.Range(2.5f, 3f);
		targetTimeTurnedAwayArray[2] = UnityEngine.Random.Range(2f, 2.5f);

		targetTimeHandsDown = 0.3f;
		SetTimersLevel(level);

		if (PlayerPrefs.HasKey("volume"))
		{
			AudioListener.volume = PlayerPrefs.GetFloat("volume");
		}
	}

	void FixedUpdate()
	{
		if (!menuControl.GetIsWinVisible() && !menuControl.GetIsLoseVisible() && !menuControl.GetIsMenuVisible() && !menuControl.GetIsWinGameVisible())
		{
			if (canvas.IsBeingClicked)
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
		if (currentPoints >= maxPoints && !menuControl.GetIsWinVisible())
		{
			if (level < 2)
				WinLevel();
			else
				WinGame();
		}

		if (kissing && !turnedAway && !menuControl.GetIsLoseVisible())
		{
			LoseGame();
		}

		if (canvas.MenuButtonWasPressed)
		{
			menuControl.ToggleMenuVisibility();
			progressBar.ToggleUIVisibility(!menuControl.IsVisible);
		}
	}

	private void SetTimersLevel(int level)
	{
		targetTimeTalking = targetTimeTalkingArray[level];
		targetTimeTurned = targetTimeTurnedAwayArray[level];
	}

	public void Continue()
	{
		menuControl.Continue();
		sfxControl.Continue();
		progressBar.ToggleUIVisibility(true);
		textControl.Continue(level);
	}

	public void Restart()
	{
		menuControl.Restart();
		ToggleWinGameVisibility(false);
		progressBar.ToggleUIVisibility(true);
		level = 0;
		textControl.Restart(level);
		Reset();
	}

	public void ToggleSettingsVisibility(bool toggle)
	{
		menuControl.ToggleSettingsVisibility(toggle);
		if (toggle == true)
			sfxControl.StopAllSounds();
		else
			sfxControl.ResetSounds(turnedAway);
	}

	public void ToggleInstructionsVisibility(bool toggle)
	{
		menuControl.ToggleInstructionsVisibility(toggle);
		if (toggle == true)
			sfxControl.StopAllSounds();
		else
			sfxControl.ResetSounds(turnedAway);
	}

	public void ToggleWinGameVisibility(bool toggle)
	{
		menuControl.ToggleWinGameVisibility(toggle);
		if (toggle == true)
			sfxControl.StopAllSounds();
		else
			sfxControl.ResetSounds(turnedAway);
	}

	void TalkingTimerEnded()
	{
		turnedAway = true;
		sfxControl.TalkingTimerEnded();
		spriteControl.TalkingTimerEnded();
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
		spriteControl.AttentionTimerEnded();
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
		spriteControl.TurnedTimerEnded();
		HandsDownTimerStarted();
	}

	void HandsDownTimerStarted()
	{
		targetTimeHandsDown -= Time.deltaTime;
		if (targetTimeHandsDown <= 0.0f)
		{
			turnedAway = false;
			spriteControl.HandsDownTimerStarted();
			sfxControl.HandsDownTimerStarted();
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

	void Reset()
	{
		currentPoints = 0;
		progressBar.SetHealthValue(0 / 100);
		ResetTimers();
		spriteControl.ResetSprites();
		kissing = false;
		turnedAway = false;
		sfxControl.ResetSounds(turnedAway);
	}

	void StartKissing()
	{
		kissing = true;
		spriteControl.StartKissing();
		sfxControl.StartKissing();
	}

	void EndKissing()
	{
		kissing = false;
		spriteControl.EndKissing();
		sfxControl.EndKissing();
	}

	void WinLevel()
	{
		menuControl.WinLevel();
		if (level < 2)
		{
			level++;
		}
		Reset();
		sfxControl.StopAllSounds();
		progressBar.ToggleUIVisibility(false);
	}

	void WinGame()
	{
		ToggleWinGameVisibility(true);
		progressBar.ToggleUIVisibility(false);
	}

	void LoseGame()
	{
		menuControl.LoseGame();
		level = 0;
		Reset();
		sfxControl.StopAllSounds();
		progressBar.ToggleUIVisibility(false);
	}

	void SetPoints()
	{
		if (currentPoints < maxPoints)
		{
			currentPoints += 0.5f;
		}
		progressBar.SetHealthValue(currentPoints / maxPoints);
	}
}
