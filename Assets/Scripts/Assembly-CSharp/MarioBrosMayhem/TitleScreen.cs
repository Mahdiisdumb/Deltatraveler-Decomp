using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MarioBrosMayhem
{
	public class TitleScreen : MonoBehaviour
	{
		public enum State
		{
			TitleAnimation = 0,
			StartScreen = 1,
			MainMenu = 2,
			Options = 3,
			Fadeout = 4
		}

		private readonly Vector3 OFFSCREEN_POS = new Vector3(320f, 0f);

		private State state;

		private float timer;

		private TitleAnimation titleAnimation;

		private int index;

		private int menuLimit;

		private bool selecting;

		private bool axisDown = true;

		private bool useHorizontalAxis;

		private bool axisDownH;

		private Transform mainmenu;

		private Transform options;

		private GameObject logo;

		private Image blackBG;

		private Image copyright;

		private Image fade;

		private AudioSource cursorSfx;

		private AudioSource soundTest;

		private float volumeHoldRate;

		private int volume;

		private string playerName = "";

		private string ipAddress = "127.0.0.1";

		private bool exitToDT;

		private int skin;

		private bool altPalette;

		private int difficulty;

		private void Awake()
		{
			titleAnimation = GetComponentInChildren<TitleAnimation>();
			cursorSfx = GetComponents<AudioSource>()[0];
			soundTest = GetComponents<AudioSource>()[1];
			mainmenu = base.transform.Find("MainMenu");
			options = base.transform.Find("Options");
			options.Find("PlayerBig").GetComponent<TitleMario>().StartWalking();
			options.Find("PlayerSmall").GetComponent<TitleMario>().StartWalking();
			logo = base.transform.Find("Logo").gameObject;
			blackBG = base.transform.Find("Black").GetComponent<Image>();
			copyright = base.transform.Find("Copyright").GetComponent<Image>();
			fade = base.transform.Find("Fade").GetComponent<Image>();
		}

		private void Start()
		{
			Util.GameManager().SetFramerate(60);
			UTInput.SetPriority(b: false);
			skin = (int)Util.GameManager().GetSessionFlag(11);
			altPalette = (int)Util.GameManager().GetSessionFlag(12) == 1;
			difficulty = (int)Util.GameManager().GetSessionFlag(13);
			Util.FindObjectOfType<Fade>().FadeIn(0);
		}

		private void Update()
		{
			if (selecting)
			{
				if (!axisDown && UTInput.GetAxis(useHorizontalAxis ? "Horizontal" : "Vertical") != 0f)
				{
					cursorSfx.Play();
					if (useHorizontalAxis)
					{
						index += (int)UTInput.GetAxis("Horizontal");
					}
					else
					{
						index -= (int)UTInput.GetAxis("Vertical");
					}
					if (index >= menuLimit)
					{
						index = 0;
					}
					else if (index < 0)
					{
						index = menuLimit - 1;
					}
					axisDown = true;
				}
				else if (axisDown && UTInput.GetAxis(useHorizontalAxis ? "Horizontal" : "Vertical") == 0f)
				{
					axisDown = false;
				}
			}
			if (state == State.TitleAnimation)
			{
				if (!titleAnimation.enabled)
				{
					LoadMainMenu();
				}
			}
			else if (state == State.StartScreen)
			{
				timer += Time.deltaTime;
				if (timer >= 8f / 15f)
				{
					timer -= 8f / 15f;
				}
				base.transform.Find("PressStart").GetComponent<Image>().enabled = timer <= 4f / 15f;
				if (UTInput.GetButtonDown("Z") || UTInput.GetButtonDown("C"))
				{
					base.transform.Find("PressStart").GetComponent<Image>().enabled = false;
					Util.GameManager().PlayGlobalSFX("mariobros/sounds/snd_menu_select");
					StartFadeout();
				}
				else if (UTInput.GetButtonDown("X"))
				{
					Util.GameManager().PlayGlobalSFX("mariobros/sounds/snd_player_kick_0");
					LoadMainMenu();
				}
			}
			else if (state == State.MainMenu)
			{
				PositionCursor();
				if (UTInput.GetButtonDown("Z") || UTInput.GetButtonDown("X") || UTInput.GetButtonDown("C"))
				{
					if (index == 2 || UTInput.GetButtonDown("X"))
					{
						selecting = false;
						Util.FindObjectOfType<MusicPlayer>().Stop();
						exitToDT = true;
						fade.rectTransform.sizeDelta = new Vector2(320f, 240f);
						StartFadeout();
					}
					else if (index == 0)
					{
						LoadStartScreen();
						Util.GameManager().PlayGlobalSFX("mariobros/sounds/snd_menu_select");
					}
					else if (index == 1)
					{
						LoadOptions();
						Util.GameManager().PlayGlobalSFX("mariobros/sounds/snd_menu_appear");
					}
				}
			}
			else if (state == State.Options)
			{
				PositionCursor();
				if (index == 1)
				{
					if (!axisDownH && UTInput.GetAxis("Horizontal") != 0f)
					{
						cursorSfx.Play();
						skin += (int)UTInput.GetAxis("Horizontal");
						if (skin >= GlobalVariables.SKIN_COUNT)
						{
							skin = 0;
						}
						else if (skin < 0)
						{
							skin = GlobalVariables.SKIN_COUNT - 1;
						}
						altPalette = false;
						UpdateOptionsText();
						axisDownH = true;
					}
					else if (axisDownH && UTInput.GetAxis("Horizontal") == 0f)
					{
						axisDownH = false;
					}
				}
				if (!UTInput.GetButtonDown("Z") && !UTInput.GetButtonDown("X") && !UTInput.GetButtonDown("C"))
				{
					return;
				}
				if (UTInput.GetButtonDown("X") || index == menuLimit - 1)
				{
					Util.GameManager().PlayGlobalSFX("mariobros/sounds/snd_player_kick_0");
					Util.GameManager().SetSessionFlag(11, skin);
					Util.GameManager().SetSessionFlag(12, altPalette ? 1 : 0);
					Util.GameManager().SetSessionFlag(13, difficulty);
					LoadMainMenu();
				}
				else if (index == 0)
				{
					Util.GameManager().PlayGlobalSFX("mariobros/sounds/snd_player_kick_0");
					difficulty++;
					if (difficulty > 2)
					{
						difficulty = 0;
					}
					UpdateOptionsText();
				}
				else if (index == 1 && UTInput.GetButtonDown("C"))
				{
					altPalette = !altPalette;
					UpdateOptionsText();
				}
			}
			else
			{
				if (state != State.Fadeout)
				{
					return;
				}
				timer += Time.deltaTime;
				fade.color = new Color(0f, 0f, 0f, timer * 4f);
				if (timer >= 1f)
				{
					if (exitToDT)
					{
						Util.GameManager().SetFramerate(30);
						UTInput.SetPriority(b: true);
					}
					SceneManager.LoadScene(exitToDT ? 132 : 104);
				}
			}
		}

		public void LoadStartScreen()
		{
			mainmenu.localPosition = OFFSCREEN_POS;
			selecting = false;
			state = State.StartScreen;
			timer = 0f;
			logo.transform.Find("ClassicSubtitle").GetComponent<Image>().enabled = true;
		}

		public void LoadMainMenu()
		{
			logo.transform.Find("ClassicSubtitle").GetComponent<Image>().enabled = false;
			mainmenu.localPosition = Vector3.zero;
			logo.SetActive(value: true);
			logo.GetComponent<TitleLogo>().Flash();
			base.transform.Find("PressStart").GetComponent<Image>().enabled = false;
			options.localPosition = OFFSCREEN_POS;
			blackBG.enabled = false;
			copyright.enabled = true;
			if (state == State.Options)
			{
				index = 1;
			}
			else
			{
				index = 0;
			}
			state = State.MainMenu;
			selecting = true;
			menuLimit = 3;
			PositionCursor();
		}

		public void LoadOptions()
		{
			options.localPosition = Vector3.zero;
			options.Find("PlayerBig").GetComponent<TitleMario>().UpdateSkin(0);
			options.Find("PlayerSmall").GetComponent<TitleMario>().UpdateSkin(0);
			UpdateOptionsText();
			mainmenu.localPosition = OFFSCREEN_POS;
			logo.SetActive(value: false);
			blackBG.enabled = true;
			copyright.enabled = false;
			state = State.Options;
			selecting = true;
			menuLimit = 3;
			index = 0;
			PositionCursor();
		}

		public void StartFadeout()
		{
			selecting = false;
			timer = 0f;
			state = State.Fadeout;
		}

		private void PositionCursor()
		{
			if (state == State.MainMenu)
			{
				Transform obj = mainmenu.Find("Cursor");
				obj.localPosition = new Vector3(obj.localPosition.x, mainmenu.GetChild(index).localPosition.y);
			}
			else if (state == State.Options)
			{
				Transform obj2 = options.Find("Cursor");
				obj2.localPosition = options.GetChild(index).localPosition - new Vector3(24f, 0f);
				obj2.GetComponent<Image>().enabled = index != 1;
				options.Find("PaletteCursorRight").GetComponent<Image>().enabled = index == 1;
				options.Find("PaletteCursorLeft").GetComponent<Image>().enabled = index == 1;
			}
		}

		private void UpdateOptionsText()
		{
			options.Find("Difficulty").GetComponent<Text>().text = (new string[3] { "Normal", "Easy", "Hard" })[difficulty];
			options.Find("Character").GetComponent<Text>().text = GlobalVariables.SKIN_NAMES[skin];
			options.Find("PlayerBig").GetComponent<TitleMario>().UpdateSkin(skin);
			options.Find("PlayerSmall").GetComponent<TitleMario>().UpdateSkin(skin);
			options.Find("PlayerBig").GetComponent<TitleMario>().UpdatePalette(skin, altPalette ? 1 : 0);
			options.Find("PlayerSmall").GetComponent<TitleMario>().UpdatePalette(skin, altPalette ? 1 : 0);
		}
	}
}
