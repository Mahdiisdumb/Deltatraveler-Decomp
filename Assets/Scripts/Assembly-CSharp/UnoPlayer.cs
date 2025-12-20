using System.Collections.Generic;
using UnityEngine;

public class UnoPlayer : MonoBehaviour
{
	protected static GameManager gm;

	protected static GameObject playersGameObject;

	protected UnoEnemy enemy;

	protected int playerID;

	protected int unoPlayerID;

	protected bool initialized;

	protected bool setUp;

	[SerializeField]
	protected string playerName = "Kris";

	protected bool pronounIsPlural;

	[SerializeField]
	protected int skin;

	protected int points;

	private TextBubble chatbox;

	private int chatFrames;

	protected bool ready;

	[SerializeField]
	protected int pronounsIndex;

	protected virtual void Awake()
	{
		playerID = -1;
		unoPlayerID = -1;
		initialized = true;
		setUp = false;
		base.transform.parent = UnoGameManager.instance.transform;
		points = 0;
		gm = Object.FindObjectOfType<GameManager>();
		playersGameObject = GameObject.Find("Players");
	}

	protected virtual void Update()
	{
		if (chatbox != null && !chatbox.IsPlaying())
		{
			chatFrames++;
			if (chatFrames >= 30)
			{
				Object.FindObjectOfType<PartyPanels>().RaiseHeads(false, false, false);
				Object.FindObjectOfType<PartyPanels>().SetRaisedPanel(-1);
				Object.Destroy(chatbox.gameObject);
			}
		}
	}

	public void Initialize(int playerID)
	{
		this.playerID = playerID;
	}

	public void SetPlayerID(int id)
	{
		playerID = id;
	}

	public void UpdatePlayerInfo(string name, int skin, int pronounsIndex)
	{
		playerName = name;
		this.skin = skin;
		this.pronounsIndex = pronounsIndex;
	}

	public void SetSkin(int skin)
	{
		this.skin = skin;
	}

	public void SetUnoPlayerID(int unoPlayerID)
	{
		this.unoPlayerID = unoPlayerID;
	}

	public void SetEnemyObject(UnoEnemy enemy)
	{
		this.enemy = enemy;
	}

	public void SayUno(int condition)
	{
		if (enemy != null)
		{
			enemy.CallUno(condition);
			return;
		}
		string skinUNODialogue = UnoGameManager.GetSkinUNODialogue(UnoGameManager.GetSkinFilename(skin), condition);
		string text = "Up";
		Vector3 localPosition = GameObject.Find("KrisStats").transform.localPosition + new Vector3(0f, 85f);
		if (Localizer.GetText(skinUNODialogue).Length <= 6)
		{
			text += "Small";
		}
		Object.FindObjectOfType<PartyPanels>().RaiseHeads(true, false, false);
		chatbox = Object.Instantiate(Resources.Load<GameObject>("ui/bubble/Speech" + text), GameObject.Find("BattleCanvas").transform).GetComponent<TextBubble>();
		chatbox.transform.localScale = new Vector2(1f, 1f);
		chatbox.transform.localPosition = localPosition;
		string skinFilename = UnoGameManager.GetSkinFilename(skin);
		string skinTextSound = UnoGameManager.GetSkinTextSound(skinFilename);
		if (skinFilename == "papyrus")
		{
			chatbox.CreateBubble(new string[1] { Localizer.GetText(skinUNODialogue) }, 0, skinTextSound, 0, false, "papyrus");
		}
		else
		{
			chatbox.CreateBubble(new string[1] { Localizer.GetText(skinUNODialogue) }, 0, skinTextSound, 0, false);
		}
		chatFrames = 0;
	}

	public int GetPlayerID()
	{
		return playerID;
	}

	public string GetPlayerName()
	{
		return playerName;
	}

	protected object[] GetDefaultPronouns(int id)
	{
		Dictionary<int, object[]> dictionary = new Dictionary<int, object[]>();
		dictionary.Add(1, new object[4] { "he", "him", "his", false });
		dictionary.Add(0, new object[4] { "she", "her", "her", false });
		dictionary.Add(2, new object[4] { "they", "them", "their", true });
		dictionary.Add(3, new object[4] { "it", "it", "its", false });
		return dictionary[id];
	}

	public string GetPronounList()
	{
		object[] defaultPronouns = GetDefaultPronouns(pronounsIndex);
		string[] array = new string[3]
		{
			(string)defaultPronouns[0],
			(string)defaultPronouns[1],
			(string)defaultPronouns[2]
		};
		if (pronounsIndex == 3)
		{
			return "(it/its)";
		}
		return "(" + array[0] + "/" + array[1] + ")";
	}

	public string GetPronoun(int context)
	{
		string[] array = new string[3]
		{
			(string)GetDefaultPronouns(pronounsIndex)[0],
			(string)GetDefaultPronouns(pronounsIndex)[1],
			(string)GetDefaultPronouns(pronounsIndex)[2]
		};
		switch (context)
		{
		case 3:
			if (!array[2].EndsWith("s"))
			{
				return array[2] + "s";
			}
			return array[2];
		case 4:
			if (!pronounIsPlural)
			{
				return array[0] + "'s";
			}
			return array[0] + "'re";
		case 5:
			if (!pronounIsPlural)
			{
				return array[0] + "'s";
			}
			return array[0] + "'ve";
		case 6:
			if (!pronounIsPlural)
			{
				return array[0] + " was";
			}
			return array[0] + " were";
		case 7:
			if (!pronounIsPlural)
			{
				return array[0] + " has";
			}
			return array[0] + " have";
		default:
			return array[context];
		}
	}

	public bool PronounIsPlural()
	{
		return (bool)GetDefaultPronouns(pronounsIndex)[3];
	}

	public int GetSkin()
	{
		return skin;
	}

	public int GetUnoPlayerID()
	{
		return unoPlayerID;
	}

	public UnoEnemy GetEnemyObject()
	{
		return enemy;
	}

	public void UpdatePoints(int points)
	{
		this.points = points;
	}

	public int GetPoints()
	{
		return points;
	}

	public bool IsInitialized()
	{
		return initialized;
	}

	public void SetReady(bool val)
	{
		ready = val;
	}

	public bool IsReady()
	{
		return ready;
	}

	public virtual bool IsHost()
	{
		return true;
	}

	public bool IsSpeaking()
	{
		return chatbox;
	}

	public virtual void SetAIDifficulty(int newDifficulty)
	{
	}
}
