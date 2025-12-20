using UnityEngine;

public class FloweyFirstAttack : AttackBase
{
	private Vector3 krisPanelPos;

	private Vector3 susiePanelPos;

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
			if (frames == 10)
			{
				Util.FindObjectOfType<FloweyCutscene>().Chat(new string[2] { "See that heart?\n^10That is your SOUL,^10 \nthe very culmination \nof your being!", "Your SOUL starts off \nweak,^10 but can " }, "RightWide", "snd_txtflw", new Vector2(163f, 56f), canSkip: true, 0);
				frames = 0;
				state = 1;
			}
		}
		if (state == 1)
		{
			if (!Util.FindObjectOfType<FloweyCutscene>().GetTextBubble())
			{
				Util.FindObjectOfType<FloweyCutscene>().GetFakeSusie().sprite = Resources.Load<Sprite>("battle/enemies/FloweyCutscene/spr_b_susie_angry_1");
				Util.FindObjectOfType<FloweyCutscene>().Chat(new string[1] { "WHY THE HELL AM \nI ON YOUR SIDE?!?" }, "RightWide", "snd_txtsus", new Vector2(42f, 166f), canSkip: true, 0);
				Util.FindObjectOfType<FloweyCutscene>().GetBody().sprite = Resources.Load<Sprite>("battle/enemies/FloweyCutscene/spr_b_flowey_annoyed");
				state = 2;
			}
			else if (Util.FindObjectOfType<FloweyCutscene>().GetTextBubble().GetCurrentStringNum() == 2 && !Util.FindObjectOfType<FloweyCutscene>().GetTextBubble().IsPlaying())
			{
				Object.Destroy(Util.FindObjectOfType<FloweyCutscene>().GetTextBubble().gameObject);
			}
			else if (Util.FindObjectOfType<FloweyCutscene>().GetTextBubble().GetCurrentStringNum() == 1 && frames == 15)
			{
				Util.FindObjectOfType<FloweyCutscene>().GetFakeSusie().sprite = Resources.Load<Sprite>("battle/enemies/FloweyCutscene/spr_b_susie_confused");
				frames++;
			}
			else if (Util.FindObjectOfType<FloweyCutscene>().GetTextBubble().GetCurrentStringNum() == 2 && frames == 16)
			{
				Util.FindObjectOfType<FloweyCutscene>().GetFakeSusie().sprite = Resources.Load<Sprite>("battle/enemies/FloweyCutscene/spr_b_susie_angry_0");
			}
			else if (frames < 15)
			{
				frames++;
			}
		}
		if (state == 2 && !Util.FindObjectOfType<FloweyCutscene>().GetTextBubble())
		{
			Util.FindObjectOfType<FloweyCutscene>().Chat(new string[2] { "Because you're a \nMONSTER.", "The human has to \n<color=#FF0000FF>FIGHT</color> the monsters,^10 \nso " }, "RightWide", "snd_txtflw", new Vector2(163f, 56f), canSkip: true, 0);
			state = 3;
		}
		if (state == 3)
		{
			if (!Util.FindObjectOfType<FloweyCutscene>().GetTextBubble())
			{
				Util.FindObjectOfType<FloweyCutscene>().Chat(new string[1] { "LIKE HELL AM I \nGONNA FIGHT WITH YOU!!" }, "RightWide", "snd_txtsus", new Vector2(42f, 166f), canSkip: true, 0);
				frames = 0;
				state = 4;
			}
			else if (Util.FindObjectOfType<FloweyCutscene>().GetTextBubble().GetCurrentStringNum() == 2 && !Util.FindObjectOfType<FloweyCutscene>().GetTextBubble().IsPlaying())
			{
				Object.Destroy(Util.FindObjectOfType<FloweyCutscene>().GetTextBubble().gameObject);
			}
		}
		if (state == 4 && !Util.FindObjectOfType<FloweyCutscene>().GetTextBubble())
		{
			frames++;
			if (frames == 1)
			{
				Util.FindObjectOfType<BattleManager>().StopMusic();
			}
			if (frames <= 15)
			{
				Util.FindObjectOfType<FloweyCutscene>().GetFakeSusie().transform.localPosition = Vector3.Lerp(new Vector3(-2.9f, 1.14f), new Vector3(8.17f, 1.14f), (float)frames / 15f);
				if (frames == 15)
				{
					Object.Destroy(Util.FindObjectOfType<FloweyCutscene>().GetFakeSusie().gameObject);
				}
			}
			else if (frames < 38)
			{
				if (frames == 16)
				{
					Util.GameManager().PlayGlobalSFX("sounds/snd_drive");
				}
				Util.FindObjectOfType<PartyPanels>().transform.Find("Party1Stats").transform.localPosition = Vector3.Lerp(new Vector3(420f, -159f), new Vector3(0f, -159f), (float)(frames - 15) / 23f);
				if (frames == 35)
				{
					Util.FindObjectOfType<FloweyCutscene>().GetBody().sprite = Resources.Load<Sprite>("battle/enemies/FloweyCutscene/spr_b_flowey_poker");
					Transform obj = Object.Instantiate(Resources.Load<GameObject>("vfx/RealisticExplosion")).transform;
					obj.position = new Vector3(0f, -3.65f);
					obj.localScale = new Vector3(10f, 2f, 1f);
				}
			}
			if (frames == 38)
			{
				Util.FindObjectOfType<PartyPanels>().transform.Find("Party0Stats").transform.localPosition = krisPanelPos;
				Util.FindObjectOfType<PartyPanels>().transform.Find("Party1Stats").transform.localPosition = susiePanelPos;
				Object.Destroy(GameObject.Find("HPUT"));
			}
			if (frames == 70)
			{
				Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
				Util.FindObjectOfType<PartyPanels>().DeactivateManualManipulation();
				Util.FindObjectOfType<PartyPanels>().DeactivateTargets();
				bb.ResetSize();
				state = 5;
			}
		}
		if (state == 5 && !bb.IsPlaying())
		{
			Util.FindObjectOfType<BattleManager>().StartText("su_wtf`* NOW GET OUTTA HERE!!!", new Vector2(-4f, -134f), "snd_txtsus");
			if (UTInput.GetButton("X") || UTInput.GetButton("C"))
			{
				Util.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
			}
			state = 6;
		}
		if (state == 6)
		{
			if ((UTInput.GetButtonDown("X") || UTInput.GetButton("C")) && Util.FindObjectOfType<BattleManager>().GetBattleText().IsPlaying())
			{
				Util.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
			}
			if ((UTInput.GetButtonDown("Z") || UTInput.GetButton("C")) && !Util.FindObjectOfType<BattleManager>().GetBattleText().IsPlaying())
			{
				Util.FindObjectOfType<BattleManager>().ResetText();
				state = 7;
				frames = 0;
				Object.Instantiate(Resources.Load<GameObject>("battle/RudeBuster")).GetComponent<RudeBusterEffect>().AssignEnemy(Util.FindObjectOfType<FloweyCutscene>());
				Util.FindObjectOfType<TPBar>().RemoveTP(50);
			}
		}
		if (state == 7)
		{
			frames++;
			if (frames == 40)
			{
				Util.FindObjectOfType<BattleManager>().FadeEndBattle();
			}
		}
		if (state > 5)
		{
			Util.FindObjectOfType<TPBar>().transform.localPosition = Vector3.Lerp(Util.FindObjectOfType<TPBar>().transform.localPosition, new Vector3(-288f, 122f), 0.4f);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		BattleButton[] array = Util.FindObjectsOfType<BattleButton>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<SpriteRenderer>().enabled = false;
		}
		Util.FindObjectOfType<TPBar>().transform.localPosition = new Vector3(-488f, 122f);
		Util.FindObjectOfType<TPBar>().AddTP(50);
		krisPanelPos = new Vector3(-130f, -159f);
		susiePanelPos = new Vector3(130f, -159f);
		Util.FindObjectOfType<PartyPanels>().ActivateManualManipulation();
		Util.FindObjectOfType<PartyPanels>().transform.Find("Party0Stats").transform.localPosition = new Vector3(0f, -500f);
		Util.FindObjectOfType<PartyPanels>().transform.Find("Party1Stats").transform.localPosition = new Vector3(420f, -159f);
		GameObject.Find("HPUT").transform.localPosition = Vector3.zero;
	}
}
