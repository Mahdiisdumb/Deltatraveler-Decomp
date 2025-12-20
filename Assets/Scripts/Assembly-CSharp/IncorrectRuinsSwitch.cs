using System;
using UnityEngine;

public class IncorrectRuinsSwitch : Interactable
{
	private int frames;

	private bool isPlaying;

	private Vector2 susieJumpDir = Vector2.right;

	private Vector3 origPos;

	private Vector3 newPos;

	private Vector3 susieOrigPos;

	private OverworldPartyMember susie;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		frames++;
		if (frames >= 30 && frames <= 120)
		{
			if (frames == 30)
			{
				UnityEngine.Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_fall");
				UnityEngine.Object.FindObjectOfType<OverworldPlayer>().GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
			}
			Vector2[] array = new Vector2[4]
			{
				Vector2.down,
				Vector2.right,
				Vector2.up,
				Vector2.left
			};
			UnityEngine.Object.FindObjectOfType<OverworldPlayer>().transform.position = Vector3.Lerp(origPos, newPos, (float)(frames - 30) / 80f);
			UnityEngine.Object.FindObjectOfType<OverworldPlayer>().ChangeDirection(array[(frames - 30) / 4 % 4]);
			if (frames == 120)
			{
				UnityEngine.Object.FindObjectOfType<OverworldPlayer>().GetComponent<SpriteRenderer>().color = Color.white;
			}
		}
		int num = frames - 30;
		float num2 = Mathf.Abs(susieOrigPos.x - newPos.x) * susieJumpDir.x * 1.5f;
		float x = Mathf.Lerp(susieOrigPos.x, newPos.x + num2, (float)num / 60f);
		float num3 = ((num < 10) ? ((float)num / 10f) : ((float)(num - 10) / 50f));
		num3 = ((num >= 10) ? (num3 * num3) : Mathf.Sin(num3 * (float)Math.PI * 0.5f));
		float y = ((num < 10) ? Mathf.Lerp(susieOrigPos.y, origPos.y + susie.GetPositionOffset().y + 1f, num3) : Mathf.Lerp(origPos.y + susie.GetPositionOffset().y + 1f, newPos.y + susieJumpDir.y * 1.5f, num3));
		if (num >= 0 && num <= 60)
		{
			susie.transform.position = new Vector3(x, y);
		}
		if (num == 0)
		{
			susie.SetSelfAnimControl(false);
			if (susieJumpDir == Vector2.up)
			{
				susie.GetComponent<Animator>().Play("FallBack");
			}
			else
			{
				susie.GetComponent<Animator>().Play("Fall");
				if (susieJumpDir == Vector2.left)
				{
					susie.GetComponent<SpriteRenderer>().flipX = true;
				}
			}
		}
		if (num == 25)
		{
			susie.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
		}
		if (num == 60)
		{
			susie.GetComponent<SpriteRenderer>().flipX = false;
			susie.GetComponent<SpriteRenderer>().color = Color.white;
			susie.SetSelfAnimControl(true);
			susie.ChangeDirection(susieJumpDir * -1f);
		}
		if (frames == 50)
		{
			UnityEngine.Object.FindObjectOfType<Fade>().FadeOut(10);
		}
		if (frames >= 60 && !UnityEngine.Object.FindObjectOfType<Fade>().IsPlaying())
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().LoadArea(30, true, Vector3.zero, Vector2.up);
		}
	}

	public override void DoInteract()
	{
		if (!isPlaying)
		{
			UnityEngine.Object.FindObjectOfType<CameraController>().SetFollowPlayer(false);
			origPos = UnityEngine.Object.FindObjectOfType<OverworldPlayer>().transform.position;
			newPos = origPos + new Vector3(0f, -16.68f);
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("overworld/ruins_objects/Hole"), origPos, Quaternion.identity, base.transform.parent);
			UnityEngine.Object.FindObjectOfType<GameManager>().DisablePlayerMovement(true);
			isPlaying = true;
			frames = 0;
			UnityEngine.Object.FindObjectOfType<OverworldPlayer>().SetCollision(false);
			susie = GameObject.Find("Susie").GetComponent<OverworldPartyMember>();
			susieOrigPos = susie.transform.position;
			susieJumpDir = UnityEngine.Object.FindObjectOfType<OverworldPlayer>().GetDirection();
		}
	}

	public override int GetEventData()
	{
		return -1;
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		Debug.LogError("THis nuts");
	}
}
