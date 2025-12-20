using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneWarp : MonoBehaviour
{
	private bool holdingHoriz;

	private bool holdingVert;

	private int dir;

	private Transform textParent;

	private OverworldPlayer player;

	private AudioSource aud;

	private int curScene;

	private void Awake()
	{
		player = Object.FindObjectOfType<OverworldPlayer>();
		holdingHoriz = false;
		holdingVert = false;
		dir = 0;
		curScene = SceneManager.GetActiveScene().buildIndex;
		base.transform.SetParent(GameObject.Find("Canvas").transform);
		base.gameObject.AddComponent<UIBackground>().CreateElement("SceneWarpBG", new Vector2(0f, 0f), new Vector2(512f, 162f));
		textParent = Object.Instantiate(Resources.Load<GameObject>("ui/debug/SceneWarpText"), base.transform).transform;
		aud = base.gameObject.AddComponent<AudioSource>();
		aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
		aud.Play();
		UpdateInfo();
	}

	private void Start()
	{
		Object.FindObjectOfType<GameManager>().DisablePlayerMovement(true);
	}

	private void Update()
	{
		if ((bool)player && player.CanMove())
		{
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(true);
		}
		if (UTInput.GetAxis("Horizontal") == -1f && !holdingHoriz)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			curScene--;
			SetBounds();
			holdingHoriz = true;
			UpdateInfo();
		}
		else if (UTInput.GetAxis("Horizontal") == 1f && !holdingHoriz)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			curScene++;
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
			curScene -= 10;
			SetBounds();
			holdingVert = true;
			UpdateInfo();
		}
		else if (UTInput.GetAxis("Vertical") == 1f && !holdingVert)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			curScene += 10;
			SetBounds();
			holdingVert = true;
			UpdateInfo();
		}
		else if (UTInput.GetAxis("Vertical") == 0f && holdingVert)
		{
			holdingVert = false;
		}
		else if (UTInput.GetButtonDown("C"))
		{
			Object.FindObjectOfType<GameManager>().ForceLoadArea(curScene);
		}
	}

	private void SetBounds()
	{
		if (curScene >= SceneManager.sceneCountInBuildSettings)
		{
			curScene = 0;
		}
		if (curScene < 0)
		{
			curScene = SceneManager.sceneCountInBuildSettings - 1;
		}
	}

	private void UpdateInfo()
	{
		string scenePathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(curScene);
		int num = scenePathByBuildIndex.LastIndexOf('/');
		string text = scenePathByBuildIndex.Substring(num + 1);
		int startIndex = text.LastIndexOf('.');
		textParent.Find("Flag").GetComponent<Text>().text = curScene + " - " + text.Remove(startIndex);
		dir = 0;
	}
}
