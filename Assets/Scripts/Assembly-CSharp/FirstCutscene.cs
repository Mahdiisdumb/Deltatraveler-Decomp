using UnityEngine;

public class FirstCutscene : CutsceneBase
{
	private Vector3 krisBasePos;

	private Vector3 susieBasePos;

	private Vector3 camOrigPos;

	private bool hardMode;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames == 45)
			{
				PlaySFX("sounds/snd_wing", 0.8f);
			}
			if (frames == 80)
			{
				susie.SetSprite("spr_su_kneel");
				PlaySFX("sounds/snd_wing", 0.9f);
			}
			if ((frames >= 45 && frames <= 48) || (frames >= 80 && frames <= 83))
			{
				int num = ((frames % 2 == 0) ? 1 : (-1));
				int num2 = ((frames > 48) ? 83 : 48) - frames;
				susie.transform.position = susieBasePos + new Vector3((float)(num2 * num) / 24f, 0f);
			}
			if (frames == 105)
			{
				StartText(new string[2] { "* Ugh...", "* ...what the hell?" }, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[2], new string[2] { "su_side", "su_side_sweat" }, 1);
				frames = 0;
				state = 1;
			}
		}
		if (state == 1 && !txt)
		{
			frames++;
			if (frames == 10)
			{
				susie.SetSprite("unhappy/spr_su_down_0_unhappy");
				PlaySFX("sounds/snd_wing");
			}
			if (frames == 40)
			{
				susie.SetSprite("unhappy/spr_su_right_0_unhappy");
			}
			if (frames == 60)
			{
				if (hardMode)
				{
					StartText(new string[1] { "* ... Is that a\n  little kid???" }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_surprised" }, 1);
				}
				else
				{
					StartText(new string[1] { "* Kris???\n^10* Is that you?" }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_surprised" }, 1);
				}
				state = 2;
				frames = 0;
			}
		}
		if (state == 2 && !txt)
		{
			frames++;
			if (frames == 15)
			{
				kris.SetSprite(hardMode ? "spr_fr_sit_injured" : "spr_kr_sit_injured");
				PlaySFX("sounds/snd_wing", 0.9f);
			}
			if (frames >= 15 && frames <= 18)
			{
				int num3 = ((frames % 2 == 0) ? 1 : (-1));
				int num4 = 18 - frames;
				kris.transform.position = krisBasePos + new Vector3((float)(num4 * num3) / 24f, 0f);
			}
			if (frames == 40)
			{
				kris.EnableAnimator();
				kris.ChangeDirection(Vector2.down);
				PlaySFX("sounds/snd_wing");
			}
			if (frames == 90)
			{
				susie.SetSprite("spr_su_pissed");
				PlaySFX("sounds/snd_whip_hard");
			}
			if (frames == 100)
			{
				if (hardMode)
				{
					StartText(new string[1] { "* GOD DAMN IT WHERE THE\n  HELL AM I" }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_wtf" }, 1);
				}
				else
				{
					StartText(new string[1] { "* GOD DAMN IT KRIS WHERE\n  THE HELL ARE WE" }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_wtf" }, 1);
				}
				state = 3;
				frames = 0;
			}
		}
		if (state == 3 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				susie.SetSprite("spr_su_up_0");
			}
			if (frames == 30)
			{
				if (hardMode)
				{
					StartText(new string[5] { "* ...^05 Y'know.", "* I have no idea\n  who you are.", "* But something about\n  you reminds me of\n  my friend.", "* I mean, it wouldn't\n  hurt to try to get\n  outta here together.", "* So... let's stick\n  together for now." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[5], new string[5] { "su_side", "su_neutral", "su_depressed", "su_smile_sweat", "su_smirk" }, 1);
				}
				else
				{
					StartText(new string[2] { "* Whatever.", "* Let's just try to find\n  a way outta here." }, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[2], new string[2] { "su_neutral", "su_smile_sweat" }, 1);
				}
				state = 4;
				frames = 0;
			}
		}
		if (state == 4 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				kris.EnableAnimator();
				susie.EnableAnimator();
			}
			cam.transform.position = Vector3.Lerp(new Vector3(0f, camOrigPos.y, -10f), camOrigPos, (float)frames / 15f);
			if (frames == 15)
			{
				cam.SetFollowPlayer(follow: true);
				gm.SetFlag(3, 1);
				EndCutscene();
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		if ((int)gm.GetFlag(108) == 1)
		{
			hardMode = true;
		}
		gm.SetPartyMembers(susie: true, noelle: false);
		kris.DisableAnimator();
		if (hardMode)
		{
			kris.SetSprite("spr_fr_ko");
			kris.transform.position -= new Vector3(0f, 0.363f);
		}
		else
		{
			kris.SetSprite("spr_kr_ko");
		}
		susie.DisableAnimator();
		susie.SetSprite("spr_su_ko");
		susie.transform.position = new Vector3(-1.109f, 0.624f);
		krisBasePos = kris.transform.position;
		susieBasePos = susie.transform.position;
		cam.SetFollowPlayer(follow: false);
		camOrigPos = cam.GetClampedPos();
		cam.transform.position = new Vector3(0f, camOrigPos.y, -10f);
	}
}
