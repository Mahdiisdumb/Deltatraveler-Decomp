using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.LowLevel;
using UnityEngine.UI;

public class ButtonPrompts : MonoBehaviour
{
	private List<Image> buttons;

	public static string[] validButtons = new string[14]
	{
		"South", "East", "West", "North", "LeftShoulder", "RightShoulder", "Select", "Start", "LeftStick", "RightStick",
		"DpadUp", "DpadDown", "DpadLeft", "DpadRight"
	};

	public static Dictionary<int, string[]> buttonDict = new Dictionary<int, string[]>
	{
		{
			6,
			new string[4] { "a", "cross_0", "a_0", "\uff00" }
		},
		{
			5,
			new string[4] { "b", "circle_0", "b_0", "！" }
		},
		{
			7,
			new string[4] { "x", "square_0", "x_0", "＂" }
		},
		{
			4,
			new string[4] { "y", "triangle_0", "y_0", "＃" }
		},
		{
			10,
			new string[4] { "left_bumper", "l1", "l_0", "＄" }
		},
		{
			11,
			new string[4] { "right_bumper", "r1", "r_0", "％" }
		},
		{
			13,
			new string[4] { "share", "touchpad", "minus_0", "＆" }
		},
		{
			12,
			new string[4] { "menu", "options", "plus_0", "＇" }
		},
		{
			8,
			new string[4] { "left_stick", "l3", "lStickClick_0", "（" }
		},
		{
			9,
			new string[4] { "right_stick", "r3", "rStickClick_0", "）" }
		},
		{
			0,
			new string[4] { "up", "dpad_up", "up_0", "＊" }
		},
		{
			1,
			new string[4] { "down", "dpad_down", "down_0", "＋" }
		},
		{
			2,
			new string[4] { "left", "dpad_left", "left_0", "，" }
		},
		{
			3,
			new string[4] { "right", "dpad_right", "right_0", "－" }
		},
		{
			-1,
			new string[4] { "questionmark", "questionmark", "questionmark", "\uffff" }
		}
	};

	public static string GetButtonGraphic(string stringName)
	{
		int key = (int)Enum.Parse(typeof(GamepadButton), stringName);
		int num = GameManager.GetOptions().buttonStyle.value;
		string text = "xbox";
		if (GameManager.GetOptions().autoButton.value == 1 && Gamepad.current != null)
		{
			if (Gamepad.current.GetType().ToString().Contains("XInput") || Gamepad.current.name.Contains("Xbox"))
			{
				num = 0;
			}
			else if (Gamepad.current.GetType().ToString().Contains("DualShock") || Gamepad.current.name.Contains("DualShock"))
			{
				num = 1;
			}
			else if (Gamepad.current.GetType().ToString() == "SwitchProControllerHID" || Gamepad.current.GetType().ToString() == "NPad")
			{
				num = 2;
			}
		}
		switch (num)
		{
		case 1:
			text = "ps4";
			break;
		case 2:
			text = "switch";
			break;
		}
		if (!buttonDict.ContainsKey(key))
		{
			return "button_questionmark";
		}
		return "button_" + text + "_" + buttonDict[key][num];
	}

	public static string GetButtonChar(string stringName)
	{
		int num = (int)Enum.Parse(typeof(GamepadButton), stringName);
		return buttonDict[buttonDict.ContainsKey(num) ? num : (-1)][3];
	}

	private void Awake()
	{
		buttons = new List<Image>();
	}

	public void AddPrompt(RectTransform p, float x, float y, string button, int size)
	{
		button = GetButtonGraphic(button);
		Image image = new GameObject("button " + button).AddComponent<Image>();
		image.sprite = Resources.Load<Sprite>("ui/buttons/" + button);
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
