using System.Collections.Generic;
using UnityEngine;

public class TorielAttackBase : AttackBase
{
	private int[] curHP;

	protected bool talking;

	private int downed = -1;

	private bool friskBeenDownedBefore;

	private bool susieBeenDownedBefore;

	private bool talkAfterAttack;

	private int talkState;

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("downed_0", new string[4] { "!!!", "My child, I am so \nsorry!", "Y-^05you can recover...", "Now,^05 back to you." });
		dictionary.Add("downed_1", new string[4] { "...Finally.", "My child,^05 get out of \nthe way now.", "I can finish her off.", "...What are you doing?" });
		return dictionary;
	}

	protected override void Awake()
	{
		base.Awake();
		SetStrings(GetDefaultStrings(), GetType());
		curHP = (int[])Object.FindObjectOfType<GameManager>().GetHPArray().Clone();
	}

	protected override void Update()
	{
		if (!talking)
		{
			if (isStarted)
			{
				frames++;
			}
			if ((JustGotDowned(0) || JustGotDowned(1)) && downed == -1)
			{
				downed = ((!JustGotDowned(0)) ? 1 : 0);
				if (downed == 0)
				{
					friskBeenDownedBefore = Object.FindObjectOfType<Toriel>().FriskHasDowned();
				}
				else if (downed == 1)
				{
					talkAfterAttack = true;
				}
				if (!friskBeenDownedBefore && downed == 0)
				{
					talking = true;
					DestroyAllObjects();
				}
			}
			if (frames >= maxFrames && !talking)
			{
				if (talkAfterAttack)
				{
					DestroyAllObjects();
					talking = true;
				}
				else
				{
					Object.Destroy(base.gameObject);
				}
			}
		}
		else if (talkState == 0)
		{
			bool flag = true;
			if (downed == 0)
			{
				if (!friskBeenDownedBefore)
				{
					flag = false;
					talkState = 1;
					Object.FindObjectOfType<Toriel>().Chat(GetStringArray("downed_" + downed), "RightWide", "snd_txttor", new Vector2(178f, 141f), true, 0);
					Object.FindObjectOfType<Toriel>().SetFace("gasp");
				}
			}
			else if (downed == 1)
			{
				if (!Object.FindObjectOfType<Toriel>().SusieHasDowned())
				{
					flag = false;
					talkState = 2;
					Object.FindObjectOfType<Toriel>().Chat(GetStringArray("downed_" + downed), "RightWide", "snd_txttor", new Vector2(178f, 141f), true, 0);
					Object.FindObjectOfType<Toriel>().SetFace("contemplating");
				}
				else
				{
					Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_heal");
					Object.FindObjectOfType<GameManager>().Heal(1, Object.FindObjectOfType<GameManager>().GetMaxHP(1) / 4);
				}
			}
			if (flag)
			{
				Object.Destroy(base.gameObject);
			}
		}
		else if (talkState == 1)
		{
			if (!Object.FindObjectOfType<Toriel>().GetTextBubble())
			{
				Object.Destroy(base.gameObject);
			}
			else if (Object.FindObjectOfType<Toriel>().GetTextBubble().GetCurrentStringNum() == 4)
			{
				Object.FindObjectOfType<Toriel>().SetFace("main");
			}
		}
		else if (talkState == 2)
		{
			if (!Object.FindObjectOfType<Toriel>().GetTextBubble())
			{
				Object.FindObjectOfType<Toriel>().SetFace("main");
				Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_heal");
				Object.FindObjectOfType<GameManager>().Heal(1, Object.FindObjectOfType<GameManager>().GetMaxHP(1) / 4);
				Object.Destroy(base.gameObject);
			}
			else if (Object.FindObjectOfType<Toriel>().GetTextBubble().GetCurrentStringNum() == 4)
			{
				Object.FindObjectOfType<Toriel>().SetFace("weird");
			}
		}
	}

	private void DestroyAllObjects()
	{
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Object.DestroyImmediate(base.transform.GetChild(0).gameObject);
		}
	}

	private bool JustGotDowned(int i)
	{
		if (curHP[i] != Object.FindObjectOfType<GameManager>().GetHP(i))
		{
			return Object.FindObjectOfType<GameManager>().GetHP(i) <= 0;
		}
		return false;
	}
}
