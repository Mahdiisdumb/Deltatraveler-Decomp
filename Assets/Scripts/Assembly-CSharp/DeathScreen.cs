using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : TranslatableBehaviour
{
	private int frames;

	private int stateText;

	private TextUT text;

	private bool toTitle;

	private bool toCredits;

	private int numDeaths;

	private bool done;

	private int skipInputs;

	private int character;

	private string asgorePhrase = "Bepis";

	private bool hardmode;

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("ralsei", new string[2] { "This is not \nyour fate...!", "Please,^20\ndon't give up!" });
		dictionary.Add("susie", new string[2] { "Come on,^20\nthat all you got!?", "Kris,^20\nget up...!" });
		dictionary.Add("noelle", new string[2] { "Kris,^20 are you \nokay?!", "Please,^20\nwake up...!" });
		dictionary.Add("asgore", new string[3] { "{0}", "Frisk!^20\nStay determined...", "" });
		dictionary.Add("gaster_hardmode", new string[3] { "THIS CONCLUDES OUR \n\"FRISK\" EXPERIMENT.", "THIS SHALL NOT \nENTER ANY OTHER \nWORLDS.", "THANK YOU FOR \nYOUR PARTICIPATION." });
		dictionary.Add("susie_bomb", new string[2] { "Kris,^10 why did \nyou push that??!", "You can get up,^10 \nright???^20\nKRIS???" });
		dictionary.Add("susie_baseball", new string[3] { "Holy shit,^10 that's \na home run!", "...", "I hope you're \nhappy,^10 Kris." });
		dictionary.Add("asgore_phrases", new string[5] { "You cannot give \nup just yet...", "Our fate rests \nupon you...", "You're going to \nbe alright!", "Don't lose hope!", "It cannot end \nnow!" });
		dictionary.Add("flowey_susie", new string[3] { "Kris,^20 this is just \na bad dream...", "And you're NEVER \nwaking up!", "Why don't you SAVE \nnext time,^05 idiot?!" });
		dictionary.Add("flowey_asgore", new string[3] { "This is all just \na bad dream...", "And you're NEVER \nwaking up!", "Why don't you SAVE \nnext time,^05 idiot?!" });
		return dictionary;
	}

	private void Awake()
	{
		SetStrings(GetDefaultStrings(), GetType());
		text = base.gameObject.GetComponent<TextUT>();
		text.SetLetterSpacing(15.3825f);
		frames = 0;
		done = false;
		character = Random.Range(0, 3);
		hardmode = (int)Object.FindObjectOfType<GameManager>().GetFlag(108) == 1;
		toTitle = (int)Util.GameManager().GetFlag(128) == 1;
		toCredits = Util.GameManager().GetEnding() == 0;
		if ((int)Util.GameManager().GetSessionFlag(7) <= -1)
		{
			if (toCredits && hardmode)
			{
				character = 4;
			}
			else if (hardmode || (int)Util.GameManager().GetFlag(107) == 1)
			{
				character = 3;
				string[] stringArray = GetStringArray("asgore_phrases");
				asgorePhrase = stringArray[Random.Range(0, stringArray.Length)];
			}
			else if ((character == 1 && !Object.FindObjectOfType<GameManager>().SusieInParty()) || (character == 2 && !Object.FindObjectOfType<GameManager>().NoelleInParty()))
			{
				character = 0;
			}
		}
		else
		{
			character = (int)Util.GameManager().GetSessionFlag(7);
		}
		if (hardmode)
		{
			GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("music/mus_gameover");
			GetComponent<Image>().sprite = Util.PackManager().GetTranslatedSprite(Resources.Load<Sprite>("ui/spr_gameover_ut"), "ui/spr_gameover_ut");
		}
		else
		{
			GetComponent<Image>().sprite = Util.PackManager().GetTranslatedSprite(GetComponent<Image>().sprite, "ui/spr_gameover");
		}
	}

	private void Start()
	{
		Object.FindObjectOfType<Fade>().transform.parent.position = Vector3.zero;
		GameObject obj = new GameObject("SOUL");
		obj.AddComponent<SOUL>();
		obj.GetComponent<SOUL>().CreateSOUL(SOUL.GetSOULColorByID(Util.GameManager().GetFlagInt(312)), false, false);
		obj.transform.position = Object.FindObjectOfType<GameManager>().GetSpawnPos();
		numDeaths = Object.FindObjectOfType<GameManager>().GetNumDeaths();
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
				Object.FindObjectOfType<GameManager>().SpawnFromLastSave(true);
			}
			if ((frames < 182 && character != 4) || (character == 4 && frames < 120))
			{
				frames++;
				if (frames == 19 && (bool)GameObject.Find("SOUL"))
				{
					GameObject.Find("SOUL").GetComponent<SOUL>().Break();
				}
				if (toTitle && character != 4)
				{
					if (frames == 120)
					{
						Object.FindObjectOfType<Fade>().FadeOut(15, Color.black);
					}
					if (frames == 135)
					{
						SceneManager.LoadScene(6, LoadSceneMode.Single);
					}
				}
				else if (character != 4)
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
			string[] array = new string[7] { "ralsei", "susie", "noelle", "asgore", "gaster_hardmode", "susie_bomb", "susie_baseball" };
			string[] array2 = new string[7] { "snd_txtral", "snd_txtsus", "snd_txtnoe", "snd_txtasg", "snd_txtgas", "snd_txtsus", "snd_txtsus" };
			string[] stringArrayFormatted = GetStringArrayFormatted(array[character], asgorePhrase);
			if (!hardmode && character == 3)
			{
				stringArrayFormatted[1] = stringArrayFormatted[1].Replace("Frisk", "Chara");
			}
			if (stateText >= stringArrayFormatted.Length)
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
				text.StartText(stringArrayFormatted[stateText], new Vector2(102f, -148f), array2[character], 1, "DTM-Mono");
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
			if (frames == 15 && toCredits && character == 4)
			{
				SceneManager.LoadScene(131, LoadSceneMode.Single);
			}
			else if (frames == 60)
			{
				Object.FindObjectOfType<GameManager>().SpawnFromLastSave(true);
			}
		}
	}
}
