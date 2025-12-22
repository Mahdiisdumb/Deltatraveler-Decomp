using UnityEngine;

public class SansIceDeathAttack : AttackBase
{
	private Sans sans;

	protected override void Awake()
	{
		base.Awake();
		bbPos = new Vector2(100f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Util.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Util.FindObjectOfType<PartyPanels>().RaiseHeads(kris: false, susie: false, noelle: false);
		Util.FindObjectOfType<PartyPanels>().transform.position = new Vector3(100f, 0f);
		Util.FindObjectOfType<TPBar>().transform.localPosition = new Vector3(-500f, 0f);
		Util.FindObjectOfType<DescriptionBox>().Vanish();
		soulPos.x = 100f;
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
		sans = Util.FindObjectOfType<Sans>();
		Util.FindObjectOfType<BattleManager>().PlayMusic("music/mus_f_wind", 1f, hasIntro: true);
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
			if (frames <= 60)
			{
				BattleButton[] array = Util.FindObjectsOfType<BattleButton>();
				foreach (BattleButton battleButton in array)
				{
					battleButton.GetComponent<SpriteRenderer>().color = new Color(battleButton.GetComponent<SpriteRenderer>().color.r, battleButton.GetComponent<SpriteRenderer>().color.g, battleButton.GetComponent<SpriteRenderer>().color.b, 1f - (float)frames / 60f);
				}
			}
			_ = 40 - frames / 2;
			_ = 4;
			if (Random.Range(0, 4) == 0)
			{
				sans.transform.position = new Vector3((Random.Range(0, 2) != 0) ? 1 : (-1), 0f) / 24f;
			}
			else
			{
				sans.transform.position = Vector3.zero;
			}
			if (frames == 60)
			{
				sans.GetEnemyObject().GetComponent<AudioSource>().Play();
				sans.SetFace("cold");
				sans.Chat(new string[4] { "it's...^15 so cold...", "the cruelty...^15\nof fate...", "h^05o^05w^05.^05.^05.^20\n^05c^05-^15c^05o^05u^05l^05d^05.^05.^05.", ".^15.^15.^15.^15." }, "snd_txtsans", Util.GameManager().IsTestMode(), 2);
				sans.GetTextBubble().GetComponent<ShakingText>().StartShake(0, "sans");
				state = 1;
				frames = 0;
			}
		}
		else
		{
			if (state != 1)
			{
				return;
			}
			if ((bool)sans.GetTextBubble())
			{
				if (sans.GetTextBubble().GetCurrentStringNum() == 4)
				{
					sans.SetFace("closed_unhappy");
				}
				if (Random.Range(0, 4) == 0)
				{
					sans.transform.position = new Vector3((Random.Range(0, 2) != 0) ? 1 : (-1), 0f) / 24f;
				}
				else
				{
					sans.transform.position = Vector3.zero;
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				sans.GetEnemyObject().GetComponent<AudioSource>().Stop();
				Util.FindObjectOfType<BattleManager>().GetComponent<MusicPlayer>().FadeOut(2f);
				sans.FreezeDeath();
				sans.transform.position = Vector3.zero;
				Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyFreeze"), sans.GetEnemyObject().transform.Find("mainbody").position, Quaternion.identity).GetComponent<EnemyFreeze>().SetSprite(sans.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite);
			}
			if (frames == 90)
			{
				Util.GameManager().AddEXP(sans.GetFinalEXP() + 50);
				Util.GameManager().AddGold(sans.GetGold());
				Util.GameManager().SetFlag(282, 1);
				Util.FindObjectOfType<BattleManager>().FadeEndBattle(1);
			}
		}
	}
}
