using UnityEngine;
using UnityEngine.UI;

public class MrSaturn : InteractKnockKnockDoor
{
	private bool doQuest;

	private bool showingQuest;

	private string prevMusic = "";

	private void Start()
	{
		if ((int)Util.GameManager().GetFlag(87) < 5)
		{
			switch (Util.GameManager().GetPlayerName())
			{
			case "BLG":
			case "IANB":
				ModifyContents(new string[7] { "* (Knock knock knock)", "*\tam in here.^10\n*\tboing!", "*\t...^15\n*\tyou ^N, ^10yes?", "*\ti hear you two people\n\tand not one.^05\n*\tding!", "*\ti also hear magic man want\n\tto see you.^05\n*\tbut who magic man?", "*\thuh?^05\n*\tyou name actually kris?", "*\tmaybe dream not real.^10\n*\tzoom!" }, new string[7] { "snd_text", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat" }, new int[7], new string[7]);
				break;
			case "KIWI":
				ModifyContents(new string[7] { "* (Knock knock knock)", "*\tam in here.^10\n*\tboing!", "*\t...^15\n*\tyou ^N, ^10yes?", "*\tyou do violence ding?", "*\ti also hear magic man want\n\tto see you.^05\n*\tbut who magic man?", "*\thuh?^05\n*\tyou name actually kris?", "*\tmaybe dream not real.^10\n*\tzoom!" }, new string[7] { "snd_text", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat" }, new int[7], new string[7]);
				break;
			case "RYNO":
			case "SCOOT":
			case "VYLET":
			case "TULIP":
				ModifyContents(new string[7] { "* (Knock knock knock)", "*\tam in here.^10\n*\tboing!", "*\t...^15\n*\tyou ^N, ^10yes?", "*\twho you fooling zoom?", "*\ti also hear magic man want\n\tto see you.^05\n*\tbut who magic man?", "*\thuh?^05\n*\tyou name actually kris?", "*\tmaybe dream not real.^10\n*\tzoom!" }, new string[7] { "snd_text", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat" }, new int[7], new string[7]);
				break;
			case "KRIS":
				ModifyContents(new string[6] { "* (Knock knock knock)", "*\tam in here.^10\n*\tboing!", "*\t...^15\n*\tyou ^N, ^10yes?", "*\ti hear magic man want\n\tto see you.^05\n*\tbut who magic man?", "*\thuh?^05\n*\thow i know name?", "*\thad weird dream.^10\n*\tzoom!" }, new string[7] { "snd_text", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat" }, new int[7], new string[7]);
				break;
			case "SUSIE":
				ModifyContents(new string[6] { "* (Knock knock knock)", "*\tam in here.^10\n*\tboing!", "*\t...^15\n*\tyou ^N, ^10yes?", "*\ti hear magic man want\n\tto see you.^05\n*\tbut who magic man?", "*\thuh?^05\n*\tyou friend name susie?", "*\thad weird dream.^10\n*\tzoom!" }, new string[7] { "snd_text", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat" }, new int[7], new string[7]);
				break;
			case "NOELLE":
				ModifyContents(new string[6] { "* (Knock knock knock)", "*\tam in here.^10\n*\tboing!", "*\t...^15\n*\tyou ^N, ^10yes?", "*\ti hear magic man want\n\tto see you.^05\n*\tbut who magic man?", "*\thuh?^05\n*\tyou friend name noelle?", "*\thad weird dream.^10\n*\tzoom!" }, new string[7] { "snd_text", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat" }, new int[7], new string[7]);
				break;
			}
		}
	}

	protected override void Update()
	{
		if (!txt && doQuest)
		{
			doQuest = false;
			showingQuest = true;
			Util.GameManager().PlayMusic("music/mus_dance_of_dog");
			GameObject obj = new GameObject("Quest", typeof(RectTransform), typeof(Image));
			obj.transform.SetParent(GameObject.Find("Canvas").transform);
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = new Vector3(1f, 1f, 1f);
			obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/spr_quest");
			obj.GetComponent<RectTransform>().sizeDelta = new Vector2(640f, 480f);
		}
		else if (showingQuest && (UTInput.GetButtonDown("Z") || UTInput.GetButtonDown("X")))
		{
			showingQuest = false;
			Object.Destroy(GameObject.Find("Quest"));
			Util.GameManager().PlayMusic(prevMusic);
			txt = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[2] { "*\thuh?^05\n*\tyou name actually kris?", "*\tmaybe dream not real.^10\n*\tzoom!" }, "snd_txtsat", 0, giveBackControl: true);
		}
	}

	public override void DoInteract()
	{
		if (Util.GameManager().GetPlayerName() == "SHAYY" && (int)Util.GameManager().GetFlag(170) == 0)
		{
			Util.GameManager().SetFlag(170, 1);
			Util.GameManager().PlayGlobalSFX("sounds/snd_knock");
			prevMusic = Util.GameManager().GetPlayingMusic();
			doQuest = true;
			txt = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[5] { "* (Knock knock knock)", "*\tam in here.^10\n*\tboing!", "*\t...^15\n*\tyou ^N, ^10yes?", "*\ti have thing to show!", "*\tready zoom?" }, new string[7] { "snd_text", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat", "snd_txtsat" }, new int[1], giveBackControl: false);
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			talkedToBefore = true;
		}
		else
		{
			base.DoInteract();
		}
	}
}
