using System;
using UnityEngine;

public class SansIntroAttack : AttackBase
{
	private GameObject blasterPrefab;

	private Sans sans;

	private Transform formation1;

	private Transform formation2;

	protected override void Awake()
	{
		base.Awake();
		sans = UnityEngine.Object.FindObjectOfType<Sans>();
		soulPos.y = -1.2f;
		bbSize = new Vector2(185f, 185f);
		bb.StartMovement(bbSize, bbPos + new Vector2(100f, 0f), true);
		UnityEngine.Object.FindObjectOfType<PartyPanels>().transform.position = new Vector3(100f, 0f);
		blasterPrefab = Resources.Load<GameObject>("battle/attacks/bullets/GasterBlaster");
		formation1 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/IntroBoneFormation1"), base.transform).transform;
		formation2 = new GameObject("Formation2").transform;
		formation2.parent = base.transform;
		GameObject original = Resources.Load<GameObject>("battle/attacks/bullets/sans/Bone");
		for (int i = 0; i < 25; i++)
		{
			float x = 3f + (float)i * 0.5f;
			float num = Mathf.Sin((float)i * 14.4f * ((float)Math.PI / 180f)) * 1.26f;
			UnityEngine.Object.Instantiate(original, new Vector3(x, 2.64f + num), Quaternion.identity, formation2).GetComponent<BoneBullet>().ChangeHeight(50f);
			UnityEngine.Object.Instantiate(original, new Vector3(x, -5.14f + num), Quaternion.identity, formation2).GetComponent<BoneBullet>().ChangeHeight(50f);
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
			if (frames == 30)
			{
				if (Util.GameManager().GetSessionFlagInt(16) == 1)
				{
					sans.Chat(new string[1] { "ready?" }, "snd_txtsans", Util.GameManager().IsTestMode(), 0);
				}
				else
				{
					sans.Chat(new string[8] { "so...", "you three are ganging \nup on me?", "some heroes you are.", "to think you ruined \nmy life...", "and came back to \nruin it again.", "heheheheh.", "you're gonna get what \nyou deserve.", "and me...?" }, "snd_txtsans", Util.GameManager().IsTestMode(), 1);
					Util.GameManager().SetSessionFlag(16, 1);
				}
				frames = 0;
				state = 1;
			}
		}
		else if (state == 1)
		{
			if ((bool)sans.GetTextBubble())
			{
				if (sans.GetTextBubble().GetCurrentStringNum() == 4)
				{
					sans.SetFace("glare");
				}
				else if (sans.GetTextBubble().GetCurrentStringNum() == 6)
				{
					sans.SetFace("closed_down");
				}
			}
			else
			{
				state = 2;
			}
		}
		if (state != 2)
		{
			return;
		}
		frames++;
		if (frames == 1)
		{
			sans.PlaySFX("sounds/snd_noise");
			UnityEngine.Object.FindObjectOfType<BattleManager>().GetBattleFade().FadeOut(0, Color.black);
			UnityEngine.Object.FindObjectOfType<BattleManager>().StopMusic();
			bb.StartMovement(bbSize, bbPos, true);
		}
		if (frames == 15)
		{
			UnityEngine.Object.FindObjectOfType<BattleManager>().GetBattleFade().FadeIn(0, Color.black);
			sans.ResetBreatheAnimation();
			sans.PlaySFX("sounds/snd_noise");
			UnityEngine.Object.FindObjectOfType<PartyPanels>().transform.position = Vector3.zero;
			UnityEngine.Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = true;
			UnityEngine.Object.FindObjectOfType<SOUL>().SetControllable(true);
			sans.SetFace("empty_down");
			sans.transform.position = new Vector3(0f, 0.979f);
		}
		if (frames == 45)
		{
			sans.Chat(new string[1] { "I'm \ngonna \nhave a \ngreat \ntime." }, "RightSmall", "", new Vector2(180f, 131f), false, 1);
			frames = 65;
		}
		if (frames == 65)
		{
			UnityEngine.Object.FindObjectOfType<BattleManager>().PlayMusic("music/mus_vsufsans", 1f, true);
		}
		if (frames == 130)
		{
			sans.GetComponent<SansGravityManager>().Slam(Vector2.right, 0f);
		}
		if (frames == 133)
		{
			UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-8.27f, 6.68f), Quaternion.identity).GetComponent<GasterBlaster>().Activate(2, 2, 90f, new Vector2(-3.75f, -1f), 20);
			GasterBlaster component = UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-8.27f, 6.68f), Quaternion.identity).GetComponent<GasterBlaster>();
			component.Mute();
			component.Activate(2, 2, 0f, new Vector2(0f, 2.9f), 20);
			UnityEngine.Object.FindObjectOfType<BattleManager>().DoSOULSparkle();
		}
		if (frames == 160)
		{
			sans.GetComponent<SansGravityManager>().Slam(Vector2.up, 0f);
		}
		if (frames == 163)
		{
			if ((bool)sans.GetTextBubble())
			{
				UnityEngine.Object.Destroy(sans.GetTextBubble().gameObject);
			}
			SansBG sansBG = UnityEngine.Object.FindObjectOfType<SansBG>();
			if ((object)sansBG != null)
			{
				sansBG.FadeIn();
			}
		}
		if (frames == 175)
		{
			UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-8.27f, 6.68f), Quaternion.identity).GetComponent<GasterBlaster>().Activate(2, 2, 90f, new Vector2(-3.79f, 0.68f), 20, false, -5);
			GasterBlaster component2 = UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-8.27f, 6.68f), Quaternion.identity).GetComponent<GasterBlaster>();
			component2.Mute();
			component2.Activate(2, 2, 0f, new Vector2(-1.34f, 2.81f), 20, false, -5);
			GasterBlaster component3 = UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-8.27f, 6.68f), Quaternion.identity).GetComponent<GasterBlaster>();
			component3.Mute();
			component3.Activate(2, 2, 180f, new Vector2(1.48f, -4.8f), 20, false, -5);
			GasterBlaster component4 = UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-8.27f, 6.68f), Quaternion.identity).GetComponent<GasterBlaster>();
			component4.Mute();
			component4.Activate(2, 2, -90f, new Vector2(4.43f, -2.83f), 20, false, -5);
		}
		if (frames == 190)
		{
			sans.ResetBreatheAnimation();
		}
		if (frames == 220)
		{
			UnityEngine.Object.FindObjectOfType<SOUL>().ChangeSOULMode(1);
			sans.GetComponent<SansGravityManager>().Slam(Vector2.right);
		}
		if (frames == 230)
		{
			sans.GetComponent<SansGravityManager>().Slam(Vector2.up);
		}
		if (frames == 250)
		{
			sans.PlaySFX("sounds/snd_bigcut");
			sans.GetComponent<SansGravityManager>().Slam(Vector2.right, 0f);
		}
		if ((bool)formation1 && frames >= 254)
		{
			formation1.position += new Vector3(1f / 6f, 0f);
			if (formation1.position.x >= 10f)
			{
				UnityEngine.Object.Destroy(formation1.gameObject);
			}
		}
		if (frames == 270)
		{
			sans.ResetBreatheAnimation();
		}
		MonoBehaviour.print(frames);
		if (frames == 320)
		{
			sans.GetComponent<SansGravityManager>().Slam(Vector2.down);
		}
		if (frames == 335)
		{
			UnityEngine.Object.FindObjectOfType<SOUL>().ChangeSOULMode(0);
			UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-8.27f, 6.68f), Quaternion.identity).GetComponent<GasterBlaster>().Activate(2, 2, -90f, new Vector2(4.43f, -2.83f), 20);
		}
		if (frames == 350)
		{
			sans.ResetBreatheAnimation();
		}
		if (frames == 370)
		{
			sans.GetComponent<SansGravityManager>().Slam(Vector2.left, 0f);
			sans.PlaySFX("sounds/snd_bigcut");
		}
		if (frames >= 374 && (bool)formation2)
		{
			formation2.position -= new Vector3(1f / 6f, 0f);
		}
		if (frames == 390)
		{
			sans.ResetBreatheAnimation();
		}
		if (frames == 480)
		{
			UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(-12f, 0f), Quaternion.Euler(0f, 0f, -90f)).GetComponent<GasterBlaster>().Activate(5, 5, 90f, new Vector2(-4.48f, -1.2f), 30);
			UnityEngine.Object.Instantiate(blasterPrefab, new Vector3(12f, 0f), Quaternion.Euler(0f, 0f, 90f)).GetComponent<GasterBlaster>().Activate(5, 5, -90f, new Vector2(4.48f, -1.2f), 30);
			GasterBlaster[] array = UnityEngine.Object.FindObjectsOfType<GasterBlaster>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetTPBuildRate(1f / 6f);
			}
		}
		if (frames == 540)
		{
			sans.transform.position = Vector3.zero;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		UnityEngine.Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
		UnityEngine.Object.FindObjectOfType<SOUL>().SetControllable(false);
		sans.SetFace("closed_down");
	}
}
