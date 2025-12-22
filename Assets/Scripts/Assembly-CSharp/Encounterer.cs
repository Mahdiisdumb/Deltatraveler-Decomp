using UnityEngine;
using UnityEngine.UI;

public class Encounterer : MonoBehaviour
{
	private bool holdingHoriz;

	private bool holdingVert;

	private int dir;

	private Transform textParent;

	private OverworldPlayer player;

	private AudioSource aud;

	private int curEncounter;

	private void Awake()
	{
		player = Util.OverworldPlayer();
		holdingHoriz = false;
		holdingVert = false;
		dir = 0;
		curEncounter = 0;
		base.transform.SetParent(GameObject.Find("Canvas").transform);
		base.gameObject.AddComponent<UIBackground>().CreateElement("SceneWarpBG", new Vector2(0f, 0f), new Vector2(512f, 162f));
		textParent = Object.Instantiate(Resources.Load<GameObject>("ui/debug/SceneWarpText"), base.transform).transform;
		textParent.Find("Title").GetComponent<Text>().text = "- ENCOUNTERER -";
		textParent.Find("Controls").GetComponent<Text>().text = "> - next  |  < - last  |  c - battle!";
		aud = base.gameObject.AddComponent<AudioSource>();
		aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
		aud.Play();
		UpdateInfo();
	}

	private void Start()
	{
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: true);
	}

	private void Update()
	{
		if ((bool)player && player.CanMove())
		{
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: true);
		}
		if (UTInput.GetAxis("Horizontal") == -1f && !holdingHoriz)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			curEncounter--;
			SetBounds();
			holdingHoriz = true;
			UpdateInfo();
		}
		else if (UTInput.GetAxis("Horizontal") == 1f && !holdingHoriz)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			curEncounter++;
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
			curEncounter -= 10;
			SetBounds();
			holdingVert = true;
			UpdateInfo();
		}
		else if (UTInput.GetAxis("Vertical") == 1f && !holdingVert)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			curEncounter += 10;
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
			Util.OverworldPlayer().InitiateBattle(curEncounter);
			Object.Destroy(base.gameObject);
		}
	}

	private void SetBounds()
	{
		if (curEncounter >= EnemyGenerator.GetEncounterCount())
		{
			curEncounter = 0;
		}
		if (curEncounter < 0)
		{
			curEncounter = EnemyGenerator.GetEncounterCount() - 1;
		}
	}

	private void UpdateInfo()
	{
		textParent.Find("Flag").GetComponent<Text>().text = curEncounter + " - " + EnemyGenerator.GetEncounterName(curEncounter);
		dir = 0;
	}
}
