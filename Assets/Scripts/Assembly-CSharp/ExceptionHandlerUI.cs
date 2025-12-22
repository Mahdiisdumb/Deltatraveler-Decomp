using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class ExceptionHandlerUI : MonoBehaviour
{
	private VideoPlayer video;

	private static readonly string[] videos = new string[3] { "Ai music ahh", "battle/attacks/bullets/jerry/v_favorites", "mariobros/v_ario" };

	private int delay;

	private void Awake()
	{
		video = GameObject.Find("Video").GetComponent<VideoPlayer>();
		delay = 0;
	}

	private void Start()
	{
		base.transform.GetChild(3).GetComponent<Text>().text = base.transform.GetChild(3).GetComponent<Text>().text.Replace("ver", Application.version);
		if (UnityEngine.Random.Range(1, 666) != 1)
		{
			video.clip = Resources.Load<VideoClip>(videos[UnityEngine.Random.Range(0, videos.Length)]);
			video.aspectRatio = VideoAspectRatio.Stretch;
		}
		if (ExceptionHandler.cond != null)
		{
			string text = ExceptionHandler.cond + "\n" + ExceptionHandler.stack;
			base.transform.GetChild(0).GetComponent<Text>().alignment = TextAnchor.UpperLeft;
			base.transform.GetChild(0).GetComponent<Text>().text = text.Trim().Replace("\n", "\n- ");
			base.transform.GetChild(0).GetComponent<Text>().fontSize = 13;
			base.transform.GetChild(0).GetComponent<TextOutline>().extent = 1f;
			base.transform.GetChild(1).GetComponent<Text>().text = "A crash log has been saved\nto the game's \"logs\" folder.";
			base.transform.GetChild(1).GetComponent<Text>().enabled = true;
		}
		if (Util.GameManager().IsTestMode())
		{
			base.transform.GetChild(1).GetComponent<Text>().text = "This may be the result of debug mode.\nContact the developers if this is\na legitimate error.";
			base.transform.GetChild(1).GetComponent<Text>().enabled = true;
		}
		Util.GameManager().Disable();
	}

	private void Update()
	{
		if (delay < 30)
		{
			delay++;
			if (delay == 30)
			{
				base.transform.GetChild(2).GetComponent<Text>().enabled = true;
				if (UTInput.joystickIsActive)
				{
					base.transform.GetChild(2).GetComponent<Text>().text = "[press any button to restart the game]";
				}
			}
			return;
		}
		bool flag = Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;
		bool flag2 = false;
		foreach (GamepadButton value in Enum.GetValues(typeof(GamepadButton)))
		{
			if (Gamepad.current != null && Gamepad.current[value].wasPressedThisFrame)
			{
				flag2 = true;
				break;
			}
		}
		if (flag || flag2)
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene(0, LoadSceneMode.Single);
			ExceptionHandler.Reset();
			Util.GameManager().Enable();
		}
	}
}
