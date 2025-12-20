using UnityEngine;
using UnityEngine.UI;

namespace MarioBrosMayhem
{
	public class TitleAnimation : MonoBehaviour
	{
		private float timer;

		private TitleLogo logo;

		private TitleMario mario;

		private TitleMario luigi;

		private Image fadeout;

		private bool startMusic;

		private bool[] jumped = new bool[3];

		private bool[] hitLetter = new bool[3];

		private void Awake()
		{
			logo = base.transform.parent.GetComponentInChildren<TitleLogo>();
			mario = base.transform.Find("Mario").GetComponent<TitleMario>();
			luigi = base.transform.Find("Luigi").GetComponent<TitleMario>();
			fadeout = base.transform.parent.Find("Black").GetComponent<Image>();
		}

		private void Update()
		{
			if (timer > 0f && !startMusic)
			{
				Util.FindObjectOfType<MusicPlayer>().Play("mus_title");
				startMusic = true;
			}
			timer += Time.deltaTime;
			if (timer < 10.166667f)
			{
				logo.transform.localPosition = new Vector3(0f, Mathf.RoundToInt(Mathf.Lerp(144f, 45f, (timer - 3.2f) / 5.133333f)));
			}
			else if (timer < 13.5f)
			{
				int num = Mathf.RoundToInt(Mathf.Lerp(-106f, -16f, (timer - 10.7f) / 1.6f));
				mario.transform.localPosition = new Vector3(num, mario.transform.localPosition.y);
				luigi.transform.localPosition = new Vector3(-num, luigi.transform.localPosition.y);
				if (timer >= 10.7f && timer < 12.3f && !mario.IsWalking())
				{
					mario.StartWalking();
					luigi.StartWalking();
				}
				else if (timer >= 12.3f && mario.IsWalking())
				{
					mario.StopWalking();
					luigi.StopWalking();
				}
				if (timer >= 10.966666f)
				{
					if (timer < 11.633333f)
					{
						if (!jumped[0])
						{
							jumped[0] = true;
							mario.Jump();
							luigi.Jump();
						}
						if (!hitLetter[0] && timer >= 11.233334f)
						{
							hitLetter[0] = true;
							logo.BumpLetter(0);
							logo.BumpLetter(4);
						}
					}
					else if (timer < 12.3f)
					{
						if (!jumped[1])
						{
							jumped[1] = true;
							mario.Jump();
							luigi.Jump();
						}
						if (!hitLetter[1] && timer >= 11.9f)
						{
							hitLetter[1] = true;
							logo.BumpLetter(1);
							logo.BumpLetter(3);
						}
					}
					else
					{
						if (!jumped[2])
						{
							jumped[2] = true;
							mario.Jump();
							luigi.Jump();
						}
						if (!hitLetter[2] && timer >= 12.566667f)
						{
							hitLetter[2] = true;
							logo.BumpLetter(2);
						}
					}
				}
			}
			else
			{
				logo.Flash();
				if (!mario.IsWalking())
				{
					mario.StartWalking();
					luigi.StartWalking();
					mario.transform.localScale = new Vector3(-1f, 1f, 1f);
					luigi.transform.localScale = new Vector3(1f, 1f, 1f);
				}
				int num2 = Mathf.RoundToInt(Mathf.Lerp(-16f, -138f, (timer - 13.5f) / 1.9f));
				mario.transform.localPosition = new Vector3(num2, mario.transform.localPosition.y);
				luigi.transform.localPosition = new Vector3(-num2, luigi.transform.localPosition.y);
				fadeout.color = new Color(0f, 0f, 0f, Mathf.Lerp(1f, 0f, (timer - 15.4f) / (4f / 15f)));
				if (timer >= 15.666667f)
				{
					Skip();
				}
			}
			if (UTInput.GetButtonDown("Z") || UTInput.GetButtonDown("C"))
			{
				Util.GameManager().PlayGlobalSFX("mariobros/sounds/snd_menu_select");
				Skip();
			}
		}

		public void Skip()
		{
			logo.Skip(flash: true);
			logo.transform.localPosition = new Vector3(0f, 45f);
			mario.transform.localPosition = new Vector3(-138f, 0f);
			luigi.transform.localPosition = new Vector3(-138f, 0f);
			mario.enabled = false;
			luigi.enabled = false;
			fadeout.color = new Color(0f, 0f, 0f, 1f);
			fadeout.enabled = false;
			base.enabled = false;
		}
	}
}
