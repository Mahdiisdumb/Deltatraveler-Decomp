using System;
using System.Collections.Generic;
using UnityEngine;

public class FloweyFinalAttack : AttackBase
{
	private int angle;

	private bool hardmode;

	private List<FloweyPelletStandard> bullets = new List<FloweyPelletStandard>();

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
				FloweyPelletStandard component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/flowey/FloweyPelletStandard"), bbPos + new Vector2(Mathf.Sin((float)angle * (MathF.PI / 180f)) * 3f, Mathf.Cos((float)angle * (MathF.PI / 180f)) * 3f), Quaternion.identity, base.transform).GetComponent<FloweyPelletStandard>();
				component.SetPremadeVelocity(Vector3.zero);
				component.SetBaseDamage(0);
				bullets.Add(component);
				angle += 5;
			}
			if (frames == 40)
			{
				if (hardmode)
				{
					Util.FindObjectOfType<Flowey>().Chat(new string[1] { ((int)Util.GameManager().GetFlag(13) == 3) ? "... You actually \nWEREN'T Frisk!" : "... You weren't like \nthem at ALL!" }, "RightWide", "snd_txtflw2", new Vector2(182f, 126f), canSkip: true, 1);
				}
				else
				{
					Util.FindObjectOfType<Flowey>().Chat(new string[1] { ((int)Util.GameManager().GetFlag(13) == 3) ? "... Must have been \nas much of an \nIDIOT as YOU!" : "... Must have been \nas much of a \nWEAKLING as YOU!" }, "RightWide", "snd_txtflw2", new Vector2(182f, 126f), canSkip: true, 1);
				}
				Util.FindObjectOfType<Flowey>().GetTextBubble().gameObject.AddComponent<ShakingText>().StartShake(0, "speechbubble");
				state = 1;
				frames = 0;
			}
		}
		if (state == 1 && !Util.FindObjectOfType<TextBubble>())
		{
			frames++;
			if (frames == 1)
			{
				Util.GameManager().PlayGlobalSFX("sounds/snd_floweylaugh");
			}
			Util.FindObjectOfType<Flowey>().SetFace((frames / 2 % 2 == 0) ? "grin_dying" : "grin_laugh_dying");
			for (int i = 0; i < bullets.Count; i++)
			{
				bullets[i].transform.position -= new Vector3(Mathf.Sin((float)(i * 5) * (MathF.PI / 180f)), Mathf.Cos((float)(i * 5) * (MathF.PI / 180f))) / 48f;
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
				Util.GameManager().HealAll(100);
				Util.GameManager().PlayGlobalSFX("sounds/snd_heal");
				frames = 0;
				state = 2;
			}
		}
		if (state == 2)
		{
			frames++;
			if (frames == 20)
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/RudeBuster")).GetComponent<RudeBusterEffect>().AssignEnemy(Util.FindObjectOfType<Flowey>());
				Util.FindObjectOfType<TPBar>().SetSpecificTPUse(1, 50);
				Util.FindObjectOfType<TPBar>().UseTP();
				Util.FindObjectOfType<Flowey>().EnableDodge();
				Util.FindObjectOfType<Flowey>().SetFace("mad_dying");
			}
			if (frames == 40)
			{
				Util.FindObjectOfType<Flowey>().Chat(new string[3] { "Like that's gonna \nwork on me the \nSECOND time,^05 idiot!", "Whatever!\n^10It'll be a matter \nof time before that \nSOUL is mine!", "This isn't the last \nyou've seen of me!" }, "RightWide", "snd_txtflw2", new Vector2(182f, 126f), canSkip: true, 0);
				state = 3;
				frames = 0;
			}
		}
		if (state != 3)
		{
			return;
		}
		TextBubble textBubble = Util.FindObjectOfType<TextBubble>();
		if ((bool)textBubble)
		{
			if (textBubble.GetCurrentStringNum() == 2)
			{
				Util.FindObjectOfType<Flowey>().SetFace("grin_dying");
			}
			if (textBubble.GetCurrentStringNum() == 3)
			{
				Util.FindObjectOfType<Flowey>().SetFace("evil_dying");
			}
			return;
		}
		frames++;
		Color color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), (float)frames / 45f);
		Util.FindObjectOfType<Flowey>().GetPart("head").GetComponent<SpriteRenderer>()
			.color = color;
		Util.FindObjectOfType<Flowey>().GetPart("stem").GetComponent<SpriteRenderer>()
			.color = color;
		Util.FindObjectOfType<Flowey>().GetPart("hole").GetComponent<SpriteRenderer>()
			.color = color;
		if (frames == 1)
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_escaped");
		}
		if (frames == 30 && hardmode)
		{
			Util.FindObjectOfType<Flowey>().TriggerKrisFalling();
		}
		if (frames == 75)
		{
			Util.FindObjectOfType<BattleManager>().FadeEndBattle(2);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Util.FindObjectOfType<BattleManager>().StopMusic();
		Util.GameManager().PlayGlobalSFX("sounds/snd_crash");
		bb.StartMovement(new Vector2(29f, 29f), new Vector2(0f, -0.52f), instant: true);
		Util.FindObjectOfType<SOUL>().transform.position = bbPos;
		Util.FindObjectOfType<Flowey>().SetFace("grin_dying");
		hardmode = (int)Util.GameManager().GetFlag(108) == 1;
	}
}
