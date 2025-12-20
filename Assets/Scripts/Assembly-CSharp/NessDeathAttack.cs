using System.Collections.Generic;
using UnityEngine;

public class NessDeathAttack : AttackBase
{
	private bool bitchass;

	private Vector2 position;

	private string bubbleType;

	private string[] diag;

	private int curDiag;

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("paula_tries_healing", new string[3] { "* Paula used Horn of Life\n  on Ness.", "* ^05.^10.^10.^10.^10.^10.^10.^10.", "* It didn't work..." });
		dictionary.Add("ness_fucking_dies", new string[3] { "Paula...", "...", "Finish the fight...^15\nfor me..." });
		dictionary.Add("paulas_shock", new string[3] { "Ness,^05 please get up!!!", "You can't die!", "The world needs you..." });
		dictionary.Add("paulas_plea", new string[1] { "Ness,^05 PLEASE!!!" });
		dictionary.Add("paula_is_pissed", new string[2] { "...", "You...^10 monsters..." });
		return dictionary;
	}

	protected override void Awake()
	{
		base.Awake();
		SetStrings(GetDefaultStrings(), GetType());
		diag = GetStringArray("paula_tries_healing");
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Object.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
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
				Object.FindObjectOfType<Ness>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ness/spr_b_ness_kill_1");
				Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_noise");
			}
			if (frames == 25)
			{
				Object.FindObjectOfType<Ness>().Chat(GetStringArray("ness_fucking_dies"), bubbleType, "snd_txtness", position, true, 2);
				Object.FindObjectOfType<Ness>().GetTextBubble().gameObject.AddComponent<ShakingText>().StartShake(0, "speechbubble");
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if ((bool)Object.FindObjectOfType<TextBubble>())
			{
				if (Object.FindObjectOfType<TextBubble>().GetCurrentStringNum() == 2 && !bitchass)
				{
					bitchass = true;
					Object.FindObjectOfType<Ness>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ness/spr_b_ness_kill_2");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				Object.FindObjectOfType<Ness>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ness/spr_b_ness_kill_3");
				Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyBlood"), Object.FindObjectOfType<Ness>().GetEnemyObject().transform.Find("mainbody").position + new Vector3(0f, 0.2f), Quaternion.identity);
				Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_nessdie");
			}
			if (frames == 40)
			{
				Object.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_gasp_down");
				Object.FindObjectOfType<Paula>().SetX(Object.FindObjectOfType<Ness>().GetEnemyObject().transform.position.x);
				Object.FindObjectOfType<Paula>().Chat(GetStringArray("paulas_shock"), bubbleType, "snd_txtpau", position, true, 0);
				state = 2;
				frames = 0;
			}
		}
		else if (state == 2 && !Object.FindObjectOfType<TextBubble>())
		{
			if (frames == 0)
			{
				frames++;
				Object.FindObjectOfType<BattleManager>().StartText(diag[0], new Vector2(-4f, -134f), "snd_txtbtl");
				if (UTInput.GetButton("X") || UTInput.GetButton("C"))
				{
					Object.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
				}
			}
			else if ((UTInput.GetButton("X") || UTInput.GetButton("C")) && Object.FindObjectOfType<BattleManager>().GetBattleText().IsPlaying())
			{
				Object.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
			}
			else
			{
				if ((!UTInput.GetButtonDown("Z") && !UTInput.GetButton("C")) || Object.FindObjectOfType<BattleManager>().GetBattleText().IsPlaying())
				{
					return;
				}
				curDiag++;
				Object.FindObjectOfType<BattleManager>().GetBattleText().DestroyOldText();
				if (curDiag < 3)
				{
					Object.FindObjectOfType<BattleManager>().StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
					if (UTInput.GetButton("X") || UTInput.GetButton("C"))
					{
						Object.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
					}
				}
				else
				{
					Object.FindObjectOfType<Paula>().Chat(GetStringArray("paulas_plea"), bubbleType, "snd_txtpau", position, true, 0);
					Object.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_cry_0");
					state = 3;
					frames = 0;
				}
			}
		}
		else if (state == 3 && !Object.FindObjectOfType<TextBubble>())
		{
			frames++;
			if (frames % 6 == 0 && frames < 59)
			{
				Object.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_cry_" + frames / 6 % 2);
			}
			if (frames == 120)
			{
				Object.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_cry_2");
				Object.FindObjectOfType<Paula>().Chat(GetStringArray("paula_is_pissed"), bubbleType, "snd_txtpau", new Vector2(Mathf.Round(position.x * ((position.x > 0f) ? 2.8f : 2.3f)), position.y), false, 2);
				state = 4;
				frames = 0;
			}
		}
		else if (state == 4)
		{
			if ((bool)Object.FindObjectOfType<TextBubble>())
			{
				if (Object.FindObjectOfType<TextBubble>().GetCurrentStringNum() == 2 && bitchass)
				{
					base.gameObject.AddComponent<AudioSource>().clip = Resources.Load<AudioClip>("sounds/snd_badsongintro");
					GetComponent<AudioSource>().Play();
					Object.FindObjectOfType<Ness>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sortingOrder = 20;
					if ((bool)Object.FindObjectOfType<EnemyBlood>())
					{
						Object.FindObjectOfType<EnemyBlood>().GetComponent<SpriteRenderer>().sortingOrder = 19;
					}
					Object.FindObjectOfType<Paula>().SeparateParts();
					Object.FindObjectOfType<Paula>().SetX(0f);
					Object.FindObjectOfType<Paula>().ActivateHeavyBreathing();
					bitchass = false;
				}
			}
			else
			{
				Object.FindObjectOfType<PartyPanels>().SetTargets(true, true, true);
				bb.StartMovement(new Vector2(165f, 140f), new Vector2(0f, -1.66f));
				Object.FindObjectOfType<SOUL>().transform.position = new Vector3(-0.055f, -1.63f);
				Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = true;
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
				Object.FindObjectOfType<SOUL>().SetControllable(true);
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PaulaTarget")).GetComponent<PaulaMeleeTarget>().Activate(5, true);
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
				Object.FindObjectOfType<PaulaMeleeTarget>().SetToDestroy();
				Object.FindObjectOfType<BattleManager>().PlayMusic("music/mus_megalovania_frakture", 1f);
				Object.Destroy(base.gameObject);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		if (GameManager.GetOptions().lowGraphics.value == 0)
		{
			Object.Destroy(Object.FindObjectOfType<ConfigureBackground>().gameObject);
		}
		position = new Vector2((Object.FindObjectOfType<Ness>().GetEnemyObject().transform.position.x > 0f) ? (-77) : 65, 127f);
		bubbleType = ((Object.FindObjectOfType<Ness>().GetEnemyObject().transform.position.x > 0f) ? "LeftWide" : "RightWide");
		Object.FindObjectOfType<SOUL>().SetControllable(false);
	}
}
