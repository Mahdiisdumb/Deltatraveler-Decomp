using UnityEngine;

public class NPCBase : Interactable
{
	[SerializeField]
	protected string[] lines;

	[SerializeField]
	private string[] sound;

	[SerializeField]
	private int[] speed;

	[SerializeField]
	private string[] portraits;

	[SerializeField]
	protected int eventType;

	[SerializeField]
	protected int eventData;

	private bool talkedTo;

	private bool doSelection;

	protected Selection sels;

	private int selection;

	[SerializeField]
	private string optionA;

	[SerializeField]
	private int optionAItem = -1;

	[SerializeField]
	protected string[] optionALines;

	[SerializeField]
	private string[] optionASound;

	[SerializeField]
	private int[] optionASpeed;

	[SerializeField]
	private string[] optionAPortraits;

	[SerializeField]
	private string optionB;

	[SerializeField]
	private int optionBItem = -1;

	[SerializeField]
	private string[] optionBLines;

	[SerializeField]
	private string[] optionBSound;

	[SerializeField]
	private int[] optionBSpeed;

	[SerializeField]
	private string[] optionBPortraits;

	[SerializeField]
	private string[] ctmLines = new string[1] { "* (You're carrying too much.)" };

	[SerializeField]
	private string[] ctmSound = new string[1] { "snd_text" };

	[SerializeField]
	private int[] ctmSpeed = new int[1];

	[SerializeField]
	private string[] ctmPortraits = new string[1];

	[SerializeField]
	protected string[] defeatStealLines;

	[SerializeField]
	private string[] defeatStealSound;

	[SerializeField]
	private int[] defeatStealSpeed;

	[SerializeField]
	private string[] defeatStealPortraits;

	[SerializeField]
	protected string[] defeatSaveLines;

	[SerializeField]
	private string[] defeatSaveSound;

	[SerializeField]
	private int[] defeatSaveSpeed;

	[SerializeField]
	private string[] defeatSavePortraits;

	[SerializeField]
	private string[] defeatKilledLines;

	[SerializeField]
	private string[] defeatKilledSound;

	[SerializeField]
	private int[] defeatKilledSpeed;

	[SerializeField]
	private string[] defeatKilledPortraits;

	[SerializeField]
	private string[] altLines;

	[SerializeField]
	private string[] altSounds;

	[SerializeField]
	private int[] altSpeed;

	[SerializeField]
	private string[] altPortraits;

	private bool doFight;

	protected Animator anim;

	protected virtual void Awake()
	{
		anim = base.transform.GetComponent<Animator>();
		doSelection = false;
		selection = -1;
		doFight = false;
		talkedTo = false;
	}

	private void Start()
	{
		if (IsFightingNPC() && (int)EndBattleHandler.GetFlagFromId(eventData) == 1)
		{
			Object.Destroy(base.gameObject);
		}
		if (eventType == 7 && (int)Util.GameManager().GetFlag(eventData) == 1)
		{
			Object.Destroy(base.gameObject);
		}
	}

