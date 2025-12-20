using UnityEngine;
using UnityEngine.UI;

namespace MarioBrosMayhem
{
	public class BonusTimer : MonoBehaviour
	{
		private Animator anim;

		private Image sign;

		private SpriteText timerText;

		private AudioSource timerSound;

		private bool activated;

		private bool wasPaused;

		private void Awake()
		{
			anim = GetComponent<Animator>();
			sign = GetComponent<Image>();
			timerText = GetComponentInChildren<SpriteText>();
			timerSound = GetComponent<AudioSource>();
		}

		private void Update()
		{
			if (activated)
			{
				if (Time.timeScale == 0f)
				{
					wasPaused = true;
					timerSound.Stop();
				}
				else if (wasPaused)
				{
					wasPaused = false;
					timerSound.Play();
				}
			}
		}

		public void Activate()
		{
			sign.enabled = true;
			timerText.enabled = true;
			anim.Play("bonus_timer_idle");
			activated = true;
		}

		public void Deactivate()
		{
			timerText.enabled = false;
			sign.enabled = false;
			StopTimer();
			activated = false;
		}

		public bool IsActivated()
		{
			return activated;
		}

		public void StartTimer()
		{
			timerSound.Play();
			anim.Play("bonus_timer_flash");
		}

		public void StopTimer()
		{
			timerSound.Stop();
			anim.StopPlayback();
		}

		public void SetTime(float time)
		{
			timerText.Text = time.ToString("00.0").Replace('.', ':');
		}
	}
}
