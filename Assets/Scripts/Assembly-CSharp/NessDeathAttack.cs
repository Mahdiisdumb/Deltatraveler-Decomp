using UnityEngine;

public class NessDeathAttack : AttackBase
{
	private bool bitchass;

	private Vector2 position;

	private string bubbleType;

	private string[] diag;

	private int curDiag;

	protected override void Awake()
	{
		base.Awake();
		diag = new string[3] { "* Paula used Horn of Life\n  on Ness.", "* ^05.^10.^10.^10.^10.^10.^10.^10.", "* It didn't work..." };
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Util.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
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
			if (frames == 1)
			{
				Util.FindObjectOfType<Ness>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ness/spr_b_ness_kill_1");
				Util.GameManager().PlayGlobalSFX("sounds/snd_noise");
			}
			if (frames == 25)
			{
				Util.FindObjectOfType<Ness>().Chat(new string[3] { "Paula...", "...", "Finish the fight...^15\nfor me..." }, bubbleType, "snd_txtness", position, canSkip: true, 2);
				Util.FindObjectOfType<Ness>().GetTextBubble().gameObject.AddComponent<ShakingText>().StartShake(0, "speechbubble");
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if ((bool)Util.FindObjectOfType<TextBubble>())
			{
				if (Util.FindObjectOfType<TextBubble>().GetCurrentStringNum() == 2 && !bitchass)
				{
					bitchass = true;
					Util.FindObjectOfType<Ness>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ness/spr_b_ness_kill_2");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				Util.FindObjectOfType<Ness>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ness/spr_b_ness_kill_3");
				Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyBlood"), Util.FindObjectOfType<Ness>().GetEnemyObject().transform.Find("mainbody").position + new Vector3(0f, 0.2f), Quaternion.identity);
				Util.GameManager().PlayGlobalSFX("sounds/snd_nessdie");
			}
			if (frames == 40)
			{
				Util.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_gasp_down");
				Util.FindObjectOfType<Paula>().SetX(Util.FindObjectOfType<Ness>().GetEnemyObject().transform.position.x);
				Util.FindObjectOfType<Paula>().Chat(new string[3] { "Ness,^05 please get up!!!", "You can't die!", "The world needs you..." }, bubbleType, "snd_txtpau", position, canSkip: true, 0);
				state = 2;
				frames = 0;
			}
		}
		else if (state == 2 && !Util.FindObjectOfType<TextBubble>())
		{
			if (frames == 0)
			{
				frames++;
				Util.FindObjectOfType<BattleManager>().StartText(diag[0], new Vector2(-4f, -134f), "snd_txtbtl");
				if (UTInput.GetButton("X") || UTInput.GetButton("C"))
				{
					Util.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
				}
			}
			else if ((UTInput.GetButton("X") || UTInput.GetButton("C")) && Util.FindObjectOfType<BattleManager>().GetBattleText().IsPlaying())
			{
				Util.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
			}
			else
			{
				if ((!UTInput.GetButtonDown("Z") && !UTInput.GetButton("C")) || Util.FindObjectOfType<BattleManager>().GetBattleText().IsPlaying())
				{
					return;
				}
				curDiag++;
				Util.FindObjectOfType<BattleManager>().GetBattleText().DestroyOldText();
				if (curDiag < 3)
				{
					Util.FindObjectOfType<BattleManager>().StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
					if (UTInput.GetButton("X") || UTInput.GetButton("C"))
					{
						Util.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
					}
				}
				else
				{
					Util.FindObjectOfType<Paula>().Chat(new string[1] { "Ness,^05 PLEASE!!!" }, bubbleType, "snd_txtpau", position, canSkip: true, 0);
					Util.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_cry_0");
					state = 3;
					frames = 0;
				}
			}
		}
		else if (state == 3 && !Util.FindObjectOfType<TextBubble>())
		{
			frames++;
			if (frames % 6 == 0 && frames < 59)
			{
				Util.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_cry_" + frames / 6 % 2);
			}
			if (frames == 120)
			{
				Util.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_cry_2");
				Util.FindObjectOfType<Paula>().Chat(new string[2] { "...", "You...^10 monsters..." }, bubbleType, "snd_txtpau", new Vector2(Mathf.Round(position.x * ((position.x > 0f) ? 2.8f : 2.3f)), position.y), canSkip: false, 2);
				state = 4;
				frames = 0;
			}
		}
		else if (state == 4)
		{
			if ((bool)Util.FindObjectOfType<TextBubble>())
			{
				if (Util.FindObjectOfType<TextBubble>().GetCurrentStringNum() == 2 && bitchass)
				{
					base.gameObject.AddComponent<AudioSource>().clip = Resources.Load<AudioClip>("sounds/snd_badsongintro");
					GetComponent<AudioSource>().Play();
					Util.FindObjectOfType<Ness>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sortingOrder = 20;
					if ((bool)Util.FindObjectOfType<EnemyBlood>())
					{
						Util.FindObjectOfType<EnemyBlood>().GetComponent<SpriteRenderer>().sortingOrder = 19;
					}
					Util.FindObjectOfType<Paula>().SeparateParts();
					Util.FindObjectOfType<Paula>().SetX(0f);
					Util.FindObjectOfType<Paula>().ActivateHeavyBreathing();
					bitchass = false;
				}
			}
			else
			{
				Util.FindObjectOfType<PartyPanels>().SetTargets(kris: true, susie: true, noelle: true);
				bb.StartMovement(new Vector2(165f, 140f), new Vector2(0f, -1.66f));
				Util.FindObjectOfType<SOUL>().transform.position = new Vector3(-0.055f, -1.63f);
				Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = true;
				state = 5;
				frames = 0;
			}
		}
		else
		{
			if (state != 5 || bb.IsPlaying())
			{
				return;
			}
			frames++;
			if (frames == 1)
			{
				Util.FindObjectOfType<SOUL>().SetControllable(boo: true);
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PaulaTarget")).GetComponent<PaulaMeleeTarget>().Activate(5, hard: true);
			}
			if (frames == 400)
			{
				if (GameManager.GetOptions().lowGraphics.value != 1)
				{
					Object.Instantiate(Resources.Load<GameObject>("vfx/BattleBGEffect/Earthbound/Paula"));
				}
				else
				{
					Object.Instantiate(Resources.Load<GameObject>("vfx/BattleBGEffect/Earthbound/PaulaLowGraphic"));
				}
				Util.FindObjectOfType<PaulaMeleeTarget>().SetToDestroy();
				Util.FindObjectOfType<BattleManager>().PlayMusic("music/mus_megalovania_frakture", 1f);
				Object.Destroy(base.gameObject);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		position = new Vector2((Util.FindObjectOfType<Ness>().GetEnemyObject().transform.position.x > 0f) ? (-77) : 65, 127f);
		bubbleType = ((Util.FindObjectOfType<Ness>().GetEnemyObject().transform.position.x > 0f) ? "LeftWide" : "RightWide");
		Util.FindObjectOfType<SOUL>().SetControllable(boo: false);
	}
}
