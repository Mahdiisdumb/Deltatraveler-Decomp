using UnityEngine;

public class CarpainterDeath : AttackBase
{
	private Carpainter carpainter;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Util.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Util.FindObjectOfType<PartyPanels>().RaiseHeads(kris: false, susie: false, noelle: false);
		Util.FindObjectOfType<SOUL>().ChangeSOULMode(0);
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
		carpainter = Util.FindObjectOfType<Carpainter>();
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
				Util.GameManager().PlayGlobalSFX("sounds/snd_noise");
				carpainter.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/MrCarpainter/spr_b_carpainter_die_1");
			}
			if (frames == 60)
			{
				carpainter.Chat(new string[8] { "... As I assumed...", "I guess...^15 I should \ngive this to \nsomeone,^10 at least.", "It's a bomb that \nwill blow up the \ncave seal to the \neast of town.", "It was sealed to \nkeep people away from \na grey door that \nappeared there.", "... Maybe it will \nheal your violent \nnature.", "Just...", "Please...", "Spare the children..." }, "RightWide", "snd_text", new Vector2(163f, 56f), canSkip: true, 1);
				state = 1;
				frames = 0;
			}
		}
		if (state != 1)
		{
			return;
		}
		if ((bool)carpainter.GetTextBubble())
		{
			if (carpainter.GetTextBubble().GetCurrentStringNum() >= 6 && carpainter.GetTextBubble().GetComponent<SwirlingText>().IsPlaying())
			{
				carpainter.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/MrCarpainter/spr_b_carpainter_die_2");
				carpainter.GetTextBubble().GetComponent<SwirlingText>().Stop();
				carpainter.GetTextBubble().gameObject.AddComponent<ShakingText>().StartShake(0, "speechbubble");
			}
			if (carpainter.GetTextBubble().GetCurrentStringNum() >= 6)
			{
				carpainter.GetEnemyObject().transform.position = new Vector3(0f, -0.2f) + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)) / 48f;
			}
			return;
		}
		frames++;
		if (frames < 30)
		{
			carpainter.GetEnemyObject().transform.position = new Vector3(0f, -0.2f) + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)) / 24f;
		}
		else if (frames == 30)
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_noise");
			carpainter.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/MrCarpainter/spr_b_carpainter_die_3");
			carpainter.GetEnemyObject().transform.position = new Vector3(0f, -0.2f);
			Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyBlood"), Vector3.zero, Quaternion.identity);
		}
		if (frames == 120)
		{
			Util.FindObjectOfType<BattleManager>().EndNormalFight(customMessage: false, "");
		}
	}
}
