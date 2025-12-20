using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersistentFlagEditor : MonoBehaviour
{
	private object[] newFlags;

	private bool holdingHoriz;

	private bool holdingVert;

	private int dir;

	private Transform textParent;

	private OverworldPlayer player;

	private AudioSource aud;

	private int curFlag;

	private void Awake()
	{
		player = Object.FindObjectOfType<OverworldPlayer>();
		holdingHoriz = false;
		holdingVert = false;
		dir = 0;
		curFlag = 0;
		base.transform.SetParent(GameObject.Find("Canvas").transform);
		base.gameObject.AddComponent<UIBackground>().CreateElement("PFlagEditorBG", new Vector2(0f, 0f), new Vector2(512f, 212f));
		textParent = Object.Instantiate(Resources.Load<GameObject>("ui/debug/FlagEditorText"), base.transform).transform;
		textParent.Find("Title").GetComponent<Text>().text = "- SESSION FLAG EDITOR -";
		newFlags = new object[100];
		for (int i = 0; i < newFlags.Length; i++)
		{
			newFlags[i] = Object.FindObjectOfType<GameManager>().GetSessionFlag(i);
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
		if (UTInput.GetAxis("Horizontal") == -1f && !holdingHoriz)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			curFlag--;
			SetBounds();
			holdingHoriz = true;
			UpdateInfo();
		}
		else if (UTInput.GetAxis("Horizontal") == 1f && !holdingHoriz)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			curFlag++;
			SetBounds();
			holdingHoriz = true;
			UpdateInfo();
		}
		else if (UTInput.GetAxis("Horizontal") == 0f && holdingHoriz)
		{
			holdingHoriz = false;
		}
		else if (UTInput.GetAxis("Vertical") == -1f && !holdingVert)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			curFlag -= 10;
			SetBounds();
			holdingVert = true;
			UpdateInfo();
		}
		else if (UTInput.GetAxis("Vertical") == 1f && !holdingVert)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			curFlag += 10;
			SetBounds();
			holdingVert = true;
			UpdateInfo();
		}
		else if (UTInput.GetAxis("Verticall") == 0f && holdingVert)
		{
			holdingVert = false;
		}
		else if (UTInput.GetButtonDown("Z"))
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
			aud.Play();
			dir = 1;
			if (FlagUtil.GetPFlagType(curFlag) == 0)
			{
				newFlags[curFlag] = (int)newFlags[curFlag] + 1;
			}
			if (FlagUtil.GetPFlagType(curFlag) == 1)
			{
				if ((int)newFlags[curFlag] == 1)
				{
					newFlags[curFlag] = 0;
				}
				else
				{
					newFlags[curFlag] = 1;
				}
			}
			UpdateInfo();
		}
		else if (UTInput.GetButtonDown("X"))
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
			aud.Play();
			dir = 2;
			if (FlagUtil.GetPFlagType(curFlag) == 0)
			{
				newFlags[curFlag] = (int)newFlags[curFlag] - 1;
			}
			if (FlagUtil.GetPFlagType(curFlag) == 1)
			{
				if ((int)newFlags[curFlag] == 1)
				{
					newFlags[curFlag] = 0;
				}
				else
				{
					newFlags[curFlag] = 1;
				}
			}
			UpdateInfo();
		}
		else if (UTInput.GetButtonDown("C"))
		{
			for (int i = 0; i < newFlags.Length; i++)
			{
				Object.FindObjectOfType<GameManager>().SetPersistentFlag(i, newFlags[i]);
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
		if (curFlag >= newFlags.Length)
		{
			curFlag = 0;
		}
		if (curFlag < 0)
		{
			curFlag = newFlags.Length - 1;
		}
	}

	private void UpdateInfo()
	{
		textParent.Find("Flag").GetComponent<Text>().text = curFlag + " - " + FlagUtil.GetPFlagName(curFlag);
		textParent.Find("FlagValue").GetComponent<Text>().text = newFlags[curFlag].ToString();
		textParent.Find("FlagValue").GetComponent<Text>().color = new Color(1f, 1f, 1f);
		dir = 0;
	}
}
