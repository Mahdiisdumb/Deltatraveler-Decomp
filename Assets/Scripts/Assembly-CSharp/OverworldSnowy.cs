using UnityEngine;

public class OverworldSnowy : OverworldEnemyBase
{
	private bool chill;

	private string curSpriteName = "";

	[SerializeField]
	private Vector2 startMove = Vector2.left;

	[SerializeField]
	private Vector2 minBound = Vector2.zero;

	[SerializeField]
	private Vector2 maxBound = Vector2.zero;

	[SerializeField]
	private bool moving;

	private int moveFrames;

	[SerializeField]
	private bool doMovement = true;

	[SerializeField]
	private bool startWithAnim;

	[SerializeField]
	private bool feral;

	protected override void Awake()
	{
		if ((int)Util.GameManager().GetFlag(180) == 1 || feral)
		{
			chill = true;
		}
		base.Awake();
		if (!chill)
		{
			base.transform.position = new Vector3(10f, 10f);
			Object.Destroy(base.transform.GetChild(1).gameObject);
			anim.SetFloat("dirX", 1f);
			anim.SetFloat("speed", 1.5f);
			return;
		}
		speed = 10f;
		anim.SetFloat("dirX", startMove.x);
		anim.SetFloat("dirY", startMove.y);
		if (startWithAnim)
		{
			anim.SetFloat("speed", 1f);
		}
	}

	public override void InstantSpareRespawn()
	{
		if ((counterFlagID != 182 || GetCounter() < 2) && Util.GameManager().NoelleInParty() && ((int)Util.GameManager().GetFlag(209) != 2 || (int)Util.GameManager().GetFlag(181) != 2 || (int)Util.GameManager().GetFlag(182) != 1))
		{
			base.InstantSpareRespawn();
			if ((int)Util.GameManager().GetFlag(205) != 0 && GetDefeatFlagID() == 201 && (bool)npcRespawn)
			{
				npcRespawn.GetComponent<InteractTextBox>().EnableSecondaryLines();
				npcRespawn.GetComponent<InteractTextBox>().ForceTalkedToBefore();
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!disabled && canDetectPlayer && !detecting && !running && chill && doMovement)
		{
			if (moving)
			{
				anim.SetFloat("speed", 1f);
				rigidbody2D.MovePosition(base.transform.position + (Vector3)startMove * 2f / 48f);
				if (moveFrames == 30)
				{
					moveFrames = 0;
					moving = false;
					anim.SetFloat("speed", 0f);
				}
			}
			else if (moveFrames == 60)
			{
				moveFrames = 0;
				moving = true;
				if (Random.Range(0, 2) == 1)
				{
					startMove = new Vector2((Random.Range(0, 2) != 1) ? 1 : (-1), 0f);
				}
				else
				{
					startMove = new Vector2(0f, (Random.Range(0, 2) != 1) ? 1 : (-1));
				}
				if ((startMove.x > 0f && base.transform.position.x > maxBound.x) || (startMove.x < 0f && base.transform.position.x < minBound.x))
				{
					startMove.x *= -1f;
				}
				if ((startMove.y > 0f && base.transform.position.y > maxBound.y) || (startMove.y < 0f && base.transform.position.y < minBound.y))
				{
					startMove.y *= -1f;
				}
				anim.SetFloat("dirX", startMove.x);
				anim.SetFloat("dirY", startMove.y);
				anim.SetFloat("speed", 1f);
			}
			moveFrames++;
		}
		sr.sortingOrder = Mathf.RoundToInt(base.transform.position.y * -5f) + sortingOrderOffset;
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
		if ((!chill && !feral) || !anim.enabled)
		{
			return;
		}
		if (sr.sprite.name != curSpriteName)
		{
			Sprite sprite = Resources.Load<Sprite>("overworld/npcs/enemies/" + sr.sprite.name + (feral ? "_feral" : "_chill"));
			if (sprite != null)
			{
				sr.sprite = sprite;
			}
		}
		curSpriteName = sr.sprite.name;
	}

	protected override void StartRunning()
	{
		if (feral)
		{
			GetComponents<AudioSource>()[2].Play();
		}
		base.StartRunning();
	}

	public override void StopRunning()
	{
		base.StopRunning();
		speed = 10f;
		moving = false;
		moveFrames = 0;
		anim.SetFloat("speed", 0f);
	}

	protected override void RunAlgorithm()
	{
		base.RunAlgorithm();
		anim.SetFloat("speed", 1.5f);
		if (speed < 18f)
		{
			speed += 0.1f;
		}
		float num = ((!runFromPlayer) ? 1 : (-1));
		anim.SetFloat("dirX", (Object.FindObjectOfType<OverworldPlayer>().transform.position.x - base.transform.position.x) * num);
		anim.SetFloat("dirY", (Object.FindObjectOfType<OverworldPlayer>().transform.position.y - base.transform.position.y) * num);
	}

	public override void DetectPlayer()
	{
		base.DetectPlayer();
		anim.SetFloat("speed", 0f);
		anim.SetFloat("dirX", Object.FindObjectOfType<OverworldPlayer>().transform.position.x - base.transform.position.x);
		anim.SetFloat("dirY", Object.FindObjectOfType<OverworldPlayer>().transform.position.y - base.transform.position.y);
	}
}
