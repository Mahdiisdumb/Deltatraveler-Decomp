using System.Collections;
using UnityEngine;

namespace MarioBrosMayhem
{
	public class MusicPlayer : MonoBehaviour
	{
		private AudioSource musicSource;

		[SerializeField]
		private string musicTrack;

		[SerializeField]
		private bool autoplay;

		[SerializeField]
		private bool autoPlayLoop = true;

		private string currentTrack;

		private void Awake()
		{
			musicSource = GetComponent<AudioSource>();
			if (autoplay)
			{
				Play(musicTrack, autoPlayLoop);
			}
		}

		private IEnumerator PlayTrack(string music, float volume, float pitch)
		{
			AudioClip musicClip = Resources.Load<AudioClip>(music);
			AudioClip audioClip = Resources.Load<AudioClip>(music + "_intro");
			musicSource.loop = false;
			musicSource.volume = volume;
			musicSource.pitch = pitch;
			if ((bool)audioClip)
			{
				musicSource.clip = audioClip;
				musicSource.Play();
				while (musicSource.isPlaying)
				{
					yield return null;
				}
			}
			musicSource.loop = true;
			musicSource.clip = musicClip;
			musicSource.Play();
			currentTrack = music;
		}

		public void PlayWithoutIntro(string music, bool loop = true, float volume = 1f, float pitch = 1f)
		{
			musicSource.loop = loop;
			musicSource.volume = volume;
			musicSource.pitch = pitch;
			musicSource.clip = Resources.Load<AudioClip>("mariobros/music/" + music);
			musicSource.Play();
			currentTrack = music;
		}

		public void Play(string music, bool loop = true, float volume = 1f, float pitch = 1f)
		{
			if (loop)
			{
				StartCoroutine(PlayTrack("mariobros/music/" + music, volume, pitch));
				return;
			}
			musicSource.loop = false;
			musicSource.volume = volume;
			musicSource.pitch = pitch;
			musicSource.clip = Resources.Load<AudioClip>("mariobros/music/" + music);
			musicSource.Play();
			currentTrack = music;
		}

		public bool IsPlaying()
		{
			return musicSource.isPlaying;
		}

		public string GetCurrentTrack()
		{
			return currentTrack;
		}

		public void Stop()
		{
			musicSource.Stop();
		}

		public float GetTime()
		{
			return musicSource.time / musicSource.clip.length;
		}
	}
}
