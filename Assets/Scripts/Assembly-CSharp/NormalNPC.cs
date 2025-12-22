using UnityEngine;

public class NormalNPC : InteractTextBox
{
	private int talkCount;

	private bool weirdCutscene;

	private int weirdFrames;

	private bool eggSound;

	protected override void Awake()
	{
		if (WeirdChecker.HasKilled(Util.GameManager()) || !WeirdChecker.HasSparedEveryEncounterInUnderfell(Util.GameManager()) || Util.GameManager().GetFlagInt(296) == 1 || Util.GameManager().GetEXP() > 0)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			base.Awake();
		}
	}

	protected override void Update()
	{
		base.Update();
		if ((bool)txt && txt.GetCurrentStringNum() == lines.Length && !eggSound)
		{
			eggSound = true;
			Util.GameManager().PlayGlobalSFX("sounds/snd_egg");
		}
		if (!weirdCutscene || (bool)txt)
		{
			return;
		}
		weirdFrames++;
		if (weirdFrames == 1)
		{
			base.transform.position = new Vector3(100f, 0f);
			Util.GameManager().StopMusic();
			Util.GameManager().PlayGlobalSFX("sounds/snd_mysterygo");
			OverworldPartyMember[] array = Util.FindObjectsOfType<OverworldPartyMember>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UseUnhappySprites();
			}
		}
		if (weirdFrames == 60)
		{
			weirdCutscene = false;
			bool flag = Util.GameManager().GetFlagInt(63) == 1 || Util.GameManager().GetFlagInt(152) == 1;
			txt = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[9]
			{
				"* What the fuck.",
				"* I...",
				"* W-^05who's \"he\"?",
				"* I dunno,^05 but I'll\n  kick his ass.",
				"* With what we've fought\n  so far,^05 it can't\n  be TOO hard,^05 right?",
				"* ...",
				"* Or this guy's just\n  messing with us.",
				flag ? "* These egg weirdos are...^10\n  really weird." : "* They give off really\n  weird vibes.",
				"* But let's not think\n  about it too hard."
			}, new string[9] { "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], giveBackControl: true, new string[9] { "su_inquisitive", "no_shocked", "no_afraid", "su_annoyed", "su_smile_side", "no_confused", "su_annoyed", "su_side", "su_neutral" });
			Object.Destroy(base.gameObject);
		}
	}

	public override void DoInteract()
	{
		if (talkedToBefore)
		{
			talkCount++;
		}
		else
		{
			Util.GameManager().SetFlag(286, 1);
			Util.GameManager().SetFlag(296, 1);
		}
		if (talkCount == 6)
		{
			weirdCutscene = true;
			txt = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[4] { "* Are you waiting for me\n  to slip up?", "* Or did you find this\n  where you shouldn't be\n  looking?", "* Haha...^05\n* I can only imagine what\n  kind of damnation awaits me.", "* After all,^05 he watches us all\n  the same." }, giveBackControl: false);
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		}
		else
		{
			base.DoInteract();
			if (talkCount > 0)
			{
				lines2[2] += "!";
			}
		}
	}
}
