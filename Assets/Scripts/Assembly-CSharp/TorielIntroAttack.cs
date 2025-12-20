using System;
using UnityEngine;

public class TorielIntroAttack : AttackBase
{
	private int flameBulletIndex;

	private FlameBullet[] bullets = new FlameBullet[10];

	private Vector3 basePos;

	protected override void Awake()
	{
		base.Awake();
		frames = 0;
		maxFrames = 50000;
		bbSize = new Vector2(165f, 140f);
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
			if (frames >= 12)
			{
				if (frames % 3 == 0 && flameBulletIndex < 10)
				{
					Vector3 position = basePos + new Vector3(Mathf.Sin((float)(36 * flameBulletIndex) * (MathF.PI / 180f)), Mathf.Cos((float)(36 * flameBulletIndex) * (MathF.PI / 180f)));
					bullets[flameBulletIndex] = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/FlameBullet"), position, Quaternion.identity, base.transform).GetComponent<FlameBullet>();
					bullets[flameBulletIndex].SetBaseDamage(15);
					bullets[flameBulletIndex].GetComponent<AudioSource>().Play();
					flameBulletIndex++;
				}
				if (frames == 75)
				{
					Util.FindObjectOfType<Toriel>().Chat(new string[1] { "You shall suffer for \ntrying to kill the \nhuman." }, "RightWide", "snd_txttor", new Vector2(178f, 141f), canSkip: true, 0);
					state = 1;
					frames = 0;
				}
			}
		}
		else if (state == 1)
		{
			if (!Util.FindObjectOfType<Toriel>().GetTextBubble())
			{
				Util.FindObjectOfType<Toriel>().Chat(new string[1] { "B...^05 but \nMs. Dreemurr, ^05I...^02 " }, "Up", "snd_txtsus", new Vector2(0f, 50f), canSkip: false, 0);
				state = 2;
			}
		}
		else if (state == 2)
		{
			if (!Util.FindObjectOfType<Toriel>().GetTextBubble())
			{
				Util.FindObjectOfType<Toriel>().SetFace("rage");
				Util.FindObjectOfType<Toriel>().Chat(new string[1] { "WHAT DID YOU JUST \nCALL ME??!" }, "RightWide", "snd_txttor", new Vector2(178f, 141f), canSkip: true, 1);
				Util.FindObjectOfType<Toriel>().GetTextBubble().gameObject.AddComponent<ShakingText>().StartShake(0, "speechbubble");
				state = 3;
				frames = 0;
			}
			else if ((bool)Util.FindObjectOfType<Toriel>().GetTextBubble())
			{
				frames++;
				if (frames > 5 && !Util.FindObjectOfType<Toriel>().GetTextBubble().IsPlaying())
				{
					UnityEngine.Object.Destroy(Util.FindObjectOfType<Toriel>().GetTextBubble().gameObject);
				}
			}
		}
		else if (state == 3)
		{
			if ((bool)Util.FindObjectOfType<Toriel>().GetTextBubble())
			{
				return;
			}
			frames++;
			float num = (float)frames / 15f;
			for (int i = 0; i < 10; i++)
			{
				if (bullets[i] != null)
				{
					bullets[i].transform.position = basePos + Vector3.Lerp(new Vector3(Mathf.Sin((float)(36 * i) * (MathF.PI / 180f)), Mathf.Cos((float)(36 * i) * (MathF.PI / 180f))), Vector3.zero, num * num * num);
				}
			}
			if (frames == 10)
			{
				Util.GameManager().PlayGlobalSFX("sounds/snd_great_shine");
				Util.FindObjectOfType<SOUL>().CreateSOUL(Color.red, monster: false, player: true);
				Util.FindObjectOfType<SOUL>().Emanate(playSound: false);
				Util.FindObjectOfType<PartyPanels>().DeactivateManualManipulation();
				Util.FindObjectOfType<PartyPanels>().SetTargets(kris: true, susie: false, noelle: false);
			}
			if (frames == 15)
			{
				Util.FindObjectOfType<Toriel>().SetFace("gasp");
			}
			if (frames == 35)
			{
				Util.FindObjectOfType<Toriel>().Chat(new string[3] { "W-^05what did you \njust...", "Did you trip...?", "My child,^05 get out \nof the way." }, "RightWide", "snd_txttor", new Vector2(178f, 141f), canSkip: true, 0);
				state = 4;
			}
		}
		else if (state == 4)
		{
			if (!Util.FindObjectOfType<Toriel>().GetTextBubble())
			{
				Util.FindObjectOfType<BattleManager>().PlayMusic("music/mus_boss1", 1f);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else if (Util.FindObjectOfType<Toriel>().GetTextBubble().GetCurrentStringNum() == 3 && frames != 69)
			{
				frames = 69;
				Util.FindObjectOfType<Toriel>().SetFace("main");
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		SOUL sOUL = SOUL.FindPlayerSOUL();
		sOUL.GetComponent<SpriteRenderer>().enabled = true;
		sOUL.GetComponent<SOUL>().CreateSOUL(Color.white, monster: true, player: false);
		UnityEngine.Object.FindFirstObjectByType<BattleManager>().StopMusic();
		basePos = Util.FindObjectOfType<SOUL>().transform.position;
		Util.FindObjectOfType<PartyPanels>().ActivateManualManipulation();
		Util.FindObjectOfType<PartyPanels>().transform.Find("Party0Stats").transform.localPosition = new Vector3(-420f, -159f);
		Util.FindObjectOfType<PartyPanels>().transform.Find("Party1Stats").transform.localPosition = new Vector3(0f, -159f);
	}
}
