using UnityEngine;

public class CultistMazeInteract : InteractTextBox
{
	public int cultistID;

	private bool talkedTo;

	private bool moved;

	private bool moving;

	private Vector2 direction;

	private Vector3 initPosition;

	private Vector3 newPosition;

	private int frames;

	private Animator anim;

	private int cultistFlag;

	protected override void Awake()
	{
		cultistFlag = 109 + cultistID;
		if (cultistID == 0 && (int)Util.GameManager().GetFlag(cultistFlag) > 0)
		{
			Object.Destroy(base.gameObject);
		}
		initPosition = base.transform.position;
		anim = GetComponent<Animator>();
		if (cultistID != 0)
		{
			switch (cultistID)
			{
			case 1:
			case 2:
			case 4:
				direction = Vector2.right;
				break;
			case 3:
				direction = Vector2.left;
				break;
			}
			newPosition = new Vector3(initPosition.x + direction.x * 5f / 6f, initPosition.y + direction.y * 5f / 6f);
			if ((int)Util.GameManager().GetFlag(cultistFlag) == 1)
			{
				base.transform.position = newPosition;
				moved = true;
				talkedToBefore = true;
			}
		}
	}

	public override void DoInteract()
	{
		base.DoInteract();
		anim.SetFloat("dirX", Util.OverworldPlayer().transform.position.x - base.transform.position.x);
		anim.SetFloat("dirY", Util.OverworldPlayer().transform.position.y - base.transform.position.y);
	}

	protected override void Update()
	{
		if (!txt && talkedToBefore && !moved)
		{
			if (cultistID == 0)
			{
				Util.OverworldPlayer().InitiateBattle(27);
				Object.Destroy(base.gameObject);
			}
			else
			{
				anim.SetFloat("dirX", direction.x);
				anim.SetBool("isMoving", value: true);
				moving = true;
				Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: true);
			}
		}
		else if (!txt && moved)
		{
			anim.SetFloat("dirX", 0f);
			anim.SetFloat("dirY", -1f);
		}
		if (moving)
		{
			frames++;
			base.transform.position = Vector3.Lerp(initPosition, newPosition, (float)frames / 30f);
			if (frames == 30)
			{
				moving = false;
				moved = true;
				anim.SetFloat("dirX", 0f);
				anim.SetBool("isMoving", value: false);
				Util.GameManager().EnablePlayerMovement();
				Util.GameManager().SetFlag(cultistFlag, 1);
			}
		}
	}
}
