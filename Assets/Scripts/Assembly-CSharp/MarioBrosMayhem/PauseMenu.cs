using UnityEngine;
using UnityEngine.UI;

namespace MarioBrosMayhem
{
	public class PauseMenu : MonoBehaviour
	{
		private Transform cursor;

		private AudioSource pauseSfx;

		private AudioSource cursorSfx;

		private bool paused;

		private bool holdFrame;

		private int index;

		private bool axisDown;

		private bool no;

		private void Awake()
		{
			pauseSfx = GetComponents<AudioSource>()[0];
			cursorSfx = GetComponents<AudioSource>()[1];
			cursor = base.transform.Find("Cursor");
		}

		private void Update()
		{
			if (no || !paused)
			{
				return;
			}
			if (holdFrame)
			{
				holdFrame = false;
				return;
			}
			if (UTInput.GetAxis("Vertical") != 0f && !axisDown)
			{
				index = (index + 1) % 2;
				axisDown = true;
				cursorSfx.Play();
				PositionCursor();
			}
			else if (UTInput.GetAxis("Vertical") == 0f && axisDown)
			{
				axisDown = false;
			}
			if (!UTInput.GetButtonDown("Z") && !UTInput.GetButtonDown("C"))
			{
				return;
			}
			if (index == 0)
			{
				pauseSfx.Play();
				Unpause();
			}
			else if (index == 1)
			{
				pauseSfx.Play();
				Util.FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().volume = 1f;
				PlayerHUD[] array = Util.FindObjectsOfType<PlayerHUD>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetLivesVisible(enabled: false);
				}
				Image[] componentsInChildren = GetComponentsInChildren<Image>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				SpriteText[] componentsInChildren2 = GetComponentsInChildren<SpriteText>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].enabled = false;
				}
				Util.FindObjectOfType<MarioBrosManager>().QuitGame();
				no = true;
			}
		}

		public void Pause()
		{
			Time.timeScale = 0f;
			Util.FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().volume = 0.5f;
			paused = true;
			holdFrame = true;
			pauseSfx.Play();
			PlayerHUD[] array = Util.FindObjectsOfType<PlayerHUD>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetLivesVisible(enabled: true);
			}
			Image[] componentsInChildren = GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = true;
			}
			SpriteText[] componentsInChildren2 = GetComponentsInChildren<SpriteText>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = true;
			}
			string text = "";
			if ((bool)Util.FindObjectOfType<MarioBrosManager>())
			{
				text = (Util.FindObjectOfType<MarioBrosManager>().GetPhaseNumber() + 1).ToString().PadLeft(2, ' ');
				base.transform.Find("PhaseTextP").GetComponent<SpriteText>().Text = "Phase " + text;
			}
			base.transform.Find("1").GetComponent<SpriteText>().Text = "Save/Quit";
			index = 0;
			PositionCursor();
		}

		private void PositionCursor()
		{
			cursor.localPosition = new Vector3(cursor.localPosition.x, base.transform.Find(index.ToString()).transform.localPosition.y);
		}

		public void Unpause()
		{
			Time.timeScale = 1f;
			Util.FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().volume = 1f;
			paused = false;
			PlayerHUD[] array = Util.FindObjectsOfType<PlayerHUD>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetLivesVisible(enabled: false);
			}
			Image[] componentsInChildren = GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			SpriteText[] componentsInChildren2 = GetComponentsInChildren<SpriteText>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
		}

		public bool Paused()
		{
			return paused;
		}
	}
}
