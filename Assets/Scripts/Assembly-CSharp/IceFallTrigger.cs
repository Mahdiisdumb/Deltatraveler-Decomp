using System.Collections.Generic;
using UnityEngine;

public class IceFallTrigger : MonoBehaviour
{
	private List<Transform> fallingTargets;

	private bool isPlaying;

	private Vector3 origPos;

	private Vector3 newPos;

	private int playerFrames;

	private void Awake()
	{
		fallingTargets = new List<Transform>();
	}

	private void Update()
	{
		foreach (Transform fallingTarget in fallingTargets)
		{
			if (!fallingTarget)
			{
				continue;
			}
			fallingTarget.position -= new Vector3(0f, 0.18f, 0f);
			if ((bool)fallingTarget.GetComponent<OverworldPlayer>())
			{
				playerFrames++;
				Vector2[] array = new Vector2[4]
				{
					Vector2.down,
					Vector2.right,
					Vector2.up,
					Vector2.left
				};
				fallingTarget.GetComponent<OverworldPlayer>().ChangeDirection(array[playerFrames / 4 % 4]);
				if (playerFrames == 30)
				{
					Util.FindObjectOfType<Fade>().FadeOut(10);
				}
				if (playerFrames == 40)
				{
					Util.GameManager().LoadArea(97, fadeIn: true, Vector2.zero, Vector2.down);
				}
			}
			else if ((bool)fallingTarget.GetComponent<OverworldPartyMember>() && fallingTarget.GetComponent<OverworldPartyMember>().GetMemberID() == 1)
			{
				fallingTarget.GetComponent<SpriteRenderer>().flipX = playerFrames / 4 % 2 == 0;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((!collision || !collision.GetComponent<OverworldPlayer>()) && !collision.GetComponent<OverworldPartyMember>())
		{
			return;
		}
		collision.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
		if ((bool)collision.GetComponent<OverworldPlayer>())
		{
			collision.enabled = false;
			Util.GameManager().PlayGlobalSFX("sounds/snd_fall");
			OverworldPlayer component = collision.GetComponent<OverworldPlayer>();
			component.SetCollision(onoff: false);
			component.EnableAnimator();
			component.ForceSendPositions();
			Util.GameManager().LockMenu();
			Util.GameManager().SetSessionFlag(10, 1);
			BoxCollider2D[] components = GetComponents<BoxCollider2D>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].size += new Vector2(0.25f, 0.25f);
			}
			MonoBehaviour.print("IceFallTrigger done");
		}
		if ((bool)collision.GetComponent<OverworldPartyMember>() && fallingTargets.Count > 0)
		{
			collision.enabled = false;
			OverworldPartyMember component2 = collision.GetComponent<OverworldPartyMember>();
			component2.Deactivate();
			component2.DisableAnimator();
			if (component2.GetMemberID() == 1)
			{
				component2.SetSprite("spr_su_freaked");
			}
			else if (component2.GetMemberID() == 2)
			{
				component2.SetSprite("spr_no_surprise");
			}
		}
		fallingTargets.Add(collision.transform);
	}
}
