using UnityEngine;
using UnityEngine.UI;

namespace MarioBrosMayhem
{
	public class GameOverContinue : MonoBehaviour
	{
		private AudioSource aud;

		private Image cursor;

		private Material[] selectionColors;

		private bool activated;

		private int state;

		private int index;

		private bool axisDown;

		private float timer;

		private bool madeSelection;

		private void Awake()
		{
			aud = GetComponent<AudioSource>();
			selectionColors = new Material[2]
			{
				Resources.Load<Material>("mariobros/materials/hud-score-tan"),
				Resources.Load<Material>("mariobros/materials/hud-score-tan-inverse")
			};
			cursor = base.transform.Find("Cursor").GetComponent<Image>();
		}

		private void Update()
		{
			if (!activated)
			{
				return;
			}
			timer += Time.deltaTime;
			if (state == 0 && timer >= 2f)
			{
				PlaySFX("mariobros/sounds/snd_menu_appear");
				SpriteText[] componentsInChildren = GetComponentsInChildren<SpriteText>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = true;
				}
				Image[] componentsInChildren2 = GetComponentsInChildren<Image>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].enabled = true;
				}
				state = 1;
				timer = 0f;
			}
			if (state != 1)
			{
				return;
			}
			if (UTInput.GetAxis("Vertical") != 0f && !axisDown)
			{
				base.transform.Find(index.ToString()).GetComponent<SpriteText>().material = selectionColors[0];
				index++;
				if (index > 1)
				{
					index = 0;
				}
				base.transform.Find(index.ToString()).GetComponent<SpriteText>().material = selectionColors[1];
				PlaySFX("mariobros/sounds/snd_menu_cursor");
				cursor.transform.localPosition = new Vector3(-24f, base.transform.Find(index.ToString()).localPosition.y);
				axisDown = true;
			}
			else if (UTInput.GetAxis("Vertical") == 0f && axisDown)
			{
				axisDown = false;
			}
			cursor.enabled = (int)(timer * 60f) / 15 % 2 == 0;
			if (!UTInput.GetButtonDown("Z") && !UTInput.GetButtonDown("C"))
			{
				return;
			}
			madeSelection = true;
			if (index == 0)
			{
				PlaySFX("mariobros/sounds/snd_coin");
				SpriteText[] componentsInChildren = GetComponentsInChildren<SpriteText>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				Image[] componentsInChildren2 = GetComponentsInChildren<Image>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].enabled = false;
				}
				base.transform.Find("GameOverText").GetComponent<SpriteText>().enabled = true;
				Object.FindObjectOfType<MarioBrosNetworkManager>().RevivePlayer(0);
				state = 2;
			}
			else if (index == 1)
			{
				Deactivate();
				Object.FindObjectOfType<MarioBrosManager>().TrueGameOver();
			}
		}

		public void Activate(int credits)
		{
			MonoBehaviour.print("activatedx");
			if (Object.FindObjectOfType<PauseMenu>().Paused())
			{
				Object.FindObjectOfType<PauseMenu>().Unpause();
			}
			string text = credits.ToString().PadLeft(2, ' ');
			base.transform.Find("Credits").GetComponent<SpriteText>().Text = "Credit" + text;
			activated = true;
			timer = 0f;
			state = 0;
			madeSelection = false;
			PlaySFX("mariobros/music/mus_game_over_continue");
			base.transform.Find("GameOverText").GetComponent<SpriteText>().enabled = true;
		}

		public void Deactivate()
		{
			activated = false;
			SpriteText[] componentsInChildren = GetComponentsInChildren<SpriteText>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			Image[] componentsInChildren2 = GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
		}

		public void PlaySFX(string sfx)
		{
			aud.clip = Resources.Load<AudioClip>(sfx);
			aud.Play();
		}

		public bool IsActive()
		{
			return activated;
		}
	}
}
