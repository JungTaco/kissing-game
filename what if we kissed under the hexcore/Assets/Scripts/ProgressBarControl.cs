using UnityEngine;
using UnityEngine.UIElements;

public class ProgressBarControl : MonoBehaviour
{
	public static ProgressBarControl Instance { get; private set; }

	private VisualElement bar;
	private VisualElement barBackground;

	private void Awake()
	{
		Instance = this;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		UIDocument uiDocument = GetComponent<UIDocument>();
		bar = uiDocument.rootVisualElement.Q<VisualElement>("Bar");
		barBackground = uiDocument.rootVisualElement.Q<VisualElement>("BarBackground");
		SetHealthValue(0f);
	}

	public void SetHealthValue(float percentage)
	{
		bar.style.width = Length.Percent(100 * percentage);
	}

	public void ToggleUIVisibility(bool toggle)
	{
		barBackground.visible = toggle;
		bar.visible = toggle;
	}
}
