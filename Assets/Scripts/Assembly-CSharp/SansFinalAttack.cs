using System;
using UnityEngine;

public class SansFinalAttack : AttackBase
{
	private readonly float SPAWN_DISTANCE = 10.36f;

	private readonly float AIM_DISTANCE = 6f;

	private Sans sans;

	private GameObject blasterPrefab;

	private Transform singularPlatform;

	private Transform formation2;

	private int slamFrames = 60;

	private int slamMaxFrames = 60;

	private bool easier;

	private int curBlastColor = -1;

	private int blastColorCount;

	private int minBlastColorCount = 1;

	private int maxBlastColorCount = 3;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(185f, 140f);
		Util.FindObjectOfType<SOUL>().ChangeSOULMode(0);
		blasterPrefab = Resources.Load<GameObject>("battle/attacks/bullets/GasterBlaster");
		singularPlatform = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/Platform"), new Vector3(0f, -6.72f), Quaternion.Euler(0f, 0f, 90f), base.transform).transform;
		singularPlatform.GetComponent<Platform>().ChangeSize(130);
		sans = Util.FindObjectOfType<Sans>();
		formation2 = new GameObject("Formation2").transform;
		formation2.parent = base.transform;
		GameObject original = Resources.Load<GameObject>("battle/attacks/bullets/sans/Bone");
		for (int i = 0; i < 50; i++)
		{
			float num = 6f + (float)i * 0.5f;
			float num2 = (0f - Mathf.Cos((float)i * 14.4f * (MathF.PI / 180f))) * 0.8f - 0.3f;
			UnityEngine.Object.Instantiate(original, new Vector3(0f - num, 2.64f + num2), Quaternion.identity, formation2).GetComponent<BoneBullet>().ChangeHeight(50f);
			UnityEngine.Object.Instantiate(original, new Vector3(0f - num, -5.14f + num2), Quaternion.identity, formation2).GetComponent<BoneBullet>().ChangeHeight(50f);
		}
		easier = !Util.FindObjectOfType<KarmaHandler>();
		if (easier)
		{
			minBlastColorCount = 2;
			maxBlastColorCount = 4;
		}
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
			if (frames <= 30)
			{
				for (int i = 0; i < 3; i++)
				{
					if (frames - i * 2 == 1)
					{
						UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-8.27f, 6.68f), Quaternion.identity).GetComponent<GasterBlaster>().Activate(1, 2, 0f, new Vector2(-1.5f + (float)i * 1.25f, 2.68f), 40 - frames, inSpearAttack: false, 5);
					}
					else if (frames - i * 2 == 7)
					{
						UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-8.27f, 6.68f), Quaternion.identity).GetComponent<GasterBlaster>().Activate(1, 2, 90f, new Vector2(-4f, 0.31f - (float)i * 1.25f), 40 - frames, inSpearAttack: false, 5);
					}
				}
			}
			else if (frames <= 90)
			{
				for (int j = 0; j < 3; j++)
				{
					if (frames - j * 2 == 61)
					{
						UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-8.27f, 6.68f), Quaternion.identity).GetComponent<GasterBlaster>().Activate(1, 2, 0f, new Vector2(-0.875f + (float)j * 1.25f, 2.68f), 100 - frames, inSpearAttack: false, 5);
					}
					else if (frames - j * 2 == 67)
					{
						UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-8.27f, 6.68f), Quaternion.identity).GetComponent<GasterBlaster>().Activate(1, 2, 90f, new Vector2(-4f, -0.315f - (float)j * 1.25f), 100 - frames, inSpearAttack: false, 5);
					}
				}
			}
			else if (frames <= 165)
			{
				for (int k = 0; k < 6; k++)
				{
					if (frames - k * 4 == 145)
					{
						UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-8.27f, 6.68f), Quaternion.identity).GetComponent<GasterBlaster>().Activate(1, 2, 0f, new Vector2(3.37f - (float)k * 0.5f, 2.68f), 190 - frames);
					}
				}
			}
			if (frames == 205)
			{
				UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-8.27f, 6.68f), Quaternion.identity).GetComponent<GasterBlaster>().Activate(5, 5, 90f, new Vector2(-4.49f, -1.18f), 30);
			}
			if (frames == 130)
			{
				bb.StartMovement(new Vector2(375f, 185f));
			}
			if (frames == 135)
			{
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.right, 50f);
				Util.FindObjectOfType<SOUL>().ChangeSOULMode(1);
			}
			if (frames <= 180)
			{
				singularPlatform.position = new Vector3(0.56f, Mathf.Lerp(-6.72f, -1.18f, (float)(frames - 140) / 40f));
			}
			if (frames == 245)
			{
				UnityEngine.Object.Destroy(singularPlatform.gameObject);
			}
			if (frames == 270)
			{
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.left, 50f);
			}
			if (frames == 280)
			{
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.right, 24f);
			}
			if (frames == 288)
			{
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.down, 50f);
			}
			if (frames == 295)
			{
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.right, 0f);
				sans.PlaySFX("sounds/snd_bigcut");
			}
			if (frames >= 300 && frames < 439)
			{
				formation2.position += new Vector3(0.25f, 0f);
			}
			if (frames == 435)
			{
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.left, 0f);
				sans.PlaySFX("sounds/snd_bigcut");
				Util.FindObjectOfType<SOUL>().ChangeSOULMode(0, makeSound: true);
			}
			if (frames == 450)
			{
				sans.ResetBreatheAnimation();
			}
			if (frames >= 439)
			{
				formation2.position -= new Vector3(1f / 3f, 0f);
			}
			if (frames == 530)
			{
				sans.SetSweat(2);
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.up, 50f);
				Util.FindObjectOfType<SOUL>().ChangeSOULMode(1);
			}
			if (frames == 538)
			{
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.down, 50f);
			}
			if (frames == 545)
			{
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.up, 50f);
			}
			if (frames == 552)
			{
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.down, 50f);
			}
			if (frames == 560)
			{
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.left, 50f);
			}
			if (frames == 568)
			{
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.right, 24f);
			}
			if (frames == 576)
			{
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.down, 50f);
				bb.StartMovement(new Vector2(185f, 185f));
			}
			if (frames == 584)
			{
				Util.FindObjectOfType<SOUL>().ChangeSOULMode(0);
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.up, 0f);
			}
			if (frames >= 585 && frames % 15 == 0 && frames / 15 <= 45)
			{
				int num = ((frames % 30 != 0) ? 45 : 0);
				int num2 = UnityEngine.Random.Range(0, 2) + 1;
				if (curBlastColor == -1)
				{
					curBlastColor = num2;
				}
				else if (blastColorCount < minBlastColorCount)
				{
					num2 = curBlastColor;
				}
				else if (curBlastColor != num2)
				{
					curBlastColor = num2;
					blastColorCount = 0;
				}
				else if (curBlastColor == num2 && blastColorCount >= maxBlastColorCount)
				{
					curBlastColor = ((curBlastColor != 1) ? 1 : 2);
					num2 = curBlastColor;
					blastColorCount = 0;
				}
				blastColorCount++;
				for (int l = 0; l < 4; l++)
				{
					num += 90;
					float num3 = Mathf.Sin((float)num * (MathF.PI / 180f));
					float num4 = Mathf.Cos((float)num * (MathF.PI / 180f));
					GasterBlaster component = UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(num3 * SPAWN_DISTANCE, -1.2f + num4 * SPAWN_DISTANCE), Quaternion.Euler(0f, 0f, -num)).GetComponent<GasterBlaster>();
					component.ChangeType(num2);
					if (l <= 1)
					{
						component.Mute();
					}
					component.Activate(7, 7, -num, new Vector3(num3 * AIM_DISTANCE, -1.2f + num4 * AIM_DISTANCE), 10);
				}
			}
			if (frames == 705)
			{
				sans.ResetBreatheAnimation();
			}
			if (frames >= 705 && frames % 2 == 1 && frames <= 915)
			{
				float num5 = (frames - 705) * (easier ? 2 : 3);
				float num6 = Mathf.Sin(num5 * (MathF.PI / 180f));
				float num7 = Mathf.Cos(num5 * (MathF.PI / 180f));
				UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(num6 * SPAWN_DISTANCE, -1.2f + num7 * SPAWN_DISTANCE), Quaternion.Euler(0f, 0f, 0f - num5)).GetComponent<GasterBlaster>().Activate(2, 2, 0f - num5, new Vector3(num6 * AIM_DISTANCE * 0.75f, -1.2f + num7 * AIM_DISTANCE * 0.75f), easier ? 15 : 10);
			}
			if (frames == 985)
			{
				frames = 0;
				state = 1;
				Util.FindObjectOfType<SOUL>().ChangeSOULMode(1);
			}
		}
		else if (state == 1)
		{
			frames++;
			slamFrames++;
			if (frames == 1)
			{
				sans.Chat(new string[1] { "heh." }, "snd_txtsans", canSkip: false, 1);
				sans.GetTextBubble().GetComponent<ShakingText>().StartShake(15, "sansb");
				sans.GetTextBubble().Disable();
			}
			if (frames == 30)
			{
				slamMaxFrames = 45;
			}
			if (frames == 70)
			{
				slamMaxFrames = 30;
				sans.DisableFaceControl();
				UnityEngine.Object.Destroy(sans.GetTextBubble().gameObject);
				sans.SetFace("goinginsane_empty");
				sans.Chat(new string[1] { "heheheh." }, "snd_txtsans", canSkip: false, 1);
				sans.GetTextBubble().GetComponent<ShakingText>().StartShake(8, "sansb");
				sans.GetTextBubble().Disable();
				Util.FindObjectOfType<BattleManager>().GetComponent<MusicPlayer>().FadeOut(2f);
			}
			if (frames == 100)
			{
				slamMaxFrames = 15;
			}
			if (frames == 140)
			{
				slamMaxFrames = 7;
				UnityEngine.Object.Destroy(sans.GetTextBubble().gameObject);
				sans.Chat(new string[1] { "heheheheheheHEHEHA\nHAHAHAHAHAHAHAHA!" }, "snd_txtsans", canSkip: false, 0);
				sans.GetTextBubble().GetComponent<ShakingText>().StartShake(0, "sansb");
				sans.GetTextBubble().Disable();
			}
			if (frames >= 140)
			{
				int num8 = frames / 5 % 2;
				sans.SetFace("insane_laugh_" + num8);
			}
			if (slamFrames < slamMaxFrames)
			{
				return;
			}
			if (frames >= 185)
			{
				UnityEngine.Object.Destroy(sans.GetTextBubble().gameObject);
				sans.SetFace("goinginsane_empty");
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.up, 0f);
				sans.PlaySFX("sounds/snd_spearappear");
				state = 2;
				frames = 0;
				Util.FindObjectOfType<SOUL>().ChangeSOULMode(0);
				Util.FindObjectOfType<SOUL>().transform.parent = bb.transform;
				return;
			}
			if (slamMaxFrames == 7)
			{
				for (int m = 0; m < 3; m++)
				{
					if (Util.GameManager().GetHP(m) > 1)
					{
						Util.GameManager().Damage(m, 1);
					}
				}
			}
			slamFrames = 0;
			bool num9 = UnityEngine.Random.Range(0, 2) == 0;
			Vector2 direction = ((UnityEngine.Random.Range(0, 2) == 0) ? Vector2.down : Vector2.up);
			if (!num9)
			{
				direction.x = direction.y;
				direction.y = 0f;
			}
			Util.FindObjectOfType<SansGravityManager>().Slam(direction, (slamMaxFrames > 15) ? 20 : 50);
		}
		else
		{
			if (state != 2)
			{
				return;
			}
			frames++;
			if (frames < 30)
			{
				bb.transform.position = new Vector3(0f, Mathf.Lerp(bb.transform.position.y, 2.25f, 0.2f));
				return;
			}
			float num10 = Mathf.Abs(Mathf.Cos((float)((frames - 30) * 18) * (MathF.PI / 180f)));
			int slow = 0;
			if (frames == 60)
			{
				sans.Chat(new string[1] { "GIVE ME YOUR SOUL!!!" }, "snd_txtsans", canSkip: false, 0);
				sans.GetTextBubble().GetComponent<ShakingText>().StartShake(0, "sansb");
			}
			if (frames == 150)
			{
				if ((bool)sans.GetTextBubble())
				{
					UnityEngine.Object.Destroy(sans.GetTextBubble().gameObject);
				}
				sans.Chat(new string[1] { "GIVE IT TO ME!!!" }, "snd_txtsans", canSkip: false, 0);
				sans.GetTextBubble().GetComponent<ShakingText>().StartShake(0, "sansb");
			}
			if (frames == 240)
			{
				if ((bool)sans.GetTextBubble())
				{
					UnityEngine.Object.Destroy(sans.GetTextBubble().gameObject);
				}
				sans.Chat(new string[1] { "I WON'T BE DENIED \nAGAIN!!!" }, "snd_txtsans", canSkip: false, 0);
				sans.GetTextBubble().GetComponent<ShakingText>().StartShake(0, "sansb");
			}
			if (frames >= 330)
			{
				num10 = Mathf.Abs(Mathf.Cos((float)((frames - 30) * 9) * (MathF.PI / 180f)));
			}
			if (frames == 330)
			{
				if ((bool)sans.GetTextBubble())
				{
					UnityEngine.Object.Destroy(sans.GetTextBubble().gameObject);
				}
				sans.Chat(new string[1] { "I WILL..." }, "snd_txtsans", canSkip: false, 0);
				sans.GetTextBubble().GetComponent<ShakingText>().StartShake(0, "sansb");
			}
			if (frames >= 430)
			{
				slow = 1;
				num10 = Mathf.Abs(Mathf.Cos((float)(frames - 30) * 4.5f * (MathF.PI / 180f)));
			}
			if (frames == 430)
			{
				Util.FindObjectOfType<SansBG>().FadeOut();
				sans.SetFace("losingit");
				if ((bool)sans.GetTextBubble())
				{
					UnityEngine.Object.Destroy(sans.GetTextBubble().gameObject);
				}
				sans.Chat(new string[1] { "i will..." }, "snd_txtsans", canSkip: false, 0);
				sans.GetTextBubble().GetComponent<ShakingText>().StartShake(5, "sansb");
			}
			if (frames >= 530)
			{
				slow = 2;
				num10 = Mathf.Abs(Mathf.Cos((float)(frames - 30) * 2.25f * (MathF.PI / 180f)));
			}
			if (frames == 530)
			{
				sans.SetFace("distracted");
				Util.FindObjectOfType<SOUL>().transform.parent = null;
				if ((bool)sans.GetTextBubble())
				{
					UnityEngine.Object.Destroy(sans.GetTextBubble().gameObject);
				}
				sans.Chat(new string[1] { "i...^10 i'll kill..." }, "snd_txtsans", canSkip: false, 0);
				sans.GetTextBubble().GetComponent<ShakingText>().StartShake(15, "sansb");
			}
			if (frames == 620)
			{
				if ((bool)sans.GetTextBubble())
				{
					UnityEngine.Object.Destroy(sans.GetTextBubble().gameObject);
				}
				sans.StartFinale();
			}
			if (frames == 680)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (num10 == 1f && frames < 580)
			{
				Util.FindObjectOfType<SansGravityManager>().Slam(Vector2.down, 0f, slow);
			}
			if (num10 <= 0.01f && frames < 510)
			{
				sans.PlaySFX("sounds/snd_crash");
				if (frames > 430)
				{
					Util.FindObjectOfType<BattleCamera>().BlastShake();
				}
				else
				{
					Util.FindObjectOfType<BattleCamera>().GiantBlastShake();
				}
			}
			if (frames < 510)
			{
				bb.transform.position = new Vector3(0f, Mathf.Lerp(-1.66f, 2.25f, num10));
			}
			else
			{
				bb.transform.position = new Vector3(0f, Mathf.Lerp(bb.transform.position.y, -1.66f, 0.1f));
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		bb.StartMovement(new Vector2(185f, 185f));
	}
}
