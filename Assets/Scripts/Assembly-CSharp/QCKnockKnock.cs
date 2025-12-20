using System.Collections.Generic;
using UnityEngine;

public class QCKnockKnock : InteractTextBox
{
	private bool fadingOut;

	private void LateUpdate()
	{
		if (!txt && talkedToBefore)
		{
			talkedToBefore = false;
			if ((int)Util.GameManager().GetFlag(178) == 0)
			{
				Util.GameManager().DisablePlayerMovement(true);
				Object.FindObjectOfType<Fade>().FadeOut(7);
				fadingOut = true;
			}
		}
		if (fadingOut && !Object.FindObjectOfType<Fade>().IsPlaying())
		{
			fadingOut = false;
			Util.GameManager().LoadArea(88, true, new Vector3(0.79f, -3.37f), Vector2.up);
		}
	}

	public override void DoInteract()
	{
		bool flag = (int)Util.GameManager().GetFlag(178) == 1;
		if ((int)Util.GameManager().GetFlag(212) == 0)
		{
			List<string> list = new List<string> { "* (Knock knock knock)" };
			if (!Util.GameManager().NoelleInParty())
			{
				list.AddRange(new string[4] { "* Is that you out there,^05\n  purple lady?", "* Yeah.", "* Uhh...^05 got my friend\n  here.", "* Why don't y'all help yourselves\n  inside?" });
				sounds = new string[5] { "snd_text", "snd_text", "snd_txtsus", "snd_txtsus", "snd_text" };
				portraits = new string[5] { "", "", "su_smirk", "su_smile_side", "" };
			}
			else
			{
				list.AddRange(new string[15]
				{
					"* (...)",
					"* Who's there?!?!",
					(flag || (int)Util.GameManager().GetFlag(179) == 1 || (int)Util.GameManager().GetFlag(179) == 2) ? "* Ya better not be here\n  to terrorize us again,\n^05  bonehead!!!" : "* If you're here to break\n  our windows again, I'll\n  kill you!!!",
					"* Whoa, whoa, whoa,^05\n  CHILL dude!!",
					"* I've never even met\n  you before.",
					"* Q.C.?!^10\n* Is that you???",
					"* (Wait,^05 like the\n  restaurant???)",
					"* ... Who told you my name??",
					"* Huh...?",
					"* ...^05 Oh!",
					"* I've...^05 just known\n  for a while.",
					"* ...",
					"* Ah,^05 whatever.",
					"* Y'all seem like you're not\n  here to kill us anyway.",
					"* Why don't y'all help yourselves\n  inside?"
				});
				sounds = new string[13]
				{
					"snd_text", "snd_text", "snd_text", "snd_text", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_text", "snd_txtnoe",
					"snd_txtnoe", "snd_txtnoe", "snd_text"
				};
				portraits = new string[13]
				{
					"", "", "", "", "su_pissed", "su_annoyed", "no_shocked", "su_surprised", "", "no_confused",
					"no_awe", "no_happy", ""
				};
			}
			if (flag)
			{
				list.Add("* ... Once you put up that torch.");
			}
			lines = list.ToArray();
			Util.GameManager().SetFlag(212, 1);
		}
		else
		{
			bool flag2 = (int)Util.GameManager().GetFlag(208) != 2;
			lines = ((!flag) ? new string[2] { "* (Knock knock knock)", "* Come on in!" } : ((!flag2) ? new string[2] { "* (Knock knock knock)", "* Please put the torch up!" } : new string[3] { "* (Knock knock knock)", "* Put up the torch and\n  I'll let y'all in!", "* I've got two torch holders\n  surrounding the door if\n  you didn't notice." }));
			sounds = new string[1] { "snd_text" };
			portraits = new string[1] { "" };
		}
		base.DoInteract();
		Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_knock");
	}
}
