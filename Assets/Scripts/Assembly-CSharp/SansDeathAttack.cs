using UnityEngine;

public class SansDeathAttack : AttackBase
{
	private GameObject slicePrefab;

	private Sans sans;

	private Transform splitHeadPart;

	private bool fuckedUp;

	private string bloodPrefix = "";

	protected override void Awake()
	{
		base.Awake();
		bbPos = new Vector2(100f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Object.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Object.FindObjectOfType<PartyPanels>().RaiseHeads(false, false, false);
		Object.FindObjectOfType<PartyPanels>().transform.position = new Vector3(100f, 0f);
		Object.FindObjectOfType<TPBar>().transform.localPosition = new Vector3(-500f, 0f);
		Object.FindObjectOfType<DescriptionBox>().Vanish();
		soulPos.x = 100f;
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
		sans = Object.FindObjectOfType<Sans>();
		splitHeadPart = sans.GetPart("body").Find("head").Find("splithead");
		slicePrefab = Resources.Load<GameObject>("battle/Slice");
		if (GameManager.GetOptions().contentSetting.value == 1)
		{
			bloodPrefix = "_r";
		}
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
			if (frames <= 60)
			{
				BattleButton[] array = Object.FindObjectsOfType<BattleButton>();
				foreach (BattleButton battleButton in array)
				{
					battleButton.GetComponent<SpriteRenderer>().color = new Color(battleButton.GetComponent<SpriteRenderer>().color.r, battleButton.GetComponent<SpriteRenderer>().color.g, battleButton.GetComponent<SpriteRenderer>().color.b, 1f - (float)frames / 60f);
				}
			}
			if (frames >= 30 && frames <= 40)
			{
				if (frames == 30)
				{
					sans.SetFace("shocked_cracked_blood" + bloodPrefix);
					sans.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_torso_sliced_bleed_0");
					sans.PlaySFX("sounds/snd_break1");
				}
				float num = (float)(40 - frames) / 48f;
				Vector3 localPosition = new Vector3(0f, 0.875f) + new Vector3(Random.Range(-1, 2), (float)Random.Range(-1, 2) / 2f) * num;
				sans.GetPart("body").Find("head").localPosition = localPosition;
			}
			if (frames == 60)
			{
				Object.Destroy(Object.FindObjectOfType<SansBG>().gameObject);
				sans.StartDeathCore();
				sans.SetFace("split_shocked" + bloodPrefix);
				if (GameManager.GetOptions().contentSetting.value == 1)
				{
					splitHeadPart.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_otherhalfofhead_r");
				}
				sans.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_torso_sliced_bleed_1");
				splitHeadPart.GetComponent<SpriteRenderer>().enabled = true;
				splitHeadPart.GetComponent<SpriteRenderer>().color = Color.white;
			}
			if (frames >= 60 && frames < 90)
			{
				float num2 = (float)(frames - 60) / 30f;
				num2 = num2 * num2 * num2 * (num2 * (6f * num2 - 15f) + 10f);
				splitHeadPart.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0f, -114f, num2));
			}
			else if (frames >= 90 && frames <= 125)
			{
				float num3 = (float)(frames - 90) / 35f;
				num3 = num3 * num3 * num3 * (num3 * (6f * num3 - 15f) + 10f);
				splitHeadPart.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(-114f, -66f, num3));
			}
			else if (frames >= 125 && frames <= 145)
			{
				float num4 = (float)(frames - 125) / 20f;
				num4 *= num4;
				splitHeadPart.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(-66f, -90f, num4));
				splitHeadPart.localPosition = Vector3.Lerp(new Vector3(-0.541f, -0.332f), new Vector3(-0.541f, -1.166f), num4);
				if (frames == 145)
				{
					sans.PlaySFX("sounds/snd_noise");
				}
			}
			if (frames == 180)
			{
				Object.FindObjectOfType<BattleManager>().PlayMusic("music/mus_f_wind", 1f, true);
				sans.SetFace("split_think" + bloodPrefix);
				sans.SetSweat(-1);
				sans.Chat(new string[9] { "heh^15 heh^15 heh^05...", "so what goes 'round...^15\ncomes 'round...", "...", "what's up,^15 susie...?", "you expecting me to \ncry out in pain?", "do you honestly \nthink that this avenges \nthat orphan?", "that poor,^15 lost little \norphan named <color=#FF00FF>suzy</color>?", "ARE YOU STUPID!?", "AS IF HER FAMILY \nCARES THAT SHE'S DEAD." }, "snd_txtsans", Util.GameManager().IsTestMode(), 1);
				sans.GetTextBubble().GetComponent<ShakingText>().StartShake(10, "sans");
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
				if (sans.GetTextBubble().GetCurrentStringNum() == 8 && !fuckedUp)
				{
					fuckedUp = true;
					Object.FindObjectOfType<BattleManager>().StopMusic();
					sans.SetFace("split_laugh_0" + bloodPrefix);
					sans.GetTextBubble().GetComponent<ShakingText>().StartShake(0, "sans");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				Util.GameManager().PlayGlobalSFX("music/mus_sansdeath");
				Object.FindObjectOfType<BattleCamera>().BlastShake();
			}
			if (frames == 10)
			{
				Object.FindObjectOfType<BattleCamera>().GiantBlastShake();
			}
			sans.SetFace("split_laugh_" + frames / 4 % 2 + bloodPrefix);
			float num5 = (float)(frames % 14) / 7f;
			if (num5 > 1f)
			{
				num5 = 2f - num5;
			}
			float t = (float)(frames - 30) / 60f;
			Object.FindObjectOfType<BattleCamera>().GetComponent<Camera>().backgroundColor = Color.Lerp(Color.black, new Color(Mathf.Lerp(0.4f, 0f, t), 0f, 0f), num5);
			if (frames >= 20 && frames <= 140)
			{
				switch ((frames - 20) % 20)
				{
				case 0:
					Object.Instantiate(slicePrefab).GetComponent<PlayerAttackAnimation>().AssignValues(sans, 1, 20f, 3);
					break;
				case 12:
					if (frames < 120)
					{
						sans.Hit(1, 20f, true);
					}
					break;
				}
			}
			if (frames == 300)
			{
				Util.GameManager().AddEXP(sans.GetFinalEXP());
				Util.GameManager().AddGold(sans.GetGold());
				Object.FindObjectOfType<BattleManager>().FadeEndBattle(1);
			}
		}
	}
}
