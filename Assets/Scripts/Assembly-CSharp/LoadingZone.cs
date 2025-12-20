using UnityEngine;

public class LoadingZone : OverworldManipulator
{
	[SerializeField]
	private int newScene = 2;

	[SerializeField]
	private Vector2 newPos = Vector2.zero;

	[SerializeField]
	private bool vertical = true;

	[SerializeField]
	private bool downOrLeft = true;

	[SerializeField]
	private int fadeType;

	[SerializeField]
	private int forceActivationFlag = -1;

	[SerializeField]
	private string denyText = "* (You felt that you shouldn't\n  advance.)";

	[SerializeField]
	private string denySound = "snd_text";

	[SerializeField]
	private string denyPortrait = "";

	[SerializeField]
	private Vector3 denyNudge = Vector3.zero;

	[SerializeField]
	private bool fadeMusic;

	private bool forceActivationTrigger;

	private Fade fade;

	private bool activated;

	private bool punchCardDetected;

	private void Start()
	{
		fade = Object.FindObjectOfType<Fade>();
		activated = false;
	}

	private void Update()
	{
		if (!activated || fade.IsPlaying())
		{
			return;
		}
		if ((int)Util.GameManager().GetSessionFlag(17) == 1)
		{
			Util.GameManager().SetSessionFlag(17, 0);
			Util.GameManager().ForceLoadArea(6);
		}
		else
		{
			if (!Object.FindObjectOfType<PunchCard>() && punchCardDetected)
			{
				if (newScene == 87 && (int)Util.GameManager().GetFlag(208) == 1 && (int)Util.GameManager().GetFlag(231) == 0)
				{
					newScene = 102;
					Util.GameManager().SetPartyMembers(false, false);
					Util.GameManager().SetFlag(178, 0);
					Util.GameManager().UnlockMenu();
				}
				Object.FindObjectOfType<GameManager>().TriggerWrongWarp();
			}
			Object.FindObjectOfType<GameManager>().EnablePlayerMovement();
			Vector2 dir = (vertical ? Vector2.up : Vector2.right);
			if (downOrLeft)
			{
				dir *= -1f;
			}
			if (newScene == 81 && (int)Util.GameManager().GetPersistentFlag(1) == 0)
			{
				int max = 50;
				if (PlayerPrefs.GetInt("CompletionState", 0) < 3 && (int)Util.GameManager().GetFlag(12) == 1 && GameManager.UsingRecordingSoftware())
				{
					max = 1;
				}
				if (Random.Range(0, max) == 0)
				{
					newScene = 101;
					if (Util.GameManager().SusieInParty())
					{
						Util.GameManager().SetSessionFlag(2, 1);
					}
					Util.GameManager().SetPartyMembers(false, false);
				}
			}
			Object.FindObjectOfType<GameManager>().LoadArea(newScene, true, newPos, dir);
		}
		activated = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!(collision.transform.tag == "Player") || activated || Object.FindObjectOfType<OverworldPlayer>().IsInitiatingBattle())
		{
			return;
		}
		if ((int)Util.GameManager().GetFlag(254) != 0)
		{
			if (newScene == 95 && (int)Util.GameManager().GetFlag(254) == 1)
			{
				int num = (int)Util.GameManager().GetFlag(255);
				Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
				Object.FindObjectOfType<OverworldPlayer>().transform.position += new Vector3(1f / 24f, 0f);
				switch (num)
				{
				case 0:
					new GameObject("txt").AddComponent<TextBox>().CreateBox(new string[2] { "* Kris,^05 we're wasting time.", "* We need to find\n  a way home." }, new string[1] { "snd_txtsus" }, new int[1], new string[2] { "su_annoyed", "su_annoyed" });
					break;
				case 1:
					new GameObject("txt").AddComponent<TextBox>().CreateBox(new string[3] { "* Dude,^05 I bet you\n  we'll find Sans...", "* Err...^05 OUR Sans...", "* I bet you we'll\n  find him up ahead." }, new string[1] { "snd_txtsus" }, new int[1], new string[3] { "su_annoyed", "su_inquisitive", "su_smirk_sweat" });
					break;
				case 2:
					new GameObject("txt").AddComponent<TextBox>().CreateBox(new string[1] { "* I can GUARANTEE you\n  we aren't missing\n  anything." }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_annoyed" });
					break;
				case 3:
					new GameObject("txt").AddComponent<TextBox>().CreateBox(new string[3] { "* ... Why do you want\n  to fight that dog\n  so BADLY?????", "* It's not even a\n  fucking challenge\n  or anything.", "* A weak ass,^05 paranoid\n  dog is NOT gonna\n  benefit us." }, new string[1] { "snd_txtsus" }, new int[1], new string[3] { "su_pissed", "su_pissed", "su_annoyed" });
					break;
				case 4:
					new GameObject("txt").AddComponent<TextBox>().CreateBox(new string[4] { "* (NOELLE,^05 DO SOMETHING.)", "* K-^05Kris...", "* Surely you can...^05\n  ummm.....", "* Do something else...?" }, new string[2] { "snd_txtsus", "snd_txtnoe" }, new int[1], new string[4] { "su_angry", "no_confused", "no_confused_side", "no_happy" });
					break;
				case 5:
					new GameObject("txt").AddComponent<TextBox>().CreateBox(new string[2] { "* Kris,^05 come on...", "* Maybe a better enemy\n  will be up ahead...?" }, new string[1] { "snd_txtnoe" }, new int[1], new string[2] { "no_depressed_side", "no_happy" });
					break;
				case 6:
					Util.GameManager().SetFlag(254, 2);
					new GameObject("txt").AddComponent<TextBox>().CreateBox(new string[10] { "* (Susie...)^05\n* (I don't think I\n  can handle this.)", "* ...", "* Okay,^05 FINE.", "* If you want to\n  murder a dog,^05 FINE.", "* But I'm not helping.", "* A-^03a-^03and I'm not coming\n  with...!", "* I...^05 I...", "* I can't...", "* ...", "* Just get this done\n  and over with." }, new string[10] { "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus" }, new int[1], new string[10] { "no_depressed_side", "su_thinking", "su_serious", "su_serious", "su_annoyed", "no_angry", "no_shocked", "no_depressed", "su_sad", "su_dejected" });
					break;
				}
				Util.GameManager().SetFlag(255, num + 1);
				return;
			}
			if ((int)Util.GameManager().GetFlag(254) == 2)
			{
				if (newScene == 95)
				{
					Util.GameManager().SetPartyMembers(true, false);
					Util.GameManager().SetSessionFlag(14, (Util.GameManager().GetCurrentZone() == 96) ? 1 : 0);
				}
				else if (newScene == 94 || newScene == 96)
				{
					if ((int)Util.GameManager().GetFlag(253) != 0)
					{
						Util.GameManager().SetFlag(254, 0);
					}
					Util.GameManager().SetPartyMembers(true, true);
				}
			}
		}
		if (newScene == 111)
		{
			Util.GameManager().SetPartyMembers(true, true);
		}
		if ((forceActivationFlag > -1 && (int)Object.FindObjectOfType<GameManager>().GetFlag(forceActivationFlag) == 0) || forceActivationTrigger)
		{
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			Object.FindObjectOfType<OverworldPlayer>().transform.position += denyNudge;
			new GameObject("txt").AddComponent<TextBox>().CreateBox(new string[1] { denyText }, new string[1] { denySound }, new int[1], new string[1] { denyPortrait });
			return;
		}
		if ((bool)Object.FindObjectOfType<PunchCard>())
		{
			punchCardDetected = true;
		}
		GameObject.Find("GameManager").GetComponent<GameManager>().DisablePlayerMovement(true);
		if (fadeType == 0)
		{
			fade.FadeOut(7);
			ActionSOUL actionSOUL = Object.FindObjectOfType<ActionSOUL>();
			BoxCollider2D boxCollider2D = (((object)actionSOUL != null) ? actionSOUL.GetComponent<BoxCollider2D>() : null);
			if (boxCollider2D != null)
			{
				boxCollider2D.enabled = false;
			}
			if (fadeMusic)
			{
				Util.GameManager().StopMusic(7f);
			}
		}
		else if (fadeType == 1)
		{
			fade.UTFadeOut();
		}
		activated = true;
	}

	public void SetForceActivationTrigger(bool forceActivationTrigger)
	{
		this.forceActivationTrigger = forceActivationTrigger;
	}

	public void ModifyContents(string text, string sound, string portrait)
	{
		denyText = text;
		denySound = sound;
		denyPortrait = portrait;
	}

	public int GetScene()
	{
		return newScene;
	}
}
