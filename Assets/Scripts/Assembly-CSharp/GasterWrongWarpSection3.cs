using UnityEngine;

public class GasterWrongWarpSection3 : CutsceneBase
{
	private void Update()
	{
		if (state == 0 && !txt)
		{
			gm.SetPartyMembers(susie: true, noelle: true);
			Util.GameManager().SetFlag(178, 1);
			Util.GameManager().LockMenu();
			Util.GameManager().LoadArea(87, fadeIn: false, new Vector2(10.04f, -15.3f), Vector2.left);
			state = 1;
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		StartText(new string[6] { "* VERY CLEVER,^10 DREEMURR...", "* HOW CLEVER OF YOU TO BE\n  LED TO THIS LOCATION.", "* HOWEVER,^10 YOU ARE NOT SUPPOSED\n  TO BE HERE.", "* AS A MATTER OF FACT,^10\n  YOU'RE NOT.", "* RETURN TO WHERE YOU MUST\n  BE,^10 AND FORGET ABOUT OUR\n  INTERACTION...", "* UNTIL WE MEET AGAIN,^10 KRIS." }, new string[6] { "#v_gaster_ww_0", "#v_gaster_ww_1", "#v_gaster_ww_2", "#v_gaster_ww_3", "#v_gaster_ww_4", "#v_gaster_ww_5" }, new int[1] { 1 }, new string[0]);
		txt.MakeUnskippable();
		gm.SetFlag(231, 1);
	}
}
