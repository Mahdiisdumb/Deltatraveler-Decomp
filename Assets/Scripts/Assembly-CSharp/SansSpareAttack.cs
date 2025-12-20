using UnityEngine;

public class SansSpareAttack : AttackBase
{
	private Sans sans;

	private int sleepFrames;

	protected override void Awake()
	{
		base.Awake();
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Object.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Object.FindObjectOfType<PartyPanels>().RaiseHeads(false, false, false);
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
		sans = Object.FindObjectOfType<Sans>();
	}

	protected override void Update()
	{
		if (!isStarted)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames == 30)
			{
				sans.SetFace("empty_down");
				sans.Chat(new string[3] { "you...^10 little...", "when i...^10 get my hands...^10 \non you...", "you'll look...^15 just like...^10 " }, "snd_txtsans", Util.GameManager().IsTestMode(), 1);
				state = 1;
				frames = 0;
			}
		}
		else
		{
			if (state != 1)
			{
				return;
			}
			if ((bool)sans.GetTextBubble())
			{
				if (sans.GetTextBubble().GetCurrentStringNum() == 2)
				{
					sans.SetFace("glare");
					sans.SetSweat(1);
					sans.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_torso_clutching");
				}
				else if (sans.GetTextBubble().GetCurrentStringNum() == 3)
				{
					if (Random.Range(0, 10) == 0)
					{
						sans.transform.position = new Vector3((float)Random.Range(-1, 2) / 24f, 0f);
					}
					else
					{
						sans.transform.position = Vector3.zero;
					}
					sans.SetFace("glare_sleepy");
					sans.SetSweat(0);
					sans.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_torso_clutching_grasp");
				}
				return;
			}
			frames++;
			if (frames < 20)
			{
				sans.transform.position = new Vector3((float)Random.Range(-1, 2) / 24f, 0f);
			}
			else
			{
				sans.transform.position = Vector3.zero;
			}
			if (frames == 20)
			{
				sans.SetSweat(-1);
				sans.SetFace("closed_sleep");
				sans.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_torso_sleep");
				sans.PlaySFX("sounds/snd_bump");
			}
			if (frames >= 20)
			{
				sleepFrames = (sleepFrames + 1) % 80;
				if (sleepFrames == 10 || sleepFrames == 20 || sleepFrames == 30)
				{
					Object.Instantiate(Resources.Load<GameObject>("battle/enemies/Sans/SansZ"), sans.GetPart("body").Find("head").position + new Vector3(0.25f, 0.45f), Quaternion.identity);
				}
			}
			if (frames == 140)
			{
				Util.GameManager().AddGold(sans.GetGold() / 2);
				Object.FindObjectOfType<BattleManager>().FadeEndBattle(2);
			}
		}
	}
}
