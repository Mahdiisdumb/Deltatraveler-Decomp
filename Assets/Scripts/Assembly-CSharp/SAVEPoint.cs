using UnityEngine;

public class SAVEPoint : Interactable
{
	[SerializeField]
	private bool doPhrase;

	[SerializeField]
	private string[] phrases = new string[1] { "* (You're filled with\n  determination.)" };

	[SerializeField]
	private string relativeSpawn = "down";

	private GameManager gm;

	private bool isSaving;

	private bool saveMenuOpen;

	[SerializeField]
	private bool force;

	[SerializeField]
	private bool allowOblitModification = true;

	private void Awake()
	{
		isSaving = false;
	}

	private void Start()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		if ((int)gm.GetFlag(13) <= 1 || !allowOblitModification)
		{
			return;
		}
		if ((int)gm.GetFlag(108) == 1)
		{
			if (WeirdChecker.GetRemainingHardModeEnemies(gm) == 0)
			{
				phrases = new string[1] { "* Determination." };
			}
			else
			{
				phrases = new string[1] { "<color=#FF0000FF>* " + WeirdChecker.GetRemainingHardModeEnemies(gm) + " left.</color>" };
			}
		}
		else if (gm.SusieInParty() && !gm.NoelleInParty())
		{
			phrases = new string[1] { "* You and Susie are filled\n  with power." };
		}
		else if (gm.SusieInParty() && gm.NoelleInParty())
		{
			phrases = new string[1] { "* You, Susie, and Noelle are\n  filled with power." };
		}
		else if (!gm.SusieInParty() && !gm.NoelleInParty() && (int)gm.GetFlag(87) >= 8)
		{
			if (Util.GameManager().GetCurrentZone() == 86 && (int)gm.GetFlag(87) < 9)
			{
				phrases = new string[1] { "* You felt an urge to climb\n  down the ladder <color=#FFFF00FF>right now</color>." };
			}
			else
			{
				phrases = new string[1] { "* The loneliness fills you\n  with dread." };
			}
		}
	}

	private void Update()
	{
		if (isSaving && txt == null && !saveMenuOpen)
		{
			Object.Instantiate(Resources.Load<GameObject>("ui/SaveMenu"), Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform).transform.localPosition = Vector3.zero;
			saveMenuOpen = true;
		}
		if (saveMenuOpen && !Object.FindObjectOfType<SaveMenu>())
		{
			saveMenuOpen = false;
			isSaving = false;
			gm.EnablePlayerMovement();
		}
	}

	public override void DoInteract()
	{
		gm.HealAll(99);
		gm.PlayGlobalSFX("sounds/snd_heal");
		gm.DisablePlayerMovement(false);
		if (doPhrase)
		{
			txt = new GameObject().AddComponent<TextBox>();
			txt.CreateBox(phrases, false);
		}
		isSaving = true;
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		isSaving = false;
	}

	public Vector3 GetSpawnPosition()
	{
		float num = 1f;
		if (relativeSpawn == "up" || relativeSpawn == "down")
		{
			if (relativeSpawn == "down")
			{
				num = -1f;
			}
			return base.transform.position + new Vector3(0f, 1.2f * num);
		}
		if (relativeSpawn == "left" || relativeSpawn == "right")
		{
			if (relativeSpawn == "left")
			{
				num = -1f;
			}
			return base.transform.position + new Vector3(num, 0.4f);
		}
		return base.transform.position + new Vector3(0f, -1.2f);
	}

	public void ModifyPhrases(string[] lines)
	{
		phrases = lines;
	}

	public void CancelSave()
	{
		isSaving = false;
		saveMenuOpen = false;
	}

	public bool IsSaving()
	{
		return isSaving;
	}

	public override int GetEventData()
	{
		return -1;
	}
}
