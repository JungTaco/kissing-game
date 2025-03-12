using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
	public static MenuControl Instance { get; private set; }

	[HideInInspector]
	public bool IsVisible = false;

	[SerializeField]
	private GameObject menu;
	[SerializeField]
	private GameObject win;
	[SerializeField]
	private GameObject lose;
	[SerializeField]
	private GameObject win_game;
	[SerializeField]
	private GameObject settings;
	[SerializeField]
	private GameObject instructions;

	private void Awake()
	{
		Instance = this;
	}

	public bool GetIsWinVisible()
	{
		return win.activeInHierarchy;
	}

	public bool GetIsLoseVisible()
	{
		return lose.activeInHierarchy;
	}

	public bool GetIsMenuVisible()
	{
		return menu.activeInHierarchy;
	}

	public bool GetIsWinGameVisible()
	{
		return win_game.activeInHierarchy;
	}

	public void MainMenu()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void ToggleMenuVisibility()
	{
		menu.SetActive(!IsVisible);
		IsVisible = !IsVisible;
	}

	public void Continue()
	{
		win.SetActive(false);
	}

	public void Restart()
	{
		lose.SetActive(false);
	}

	public void ToggleSettingsVisibility(bool toggle)
	{
		settings.SetActive(toggle);
	}

	public void ToggleInstructionsVisibility(bool toggle)
	{
		instructions.SetActive(toggle);
	}

	public void ToggleWinGameVisibility(bool toggle)
	{
		win_game.SetActive(toggle);
	}

	public void WinLevel()
	{
		win.SetActive(true);
	}

	public void WinGame()
	{
		ToggleWinGameVisibility(true);
	}

	public void LoseGame()
	{
		lose.SetActive(true);
	}
}
