using System;
using System.Reflection;
using UnityEngine;

public class SpriteControl : MonoBehaviour
{
	public static SpriteControl Instance { get; private set; }

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
	
	private void Awake()
	{
		Instance = this;
	}

	public void TalkingTimerEnded()
	{
		heimerdingerFront.enabled = false;
		heimerdingerAttention.enabled = true;
		hexcoreCalm.enabled = false;
		hexcoreAggitated.enabled = true;
	}

	public void AttentionTimerEnded()
	{
		heimerdingerAttention.enabled = false;
		heimerdingerBack.enabled = true;
	}

	public void TurnedTimerEnded()
	{
		heimerdingerBack.enabled = false;
		heimerdingerHandsDown.enabled = true;
	}

	public void HandsDownTimerStarted()
	{
		heimerdingerHandsDown.enabled = false;
		heimerdingerFront.enabled = true;
		hexcoreAggitated.enabled = false;
		hexcoreCalm.enabled = true;
	}

	public void ResetSprites()
	{
		heimerdingerBack.enabled = false;
		heimerdingerHandsDown.enabled = false;
		heimerdingerFront.enabled = true;
		hexcoreAggitated.enabled = false;
		hexcoreCalm.enabled = true;
	}

	public void StartKissing()
	{
		jayvikIdleRenderer.enabled = false;
		jayvikKissRenderer.enabled = true;
	}

	public void EndKissing()
	{
		jayvikIdleRenderer.enabled = true;
		jayvikKissRenderer.enabled = false;
	}
}
