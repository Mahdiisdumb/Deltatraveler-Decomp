using UnityEngine;

public class SusieLDAttack : AttackBase
{
	private GameObject axePrefab;

	private SusieLD susie;

	private GameObject rudeBusterPrefab;

	private int axeSpawnFrames;

	private int axeSpawnRate = 30;

	private bool removeRate = true;

	private bool debug;

	private bool spawnOnRightFirst;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector3(165f, 140f);
		Object.FindObjectOfType<BattleManager>().GetComponent<MusicPlayer>().FadeOut(2f);
		axePrefab = Resources.Load<GameObject>("battle/attacks/bullets/snowdin/SusieLDAxeBullet");
		rudeBusterPrefab = Resources.Load<GameObject>("battle/attacks/bullets/snowdin/SusieLDBigRudeBuster");
		susie = Object.FindObjectOfType<SusieLD>();
	}

	protected override void Update()
	{
		if (state == 0)
		{
			frames++;
			susie.GetEnemyObject().transform.position = new Vector3(Mathf.Lerp(8f, 0f, (float)(frames - 30) / 60f), 1.294f);
			if (frames == 105)
			{
				bool flag = (int)Util.GameManager().GetFlag(261) == 1;
				susie.Chat(new string[10]
				{
					flag ? "Of course you did \nthat." : "Huh...",
					flag ? "But,^05 umm...^05\nI..." : "I,^05 uhh...",
					"Didn't think we'd be \ndoing this so soon.",
					"Ummm...",
					"...",
					"You're right,^05 I guess.",
					"But I don't really \nknow any magic,^05 dude.",
					"...",
					"Think about what I \ndid in the <color=#FF0000FF>Dark \nWorld</color>?",
					"Hmm..."
				}, "RightWide", "snd_txtsus", Vector2.zero, true, 0);
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if ((bool)susie.GetTextBubble())
			{
				if (susie.GetTextBubble().GetCurrentStringNum() == 2 || susie.GetTextBubble().GetCurrentStringNum() == 4)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_side_sweat");
				}
				else if (susie.GetTextBubble().GetCurrentStringNum() == 3 || susie.GetTextBubble().GetCurrentStringNum() == 6)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_smile_sweat");
				}
				else if (susie.GetTextBubble().GetCurrentStringNum() == 5)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_neutral");
				}
				else if (susie.GetTextBubble().GetCurrentStringNum() == 7)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_annoyed");
				}
				else if (susie.GetTextBubble().GetCurrentStringNum() == 8)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_side");
				}
				else if (susie.GetTextBubble().GetCurrentStringNum() == 9)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_neutral_crossed");
				}
				else if (susie.GetTextBubble().GetCurrentStringNum() == 10)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_thinking");
				}
			}
			else
			{
				state = 2;
			}
		}
		else if (state == 2)
		{
			frames++;
			axeSpawnFrames++;
			if (frames >= 350)
			{
				removeRate = false;
			}
			if (axeSpawnFrames >= axeSpawnRate)
			{
				removeRate = !removeRate;
				if (axeSpawnRate > 6 && removeRate)
				{
					axeSpawnRate--;
				}
				Object.Instantiate(axePrefab, base.transform);
				axeSpawnFrames = 0;
			}
			if (frames == 90)
			{
				susie.Chat(new string[1] { "(So like when me \nand Lancer fought \nKris and Ralsei...)" }, "RightWide", "snd_txtsus", Vector2.zero, false, 0);
				susie.GetTextBubble().Disable();
			}
			if (frames == 220)
			{
				susie.Chat(new string[1] { "(But that's lame...)" }, "RightWide", "snd_txtsus", Vector2.zero, false, 0);
				susie.GetTextBubble().Disable();
			}
			if (frames == 420)
			{
				susie.Chat(new string[1] { "(Hm^03m^03m^03m^03m^03m^03...)" }, "RightWide", "snd_txtsus", Vector2.zero, false, 0);
				susie.GetTextBubble().Disable();
			}
			if (frames == 160 || frames == 280 || frames == 520)
			{
				Object.Destroy(susie.GetTextBubble().gameObject);
			}
			if (frames == 660 || (frames == 1 && debug))
			{
				Util.GameManager().PlayGlobalSFX("sounds/snd_bell");
				susie.Chat(new string[2] { "Oh wait no duh.", "RUDE BUSTER!!!!!!" }, "RightWide", "snd_txtsus", Vector2.zero, false, 0);
				susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_notice");
				frames = 0;
				state = 3;
			}
		}
		else if (state == 3)
		{
			if ((bool)susie.GetTextBubble() && susie.GetTextBubble().GetCurrentStringNum() == 2)
			{
				susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_excited");
			}
			else if (!susie.GetTextBubble())
			{
				frames++;
				int num = frames / 3;
				if (num > 7)
				{
					num = 7;
				}
				susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_rudebuster_" + num);
				if (frames == 9)
				{
					Object.FindObjectOfType<TPBar>().RemoveTP(50);
					susie.PlaySFX("sounds/snd_rudebuster_swing");
					spawnOnRightFirst = Object.FindObjectOfType<SOUL>().transform.position.x >= 0f;
					int num2 = (spawnOnRightFirst ? 1 : (-1));
					Object.Instantiate(rudeBusterPrefab, new Vector3(0.45f * (float)num2, 4f), Quaternion.Euler(0f, 0f, 90 - 30 * num2), base.transform);
					Object.Instantiate(rudeBusterPrefab, new Vector3(0f, 4.5f), Quaternion.Euler(0f, 0f, 90f), base.transform);
				}
				if (frames == 29)
				{
					int num3 = (spawnOnRightFirst ? 1 : (-1));
					Object.Instantiate(rudeBusterPrefab, new Vector3(-0.4f * (float)num3, 4f), Quaternion.Euler(0f, 0f, 90 + 55 * num3), base.transform);
				}
				if (frames == 39)
				{
					int num4 = (spawnOnRightFirst ? 1 : (-1));
					Object.Instantiate(rudeBusterPrefab, new Vector3(0.4f * (float)num4, 4f), Quaternion.Euler(0f, 0f, 90 - 55 * num4), base.transform);
				}
				if (frames == 79)
				{
					susie.Chat(new string[10] { "I dunno why I \ndidn't think of that \nsooner.", "Huh?^05\nI was shooting \nbullets at you?", "No way.", "I wasn't even doing \nanything.", "I was thinking about \nhow I could try to \nthrow a bunch of \naxes at your heart.", "... That's actually \nwhat HAPPENED?????", "...", "Damn,^05 I'll keep that \nin mind next time \nI have to do \nthis.", "But,^05 uhh...", "I guess we should \nget going." }, "RightWide", "snd_txtsus", Vector2.zero, true, 0);
					state = 4;
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_confident");
				}
			}
		}
		else
		{
			if (state != 4)
			{
				return;
			}
			if ((bool)susie.GetTextBubble())
			{
				if (susie.GetTextBubble().GetCurrentStringNum() == 2)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_neutral_crossed");
				}
				else if (susie.GetTextBubble().GetCurrentStringNum() == 3)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_annoyed");
				}
				else if (susie.GetTextBubble().GetCurrentStringNum() == 5)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_thinking");
				}
				else if (susie.GetTextBubble().GetCurrentStringNum() == 6)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_notice");
				}
				else if (susie.GetTextBubble().GetCurrentStringNum() == 8)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_smile_sweat");
				}
				else if (susie.GetTextBubble().GetCurrentStringNum() == 9)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_side");
				}
				else if (susie.GetTextBubble().GetCurrentStringNum() == 10)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_neutral");
				}
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
