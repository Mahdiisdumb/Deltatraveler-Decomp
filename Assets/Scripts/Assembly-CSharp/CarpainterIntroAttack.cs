using UnityEngine;

public class CarpainterIntroAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Util.FindObjectOfType<PartyPanels>().DeactivateTargets();
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
				Util.FindObjectOfType<Carpainter>().Hit(3, 80f, playSound: true);
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1 && !Util.FindObjectOfType<Carpainter>().IsShaking())
		{
			frames++;
			if (frames == 1)
			{
				frames = 1;
				Util.FindObjectOfType<Carpainter>().CombineParts();
			}
			if (frames == 20)
			{
				Util.FindObjectOfType<BattleManager>().PlayMusic("music/mus_sanctuary_challenge", 1f);
				state = 2;
				Util.FindObjectOfType<Carpainter>().Chat(new string[2] { "Of course,^05 a Franklin \nBadge.", "Now you shall face \nthe wrath of my <color=#0000FFFF>blue-\nblue martial arts!</color>" }, "RightWide", "snd_text", new Vector2(163f, 56f), canSkip: true, 0);
			}
		}
		else if (state == 2 && !Util.FindObjectOfType<TextBubble>())
		{
			Util.FindObjectOfType<Carpainter>().SeparateParts();
			Util.FindObjectOfType<BattleManager>().PlayMusic("music/mus_otherworldfoe", 1f, hasIntro: true);
			Object.Destroy(base.gameObject);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Util.FindObjectOfType<BattleManager>().StopMusic();
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
	}
}
