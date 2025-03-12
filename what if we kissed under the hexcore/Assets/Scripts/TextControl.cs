using UnityEngine;
using TMPro;

public class TextControl : MonoBehaviour
{
	public static TextControl Instance { get; private set; }

	[SerializeField]
	private TextMeshProUGUI level_text;
	private LevelText level_text_script;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		level_text_script = level_text.GetComponent<LevelText>();
		level_text_script.Show();
	}

	public void Continue(int level)
	{
		level_text_script.SetText("Level " + (level + 1));
		level_text_script.Show();
	}

	public void Restart(int level)
	{
		level_text_script.SetText("Level " + (level + 1));
		level_text_script.Show();
	}
}
