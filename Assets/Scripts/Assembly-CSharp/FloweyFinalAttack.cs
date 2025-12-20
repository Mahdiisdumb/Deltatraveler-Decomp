using System;
using System.Collections.Generic;
using UnityEngine;

public class FloweyFinalAttack : AttackBase
{
	private int angle;

	private bool hardmode;

	private List<FloweyPelletStandard> bullets = new List<FloweyPelletStandard>();

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("start_kill_hm", new string[2] { "... You weren't like \nthem at ALL!", "... You actually \nWEREN'T Frisk!" });
		dictionary.Add("start_kill", new string[2] { "... Must have been \nas much of a \nWEAKLING as YOU!", "... Must have been \nas much of an \nIDIOT as YOU!" });
		dictionary.Add("after_rude_buster", new string[3] { "Like that's gonna \nwork on me the \nSECOND time,^05 idiot!", "Whatever!\n^10It'll be a matter \nof time before that \nSOUL is mine!", "This isn't the last \nyou've seen of me!" });
		return dictionary;
	}

	protected override void Awake()
	{
		base.Awake();
		SetStrings(GetDefaultStrings(), GetType());
	}

	protected override void Update()
	{
		if (!isStarted)
		{
			return;
		}
		if (state == 0)
		{
			if (frames < 10 || angle == 360)
			{
				frames++;
			}
			if (frames == 10 && angle < 360)
			{
				FloweyPelletStandard component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/flowey/FloweyPelletStandard"), bbPos + new Vector2(Mathf.Sin((float)angle * ((float)Math.PI / 180f)) * 3f, Mathf.Cos((float)angle * ((float)Math.PI / 180f)) * 3f), Quaternion.identity, base.transform).GetComponent<FloweyPelletStandard>();
				component.SetPremadeVelocity(Vector3.zero);
				component.SetBaseDamage(0);
				bullets.Add(component);
				angle += 5;
			}
			if (frames == 40)
			{
				if (hardmode)
				{
					UnityEngine.Object.FindObjectOfType<Flowey>().Chat(new string[1] { GetString("start_kill_hm", ((int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(13) == 3) ? 1 : 0) }, "RightWide", "snd_txtflw2", new Vector2(182f, 126f), true, 1);
				}
				else
				{
					UnityEngine.Object.FindObjectOfType<Flowey>().Chat(new string[1] { GetString("start_kill", ((int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(13) == 3) ? 1 : 0) }, "RightWide", "snd_txtflw2", new Vector2(182f, 126f), true, 1);
				}
				UnityEngine.Object.FindObjectOfType<Flowey>().GetTextBubble().gameObject.AddComponent<ShakingText>().StartShake(0, "speechbubble");
				state = 1;
				frames = 0;
			}
		}
		if (state == 1 && !UnityEngine.Object.FindObjectOfType<TextBubble>())
		{
			frames++;
			if (frames == 1)
			{
				UnityEngine.Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_floweylaugh");
			}
			UnityEngine.Object.FindObjectOfType<Flowey>().SetFace((frames / 2 % 2 == 0) ? "grin_dying" : "grin_laugh_dying");
			for (int i = 0; i < bullets.Count; i++)
			{
				bullets[i].transform.position -= new Vector3(Mathf.Sin((float)(i * 5) * ((float)Math.PI / 180f)), Mathf.Cos((float)(i * 5) * ((float)Math.PI / 180f))) / 48f;
			}
			if (frames == 127)
			{
				int count = bullets.Count;
				for (int j = 0; j < count; j++)
				{
					GameObject obj = bullets[0].gameObject;
					bullets.RemoveAt(0);
					UnityEngine.Object.Destroy(obj);
				}
				UnityEngine.Object.FindObjectOfType<GameManager>().HealAll(100);
				UnityEngine.Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_heal");
				frames = 0;
				state = 2;
			}
		}
		if (state == 2)
		{
			frames++;
			if (frames == 20)
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/RudeBuster")).GetComponent<RudeBusterEffect>().AssignEnemy(UnityEngine.Object.FindObjectOfType<Flowey>());
				UnityEngine.Object.FindObjectOfType<TPBar>().SetSpecificTPUse(1, 50);
				UnityEngine.Object.FindObjectOfType<TPBar>().UseTP();
				UnityEngine.Object.FindObjectOfType<Flowey>().EnableDodge();
				UnityEngine.Object.FindObjectOfType<Flowey>().SetFace("mad_dying");
			}
			if (frames == 40)
			{
				UnityEngine.Object.FindObjectOfType<Flowey>().Chat(GetStringArray("after_rude_buster"), "RightWide", "snd_txtflw2", new Vector2(182f, 126f), true, 0);
				state = 3;
				frames = 0;
			}
		}
		if (state != 3)
		{
			return;
		}
		TextBubble textBubble = UnityEngine.Object.FindObjectOfType<TextBubble>();
		if ((bool)textBubble)
		{
			if (textBubble.GetCurrentStringNum() == 2)
			{
				UnityEngine.Object.FindObjectOfType<Flowey>().SetFace("grin_dying");
			}
			if (textBubble.GetCurrentStringNum() == 3)
			{
				UnityEngine.Object.FindObjectOfType<Flowey>().SetFace("evil_dying");
			}
			return;
		}
		frames++;
		Color color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), (float)frames / 45f);
		UnityEngine.Object.FindObjectOfType<Flowey>().GetPart("head").GetComponent<SpriteRenderer>()
			.color = color;
		UnityEngine.Object.FindObjectOfType<Flowey>().GetPart("stem").GetComponent<SpriteRenderer>()
			.color = color;
		UnityEngine.Object.FindObjectOfType<Flowey>().GetPart("hole").GetComponent<SpriteRenderer>()
			.color = color;
		if (frames == 1)
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_escaped");
		}
		if (frames == 30 && hardmode)
		{
			UnityEngine.Object.FindObjectOfType<Flowey>().TriggerKrisFalling();
		}
		if (frames == 75)
		{
			UnityEngine.Object.FindObjectOfType<BattleManager>().FadeEndBattle(2);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		UnityEngine.Object.FindObjectOfType<BattleManager>().StopMusic();
		UnityEngine.Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_crash");
		bb.StartMovement(new Vector2(29f, 29f), new Vector2(0f, -0.52f), true);
		UnityEngine.Object.FindObjectOfType<SOUL>().transform.position = bbPos;
		UnityEngine.Object.FindObjectOfType<Flowey>().SetFace("grin_dying");
		hardmode = (int)Util.GameManager().GetFlag(108) == 1;
	}
}
