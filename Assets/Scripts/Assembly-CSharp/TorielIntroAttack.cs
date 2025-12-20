using System;
using System.Collections.Generic;
using UnityEngine;

public class TorielIntroAttack : AttackBase
{
	private int flameBulletIndex;

	private FlameBullet[] bullets = new FlameBullet[10];

	private Vector3 basePos;

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("intro", new string[3] { "You shall suffer for \ntrying to kill the \nhuman.", "B...^05 but \nMs. Dreemurr, ^05I...^02 ", "WHAT DID YOU JUST \nCALL ME??!" });
		dictionary.Add("frisk_defends", new string[3] { "W-^05what did you \njust...", "Did you trip...?", "My child,^05 get out \nof the way." });
		return dictionary;
	}

	protected override void Awake()
	{
		base.Awake();
		SetStrings(GetDefaultStrings(), GetType());
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
					Vector3 position = basePos + new Vector3(Mathf.Sin((float)(36 * flameBulletIndex) * ((float)Math.PI / 180f)), Mathf.Cos((float)(36 * flameBulletIndex) * ((float)Math.PI / 180f)));
					bullets[flameBulletIndex] = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/FlameBullet"), position, Quaternion.identity, base.transform).GetComponent<FlameBullet>();
					bullets[flameBulletIndex].SetBaseDamage(15);
					bullets[flameBulletIndex].GetComponent<AudioSource>().Play();
					flameBulletIndex++;
				}
				if (frames == 75)
				{
					UnityEngine.Object.FindObjectOfType<Toriel>().Chat(new string[1] { GetString("intro", 0) }, "RightWide", "snd_txttor", new Vector2(178f, 141f), true, 0);
					state = 1;
					frames = 0;
				}
			}
		}
		else if (state == 1)
		{
			if (!UnityEngine.Object.FindObjectOfType<Toriel>().GetTextBubble())
			{
				UnityEngine.Object.FindObjectOfType<Toriel>().Chat(new string[1] { GetString("intro", 1) }, "Up", "snd_txtsus", new Vector2(0f, 50f), false, 0);
				state = 2;
			}
		}
		else if (state == 2)
		{
			if (!UnityEngine.Object.FindObjectOfType<Toriel>().GetTextBubble())
			{
				UnityEngine.Object.FindObjectOfType<Toriel>().SetFace("rage");
				UnityEngine.Object.FindObjectOfType<Toriel>().Chat(new string[1] { GetString("intro", 2) }, "RightWide", "snd_txttor", new Vector2(178f, 141f), true, 1);
				UnityEngine.Object.FindObjectOfType<Toriel>().GetTextBubble().gameObject.AddComponent<ShakingText>().StartShake(0, "speechbubble");
				state = 3;
				frames = 0;
			}
			else if ((bool)UnityEngine.Object.FindObjectOfType<Toriel>().GetTextBubble())
			{
				frames++;
				if (frames > 5 && !UnityEngine.Object.FindObjectOfType<Toriel>().GetTextBubble().IsPlaying())
				{
					UnityEngine.Object.Destroy(UnityEngine.Object.FindObjectOfType<Toriel>().GetTextBubble().gameObject);
				}
			}
		}
		else if (state == 3)
		{
			if ((bool)UnityEngine.Object.FindObjectOfType<Toriel>().GetTextBubble())
			{
				return;
			}
			frames++;
			float num = (float)frames / 15f;
			for (int i = 0; i < 10; i++)
			{
				if (bullets[i] != null)
				{
					bullets[i].transform.position = basePos + Vector3.Lerp(new Vector3(Mathf.Sin((float)(36 * i) * ((float)Math.PI / 180f)), Mathf.Cos((float)(36 * i) * ((float)Math.PI / 180f))), Vector3.zero, num * num * num);
				}
			}
			if (frames == 10)
			{
				UnityEngine.Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_great_shine");
				UnityEngine.Object.FindObjectOfType<SOUL>().CreateSOUL(Color.red, false, true);
				UnityEngine.Object.FindObjectOfType<SOUL>().Emanate(false);
				UnityEngine.Object.FindObjectOfType<PartyPanels>().DeactivateManualManipulation();
				UnityEngine.Object.FindObjectOfType<PartyPanels>().SetTargets(true, false, false);
			}
			if (frames == 15)
			{
				UnityEngine.Object.FindObjectOfType<Toriel>().SetFace("gasp");
			}
			if (frames == 35)
			{
				UnityEngine.Object.FindObjectOfType<Toriel>().Chat(GetStringArray("frisk_defends"), "RightWide", "snd_txttor", new Vector2(178f, 141f), true, 0);
				state = 4;
			}
		}
		else if (state == 4)
		{
			if (!UnityEngine.Object.FindObjectOfType<Toriel>().GetTextBubble())
			{
				UnityEngine.Object.FindObjectOfType<BattleManager>().PlayMusic("music/mus_boss1", 1f);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else if (UnityEngine.Object.FindObjectOfType<Toriel>().GetTextBubble().GetCurrentStringNum() == 3 && frames != 69)
			{
				frames = 69;
				UnityEngine.Object.FindObjectOfType<Toriel>().SetFace("main");
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		basePos = UnityEngine.Object.FindObjectOfType<SOUL>().transform.position;
		UnityEngine.Object.FindObjectOfType<PartyPanels>().ActivateManualManipulation();
		UnityEngine.Object.FindObjectOfType<PartyPanels>().transform.Find("KrisStats").transform.localPosition = new Vector3(-420f, -159f);
		UnityEngine.Object.FindObjectOfType<PartyPanels>().transform.Find("SusieStats").transform.localPosition = new Vector3(0f, -159f);
	}
}
