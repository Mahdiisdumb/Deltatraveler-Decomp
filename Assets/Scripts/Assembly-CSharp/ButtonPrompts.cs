using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class ButtonPrompts : MonoBehaviour
{
	public enum ButtonType
	{
		Normal = 0,
		Small = 1,
		Big = 2,
		BigSpeech = 3
	}

	private List<Image> buttons;

	private static readonly int QUESTION_MARK_INDEX = 16;

	private static readonly List<string> BUTTON_NAMES = new List<string>
	{
		"ps4East", "ps4South", "ps4DpadDown", "ps4DpadLeft", "ps4DpadRight", "ps4DpadUp", "ps4LeftShoulder", "ps4LeftTrigger", "ps4LeftStick", "ps4Start",
		"ps4RightShoulder", "ps4RightTrigger", "ps4RightStick", "ps4West", "ps4Touchpad", "ps4North", "questionmark", "switchSouth", "switchEast", "switchCapture",
		"switchDpadDown", "switchHome", "switchLeftShoulder", "switchDpadLeft", "switchLeftStick", "switchSelect", "switchStart", "switchRightShoulder", "switchDpadRight", "switchRightStick",
		"switchSL", "switchSR", "switchDpadUp", "switchWest", "switchNorth", "switchLeftTrigger", "switchRightTrigger", "xboxSouth", "xboxEast", "xboxDpadDown",
		"xboxLeftShoulder", "xboxLeftStick", "xboxLeftTrigger", "xboxDpadLeft", "xboxStart", "xboxRightShoulder", "xboxRightStick", "xboxRightTrigger", "xboxDpadRight", "xboxSelect",
		"xboxDpadUp", "xboxWest", "xboxNorth", "ps4Select", "ps3Start", "ps3Select", "ps5Start", "ps5Select"
	};

	public static string[] validButtons = new string[14]
	{
		"South", "East", "West", "North", "LeftShoulder", "RightShoulder", "Select", "Start", "LeftStick", "RightStick",
		"DpadUp", "DpadDown", "DpadLeft", "DpadRight"
	};

	public static Dictionary<GamepadButton, string> buttonChars = new Dictionary<GamepadButton, string>
	{
		{
			GamepadButton.South,
			"\uff00"
		},
		{
			GamepadButton.East,
			"！"
		},
		{
			GamepadButton.West,
			"＂"
		},
		{
			GamepadButton.North,
			"＃"
		},
		{
			GamepadButton.LeftShoulder,
			"＄"
		},
		{
			GamepadButton.RightShoulder,
			"％"
		},
		{
			GamepadButton.Select,
			"＆"
		},
		{
			GamepadButton.Start,
			"＇"
		},
		{
			GamepadButton.LeftStick,
			"（"
		},
		{
			GamepadButton.RightStick,
			"）"
		},
		{
			GamepadButton.DpadUp,
			"＊"
		},
		{
			GamepadButton.DpadDown,
			"＋"
		},
		{
			GamepadButton.DpadLeft,
			"，"
		},
		{
			GamepadButton.DpadRight,
			"－"
		}
	};

	public static string GetButtonStyle()
	{
		if (GameManager.GetOptions().autoButton.value == 1 && Gamepad.current != null)
		{
			if (Gamepad.current.GetType().ToString().Contains("XInput") || Gamepad.current.GetType().ToString().Contains("Xbox"))
			{
				return "xbox";
			}
			if (Gamepad.current.GetType().ToString().Contains("DualSense"))
			{
				return "ps5";
			}
			if (Gamepad.current.GetType().ToString().Contains("DualShock3"))
			{
				return "ps3";
			}
			if (Gamepad.current.GetType().ToString().Contains("DualShock"))
			{
				return "ps4";
			}
			if (Gamepad.current.GetType().ToString().EndsWith("SwitchProControllerHID") || Gamepad.current.GetType().ToString().EndsWith("NPad"))
			{
				return "switch";
			}
		}
		int value = GameManager.GetOptions().buttonStyle.value;
		string result = "xbox";
		switch (value)
		{
		case 1:
			result = "ps4";
			break;
		case 2:
			result = "switch";
			break;
		case 3:
			result = "ps5";
			break;
		case 4:
			result = "ps3";
			break;
		}
		return result;
	}

	public static Sprite GetButtonGraphic(string stringName, ButtonType type = ButtonType.Normal)
	{
		string[] array = new string[4] { "", "_small", "_big", "_big_speech" };
		Sprite[] array2 = Resources.LoadAll<Sprite>("ui/spr_buttons" + array[(int)type]);
		string buttonStyle = GetButtonStyle();
		int num = BUTTON_NAMES.IndexOf(buttonStyle + stringName);
		if (num == -1)
		{
			if (buttonStyle == "ps3" || buttonStyle == "ps5")
			{
				num = BUTTON_NAMES.IndexOf("ps4" + stringName);
				if (num == -1)
				{
					num = QUESTION_MARK_INDEX;
				}
			}
			else
			{
				num = QUESTION_MARK_INDEX;
			}
		}
		return array2[num];
	}

	public static string GetButtonChar(string stringName)
	{
		GamepadButton key = (GamepadButton)Enum.Parse(typeof(GamepadButton), stringName);
		if (!buttonChars.ContainsKey(key))
		{
			return "\uffff";
		}
		return buttonChars[key];
	}

	public static void UpdateImageWithGraphic(string keyName, Image img, float scale = 2f, ButtonType type = ButtonType.Normal)
	{
		Sprite sprite = (img.sprite = GetButtonGraphic(UTInput.GetButtonName(keyName), type));
		img.rectTransform.sizeDelta = sprite.rect.size * scale;
		img.transform.localScale = Vector3.one;
	}

	private void Awake()
	{
		buttons = new List<Image>();
	}

	public void AddPrompt(RectTransform p, float x, float y, string button, int size, ButtonType type)
	{
		Image image = new GameObject("button " + button).AddComponent<Image>();
		image.sprite = GetButtonGraphic(button, type);
		image.rectTransform.SetParent(p);
		image.rectTransform.localScale = new Vector3(1f, 1f, 1f);
		int num = ((size != 2) ? 9 : 0);
		image.rectTransform.localPosition = new Vector2(Mathf.Round(p.rect.width / -2f) + 16f + x, Mathf.Round(p.rect.height / 2f) - 16f + y + (float)num);
		image.rectTransform.sizeDelta = new Vector2(image.sprite.textureRect.width, image.sprite.textureRect.height) * size;
		if ((bool)image)
		{
			buttons.Add(image);
		}
	}

	public void DeleteButtons()
	{
		foreach (Image button in buttons)
		{
			if ((bool)button)
			{
				UnityEngine.Object.Destroy(button.gameObject);
			}
		}
	}
}
