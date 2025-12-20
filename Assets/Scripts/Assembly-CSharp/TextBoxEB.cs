using UnityEngine;
using UnityEngine.UI;

public class TextBoxEB : UIComponent
{
	private string text;

	[SerializeField]
	private string currentText;

	private string remainingText;

	[SerializeField]
	private Text txt;

	private bool doesExist;

	private Vector3 pos;

	private bool isPlaying;

	private bool isControllable;

	private int currentPos;

	private int finalPos;

	private int wait;

	private AudioSource as1;

	private AudioSource as2;

	private AudioClip sound;

	private int soundRandom;

	private Image[] lines;

	[SerializeField]
	private Sprite[] arrowSprites;

	private Image arrow;

	private int curLines;

	private int textSpeed;

	private string[] dialog;

	private int currentString;

	private int lastString;

	private bool hasStarted;

	private int frames;

	private int soundFrames;

	private int arrowFrames;

	private bool scrollDown;

	private Coroutine currentFadeOut;

	[SerializeField]
	private GameObject prefabObj;

	private Transform parent;

	private void Awake()
	{
		as1 = base.gameObject.AddComponent<AudioSource>();
		as2 = base.gameObject.AddComponent<AudioSource>();
		text = "Sample Text";
		sound = Resources.Load<AudioClip>("sounds/snd_txteb");
		soundRandom = 0;
		hasStarted = false;
		isPlaying = false;
		textSpeed = 0;
		parent = base.transform;
		frames = 0;
		scrollDown = false;
		lines = new Image[3]
		{
			base.transform.GetChild(1).GetComponent<Image>(),
			base.transform.GetChild(2).GetComponent<Image>(),
			base.transform.GetChild(3).GetComponent<Image>()
		};
		arrow = base.transform.GetChild(4).GetComponent<Image>();
	}

	private void Update()
	{
		if (!hasStarted)
		{
			return;
		}
		if (as1.volume == 0.5f)
		{
			as1.volume = 1f;
			as1.Stop();
		}
		if (as2.volume == 0.5f)
		{
			as2.volume = 1f;
			as2.Stop();
		}
		if (isPlaying)
		{
			string text = this.text.Substring(currentPos, 1);
			if (currentText.Split('\n').Length == 3 && text.EndsWith("\n"))
			{
				NewLine();
			}
			if (wait > 0)
			{
				wait--;
				return;
			}
			if (soundFrames == 0)
			{
				if (as1.isPlaying)
				{
					as1.volume = 0.5f;
					as2.Stop();
					as2.volume = 1f;
					as2.clip = sound;
					as2.Play();
				}
				else if (as2.isPlaying)
				{
					as2.volume = 0.5f;
					as1.Stop();
					as1.volume = 1f;
					as1.clip = sound;
					as1.Play();
				}
				else
				{
					if (currentFadeOut != null)
					{
						StopCoroutine(currentFadeOut);
					}
					as1.Stop();
					as1.volume = 1f;
					as1.clip = sound;
					as1.Play();
				}
			}
			soundFrames = (soundFrames + 1) % 5;
			TextRoutine(text);
			return;
		}
		if (currentString < lastString)
		{
			arrowFrames++;
			arrow.sprite = arrowSprites[arrowFrames / 10 % 2];
			arrow.enabled = true;
		}
		if (!UTInput.GetButtonDown("Z") && !UTInput.GetButton("C"))
		{
			return;
		}
		arrowFrames = 0;
		arrow.enabled = false;
		soundFrames = 0;
		frames = 0;
		if (currentString == lastString)
		{
			if (isControllable)
			{
				Object.FindObjectOfType<GameManager>().EnablePlayerMovement();
			}
			Object.Destroy(base.gameObject);
		}
		else
		{
			currentString++;
			StartNewText();
		}
	}

	private void TextRoutine(string charc)
	{
		currentText += charc;
		txt.text = currentText;
		currentPos += charc.Length;
		wait = textSpeed;
		if (charc == "\n")
		{
			curLines++;
		}
		if (finalPos - currentPos > 2 && text.Substring(currentPos, 1) == "^")
		{
			currentPos++;
			wait = int.Parse(text.Substring(currentPos, 2));
			currentPos += 2;
		}
		if (currentPos > finalPos)
		{
			isPlaying = false;
			wait = 0;
		}
	}

	private void NewLine()
	{
		curLines--;
		lines[0].enabled = lines[1].enabled;
		lines[1].enabled = lines[2].enabled;
		lines[2].enabled = false;
		currentText = currentText.Substring(currentText.IndexOf('\n') + 1);
		txt.text = currentText;
	}

	public void StartTextBox(string[] newDialog, bool allowMovementOnDestroy)
	{
		Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
		dialog = newDialog;
		currentText = "";
		txt = base.transform.GetComponentInChildren<Text>();
		hasStarted = true;
		doesExist = true;
		currentString = 0;
		lastString = dialog.Length - 1;
		SetTheme();
		StartNewText();
		isControllable = allowMovementOnDestroy;
	}

	private void StartNewText()
	{
		text = Util.Unescape(dialog[currentString]);
		currentPos = 0;
		if (curLines != 0)
		{
			currentText += "\n";
		}
		if (curLines >= 3)
		{
			NewLine();
			curLines = 2;
		}
		lines[curLines].enabled = true;
		curLines++;
		finalPos = text.Length - 1;
		isPlaying = true;
	}

	public void DoText()
	{
		isPlaying = true;
	}

	public bool IsPlaying()
	{
		return isPlaying;
	}

	public bool Exists()
	{
		return doesExist;
	}

	public void SetSound(string sound)
	{
		this.sound = Resources.Load<AudioClip>("sounds/" + sound);
	}

	public GameObject GetGameObject()
	{
		return prefabObj;
	}

	public void SetParent(Transform trans)
	{
		parent = trans;
	}

	public int GetCurrentStringNum()
	{
		return currentString;
	}

	private void SetTheme()
	{
		string text = "";
		switch ((int)Object.FindObjectOfType<GameManager>().GetFlag(223))
		{
		case 1:
			text = "_mint";
			break;
		case 2:
			text += "_strawberry";
			break;
		case 3:
			text += "_banana";
			break;
		case 4:
			text += "_buttspie";
			break;
		case 5:
			text += "_blueberry";
			break;
		case 6:
			text += "_cinnamon";
			break;
		case 7:
			text += "_moss";
			break;
		case 8:
			text += "_cottoncandy";
			break;
		case 9:
			text += "_milkshake";
			break;
		case 10:
			text += "_eggplant";
			break;
		}
		for (int i = 0; i < arrowSprites.Length; i++)
		{
			arrowSprites[i] = Resources.Load<Sprite>("ui/eb/spr_eb_textbox_arrow_" + i + text);
		}
		GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/eb/spr_eb_textbox" + text);
		if (text != "")
		{
			txt.color = new Color32(240, 240, 184, byte.MaxValue);
			for (int j = 0; j < lines.Length; j++)
			{
				lines[j].color = new Color32(240, 240, 184, byte.MaxValue);
			}
		}
	}
}
