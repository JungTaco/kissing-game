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
	[HideInInspector]
	public bool IsBeingClicked = false;
	[HideInInspector]
	public bool MenuButtonWasPressed = false;

	void FixedUpdate()
	{
		if (Input.GetMouseButton(0))
		{
			IsBeingClicked = true;
		}
		else
		{
			IsBeingClicked = false;
		}
	}

	private void Update()
	{ 
		if (Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonUp(1))
		{
			MenuButtonWasPressed = true;
		}
		else
		{
			MenuButtonWasPressed = false;
		}
	}
}
