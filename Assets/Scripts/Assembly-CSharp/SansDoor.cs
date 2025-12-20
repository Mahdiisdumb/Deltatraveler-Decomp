using UnityEngine;

public class SansDoor : InteractTextBox
{
	private bool enteringRoom;

	private bool unlocking;

	protected override void Update()
	{
		base.Update();
		if (!txt && unlocking)
		{
			unlocking = false;
			enteringRoom = true;
			Util.GameManager().StopMusic(7f);
			Util.FindObjectOfType<Fade>().FadeOut(7);
		}
		if (enteringRoom && !Util.FindObjectOfType<Fade>().IsPlaying())
		{
			enteringRoom = false;
			Util.GameManager().SetPartyMembers(susie: false, noelle: false);
			Util.GameManager().LoadArea(117, fadeIn: true, new Vector2(-4.182f, 0.286f), Vector2.down);
		}
	}

	public override void DoInteract()
	{
		if (Util.GameManager().GetFlagInt(294) == 1)
		{
			enteringRoom = true;
			Util.FindObjectOfType<Fade>().FadeOut(7);
			Util.GameManager().StopMusic(7f);
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		}
		else if (Util.GameManager().GetFlagInt(292) == 1)
		{
			Util.GameManager().SetFlag(294, 1);
			GetComponent<AudioSource>().Play();
			bool flag = Util.GameManager().GetFlagInt(299) == 1;
			txt = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[10]
			{
				"* (You unlocked the door using\n  the silver key.)",
				flag ? "* So,^05 this is their\n  basement?" : "* Whoa,^05 Kris,^05 you breaking\n  and entering?",
				"* ...",
				flag ? "* Let's check it out." : "* Sounds pretty sweet.",
				flag ? "* Wait,^05 what if someone\n  thinks we're breaking\n  in?!" : "* Wait,^05 what if we\n  get caught?!",
				"* Hmm...",
				"* Y'know,^05 good point.",
				"* Kris,^05 why don't you go\n  in alone,^05 since you're\n  really quiet.",
				"* And me and Noelle will\n  act all nice and proper\n  until you come out.",
				"* Just don't break\n  anything."
			}, new string[10] { "snd_text", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], giveBackControl: false, new string[10] { "", "su_side", "su_side", "su_smile", "no_scared", "su_side", "su_smirk_sweat", "su_neutral", "su_smile_sweat", "su_annoyed" });
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			unlocking = true;
		}
		else
		{
			base.DoInteract();
		}
	}
}
