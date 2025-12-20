using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
	private enum Character
	{
		Ralsei = 0,
		Susie = 1,
		Noelle = 2,
		Asgore = 3,
		GasterHardmode = 4,
		SusieBomb = 5,
		SusieBaseball = 6
	}

	private int frames;

	private int stateText;

	private TextUT text;

	private bool toTitle;

	private bool toCredits;

	private int numDeaths;

	private bool done;

	private int skipInputs;

	private bool hardmode;

	private Character character;

	private static readonly string[] DIALOG_RALSEI = new string[2] { "snd_txtral`This is not \nyour fate...!", "snd_txtral`Please,^20\ndon't give up!" };

	private static readonly string[] DIALOG_SUSIE = new string[2] { "snd_txtsus`Come on,^20\nthat all you got!?", "snd_txtsus`Kris,^20\nget up...!" };

	private static readonly string[] DIALOG_NOELLE = new string[2] { "snd_txtnoe`Kris,^20 are you \nokay?!", "snd_txtnoe`Please,^20\nwake up...!" };

	private static readonly string[] DIALOG_ASGORE = new string[3] { "snd_txtasg`{0}", "snd_txtasg`{1}!^20\nStay determined...", "`" };

	private static readonly string[] DIALOG_GASTER_HARDMODE = new string[3] { "#v_gaster_death_hm_0`THIS CONCLUDES OUR \n\"FRISK\" EXPERIMENT.", "#v_gaster_death_hm_1`THIS SHALL NOT \nENTER ANY OTHER \nWORLDS.", "#v_gaster_death_hm_2`THANK YOU FOR \nYOUR PARTICIPATION." };

	private static readonly string[] DIALOG_SUSIE_BOMB = new string[2] { "snd_txtsus`Kris,^10 why did \nyou push that??!", "snd_txtsus`You can get up,^10 \nright???^20\nKRIS???" };

	private static readonly string[] DIALOG_SUSIE_BASEBALL = new string[3] { "snd_txtsus`Holy shit,^10 that's \na home run!", "snd_txtsus`...", "snd_txtsus`I hope you're \nhappy,^10 Kris." };

	private static readonly string[] ASGORE_PHRASES = new string[5] { "You cannot give \nup just yet...", "Our fate rests \nupon you...", "You're going to \nbe alright!", "Don't lose hope!", "It cannot end \nnow!" };

	private string[] GetDialogue()
	{
		return character switch
		{
			Character.Ralsei => DIALOG_RALSEI, 
			Character.Susie => DIALOG_SUSIE, 
			Character.Noelle => DIALOG_NOELLE, 
			Character.Asgore => Localizer.FormatArray(DIALOG_ASGORE, ASGORE_PHRASES[Random.Range(0, ASGORE_PHRASES.Length)], hardmode ? "Frisk" : "Chara"), 
			Character.GasterHardmode => DIALOG_GASTER_HARDMODE, 
			Character.SusieBomb => DIALOG_SUSIE_BOMB, 
			Character.SusieBaseball => DIALOG_SUSIE_BASEBALL, 
			_ => new string[1] { "snd_text`Bepis" }, 
		};
	}

	private void Awake()
	{
		text = base.gameObject.GetComponent<TextUT>();
		text.SetLetterSpacing(15.3825f);
		frames = 0;
		done = false;
		character = (Character)Random.Range(0, 3);
		hardmode = (int)Util.GameManager().GetFlag(108) == 1;
		toTitle = (int)Util.GameManager().GetFlag(128) == 1;
		toCredits = Util.GameManager().GetEnding() == 0;
		if ((int)Util.GameManager().GetSessionFlag(7) <= -1)
		{
			if (toCredits && hardmode)
			{
				character = Character.GasterHardmode;
			}
			else if (hardmode || Util.GameManager().GetPartyMember(0) == 6)
			{
				character = Character.Asgore;
			}
			else if ((character == Character.Susie && !Util.GameManager().SusieInParty()) || (character == Character.Noelle && !Util.GameManager().NoelleInParty()))
			{
				character = Character.Ralsei;
			}
		}
		else
		{
			character = (Character)Util.GameManager().GetSessionFlagInt(7);
		}
		if (hardmode)
		{
			GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("music/mus_gameover");
			GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/spr_gameover_ut");
		}
	}

	private void Start()
	{
		Util.FindObjectOfType<Fade>().transform.parent.position = Vector3.zero;
		GameObject obj = new GameObject("SOUL");
		obj.AddComponent<SOUL>();
		obj.GetComponent<SOUL>().CreateSOUL(SOUL.GetSOULColorByID(Util.GameManager().GetFlagInt(312)), monster: false, player: false);
		obj.transform.position = Util.GameManager().GetSpawnPos();
		numDeaths = Util.GameManager().GetNumDeaths();
	}

	private void Update()
	{
		if (!done)
		{
			if ((UTInput.GetButtonDown("Z") || UTInput.GetButtonDown("X") || UTInput.GetButtonDown("C")) && !text.Exists())
			{
				skipInputs++;
			}
			if (skipInputs >= 20 && !toTitle && !toCredits)
			{
				Util.GameManager().SpawnFromLastSave(respawn: true);
			}
			if ((frames < 182 && character != Character.GasterHardmode) || (character == Character.GasterHardmode && frames < 120))
			{
				frames++;
				if (frames == 19 && (bool)GameObject.Find("SOUL"))
				{
					GameObject.Find("SOUL").GetComponent<SOUL>().Break();
				}
				if (toTitle && character != Character.GasterHardmode)
				{
					if (frames == 120)
					{
						Util.FindObjectOfType<Fade>().FadeOut(15, Color.black);
					}
					if (frames == 135)
					{
						SceneManager.LoadScene(6, LoadSceneMode.Single);
					}
				}
				else if (character != Character.GasterHardmode)
				{
					if (frames == 90)
					{
						GetComponent<AudioSource>().Play();
					}
					if (frames <= 140 && frames >= 90)
					{
						GetComponent<Image>().color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)(frames - 90) / 50f);
					}
				}
				return;
			}
			string[] dialogue = GetDialogue();
			if (stateText >= dialogue.Length)
			{
				done = true;
				frames = 0;
			}
			else if (text.Exists())
			{
				if (!text.IsPlaying() && UTInput.GetButtonDown("Z"))
				{
					stateText++;
					text.DestroyOldText();
				}
			}
			else
			{
				string[] array = dialogue[stateText].Split('`');
				text.StartText(array[1], new Vector2(102f, -148f), array[0], 1, "DTM-Mono");
			}
		}
		else
		{
			frames++;
			if (!toTitle && !toCredits)
			{
				GetComponent<Image>().color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), (float)frames / 34f);
			}
			GetComponent<AudioSource>().volume = Mathf.Lerp(1f, 0f, (float)frames / 50f);
			if (frames == 15 && toCredits && character == Character.GasterHardmode)
			{
				SceneManager.LoadScene(131, LoadSceneMode.Single);
			}
			else if (frames == 60)
			{
				Util.GameManager().SpawnFromLastSave(respawn: true);
			}
		}
	}
}
