using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlagEditor : MonoBehaviour
{
	private object[] newFlags;

	private Transform textParent;

	private OverworldPlayer player;

	private AudioSource aud;

	private static int curFlag;

	private void Awake()
	{
		player = Object.FindObjectOfType<OverworldPlayer>();
		base.transform.SetParent(GameObject.Find("Canvas").transform);
		base.gameObject.AddComponent<UIBackground>().CreateElement("FlagEditorBG", new Vector2(0f, 0f), new Vector2(512f, 212f));
		textParent = Object.Instantiate(Resources.Load<GameObject>("ui/debug/FlagEditorText"), base.transform).transform;
		newFlags = new object[1000];
		for (int i = 0; i < newFlags.Length; i++)
		{
			newFlags[i] = Object.FindObjectOfType<GameManager>().GetFlag(i);
		}
		aud = base.gameObject.AddComponent<AudioSource>();
		aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
		aud.Play();
		UpdateInfo();
	}

	private void Start()
	{
		Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
	}

	private void Update()
	{
		if ((bool)player && player.CanMove())
		{
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
		}
		if (UTInput.GetButtonDown("Left"))
		{
			curFlag--;
			SetBounds();
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			UpdateInfo();
		}
		if (UTInput.GetButtonDown("Right"))
		{
			curFlag++;
			SetBounds();
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			UpdateInfo();
		}
		if (UTInput.GetButtonDown("Up"))
		{
			curFlag += 10;
			SetBounds();
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			UpdateInfo();
		}
		if (UTInput.GetButtonDown("Down"))
		{
			curFlag -= 10;
			SetBounds();
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			UpdateInfo();
		}
		if (UTInput.GetButtonDown("Z") || UTInput.GetButtonDown("X"))
		{
			int num = 0;
			if (UTInput.GetButtonDown("Z"))
			{
				num++;
			}
			if (UTInput.GetButtonDown("X"))
			{
				num--;
			}
			int num2 = 0;
			if (newFlags[curFlag].GetType() == typeof(int))
			{
				num2 = (int)newFlags[curFlag];
			}
			float num3 = 0f;
			if (newFlags[curFlag].GetType() == typeof(float))
			{
				num3 = (float)newFlags[curFlag];
			}
			switch (FlagUtil.GetFlagType(curFlag))
			{
			case FlagType.IntBool:
				switch (num)
				{
				case 1:
					num2 = 1;
					break;
				case -1:
					num2 = 0;
					break;
				}
				newFlags[curFlag] = num2;
				break;
			case FlagType.WinState:
				num2 += num;
				if (num2 > 3)
				{
					num2 = 0;
				}
				if (num2 < 0)
				{
					num2 = 3;
				}
				newFlags[curFlag] = num2;
				break;
			case FlagType.MiniPartyMember:
				num2 += num;
				if (num2 > 2)
				{
					num2 = 0;
				}
				if (num2 < 0)
				{
					num2 = 2;
				}
				newFlags[curFlag] = num2;
				Object.FindObjectOfType<GameManager>().SetMiniPartyMember(num2);
				break;
			case FlagType.IntNumber:
				num2 += num;
				newFlags[curFlag] = num2;
				break;
			case FlagType.Float:
				num3 += (float)num / 10f;
				newFlags[curFlag] = num3;
				break;
			case FlagType.Item:
				num2 = (int)newFlags[curFlag];
				num2++;
				if (num2 > Items.NumOfItems() - 1)
				{
					num2 = 0;
				}
				if (num2 < 0)
				{
					num2 = Items.NumOfItems() - 1;
				}
				newFlags[curFlag] = num2;
				break;
			}
			aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
			aud.Play();
			UpdateInfo();
		}
		if (UTInput.GetButtonDown("C"))
		{
			for (int i = 0; i < newFlags.Length; i++)
			{
				Object.FindObjectOfType<GameManager>().SetFlag(i, newFlags[i]);
			}
			if ((bool)player)
			{
				Object.FindObjectOfType<GameManager>().LoadArea(SceneManager.GetActiveScene().buildIndex, true, player.transform.position, player.GetDirection());
			}
			else
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}
	}

	private void SetBounds()
	{
		if (curFlag >= FlagUtil.GetFlagCount())
		{
			curFlag = 0;
		}
		if (curFlag < 0)
		{
			curFlag = FlagUtil.GetFlagCount() - 1;
		}
	}

	private void UpdateInfo()
	{
		textParent.Find("Flag").GetComponent<Text>().text = curFlag + " - " + FlagUtil.GetFlagName(curFlag);
		textParent.Find("FlagValue").GetComponent<Text>().color = new Color(1f, 1f, 1f);
		switch (FlagUtil.GetFlagType(curFlag))
		{
		case FlagType.IntBool:
		{
			string text = "FALSE";
			if ((int)newFlags[curFlag] == 1)
			{
				text = "TRUE";
			}
			textParent.Find("FlagValue").GetComponent<Text>().text = text;
			break;
		}
		case FlagType.WinState:
		{
			string[] array2 = new string[4] { "None", "Killed", "Spared", "Varied" };
			textParent.Find("FlagValue").GetComponent<Text>().text = array2[(int)newFlags[curFlag]];
			break;
		}
		case FlagType.String:
			textParent.Find("FlagValue").GetComponent<Text>().color = new Color(0.753f, 0.753f, 0.753f);
			textParent.Find("FlagValue").GetComponent<Text>().text = newFlags[curFlag].ToString();
			break;
		case FlagType.Item:
			textParent.Find("FlagValue").GetComponent<Text>().text = Items.ItemName((int)newFlags[curFlag]);
			break;
		case FlagType.MiniPartyMember:
		{
			string[] array = new string[3] { "No one", "Paula", "Chara" };
			textParent.Find("FlagValue").GetComponent<Text>().text = array[(int)newFlags[curFlag]];
			break;
		}
		default:
			textParent.Find("FlagValue").GetComponent<Text>().text = newFlags[curFlag].ToString();
			break;
		}
	}
}
