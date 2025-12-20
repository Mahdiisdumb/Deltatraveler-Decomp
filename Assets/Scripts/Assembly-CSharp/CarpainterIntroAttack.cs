using System.Collections.Generic;
using UnityEngine;

public class CarpainterIntroAttack : AttackBase
{
	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("intro", new string[2] { "Of course,^05 a Franklin \nBadge.", "Now you shall face \nthe wrath of my <color=#0000FFFF>blue-\nblue martial arts!</color>" });
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
			frames++;
			if (frames == 10)
			{
				Object.FindObjectOfType<Carpainter>().Hit(3, 80f, true);
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1 && !Object.FindObjectOfType<Carpainter>().IsShaking())
		{
			frames++;
			if (frames == 1)
			{
				frames = 1;
				Object.FindObjectOfType<Carpainter>().CombineParts();
			}
			if (frames == 20)
			{
				Object.FindObjectOfType<BattleManager>().PlayMusic("music/mus_sanctuary_challenge", 1f);
				state = 2;
				Object.FindObjectOfType<Carpainter>().Chat(GetStringArray("intro"), "RightWide", "snd_text", new Vector2(163f, 56f), true, 0);
			}
		}
		else if (state == 2 && !Object.FindObjectOfType<TextBubble>())
		{
			Object.FindObjectOfType<Carpainter>().SeparateParts();
			Object.FindObjectOfType<BattleManager>().PlayMusic("music/mus_otherworldfoe", 1f, true);
			Object.Destroy(base.gameObject);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.FindObjectOfType<BattleManager>().StopMusic();
	}
}
