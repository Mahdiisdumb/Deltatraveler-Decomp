using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextUT : MonoBehaviour
{
	private string text;

	[SerializeField]
	private string currentText;

	private string remainingText;

	private Font font;

	[SerializeField]
	private Text txtObj;

	private bool doesExist;

	private Vector3 pos;

	private bool playing;

	private bool isControllable;

	private bool isColoring;

	private int colorPos;

	private int currentPos;

	private int finalPos;

	private int wait;

	private bool muted;

	private AudioSource as1;

	private AudioSource as2;

	private AudioClip sound;

	private string soundName;

	private int soundRandom;

	private bool enableGasterText;

	private int textSpeed;

	private float spacing;

	private Coroutine currentFadeOut;

	[SerializeField]
	private GameObject prefabObj;

	private Transform parent;

	private ButtonPrompts prompts;

	private Portrait miniPortrait;

	private int column;

	private int row;

	private AudioSource voiceSource;

	private Coroutine voiceSequenceRoutine;

	private bool voiceSequenceRunning;

	private static string[] COMMANDS = new string[2] { "P", "MP" };

	private void Awake()
	{
		as1 = base.gameObject.AddComponent<AudioSource>();
		as2 = base.gameObject.AddComponent<AudioSource>();
		text = "Sample Text";
		font = Resources.Load<Font>("fonts/DTM-Mono");
		sound = Resources.Load<AudioClip>("sounds/snd_text");
		soundRandom = 0;
		playing = false;
		parent = base.transform;
		voiceSource = base.gameObject.AddComponent<AudioSource>();
	}

	private void Update()
	{
		for (int i = 0; i <= currentPos; i++)
		{
			Random.Range(0, 1);
		}
		if (!playing)
		{
			return;
		}
		if (soundRandom > 0)
		{
			sound = Resources.Load<AudioClip>("sounds/" + soundName + "_" + Random.Range(0, soundRandom));
		}
		if (!muted)
		{
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
		}
		if (wait > 0)
		{
			wait--;
		}
		else if (this.text.Length - currentPos >= 1)
		{
			int num = 1;
			string text = this.text.Substring(currentPos, 1);
			if (soundName.Contains("txtmtt"))
			{
				num = 3;
			}
			if (text != " " && text != "\t")
			{
				if ((bool)miniPortrait)
				{
					miniPortrait.Update();
				}
				if (!muted)
				{
					PlayPitchedBlip();
				}
			}
			for (int j = 0; j < num; j++)
			{
				if (!playing)
				{
					break;
				}
				if (wait != 0)
				{
					break;
				}
				text = this.text.Substring(currentPos, 1);
				TextRoutine(text);
				if (j == num - 1 && wait == textSpeed)
				{
					wait += j;
				}
			}
		}
		else if (this.text.Length - currentPos <= 0)
		{
			playing = false;
		}
	}

	private void PlayPitchedBlip()
	{
		if (soundName.Contains("txtspam") || soundName.Contains("txtq_2"))
		{
			float pitch = 0.7f;
			if (soundName.Contains("txtq_2"))
			{
				pitch = Random.Range(0.95f, 1.05f);
			}
			if (currentPos % 2 == 0)
			{
				PlayBlipAudio(pitch);
			}
		}
		else
		{
			PlayBlipAudio();
		}
	}

	private void PlayBlipAudio(float pitch = 1f)
	{
		if (as1.isPlaying)
		{
			as1.volume = 0.5f;
			as2.Stop();
			as2.volume = 1f;
			as2.pitch = pitch;
			as2.clip = sound;
			as2.Play();
			return;
		}
		if (as2.isPlaying)
		{
			as2.volume = 0.5f;
			as1.Stop();
			as1.volume = 1f;
			as1.pitch = pitch;
			as1.clip = sound;
			as1.Play();
			return;
		}
		if (currentFadeOut != null)
		{
			StopCoroutine(currentFadeOut);
		}
		as1.Stop();
		as1.volume = 1f;
		as1.pitch = pitch;
		as1.clip = sound;
		as1.Play();
	}

	private void TextRoutine(string charc)
	{
		bool flag = false;
		int num = 0;
		if ((bool)prompts)
		{
			for (int i = 0; i < ButtonPrompts.validButtons.Length; i++)
			{
				if (charc == ButtonPrompts.GetButtonChar(ButtonPrompts.validButtons[i]))
				{
					ButtonPrompts.ButtonType buttonType = ButtonPrompts.ButtonType.Normal;
					int num2 = ((txtObj.fontSize <= 20) ? 1 : 2);
					if (font.name == "speechbubble" || (font.name == "papyrus" && num2 == 1))
					{
						buttonType = ButtonPrompts.ButtonType.BigSpeech;
					}
					else if (font.name == "papyrus")
					{
						buttonType = ButtonPrompts.ButtonType.Big;
					}
					float num3 = (-31f - (txtObj.lineSpacing - 1f) / 0.15f * 5f) / 2f * (float)num2;
					int num4 = row * 8 * num2;
					int num5 = Mathf.RoundToInt((float)column * num3);
					if (buttonType == ButtonPrompts.ButtonType.BigSpeech)
					{
						num4 -= 6;
					}
					if (font.name == "papyrus")
					{
						num5 -= 3 * num2;
					}
					prompts.AddPrompt(txtObj.rectTransform, num4, num5, ButtonPrompts.validButtons[i], num2, buttonType);
					charc = " ";
					flag = true;
					break;
				}
			}
		}
		string[] cOMMANDS = COMMANDS;
		foreach (string text in cOMMANDS)
		{
			if (finalPos - currentPos > text.Length && this.text.Substring(currentPos, text.Length + 1) == ";" + text)
			{
				currentPos += text.Length + 1;
				string text2 = "";
				while (this.text.Substring(currentPos, 1) != ";")
				{
					text2 += this.text.Substring(currentPos, 1);
					currentPos++;
				}
				charc = "";
				HandleCommand(text, text2);
				currentPos++;
				return;
			}
		}
		if (charc != "\b")
		{
			currentText += charc;
		}
		if (flag)
		{
			currentText += " ";
		}
		row += ((!flag) ? 1 : 2);
		if (charc == "\n")
		{
			column++;
			row = 0;
		}
		if (finalPos - currentPos > 1)
		{
			if (charc == "\n" && this.text.Substring(currentPos + 1, 1) == " ")
			{
				currentText += " ";
				row++;
				currentPos++;
			}
			if (charc == "\b")
			{
				while (this.text.Substring(currentPos + 1, 1) == " ")
				{
					currentText += " ";
					row++;
					currentPos++;
				}
			}
		}
		if (isColoring)
		{
			currentText = currentText.Remove(colorPos, 8);
			colorPos = currentText.Length;
			currentText += "</color>";
		}
		if (txtObj != null)
		{
			txtObj.text = currentText;
		}
		currentPos += charc.Length + num;
		if (charc.Length > 0)
		{
			wait = textSpeed + (charc.Length - 1);
		}
		if (finalPos - currentPos > 6)
		{
			if (this.text.Substring(currentPos, 8) == "<color=#")
			{
				currentText += this.text.Substring(currentPos, 17);
				currentPos += 17;
				isColoring = true;
				colorPos = currentText.Length;
				currentText += "</color>";
			}
			else if (this.text.Substring(currentPos, 8) == "</color>")
			{
				currentPos += 8;
				isColoring = false;
			}
		}
		if (finalPos - currentPos > 2 && this.text.Substring(currentPos, 1) == "^")
		{
			currentPos++;
			wait = int.Parse(this.text.Substring(currentPos, 2));
			currentPos += 2;
		}
		if (currentPos > finalPos)
		{
			playing = false;
			wait = 0;
		}
	}

	private void HandleCommand(string commandType, string commandArg)
	{
		if (!(commandType == "P"))
		{
			if (commandType == "MP")
			{
				float num = 0f - 31f * txtObj.lineSpacing;
				int num2 = row * 16;
				int num3 = Mathf.RoundToInt((float)column * num);
				miniPortrait = Portrait.CreatePortrait("mini;" + commandArg);
				miniPortrait.transform.SetParent(txtObj.transform);
				float num4 = Mathf.Round(txtObj.rectTransform.rect.width / -2f) + 8f;
				float num5 = Mathf.Round(txtObj.rectTransform.rect.height / 2f) - 16f;
				miniPortrait.transform.localPosition = new Vector3(num4 + (float)num2, num5 + (float)num3);
				miniPortrait.transform.localScale = Vector3.one;
				miniPortrait.enabled = false;
				currentText += " ";
			}
		}
		else
		{
			Portrait componentInChildren = base.transform.parent.GetComponentInChildren<Portrait>();
			if ((bool)componentInChildren)
			{
				componentInChildren.SetImage(commandArg);
			}
		}
	}

	public void StartText(string theText, Vector2 thePos)
	{
		row = 0;
		column = 0;
		if ((bool)prompts)
		{
			prompts.DeleteButtons();
			Object.Destroy(prompts);
		}
		theText = Util.Unescape(theText);
		theText = theText.Replace("^N", GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayerName());
		if ((theText.Contains("^Z") || theText.Contains("^X") || theText.Contains("^C")) && UTInput.joystickIsActive)
		{
			prompts = base.gameObject.AddComponent<ButtonPrompts>();
		}
		theText = theText.Replace("^Z", UTInput.GetKeyOrButtonReplacement("Confirm"));
		theText = theText.Replace("^X", UTInput.GetKeyOrButtonReplacement("Cancel"));
		theText = theText.Replace("^C", UTInput.GetKeyOrButtonReplacement("Menu"));
		currentFadeOut = StartCoroutine(AudioFadeOut.FadeOut(as1, 0.1f));
		text = theText;
		pos = thePos;
		currentText = "";
		GameObject original = Resources.Load<GameObject>("ui/TextBase");
		if (enableGasterText)
		{
			original = Resources.Load<GameObject>("ui/TextBaseGaster");
		}
		prefabObj = Object.Instantiate(original, parent.position, Quaternion.identity);
		prefabObj.transform.SetParent(parent);
		prefabObj.transform.localPosition = pos;
		if (parent.gameObject.name == "BattleCanvas")
		{
			prefabObj.transform.localScale = new Vector2(1f, 1f);
		}
		txtObj = prefabObj.GetComponent<Text>();
		txtObj.GetComponent<LetterSpacing>().spacing = spacing;
		if (parent.gameObject.name.StartsWith("Speech"))
		{
			prefabObj.transform.localScale = new Vector2(1f, 1f);
			txtObj.fontSize = 13;
			txtObj.lineSpacing = 1.3f;
			txtObj.color = new Color(0f, 0f, 0f);
		}
		currentPos = 0;
		colorPos = 0;
		finalPos = text.Length - 1;
		playing = true;
		doesExist = true;
		if (text.Length >= 8 && text.Substring(0, 8) == "<color=#")
		{
			currentText += text.Substring(currentPos, 17);
			currentPos += 17;
			isColoring = true;
			colorPos = currentText.Length;
			currentText += "</color>";
		}
	}

	public void StartText(string theText, Vector2 thePos, string theSound)
	{
		if (voiceSource.isPlaying)
		{
			voiceSequenceRunning = false;
			voiceSource.Stop();
		}
		bool flag = DetermineUseVoiceSound(theSound);
		if (flag)
		{
			soundName = "";
		}
		else
		{
			soundName = theSound;
			if (soundName.Contains("txtmtt"))
			{
				soundRandom = 9;
				theSound = theSound + "_" + Random.Range(0, soundRandom);
			}
			else if (soundName.Contains("txtwd") || soundName.Contains("txtwdc"))
			{
				soundRandom = 7;
				theSound = theSound + "_" + Random.Range(0, soundRandom);
			}
			else if (soundName.Contains("txttem"))
			{
				soundRandom = 6;
				theSound = theSound + "_" + Random.Range(0, soundRandom);
			}
			else
			{
				soundRandom = 0;
			}
		}
		sound = Resources.Load<AudioClip>("sounds/" + theSound);
		muted = flag;
		StartText(theText, thePos);
	}

	public void StartText(string theText, Vector2 thePos, string theSound, int speed)
	{
		textSpeed = speed;
		StartText(theText, thePos, theSound);
	}

	public void StartText(string theText, Vector2 thePos, string theSound, int speed, string theFont)
	{
		bool num = theText.StartsWith("/WD");
		if (num)
		{
			theText = theText.Replace("/WD", "");
		}
		StartText(theText, thePos, theSound, speed);
		txtObj.font = Resources.Load<Font>("fonts/" + theFont);
		if (theFont == "papyrus" || theFont == "wingdings")
		{
			if (txtObj.fontSize > 20)
			{
				txtObj.fontSize = 32;
			}
			else
			{
				txtObj.fontSize = 16;
			}
		}
		else if (theFont.StartsWith("sans"))
		{
			if (txtObj.fontSize > 20)
			{
				txtObj.fontSize = 30;
			}
			else
			{
				txtObj.fontSize = 15;
			}
		}
		else if (txtObj.fontSize > 20)
		{
			txtObj.fontSize = 26;
		}
		else
		{
			txtObj.fontSize = 13;
		}
		if ((theFont == "DTM-Mono" || theFont == "speechbubble") && soundName.Contains("txtsans"))
		{
			bool flag = GetComponent<TextBubble>();
			txtObj.font = Resources.Load<Font>(flag ? "fonts/sansb" : "fonts/sans");
			if (txtObj.fontSize > 20)
			{
				txtObj.fontSize = 30;
			}
			else
			{
				txtObj.fontSize = 15;
			}
			txtObj.lineSpacing = (flag ? 1f : 0.9f);
		}
		if ((theFont == "DTM-Mono" || theFont == "speechbubble") && soundName.Contains("txtpap"))
		{
			txtObj.font = Resources.Load<Font>("fonts/papyrus");
			if (txtObj.fontSize > 20)
			{
				txtObj.fontSize = 32;
			}
			else
			{
				txtObj.fontSize = 16;
			}
		}
		if (num)
		{
			txtObj.font = Resources.Load<Font>("fonts/wingdings");
			if (txtObj.fontSize > 20)
			{
				txtObj.fontSize = 32;
			}
			else
			{
				txtObj.fontSize = 16;
			}
		}
		if (theFont == "DTM-Mono" && soundName.Contains("txtsat"))
		{
			txtObj.font = Resources.Load<Font>("fonts/saturn");
			if (txtObj.fontSize > 20)
			{
				txtObj.fontSize = 32;
			}
			else
			{
				txtObj.fontSize = 16;
			}
		}
		font = txtObj.font;
	}

	public void DoText()
	{
		playing = true;
	}

	public void SkipText(bool sound = true)
	{
		if (playing)
		{
			if (sound)
			{
				PlayPitchedBlip();
			}
			playing = false;
			while (currentPos <= finalPos)
			{
				TextRoutine(text.Substring(currentPos, 1));
			}
			wait = 0;
		}
	}

	public bool IsPlaying()
	{
		return playing;
	}

	public void DestroyOldText()
	{
		SkipText(sound: false);
		Object.Destroy(prefabObj);
		doesExist = false;
		muted = false;
	}

	public bool Exists()
	{
		return doesExist;
	}

	public GameObject GetGameObject()
	{
		return prefabObj;
	}

	public Text GetText()
	{
		return txtObj;
	}

	public void SetParent(Transform trans)
	{
		parent = trans;
	}

	public void OnDestroy()
	{
		if (Exists())
		{
			DestroyOldText();
		}
	}

	public void SetLetterSpacing(float spacing)
	{
		this.spacing = spacing;
	}

	public void EnableGasterEffect()
	{
		enableGasterText = true;
	}

	private bool DetermineUseVoiceSound(string sound)
	{
		if (sound.StartsWith("#"))
		{
			if (sound.Contains("`"))
			{
				StartCoroutine(PlaySequentialVoiceLines(sound.Split('`')));
			}
			else
			{
				string path = "sounds/voice/" + sound.Trim('#');
				voiceSource.clip = Resources.Load<AudioClip>(path);
				voiceSource.Play();
			}
			return true;
		}
		return false;
	}

	private IEnumerator PlaySequentialVoiceLines(string[] voices)
	{
		voiceSequenceRunning = true;
		foreach (string text in voices)
		{
			string path = "sounds/voice/" + text.Trim('#');
			voiceSource.clip = Resources.Load<AudioClip>(path);
			voiceSource.Play();
			while (voiceSource.isPlaying && voiceSequenceRunning)
			{
				yield return null;
			}
		}
	}
}
