using UnityEngine;
using UnityEngine.UI;

public class TextZelda : MonoBehaviour
{
	private string text;

	[SerializeField]
	private string currentText;

	private string remainingText;

	private Font font;

	private Text txtObj;

	private bool doesExist;

	private Vector3 pos;

	private bool isPlaying;

	private bool isControllable;

	private bool isColoring;

	private int colorPos;

	private int currentPos;

	private int finalPos;

	private int wait;

	private AudioSource as1;

	private AudioSource as2;

	private AudioClip sound;

	private AudioClip soundDone;

	private string soundName;

	private int soundRandom;

	private int textSpeed;

	private string[] dialog;

	private int currentString;

	private int lastString;

	private bool hasStarted;

	private int frames;

	private bool scrollDown;

	private bool skipLines;

	private Coroutine currentFadeOut;

	[SerializeField]
	private GameObject prefabObj;

	private Transform parent;

	private void Awake()
	{
		as1 = base.gameObject.AddComponent<AudioSource>();
		as2 = base.gameObject.AddComponent<AudioSource>();
		text = "Sample Text";
		font = Resources.Load<Font>("fonts/ganon");
		font.material.mainTexture.filterMode = FilterMode.Point;
		sound = Resources.Load<AudioClip>("sounds/zelda/snd_text");
		soundDone = Resources.Load<AudioClip>("sounds/zelda/snd_text_done");
		soundRandom = 0;
		hasStarted = false;
		isPlaying = false;
		textSpeed = 2;
		parent = base.transform;
		frames = 0;
		scrollDown = false;
		skipLines = false;
		GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("ui/zelda/spr_textbox_zelda")[Util.GameManager().GetFlagInt(223)];
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
			string text;
			if (currentPos == -1)
			{
				text = "\n";
			}
			else if (skipLines || UTInput.GetButton("X") || UTInput.GetButton("C"))
			{
				text = "";
				int i;
				for (i = currentPos; i < finalPos; i++)
				{
					text = this.text.Substring(currentPos, 1);
					if (text == "\n")
					{
						break;
					}
					TextRoutine(text);
					wait = 0;
				}
				if (i == finalPos)
				{
					text = this.text.Substring(currentPos, 1);
				}
			}
			else
			{
				text = this.text.Substring(currentPos, 1);
			}
			if (scrollDown)
			{
				frames++;
				if (frames % 2 == 0)
				{
					base.transform.GetChild(0).Find("TextBaseZ").transform.localPosition = Vector2.Lerp(Vector2.zero, new Vector2(0f, 16.5f), (float)frames / 10f);
				}
				if (frames == 10)
				{
					currentText = currentText.Substring(currentText.IndexOf('\n') + 1);
					txtObj.text = currentText;
					frames = 0;
					base.transform.GetChild(0).Find("TextBaseZ").transform.localPosition = Vector2.zero;
					TextRoutine(text);
					scrollDown = false;
				}
				return;
			}
			if (currentText.Split('\n').Length == 3 && text.EndsWith("\n"))
			{
				scrollDown = true;
				return;
			}
			if (wait > 0)
			{
				wait--;
				return;
			}
			if (text != " ")
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
			TextRoutine(text);
			return;
		}
		if (as1.clip != soundDone)
		{
			frames++;
			if (frames >= 30)
			{
				as1.clip = soundDone;
				Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/zelda/snd_text_done");
				frames = 0;
			}
		}
		if (!UTInput.GetButtonDown("Z") && !UTInput.GetButton("C"))
		{
			return;
		}
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
		txtObj.text = currentText;
		currentPos += charc.Length;
		wait = textSpeed;
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

	public void StartTextBox(string[] newDialog, bool skipText = true, bool hideBorder = false, bool allowMovementOnDestroy = true, string sound = "sounds/zelda/snd_text")
	{
		dialog = newDialog;
		currentText = "";
		txtObj = base.transform.GetComponentInChildren<Text>();
		skipLines = skipText;
		if (skipLines)
		{
			textSpeed = 6;
		}
		hasStarted = true;
		doesExist = true;
		base.transform.GetComponent<Image>().enabled = !hideBorder;
		this.sound = Resources.Load<AudioClip>(sound);
		currentString = 0;
		lastString = dialog.Length - 1;
		StartNewText();
		isControllable = allowMovementOnDestroy;
	}

	private void StartNewText()
	{
		text = Util.Unescape(dialog[currentString]);
		if (currentString > 0)
		{
			currentPos = -1;
		}
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

	public GameObject GetGameObject()
	{
		return prefabObj;
	}

	public void SetParent(Transform trans)
	{
		parent = trans;
	}
}
