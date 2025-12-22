using System;
using UnityEngine;
using UnityEngine.UI;

public class PaulaDeathAttack : AttackBase
{
	private bool bitchass;

	private Vector2 position = new Vector2(182f, 127f);

	private string bubbleType = "RightWide";

	private int shakeRate = 6;

	private SpriteRenderer screenCrack;

	private SOUL paulaNessSoul;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Util.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
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
			if (frames == 20)
			{
				Util.FindObjectOfType<Paula>().Chat(new string[6] { "Y...^10 you...", "...", "No...^10 NO...!", "I...^10 WON'T GIVE \nUP!!!", "I WON'T DIE!!!", "I can't..." }, bubbleType, "snd_txtpau", position, canSkip: false, 2);
				Util.FindObjectOfType<Paula>().GetTextBubble().gameObject.AddComponent<ShakingText>().StartShake(0, "speechbubble");
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if ((bool)Util.FindObjectOfType<TextBubble>())
			{
				if (Util.FindObjectOfType<TextBubble>().GetCurrentStringNum() == 2 && !bitchass)
				{
					Util.FindObjectOfType<PartyPanels>().SetTargets(kris: true, susie: true, noelle: true);
					bb.StartMovement(new Vector2(165f, 140f), new Vector2(0f, -1.66f));
					Util.FindObjectOfType<SOUL>().transform.position = new Vector3(-0.055f, -1.63f);
					Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = true;
					bitchass = true;
					Util.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_kill_1");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PaulaTarget")).GetComponent<PaulaMeleeTarget>().Activate(5, hard: true);
				screenCrack = new GameObject("ScreenCrack").AddComponent<SpriteRenderer>();
				screenCrack.sortingOrder = 1000;
			}
			float num = 0f;
			if (UnityEngine.Random.Range(0, shakeRate) == 0)
			{
				num = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
			}
			Util.FindObjectOfType<Paula>().GetEnemyObject().transform.parent.position = new Vector3(num / 48f, 0f);
			if (GameManager.GetOptions().lowGraphics.value == 0)
			{
				Util.FindObjectOfType<ConfigureBackground>().opacity = 1f - (float)frames / 360f;
			}
			else
			{
				BattleBGPiece[] array = Util.FindObjectsOfType<BattleBGPiece>();
				foreach (BattleBGPiece obj in array)
				{
					Color color = obj.GetComponent<SpriteRenderer>().color;
					obj.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1f - (float)frames / 360f);
				}
			}
			if (frames % 90 == 1 && frames > 90)
			{
				int num2 = (frames - 90) / 90;
				if (num2 < 4)
				{
					Util.FindObjectOfType<BattleCamera>().HurtShake();
					Util.GameManager().PlayGlobalSFX("sounds/snd_crack");
					screenCrack.sprite = Resources.Load<Sprite>("ui/spr_screen_crack_" + num2);
				}
				switch (num2)
				{
				case 0:
					Util.FindObjectOfType<PaulaMeleeTarget>().SetToImperfect();
					break;
				case 1:
					shakeRate = 4;
					Util.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_kill_2");
					break;
				case 2:
					shakeRate = 1;
					Util.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_kill_3");
					break;
				case 4:
					Util.GameManager().PlayGlobalSFX("sounds/snd_nessdie");
					Util.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_kill_4");
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyBlood"), Util.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").position + new Vector3(0f, 0.2f), Quaternion.identity).GetComponent<SpriteRenderer>().sortingOrder = 18;
					Util.FindObjectOfType<Paula>().GetEnemyObject().transform.parent.position = Vector3.zero;
					state = 2;
					frames = 0;
					Util.FindObjectOfType<PaulaMeleeTarget>().SetToDestroy();
					break;
				}
			}
		}
		else
		{
			if (state != 2)
			{
				return;
			}
			frames++;
			if (frames >= 90 && frames <= 150)
			{
				screenCrack.color = new Color(1f, 1f, 1f, 1f - (float)(frames - 90) / 60f);
				BattleButton[] array2 = Util.FindObjectsOfType<BattleButton>();
				foreach (BattleButton battleButton in array2)
				{
					battleButton.GetComponent<SpriteRenderer>().color = new Color(battleButton.GetComponent<SpriteRenderer>().color.r, battleButton.GetComponent<SpriteRenderer>().color.g, battleButton.GetComponent<SpriteRenderer>().color.b, 1f - (float)(frames - 90) / 60f);
				}
			}
			if (frames == 90)
			{
				Util.FindObjectOfType<SOUL>().SetControllable(boo: false);
				Util.FindObjectOfType<TPBar>().Disable();
				Image[] componentsInChildren = Util.FindObjectOfType<TPBar>().GetComponentsInChildren<Image>();
				Image[] componentsInChildren2 = Util.FindObjectOfType<PartyPanels>().GetComponentsInChildren<Image>();
				Image[] array3 = new Image[componentsInChildren.Length + componentsInChildren2.Length];
				Array.Copy(componentsInChildren, array3, componentsInChildren.Length);
				Array.Copy(componentsInChildren2, 0, array3, componentsInChildren.Length, componentsInChildren2.Length);
				Text[] componentsInChildren3 = Util.FindObjectOfType<TPBar>().GetComponentsInChildren<Text>();
				Text[] componentsInChildren4 = Util.FindObjectOfType<PartyPanels>().GetComponentsInChildren<Text>();
				Text[] array4 = new Text[componentsInChildren3.Length + componentsInChildren4.Length];
				Array.Copy(componentsInChildren3, array4, componentsInChildren3.Length);
				Array.Copy(componentsInChildren4, 0, array4, componentsInChildren3.Length, componentsInChildren4.Length);
				Image[] array5 = array3;
				for (int i = 0; i < array5.Length; i++)
				{
					array5[i].enabled = false;
				}
				Text[] array6 = array4;
				for (int i = 0; i < array6.Length; i++)
				{
					array6[i].enabled = false;
				}
				SpriteRenderer[] componentsInChildren5 = bb.GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren5.Length; i++)
				{
					componentsInChildren5[i].enabled = false;
				}
				paulaNessSoul = new GameObject("SOUL").AddComponent<SOUL>();
				paulaNessSoul.GetComponent<SOUL>().CreateSOUL(new Color(1f, 0f, 0f), monster: false, player: false);
			}
			if (frames >= 90 && frames <= 180)
			{
				paulaNessSoul.transform.position = new Vector3(0f, 2.5f) + new Vector3(UnityEngine.Random.Range(-2, 3), UnityEngine.Random.Range(-2, 3)) / 48f;
				paulaNessSoul.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, (float)(frames - 90) / 90f);
				if (frames == 180)
				{
					paulaNessSoul.Break();
				}
			}
			if (frames == 360)
			{
				Util.GameManager().AddEXP(1000);
				Util.FindObjectOfType<PartyPanels>().UpdateHP(Util.GameManager().GetHPArray());
				Util.FindObjectOfType<BattleManager>().FadeEndBattle(1);
			}
		}
	}
}
