using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Notifications : MonoBehaviour
{
	public enum ID
	{
		Flavor = 0,
		Unotraveler = 1,
		MarioBros = 2,
		Arena = 3,
		Arena2 = 4
	}

	private readonly int TOP_POS = 218;

	private readonly int BOTTOM_POS = 292;

	private int doNotCheckTimer = 10;

	private bool inExtrasMenu;

	private List<ID> notifsToShow = new List<ID>();

	private bool arenaIsNew;

	private int prevArenaSectionUnlock;

	private int newArenaSectionUnlock;

	private bool activated;

	private int frames;

	private Text unlockText;

	private Text unlockSubtext;

	private Text backText;

	private void Awake()
	{
		unlockText = base.transform.Find("UnlockText").GetComponent<Text>();
		unlockSubtext = base.transform.Find("UnlockSubtext").GetComponent<Text>();
		backText = base.transform.Find("BackText").GetComponent<Text>();
		if (SceneManager.GetActiveScene().buildIndex == 132)
		{
			inExtrasMenu = true;
			doNotCheckTimer = 30;
		}
	}

	private void Update()
	{
		if (doNotCheckTimer > 0)
		{
			doNotCheckTimer--;
			if (doNotCheckTimer == 0)
			{
				CheckForNewNotifications();
			}
		}
		else if (activated)
		{
			frames++;
			if (UTInput.GetButtonDown("C"))
			{
				frames = 120;
			}
			if (frames <= 15)
			{
				float num = (float)frames / 15f;
				base.transform.localPosition = new Vector3(0f, Mathf.Lerp(BOTTOM_POS, TOP_POS, Mathf.Sin(num * MathF.PI * 0.5f)));
			}
			else if (frames >= 105)
			{
				float num2 = (float)(frames - 105) / 15f;
				num2 *= num2;
				base.transform.localPosition = new Vector3(0f, Mathf.Lerp(TOP_POS, BOTTOM_POS, num2));
			}
			if (frames >= 120)
			{
				CheckForNewNotifications();
			}
			UpdateBackText();
		}
	}

	public void CheckForNewNotifications(bool playSound = true)
	{
		if (PersistentSAVE.GetInt("flavor-notification", 0) == 0 && PersistentSAVE.GetInt("completion", 0) >= 2)
		{
			notifsToShow.Add(ID.Flavor);
			PersistentSAVE.SetInt("flavor-notification", 1);
		}
		else if (PersistentSAVE.GetInt("uno-notification", 0) == 0 && PersistentSAVE.GetInt("completion", 0) >= 3)
		{
			notifsToShow.Add(ID.Unotraveler);
			PersistentSAVE.SetInt("uno-notification", 1);
		}
		else if (PersistentSAVE.GetInt("mario-notification", 0) == 0 && PersistentSAVE.GetInt("mario-unlocked", 0) == 1)
		{
			notifsToShow.Add(ID.MarioBros);
			PersistentSAVE.SetInt("mario-notification", 1);
		}
		if (notifsToShow.Count > 0)
		{
			ShowNewNotification(playSound);
		}
		else
		{
			activated = false;
		}
	}

	private void ShowNewNotification(bool playSound)
	{
		frames = 0;
		activated = true;
		if (!inExtrasMenu)
		{
			PersistentSAVE.SetInt("new-extra", 1);
		}
		base.transform.localPosition = new Vector3(0f, BOTTOM_POS);
		if (playSound)
		{
			GetComponent<AudioSource>().Play();
		}
		ID iD = notifsToShow[0];
		notifsToShow.RemoveAt(0);
		switch (iD)
		{
		case ID.Flavor:
			unlockText.text = "You unlocked Starting Flavor!";
			unlockSubtext.text = "Extras Menu | Visuals";
			break;
		case ID.Unotraveler:
			unlockText.text = "You unlocked UNOTRAVELER!";
			unlockSubtext.text = "Extras Menu | Minigames";
			break;
		case ID.MarioBros:
			unlockText.text = "You unlocked Mario Bros.!";
			unlockSubtext.text = "Extras Menu | Minigames";
			break;
		case ID.Arena:
		{
			unlockText.text = (arenaIsNew ? "You unlocked Arena Mode!" : "* You got new Arena Mode matches!");
			int num = 0;
			int num2 = 0;
			unlockSubtext.text = $"Extras Menu | Minigames | Matches {num}-{num2}";
			break;
		}
		default:
			unlockText.text = "You got  Bepis!";
			unlockSubtext.text = "";
			break;
		}
	}

	private void UpdateBackText()
	{
		bool joystickIsActive = UTInput.joystickIsActive;
		if (joystickIsActive)
		{
			backText.text = "press    to dismiss";
			ButtonPrompts.UpdateImageWithGraphic("Menu", backText.transform.Find("Cancel").GetComponent<Image>(), 2f, ButtonPrompts.ButtonType.Small);
		}
		else
		{
			backText.text = string.Format("press {0} to dismiss", UTInput.GetKeyName("Menu").ToLower());
		}
		backText.transform.Find("Cancel").GetComponent<Image>().enabled = joystickIsActive;
	}
}
