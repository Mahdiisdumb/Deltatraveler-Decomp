using UnityEngine;
using UnityEngine.UI;

public class PunchCard : MonoBehaviour
{
	private bool canActivate;

	private bool activated;

	private int waitFrames;

	[SerializeField]
	private bool unoCard;

	private void Awake()
	{
		if ((int)Util.GameManager().GetFlag(170) == 1 && (int)Util.GameManager().GetFlag(171) == 0)
		{
			Util.GameManager().SetFlag(171, 1);
			GetComponent<Image>().sprite = Resources.Load<Sprite>($"ui/{GetComponent<SpriteRenderer>().sprite.name}_quest");
		}
		else if (unoCard && Util.GameManager().GetFlagInt(301) == 0 && Util.GameManager().GetPlayerName() == "SHAYY")
		{
			Util.GameManager().SetFlag(301, 1);
			Util.GameManager().PauseMusic();
			GetComponent<AudioSource>().Play();
			int flagInt = Util.GameManager().GetFlagInt(312);
			if (flagInt == 1)
			{
				GetComponent<Image>().color = UnoCard.BLUE.color;
			}
			else
			{
				GetComponent<Image>().color = SOUL.GetSOULColorByID(flagInt, forceNormal: true);
			}
			waitFrames = 30;
		}
	}

	private void Update()
	{
		if (waitFrames > 0)
		{
			waitFrames--;
			return;
		}
		if (!canActivate)
		{
			canActivate = true;
			return;
		}
		if (!activated)
		{
			activated = true;
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		}
		if (UTInput.GetButtonDown("Z") || UTInput.GetButtonDown("X"))
		{
			if (GetComponent<AudioSource>().isPlaying)
			{
				Util.GameManager().ResumeMusic();
			}
			Util.OverworldPlayer().SetCollision(onoff: true);
			Util.GameManager().EnablePlayerMovement();
			Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if ((bool)Util.OverworldPlayer())
		{
			Util.OverworldPlayer().SetCollision(onoff: true);
		}
	}
}