	protected virtual void Update()
	{
		if (txt != null)
		{
			if (txt.GetComponent<TextBox>().IsPlaying() && anim != null)
			{
				anim.SetFloat("spdmulti", 1f);
			}
			else
			{
				if (txt.GetComponent<TextBox>().IsPlaying())
				{
					return;
				}
				if (doSelection && txt.GetComponent<TextBox>().AtLastText() && !sels.IsEnabled())
				{
					txt.GetComponent<TextBox>().EnableChoice();
					int num = 0;
					if (GameObject.Find("Player").transform.position[1] - GameObject.Find("Camera").transform.position[1] < -0.9f)
					{
						num = 310;
					}
					sels.CreateSelections(new string[1, 2] { { optionA, optionB } }, new Vector2(-116f, -283 + num), new Vector2(192f, 0f), new Vector2(-15f, 94f), "DTM-Mono", useSoul: true, makeSound: false, this, 0);
				}
				if (anim != null)
				{
					anim.SetFloat("spdmulti", 0f);
					anim.Play("Talk", 0, 0f);
				}
			}
		}
		else if (selection > -1)
		{
			doSelection = false;
			bool flag = false;
			txt = new GameObject().AddComponent<TextBox>();
			if (Util.GameManager().FirstFreeItemSpace(equipment: false) == -1 && ((selection == 0 && optionAItem > -1) || (selection == 1 && optionBItem > -1)))
			{
				txt.CreateBox(ctmLines, ctmSound, ctmSpeed, ctmPortraits);
			}
			else if (selection == 0)
			{
				if (eventType == 5 && optionAItem == -2)
				{
					doFight = true;
				}
				Util.GameManager().AddItem(optionAItem);
				if (optionAItem > -1)
				{
					flag = true;
					if (eventData > -1)
					{
						Util.GameManager().SetFlag(eventData, 1);
					}
				}
				if (optionALines != null && optionALines.Length != 0)
				{
					txt.CreateBox(optionALines, optionASound, optionASpeed, optionAPortraits);
				}
				else
				{
					Object.Destroy(txt);
					Util.GameManager().EnablePlayerMovement();
				}
			}
			else if (selection == 1)
			{
				if (eventType == 5 && optionBItem == -2)
				{
					doFight = true;
				}
				Util.GameManager().AddItem(optionBItem);
				if (optionBItem > -1)
				{
					flag = true;
					if (eventData > -1)
					{
						Util.GameManager().SetFlag(eventData, 1);
					}
				}
				if (optionBLines != null && optionBLines.Length != 0)
				{
					txt.CreateBox(optionBLines, optionBSound, optionBSpeed, optionBPortraits);
				}
				else
				{
					Object.Destroy(txt);
					Util.GameManager().EnablePlayerMovement();
				}
			}
			selection = -1;
			if (flag && eventType == 7)
			{
				Object.Destroy(base.gameObject);
			}
		}
		else if (doFight)
		{
			doFight = false;
			Util.OverworldPlayer().InitiateBattle();
		}
	}

	public override int GetEventData()
	{
		return eventData;
	}

	public bool IsFightingNPC()
	{
		if (eventType != 1 && eventType != 2)
		{
			return eventType == 5;
		}
		return true;
	}

	public override void DoInteract()
	{
		if (IsFightingNPC() && (int)EndBattleHandler.GetFlagFromId(eventData) > 1)
		{
			txt = new GameObject().AddComponent<TextBox>();
			if ((int)EndBattleHandler.GetFlagFromId(eventData) == 2)
			{
				txt.CreateBox(defeatStealLines, defeatStealSound, defeatStealSpeed, defeatStealPortraits);
			}
			else
			{
				txt.CreateBox(defeatSaveLines, defeatSaveSound, defeatSaveSpeed, defeatSavePortraits);
			}
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		}
		else if (eventType != 1 && eventType != 3)
		{
			if (eventType == 2)
			{
				doFight = true;
			}
			txt = new GameObject().AddComponent<TextBox>();
			if ((eventType == 6 || eventType == 8 || eventType == 9) && (int)EndBattleHandler.GetFlagFromId(eventData) == 1)
			{
				txt.CreateBox(defeatKilledLines, defeatKilledSound, defeatKilledSpeed, defeatKilledPortraits);
				talkedTo = true;
			}
			else if ((eventType == 6 || eventType == 8 || eventType == 9) && (int)EndBattleHandler.GetFlagFromId(eventData) == 2)
			{
				txt.CreateBox(defeatStealLines, defeatStealSound, defeatStealSpeed, defeatStealPortraits);
				talkedTo = true;
			}
			else if ((eventType == 6 || eventType == 8 || eventType == 9) && (int)EndBattleHandler.GetFlagFromId(eventData) == 3)
			{
				txt.CreateBox(defeatSaveLines, defeatSaveSound, defeatSaveSpeed, defeatSavePortraits);
				talkedTo = true;
			}
			else if ((eventType == 6 || eventType == 9) && talkedTo)
			{
				txt.CreateBox(altLines, altSounds, altSpeed, altPortraits);
			}
			else
			{
				txt.CreateBox(lines, sound, speed, portraits);
			}
			if (eventType == 4 || eventType == 5 || (eventType == 6 && !talkedTo) || eventType == 7)
			{
				doSelection = true;
				sels = txt.GetUIBox().AddComponent<Selection>();
				txt.DisablePlayerControlOnDestroy();
			}
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			talkedTo = true;
		}
		else if (eventType == 3)
		{
			CutsceneHandler.GetCutscene(eventData).StartCutscene();
		}
		else
		{
			Util.OverworldPlayer().InitiateBattle();
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		Object.Destroy(txt);
		selection = (int)index[1];
	}
}
