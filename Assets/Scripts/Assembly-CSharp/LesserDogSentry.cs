using UnityEngine;

public class LesserDogSentry : InteractSelectionBase
{
	private SpriteRenderer ldog;

	private bool niceDog;

	private bool hideDog;

	private bool startFight;

	private bool petting;

	private int pettingFrames;

	private int state;

	[SerializeField]
	private Sprite[] krisReachFrames;

	[SerializeField]
	private Sprite[] krisPetFrames;

	[SerializeField]
	private Sprite[] dogSprites;

	private void Awake()
	{
		ldog = base.transform.Find("Dog").GetComponent<SpriteRenderer>();
		if ((int)Util.GameManager().GetFlag(253) == 1)
		{
			KilledDog();
			Object.Destroy(GameObject.Find("KrisStatue"));
			Object.Destroy(GameObject.Find("SusieStatue"));
			Object.Destroy(GameObject.Find("NoelleStatue"));
			Object.Destroy(GameObject.Find("RalseiStatue"));
		}
		else if ((int)Util.GameManager().GetFlag(253) == 2)
		{
			SparedDog();
			GameObject.Find("DogStatue").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* (It's a dog sculpture.)", "* (Looks like it found the\n  will to complete it.)" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
			GameObject.Find("DogStatue").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/snow_objects/spr_snowstatue_dog_complete");
			if (WeirdChecker.HasKilled(Util.GameManager()) || !WeirdChecker.HasSparedEveryEncounterInUnderfell(Util.GameManager()))
			{
				Object.Destroy(GameObject.Find("RalseiStatue"));
			}
			if ((int)Util.GameManager().GetFlag(258) == 1)
			{
				Object.Destroy(GameObject.Find("KrisStatue"));
				Object.Destroy(GameObject.Find("SusieStatue"));
				Object.Destroy(GameObject.Find("NoelleStatue"));
			}
			else if ((int)Util.GameManager().GetFlag(244) == 1)
			{
				Object.Destroy(GameObject.Find("NoelleStatue"));
				GameObject.Find("SusieStatue").GetComponent<InteractTextBox>().ModifyContents(new string[4] { "* You guys...^05 spared them?", "* Yeah.", "* ...", "* (Humans are very\n  weird...)" }, new string[4] { "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtnoe" }, new int[1], new string[4] { "no_confused", "su_smile", "no_curious", "no_thinking" });
			}
		}
		else
		{
			Object.Destroy(GameObject.Find("KrisStatue"));
			Object.Destroy(GameObject.Find("SusieStatue"));
			Object.Destroy(GameObject.Find("NoelleStatue"));
			Object.Destroy(GameObject.Find("RalseiStatue"));
		}
	}

	public void KilledDog()
	{
		Object.Destroy(ldog.gameObject);
		GameObject.Find("DogStatue").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* No one will ever complete\n  this sculpture." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
	}

	public void SparedDog()
	{
		if ((int)Util.GameManager().GetFlag(258) == 1)
		{
			Object.Destroy(ldog.gameObject);
			return;
		}
		niceDog = true;
		ldog.sprite = dogSprites[0];
		ldog.transform.localPosition = new Vector3(0f, -0.878f);
	}

	protected override void Update()
	{
		base.Update();
		if (!txt && startFight)
		{
			startFight = false;
			Object.FindObjectOfType<OverworldPlayer>().InitiateBattle(68);
		}
		if (!niceDog && (bool)ldog)
		{
			if ((bool)Object.FindObjectOfType<OverworldPlayer>() && Vector3.Distance(Object.FindObjectOfType<OverworldPlayer>().transform.position, base.transform.position) < 3.5f)
			{
				hideDog = true;
			}
			if (hideDog && (bool)ldog && ldog.transform.localPosition.y > -0.166f)
			{
				ldog.transform.localPosition -= new Vector3(0f, 1f / 12f);
			}
		}
		else if (niceDog && Mathf.Abs(Object.FindObjectOfType<OverworldPlayer>().transform.position.x - ldog.transform.position.x) > 1f / 48f)
		{
			if (Object.FindObjectOfType<OverworldPlayer>().transform.position.x < ldog.transform.position.x && ldog.transform.localPosition.x > -0.459f)
			{
				ldog.transform.localPosition -= new Vector3(1f / 24f, 0f);
			}
			else if (Object.FindObjectOfType<OverworldPlayer>().transform.position.x > ldog.transform.position.x && ldog.transform.localPosition.x < 0.459f)
			{
				ldog.transform.localPosition += new Vector3(1f / 24f, 0f);
			}
		}
		if (!petting)
		{
			return;
		}
		pettingFrames++;
		if (state == 0)
		{
			int num = pettingFrames / 4;
			if (num > 3)
			{
				num = 3;
			}
			Object.FindObjectOfType<OverworldPlayer>().SetSprite(krisReachFrames[num]);
			if (num == 3)
			{
				ldog.sprite = dogSprites[1];
				ldog.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
				ldog.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = krisPetFrames[pettingFrames / 10 % 2];
			}
			if (pettingFrames == 60)
			{
				state = 1;
				pettingFrames = 0;
			}
		}
		else if (state == 1)
		{
			if (pettingFrames == 1)
			{
				ldog.sprite = dogSprites[0];
				ldog.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
			}
			int num2 = 3 - pettingFrames / 4;
			if (num2 < 0)
			{
				num2 = 0;
			}
			Object.FindObjectOfType<OverworldPlayer>().SetSprite(krisReachFrames[num2]);
			if (pettingFrames == 20)
			{
				ldog.sprite = dogSprites[2];
				Util.GameManager().PlayGlobalSFX("sounds/snd_pombark");
			}
			if (pettingFrames == 30)
			{
				ldog.sprite = dogSprites[0];
			}
			if (pettingFrames == 40)
			{
				Object.FindObjectOfType<OverworldPlayer>().EnableAnimator();
				Util.GameManager().EnablePlayerMovement();
				petting = false;
			}
		}
	}

	public override void DoInteract()
	{
		if ((int)Util.GameManager().GetFlag(253) == 1)
		{
			txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[1] { "* (Perhaps its best to let it\n  finally rest.)" }, true);
			Util.GameManager().DisablePlayerMovement(false);
		}
		else if ((int)Util.GameManager().GetFlag(253) == 2)
		{
			txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
			if ((int)Util.GameManager().GetFlag(258) == 1)
			{
				txt.CreateBox(new string[1] { "* (The dog is finally getting\n  some well-deserved sleep.)" }, true);
			}
			else
			{
				txt.CreateBox(new string[2] { "* (The dog looks happy to see\n  you.)", "* (Pet the dog?)" }, false);
				txt.EnableSelectionAtEnd();
				left = "Pet";
			}
			Util.GameManager().DisablePlayerMovement(false);
		}
		else
		{
			base.DoInteract();
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		selectActivated = false;
		if (index == Vector2.left)
		{
			if (niceDog)
			{
				pettingFrames = 0;
				petting = true;
				state = 0;
				Object.FindObjectOfType<OverworldPlayer>().ChangeDirection(Vector2.down);
				Object.FindObjectOfType<OverworldPlayer>().DisableAnimator();
			}
			else if ((int)Util.GameManager().GetFlag(13) >= 9 && (int)Util.GameManager().GetFlag(254) == 0)
			{
				Util.GameManager().SetFlag(254, 1);
				CutsceneHandler.GetCutscene(91).StartCutscene();
			}
			else
			{
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(new string[1] { "* (There's a dog hiding in here.)" }, false);
				startFight = true;
			}
		}
		else
		{
			txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[1] { niceDog ? "* (You wait to pet the dog\n  later.)" : "* (You leave the station\n  undisturbed.)" }, true);
		}
	}
}
