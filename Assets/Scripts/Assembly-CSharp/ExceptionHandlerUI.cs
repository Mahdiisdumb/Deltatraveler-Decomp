using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class ExceptionHandlerUI : MonoBehaviour
{
	private VideoPlayer video;

	private string[] randomText = new string[1] { "* Whoops! Looks like a crash...\n* Please report this to the\n  developers." };

	private readonly string[] videos = new string[3] { "overworld/npcs/v_ralsei", "battle/attacks/bullets/jerry/v_favorites", "mariobros/v_ario" };

	private bool listed;

	private bool holding;

	private int delay;

	private void Awake()
	{
		video = GameObject.Find("v_ralsei").GetComponent<VideoPlayer>();
		listed = false;
		holding = false;
		delay = 0;
		base.transform.GetChild(0).GetComponent<Text>().text = randomText[Random.Range(0, randomText.Length)];
	}

	private void Start()
	{
		Util.GameManager().StopMusic();
		base.transform.GetChild(3).GetComponent<Text>().text = base.transform.GetChild(3).GetComponent<Text>().text.Replace("ver", Object.FindObjectOfType<GameManager>().GetVersionBuild());
		if (Random.Range(1, 666) == 1)
		{
			video.clip = Resources.Load<VideoClip>("overworld/lostcore_objects/gaster_beatboxing");
		}
		else
		{
			video.clip = Resources.Load<VideoClip>(videos[Random.Range(0, videos.Length)]);
		}
	}

	private void Update()
	{
		if (!listed)
		{
			Object.FindObjectOfType<ExceptionHandler>().ListException();
		}
		if (UTInput.GetButtonDown("Z"))
		{
			Object.FindObjectOfType<GameManager>().StopMusic();
			Time.timeScale = 1f;
			SceneManager.LoadScene(0, LoadSceneMode.Single);
			ExceptionHandler.alreadyCrashed = false;
		}
		if (UTInput.GetButtonDown("C"))
		{
			video.playbackSpeed = 1f;
		}
		if (delay == 30 && UTInput.GetButton("X"))
		{
			if (UTInput.GetAxis("Horizontal") != 0f)
			{
				if (!holding)
				{
					holding = true;
					video.playbackSpeed += Mathf.Ceil(UTInput.GetAxis("Horizontal")) * 0.5f;
				}
			}
			else
			{
				holding = false;
			}
			if (UTInput.GetAxis("Vertical") != 0f)
			{
				video.playbackSpeed += UTInput.GetAxis("Vertical") * (1f / 60f);
			}
			video.playbackSpeed = Mathf.Clamp(video.playbackSpeed, 0.2f, 5f);
		}
		if (delay < 30)
		{
			delay++;
		}
	}

	public void ListException(string condition, string stackTrace)
	{
		listed = true;
		if (condition != null)
		{
			string text = condition + "\n" + stackTrace;
			text = text.Replace("\n", "\n- ");
			base.transform.GetChild(1).GetComponent<Text>().text = "Exception: " + text.Substring(0, text.Length - 2);
			base.transform.GetChild(1).GetComponent<Text>().fontSize = 13;
			base.transform.GetChild(4).GetComponent<Text>().enabled = true;
		}
	}
}
