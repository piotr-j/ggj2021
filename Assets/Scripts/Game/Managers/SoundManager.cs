using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
	public enum SFXType {
		None,
		ActionDenied,
		Deselect,
		Dig,
		DigFail,
		GotArtifact,
		Jump,
		Land,
		MoveOrder,
		Pop,
		Select
	}

	public static SoundManager Instance { get; private set; }

	[SerializeField] AudioSource m_soundPrefab;
	
	[SerializeField] AudioClip m_actionDenied;
	[SerializeField] AudioClip m_deselect;
	[SerializeField] AudioClip m_dig;
	[SerializeField] AudioClip m_digFail;
	[SerializeField] AudioClip m_gotArtifact;
	[SerializeField] List<AudioClip> m_jump;
	[SerializeField] List<AudioClip> m_land;
	[SerializeField] AudioClip m_moveOrder;
	[SerializeField] List<AudioClip> m_pop;
	[SerializeField] AudioClip m_select;
	
	
	[SerializeField, Header("Volume tuning")]
	public List<VolumeOverride> volumeOverrides;
	// {
	// 	{SFXType.ShootFireball, 0.20f},
	// 	{SFXType.ExplodeFireball, 0.10f},
	// 	{SFXType.ShootPistol, 0.1f},
	// 	{SFXType.ShootPistol2, 0.1f}
	// }
	
	[Serializable]
	public class VolumeOverride
	{
		public SFXType sfxType;
		public float volumeMul;
	}
	
	private void Awake ()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public void PlaySound ( SFXType sfxType)
	{
		AudioClip targetClip = null;

		switch (sfxType)
		{
			case SFXType.None:
				break;
			case SFXType.ActionDenied:
				targetClip = m_actionDenied;
				break;
			case SFXType.Deselect:
				targetClip = m_deselect;
				break;
			case SFXType.Dig:
				targetClip = m_dig;
				break;
			case SFXType.DigFail:
				targetClip = m_digFail;
				break;
			case SFXType.GotArtifact:
				targetClip = m_gotArtifact;
				break;
			case SFXType.Jump:
				targetClip = m_jump.Random();
				break;
			case SFXType.Land:
				targetClip = m_land.Random();				
				break;
			case SFXType.MoveOrder:
				targetClip = m_moveOrder;
				break;
			case SFXType.Pop:
				targetClip = m_pop.Random();
				break;
			case SFXType.Select:
				targetClip = m_select;
				break;
			default:
				break;
		}
		
		if(!targetClip)
			return;

		float volume = 1;
		var volumeOverride = volumeOverrides.FirstOrDefault(r => r.sfxType == sfxType);
		if (volumeOverride != null)
			volume = volumeOverride.volumeMul;
		
		AudioSource newAudioSource = Instantiate(m_soundPrefab);
		newAudioSource.clip = targetClip;
		newAudioSource.pitch = Random.Range(0.9f, 1.1f);
		newAudioSource.volume = newAudioSource.volume * volume;
		newAudioSource.Play();
		
		Destroy(newAudioSource.gameObject, newAudioSource.clip.length + 0.1f);
	}
}
