using UnityEngine;
using UnityEngine.UI;

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
				Object.FindObjectOfType<FloweyCutscene>().Chat(new string[2] { "See that heart?\n^10That is your SOUL,^10 \nthe very culmination \nof your being!", "Your SOUL starts off \nweak,^10 but can " }, "RightWide", "snd_txtflw", new Vector2(163f, 56f), true, 0);
				frames = 0;
				state = 1;
			}
		}
		if (state == 1)
		{
			if (!Object.FindObjectOfType<FloweyCutscene>().GetTextBubble())
			{
				Object.FindObjectOfType<FloweyCutscene>().GetFakeSusie().sprite = Resources.Load<Sprite>("battle/enemies/FloweyCutscene/spr_b_susie_angry_1");
				Object.FindObjectOfType<FloweyCutscene>().Chat(new string[1] { "WHY THE HELL AM \nI ON YOUR SIDE?!?" }, "RightWide", "snd_txtsus", new Vector2(42f, 166f), true, 0);
				Object.FindObjectOfType<FloweyCutscene>().GetBody().sprite = Resources.Load<Sprite>("battle/enemies/FloweyCutscene/spr_b_flowey_annoyed");
				state = 2;
			}
			else if (Object.FindObjectOfType<FloweyCutscene>().GetTextBubble().GetCurrentStringNum() == 2 && !Object.FindObjectOfType<FloweyCutscene>().GetTextBubble().IsPlaying())
			{
				Object.Destroy(Object.FindObjectOfType<FloweyCutscene>().GetTextBubble().gameObject);
			}
			else if (Object.FindObjectOfType<FloweyCutscene>().GetTextBubble().GetCurrentStringNum() == 1 && frames == 15)
			{
				Object.FindObjectOfType<FloweyCutscene>().GetFakeSusie().sprite = Resources.Load<Sprite>("battle/enemies/FloweyCutscene/spr_b_susie_confused");
				frames++;
			}
			else if (Object.FindObjectOfType<FloweyCutscene>().GetTextBubble().GetCurrentStringNum() == 2 && frames == 16)
			{
				Object.FindObjectOfType<FloweyCutscene>().GetFakeSusie().sprite = Resources.Load<Sprite>("battle/enemies/FloweyCutscene/spr_b_susie_angry_0");
			}
			else if (frames < 15)
			{
				frames++;
			}
		}
		if (state == 2 && !Object.FindObjectOfType<FloweyCutscene>().GetTextBubble())
		{
			Object.FindObjectOfType<FloweyCutscene>().Chat(new string[2] { "Because you're a \nMONSTER.", "The human has to \n<color=#FF0000FF>FIGHT</color> the monsters,^10 \nso " }, "RightWide", "snd_txtflw", new Vector2(163f, 56f), true, 0);
			state = 3;
		}
		if (state == 3)
		{
			if (!Object.FindObjectOfType<FloweyCutscene>().GetTextBubble())
			{
				Object.FindObjectOfType<FloweyCutscene>().Chat(new string[1] { "LIKE HELL AM I \nGONNA FIGHT WITH YOU!!" }, "RightWide", "snd_txtsus", new Vector2(42f, 166f), true, 0);
				frames = 0;
				state = 4;
			}
			else if (Object.FindObjectOfType<FloweyCutscene>().GetTextBubble().GetCurrentStringNum() == 2 && !Object.FindObjectOfType<FloweyCutscene>().GetTextBubble().IsPlaying())
			{
				Object.Destroy(Object.FindObjectOfType<FloweyCutscene>().GetTextBubble().gameObject);
			}
		}
		if (state == 4 && !Object.FindObjectOfType<FloweyCutscene>().GetTextBubble())
		{
			frames++;
			if (frames == 1)
			{
				Object.FindObjectOfType<BattleManager>().StopMusic();
			}
			if (frames <= 15)
			{
				Object.FindObjectOfType<FloweyCutscene>().GetFakeSusie().transform.localPosition = Vector3.Lerp(new Vector3(-2.9f, 1.14f), new Vector3(8.17f, 1.14f), (float)frames / 15f);
				if (frames == 15)
				{
					Object.Destroy(Object.FindObjectOfType<FloweyCutscene>().GetFakeSusie().gameObject);
				}
			}
			else if (frames < 38)
			{
				if (frames == 16)
				{
					Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_drive");
				}
				Object.FindObjectOfType<PartyPanels>().transform.Find("SusieStats").transform.localPosition = Vector3.Lerp(new Vector3(420f, -159f), new Vector3(0f, -159f), (float)(frames - 15) / 23f);
				if (frames == 35)
				{
					Object.FindObjectOfType<FloweyCutscene>().GetBody().sprite = Resources.Load<Sprite>("battle/enemies/FloweyCutscene/spr_b_flowey_poker");
					Transform obj = Object.Instantiate(Resources.Load<GameObject>("vfx/RealisticExplosion")).transform;
					obj.position = new Vector3(0f, -3.65f);
					obj.localScale = new Vector3(10f, 2f, 1f);
				}
			}
			if (frames == 38)
			{
				Object.FindObjectOfType<PartyPanels>().transform.Find("KrisStats").transform.localPosition = krisPanelPos;
				Object.FindObjectOfType<PartyPanels>().transform.Find("SusieStats").transform.localPosition = susiePanelPos;
				Object.Destroy(GameObject.Find("HPFake"));
			}
			if (frames == 70)
			{
				Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
				Object.FindObjectOfType<PartyPanels>().DeactivateManualManipulation();
				Object.FindObjectOfType<PartyPanels>().DeactivateTargets();
				bb.ResetSize();
				state = 5;
			}
		}
		if (state == 5 && !bb.IsPlaying())
		{
			Object.FindObjectOfType<BattleManager>().StartText("su_wtf`* NOW GET OUTTA HERE!!!", new Vector2(-4f, -134f), "snd_txtsus");
			if (UTInput.GetButton("X") || UTInput.GetButton("C"))
			{
				Object.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
			}
			state = 6;
		}
		if (state == 6)
		{
			if ((UTInput.GetButtonDown("X") || UTInput.GetButton("C")) && Object.FindObjectOfType<BattleManager>().GetBattleText().IsPlaying())
			{
				Object.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
			}
			if ((UTInput.GetButtonDown("Z") || UTInput.GetButton("C")) && !Object.FindObjectOfType<BattleManager>().GetBattleText().IsPlaying())
			{
				Object.FindObjectOfType<BattleManager>().ResetText();
				state = 7;
				frames = 0;
				Object.Instantiate(Resources.Load<GameObject>("battle/RudeBuster")).GetComponent<RudeBusterEffect>().AssignEnemy(Object.FindObjectOfType<FloweyCutscene>());
				Object.FindObjectOfType<TPBar>().RemoveTP(50);
			}
		}
		if (state == 7)
		{
			frames++;
			if (frames == 40)
			{
				Object.FindObjectOfType<BattleManager>().FadeEndBattle();
			}
		}
		if (state > 5)
		{
			Object.FindObjectOfType<TPBar>().transform.localPosition = Vector3.Lerp(Object.FindObjectOfType<TPBar>().transform.localPosition, new Vector3(-278f, 108f), 0.4f);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		BattleButton[] array = Object.FindObjectsOfType<BattleButton>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<SpriteRenderer>().enabled = false;
		}
		Object.FindObjectOfType<TPBar>().transform.localPosition = new Vector3(-478f, 108f);
		Object.FindObjectOfType<TPBar>().AddTP(50);
		krisPanelPos = new Vector3(-130f, -159f);
		susiePanelPos = new Vector3(130f, -159f);
		Object.FindObjectOfType<PartyPanels>().ActivateManualManipulation();
		Object.FindObjectOfType<PartyPanels>().transform.Find("KrisStats").transform.localPosition = new Vector3(0f, -500f);
		Object.FindObjectOfType<PartyPanels>().transform.Find("SusieStats").transform.localPosition = new Vector3(420f, -159f);
		GameObject.Find("HPFake").transform.localPosition = Vector3.zero;
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(108) == 1)
		{
			GameObject.Find("HPFake").transform.Find("NameText").GetComponent<Text>().text = GameObject.Find("HPFake").transform.Find("NameText").GetComponent<Text>().text.Replace("Kris", "Frisk");
		}
	}
}
