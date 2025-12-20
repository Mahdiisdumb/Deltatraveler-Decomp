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

	protected override void Awake()
	{
		base.Awake();
		curHP = (int[])Util.GameManager().GetHPArray().Clone();
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
					friskBeenDownedBefore = Util.FindObjectOfType<Toriel>().FriskHasDowned();
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
					Util.FindObjectOfType<Toriel>().Chat(new string[4] { "!!!", "My child, I am so \nsorry!", "Y-^05you can recover...", "Now,^05 back to you." }, "RightWide", "snd_txttor", new Vector2(178f, 141f), canSkip: true, 0);
					Util.FindObjectOfType<Toriel>().SetFace("gasp");
				}
			}
			else if (downed == 1)
			{
				if (!Util.FindObjectOfType<Toriel>().SusieHasDowned())
				{
					flag = false;
					talkState = 2;
					Util.FindObjectOfType<Toriel>().Chat(new string[4] { "...Finally.", "My child,^05 get out of \nthe way now.", "I can finish her off.", "...What are you doing?" }, "RightWide", "snd_txttor", new Vector2(178f, 141f), canSkip: true, 0);
					Util.FindObjectOfType<Toriel>().SetFace("contemplating");
				}
				else
				{
					Util.GameManager().PlayGlobalSFX("sounds/snd_heal");
					Util.GameManager().Heal(1, Util.GameManager().GetMaxHP(1) / 4);
				}
			}
			if (flag)
			{
				Object.Destroy(base.gameObject);
			}
		}
		else if (talkState == 1)
		{
			if (!Util.FindObjectOfType<Toriel>().GetTextBubble())
			{
				Object.Destroy(base.gameObject);
			}
			else if (Util.FindObjectOfType<Toriel>().GetTextBubble().GetCurrentStringNum() == 4)
			{
				Util.FindObjectOfType<Toriel>().SetFace("main");
			}
		}
		else if (talkState == 2)
		{
			if (!Util.FindObjectOfType<Toriel>().GetTextBubble())
			{
				Util.FindObjectOfType<Toriel>().SetFace("main");
				Util.GameManager().PlayGlobalSFX("sounds/snd_heal");
				Util.GameManager().Heal(1, Util.GameManager().GetMaxHP(1) / 4);
				Object.Destroy(base.gameObject);
			}
			else if (Util.FindObjectOfType<Toriel>().GetTextBubble().GetCurrentStringNum() == 4)
			{
				Util.FindObjectOfType<Toriel>().SetFace("weird");
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
		if (curHP[i] != Util.GameManager().GetHP(i))
		{
			return Util.GameManager().GetHP(i) <= 0;
		}
		return false;
	}
}
