using UnityEngine;

public class PunchCard : MonoBehaviour
{
	private bool canActivate;

	private bool activated;

	private int waitFrames;

	private void Awake()
	{
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
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
		}
		if (UTInput.GetButtonDown("Z") || UTInput.GetButtonDown("X"))
		{
			if (GetComponent<AudioSource>().isPlaying)
			{
				Util.GameManager().ResumeMusic();
			}
			Object.FindObjectOfType<OverworldPlayer>().SetCollision(true);
			Object.FindObjectOfType<GameManager>().EnablePlayerMovement();
			Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if ((bool)Object.FindObjectOfType<OverworldPlayer>())
		{
			Object.FindObjectOfType<OverworldPlayer>().SetCollision(true);
		}
	}
}
