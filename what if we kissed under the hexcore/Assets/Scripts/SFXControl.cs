using System;
using UnityEngine;

public class SFXControl : MonoBehaviour
{
	public static SFXControl Instance { get; private set; }

	[SerializeField]
	private AudioSource heimerginderAudio;
	[SerializeField]
	private AudioSource kissingAudio;
	[SerializeField]
	private AudioSource hexcoreAudio;

	private void Awake()
	{
		Instance = this;
	}

	public void Continue()
	{
		heimerginderAudio.Play();
	}

	public void TalkingTimerEnded()
	{
		heimerginderAudio.Stop();
		hexcoreAudio.Play();
	}

	public void HandsDownTimerStarted()
	{
		heimerginderAudio.Play();
		hexcoreAudio.Stop();
	}

	public void ResetSounds(bool turnedAway)
	{
		if (turnedAway)
			hexcoreAudio.Play();
		else
		{
			hexcoreAudio.Stop();
			heimerginderAudio.Play();
		}
	}

	public void StartKissing()
	{
		kissingAudio.Play();
	}

	public void EndKissing()
	{
		kissingAudio.Stop();
	}

	public void StopAllSounds()
	{
		heimerginderAudio.Stop();
		kissingAudio.Stop();
		hexcoreAudio.Stop();
	}
}
