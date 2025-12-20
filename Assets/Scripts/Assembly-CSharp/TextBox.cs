using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextBox : UIComponent
{
	private readonly int[] TEXTBOX_PORTRAIT_OFFSETS = new int[3] { 0, 116, -221 };

	private string[] dialog;

	private int boxPos;

	private Vector2 mainTextPos;

	private GameObject menu;

	private TextUT text;

	private int[] textSpeeds;

	private string[] textSounds;

	private List<Remark> textRemarks;

	private string font;

	private int currentString;

	private int lastString;

	private bool firstString;

	private string[] portraits;

	private Portrait portrait;

	private int currentPortrait;

	private TextRemark remark;

	private Vector3[] remarkLocations;

	private Queue<Remark> remarkQueue;

	private int toNewTextFrames;

	private bool giveControl;

	private bool isControllable;

	private bool canSkip;

	private bool disabled;

	private bool selectionEnabled;

	private bool canLoadSelection;

	private bool forceAdvance;

	private string lastSound = "snd_text";

	private int lastSpeed;

	private int frostedOffsetType;

	private void Awake()
	{
		canSkip = true;
		lastString = 0;
		firstString = false;
		isControllable = true;
		disabled = false;
		remarkLocations = new Vector3[2]
		{
			new Vector3(-320f, 210f),
			new Vector3(-320f, -100f)
		};
		font = "DTM-Mono";
		textRemarks = new List<Remark>();
		remarkQueue = new Queue<Remark>();
		if (SceneManager.GetActiveScene().buildIndex == 123)
		{
			frostedOffsetType = 1;
		}
	}

	private void Update()
	{
		if (!this.text || lastString < 0)
		{
			return;
		}
		if (currentString == 0 && toNewTextFrames == 0)
		{
			if (!PortraitIsEmpty(0))
			{
				toNewTextFrames = 7;
			}
			else
			{
				toNewTextFrames = 9;
			}
		}
		if ((bool)this.text.GetGameObject())
		{
			if (this.text.IsPlaying())
			{
				if ((UTInput.GetButton("X") || UTInput.GetButton("C")) && canSkip)
				{
					this.text.SkipText();
				}
			}
			else
			{
				if ((bool)portrait)
				{
					portrait.Stop();
				}
				if (remarkQueue.Count > 0 && !canLoadSelection)
				{
					if (!remark)
					{
						remark = Object.Instantiate(Resources.Load<GameObject>("ui/TextRemark"), menu.transform).GetComponent<TextRemark>();
						remark.StartRemark(remarkLocations[boxPos], remarkQueue.Dequeue());
					}
					if ((UTInput.GetButtonDown("X") || UTInput.GetButton("C")) && remark.CanAdvance())
					{
						remark.Skip();
					}
					if (!remark.CanAdvance())
					{
						remark = null;
					}
				}
				else if ((UTInput.GetButtonDown("Z") || UTInput.GetButton("C") || forceAdvance) && !disabled)
				{
					forceAdvance = false;
					this.text.DestroyOldText();
					if ((bool)portrait)
					{
						Object.Destroy(portrait.gameObject);
					}
					TextRemark[] componentsInChildren = menu.GetComponentsInChildren<TextRemark>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						Object.Destroy(componentsInChildren[i].gameObject);
					}
					if (currentString <= lastString)
					{
						toNewTextFrames = 9;
						if (!PortraitIsEmpty(currentString) && !PortraitIsEmpty(currentString - 1))
						{
							string text = portraits[currentString];
							string text2 = portraits[currentString - 1];
							if (text.Contains(";"))
							{
								text = text.Split(';')[1];
							}
							if (text2.Contains(";"))
							{
								text2 = text2.Split(';')[1];
							}
							string[] array = text.Split('_');
							string[] array2 = text2.Split('_');
							if (array[0] != array2[0])
							{
								toNewTextFrames = 3;
							}
						}
						else if (!PortraitIsEmpty(currentString) && PortraitIsEmpty(currentString - 1))
						{
							toNewTextFrames = 3;
						}
						if (UTInput.GetButton("X") || UTInput.GetButton("C"))
						{
							toNewTextFrames = 9;
						}
					}
					else if (!selectionEnabled)
					{
						Object.Destroy(base.gameObject);
					}
					else
					{
						canLoadSelection = true;
					}
				}
			}
		}
		if (toNewTextFrames >= 10)
		{
			return;
		}
		toNewTextFrames++;
		if (toNewTextFrames != 10)
		{
			return;
		}
		Vector2 vector = Vector2.zero;
		string theText = "* No text here.";
		string theSound = lastSound;
		int speed = lastSpeed;
		if (currentString < dialog.Length)
		{
			theText = dialog[currentString];
		}
		if (currentString < textSounds.Length)
		{
			theSound = (lastSound = textSounds[currentString]);
		}
		if (currentString < textSpeeds.Length)
		{
			speed = (lastSpeed = textSpeeds[currentString]);
		}
		this.text.StartText(theText, mainTextPos, theSound, speed, font);
		if (this.text.GetText().font.name == "sans")
		{
			vector = new Vector2(0f, -5f);
		}
		if (!PortraitIsEmpty(currentString))
		{
			vector.x += TEXTBOX_PORTRAIT_OFFSETS[1];
			if (frostedOffsetType != 0)
			{
				vector.x += 2f;
			}
			this.text.GetGameObject().transform.localPosition = mainTextPos + vector;
			StartPortrait();
		}
		else
		{
			if ((bool)portrait)
			{
				Object.Destroy(portrait.gameObject);
			}
			this.text.GetGameObject().transform.localPosition = mainTextPos + vector;
		}
		currentString++;
		QueueRemarks();
		if ((UTInput.GetButton("X") || UTInput.GetButton("C")) && canSkip)
		{
			this.text.SkipText();
		}
	}

	private void StartPortrait()
	{
		if (portrait != null)
		{
			Object.Destroy(portrait.gameObject);
		}
		currentPortrait = currentString;
		string portString = portraits[currentPortrait];
		portrait = Portrait.CreatePortrait(portString);
		portrait.transform.SetParent(text.transform, worldPositionStays: true);
		portrait.transform.localPosition = mainTextPos + new Vector2(TEXTBOX_PORTRAIT_OFFSETS[2], 12f);
		portrait.transform.localScale = Vector3.one;
		portrait.Play();
	}

	private void QueueRemarks()
	{
		foreach (Remark textRemark in textRemarks)
		{
			if (textRemark.line == currentString)
			{
				remarkQueue.Enqueue(textRemark);
			}
		}
	}

	public void CreateBox(string[] stuffToSay, string[] sound, int[] speed, int location, bool giveBackControl, string[] portraitNames)
	{
		textSounds = sound;
		textSpeeds = speed;
		dialog = stuffToSay;
		lastString = dialog.Length - 1;
		currentString = 0;
		firstString = true;
		boxPos = location;
		portraits = portraitNames;
		GameObject gameObject = GameObject.Find("Canvas");
		menu = new GameObject("TextBox");
		menu.layer = 5;
		menu.AddComponent<RectTransform>();
		menu.transform.SetParent(gameObject.transform);
		menu.AddComponent<UIBackground>();
		Vector2[] array = new Vector2[4]
		{
			new Vector2(1f, 154f),
			new Vector2(1f, -156f),
			new Vector2(4f, 142f),
			new Vector2(4f, -168f)
		};
		Vector2 defSize = new Vector2(578f, 152f);
		if (frostedOffsetType == 1)
		{
			array[0].x = 0f;
			array[1].x = 0f;
			array[1].y += 2f;
			array[2] += new Vector2(-2f, 0f);
			array[3] += new Vector2(-2f, 2f);
			defSize = new Vector2(584f, 156f);
		}
		else if (frostedOffsetType == 2)
		{
			defSize = new Vector2(584f, 166f);
			defSize.y = 166f;
			array[0] += new Vector2(-1f, -5f);
			array[1] += new Vector2(-1f, 7f);
			array[2] += new Vector2(-2f, -16f);
			array[3] += new Vector2(-2f, -4f);
		}
		menu.GetComponent<UIBackground>().setUpInfo("menu", array[location], defSize);
		menu.GetComponent<UIBackground>().CreateElement();
		menu.AddComponent<AudioSource>();
		menu.AddComponent<AudioSource>();
		menu.AddComponent<TextUT>();
		text = menu.GetComponent<TextUT>();
		mainTextPos = array[location + 2];
		giveControl = giveBackControl;
		if (SceneManager.GetActiveScene().buildIndex == 123)
		{
			menu.AddComponent<FrostedBox>().Create(this);
		}
	}

	public void CreateBox(string[] stuffToSay, string[] sound, int[] speed, int location, bool giveBackControl, string[] portraitNames, string font)
	{
		this.font = font;
		CreateBox(stuffToSay, sound, speed, location, giveBackControl, portraitNames);
	}

	public void CreateBox(string[] stuffToSay, string[] sound, int[] speed, int location, bool giveBackControl)
	{
		string[] array = new string[stuffToSay.Length];
		for (int i = 0; i < stuffToSay.Length; i++)
		{
			array[i] = null;
		}
		CreateBox(stuffToSay, sound, speed, location, giveBackControl, array);
	}

	public void CreateBox(string[] stuffToSay, string sound, int speed, int location, bool giveBackControl)
	{
		string[] array = new string[stuffToSay.Length];
		int[] array2 = new int[stuffToSay.Length];
		for (int i = 0; i < stuffToSay.Length; i++)
		{
			array[i] = sound;
			array2[i] = speed;
		}
		CreateBox(stuffToSay, array, array2, location, giveBackControl);
	}

	public void CreateBox(string[] stuffToSay, string[] sound, int[] speed, bool giveBackControl)
	{
		if (GameObject.Find("Player").transform.position[1] - GameObject.Find("Camera").transform.position[1] < -0.9f)
		{
			CreateBox(stuffToSay, sound, speed, 0, giveBackControl);
		}
		else
		{
			CreateBox(stuffToSay, sound, speed, 1, giveBackControl);
		}
	}

	public void CreateBox(string[] stuffToSay, string[] sound, int[] speed, bool giveBackControl, string[] portraitNames)
	{
		if (GameObject.Find("Player").transform.position[1] - GameObject.Find("Camera").transform.position[1] < -0.9f)
		{
			CreateBox(stuffToSay, sound, speed, 0, giveBackControl, portraitNames);
		}
		else
		{
			CreateBox(stuffToSay, sound, speed, 1, giveBackControl, portraitNames);
		}
	}

	public void CreateBox(string[] stuffToSay, string[] sound, int[] speed)
	{
		if (GameObject.Find("Player").transform.position[1] - GameObject.Find("Camera").transform.position[1] < -0.9f)
		{
			CreateBox(stuffToSay, sound, speed, 0, giveBackControl: true);
		}
		else
		{
			CreateBox(stuffToSay, sound, speed, 1, giveBackControl: true);
		}
	}

	public void CreateBox(string[] stuffToSay, string[] sound, int[] speed, string[] portraitNames)
	{
		if (GameObject.Find("Player").transform.position[1] - GameObject.Find("Camera").transform.position[1] < -0.9f)
		{
			CreateBox(stuffToSay, sound, speed, 0, giveBackControl: true, portraitNames);
		}
		else
		{
			CreateBox(stuffToSay, sound, speed, 1, giveBackControl: true, portraitNames);
		}
	}

	public void CreateBox(string[] stuffToSay, string sound, int speed, bool giveBackControl)
	{
		if (GameObject.Find("Player").transform.position[1] - GameObject.Find("Camera").transform.position[1] < -0.9f)
		{
			CreateBox(stuffToSay, sound, speed, 0, giveBackControl);
		}
		else
		{
			CreateBox(stuffToSay, sound, speed, 1, giveBackControl);
		}
	}

	public void CreateBox(string[] stuffToSay, bool giveBackControl)
	{
		CreateBox(stuffToSay, "snd_text", 0, giveBackControl);
	}

	public void CreateBox(string[] stuffToSay)
	{
		CreateBox(stuffToSay, giveBackControl: true);
	}

	public bool AtLastText()
	{
		if (lastString < currentString)
		{
			return true;
		}
		return false;
	}

	public bool IsPlaying()
	{
		return text.IsPlaying();
	}

	private void OnDestroy()
	{
		Object.Destroy(menu);
		if (giveControl && (bool)Util.GameManager())
		{
			Util.GameManager().EnablePlayerMovement();
		}
	}

	public void EnableChoice()
	{
		isControllable = false;
	}

	public void EnableSelectionAtEnd()
	{
		selectionEnabled = true;
	}

	public void DisableSelectionAtEnd()
	{
		selectionEnabled = false;
	}

	public bool IsSelectionEnabled()
	{
		return selectionEnabled;
	}

	public bool CanLoadSelection()
	{
		return canLoadSelection;
	}

	public GameObject GetUIBox()
	{
		return menu;
	}

	public int GetCurrentStringNum()
	{
		return currentString;
	}

	public void MakeUnskippable()
	{
		canSkip = false;
	}

	public void MakeSkippable()
	{
		canSkip = true;
	}

	public void Disable()
	{
		disabled = true;
	}

	public void Enable()
	{
		disabled = false;
	}

	public void ForceAdvanceCurrentLine()
	{
		forceAdvance = true;
	}

	private bool PortraitIsEmpty(int stringNum)
	{
		if (portraits == null)
		{
			return true;
		}
		if (portraits.Length == 0)
		{
			return true;
		}
		if (portraits.Length <= stringNum)
		{
			return true;
		}
		if (portraits[stringNum] != null)
		{
			return portraits[stringNum] == "";
		}
		return true;
	}

	public Portrait GetPortrait()
	{
		if ((bool)portrait)
		{
			return portrait;
		}
		return null;
	}

	public string GetCurrentSound()
	{
		int num = currentString - 1;
		if (num < 0)
		{
			return textSounds[0];
		}
		if (num >= textSounds.Length)
		{
			return textSounds[textSounds.Length - 1];
		}
		return textSounds[num];
	}

	public TextUT GetTextUT()
	{
		return text;
	}

	public Vector2 GetTextPos()
	{
		return mainTextPos;
	}

	public void EnableGasterText()
	{
		text.EnableGasterEffect();
	}

	public void DisablePlayerControlOnDestroy()
	{
		giveControl = false;
	}

	public override void CancelControlReturn()
	{
		DisablePlayerControlOnDestroy();
	}

	public void AddRemark(Remark remark)
	{
		textRemarks.Add(remark);
	}

	public void AddRemarks(List<Remark> remarks)
	{
		textRemarks.AddRange(remarks);
	}

	public void SetFrostedOffset(int frostedOffset)
	{
		frostedOffsetType = frostedOffset;
	}

	public int GetFrostedOffset()
	{
		return frostedOffsetType;
	}
}
