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
		if (cultistID == 0 && (int)Object.FindObjectOfType<GameManager>().GetFlag(cultistFlag) > 0)
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
			if ((int)Object.FindObjectOfType<GameManager>().GetFlag(cultistFlag) == 1)
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
		anim.SetFloat("dirX", Object.FindObjectOfType<OverworldPlayer>().transform.position.x - base.transform.position.x);
		anim.SetFloat("dirY", Object.FindObjectOfType<OverworldPlayer>().transform.position.y - base.transform.position.y);
	}

	protected override void Update()
	{
		if (!txt && talkedToBefore && !moved)
		{
			if (cultistID == 0)
			{
				Object.FindObjectOfType<OverworldPlayer>().InitiateBattle(27);
				Object.Destroy(base.gameObject);
			}
			else
			{
				anim.SetFloat("dirX", direction.x);
				anim.SetBool("isMoving", true);
				moving = true;
				Object.FindObjectOfType<GameManager>().DisablePlayerMovement(true);
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
				anim.SetBool("isMoving", false);
				Object.FindObjectOfType<GameManager>().EnablePlayerMovement();
				Object.FindObjectOfType<GameManager>().SetFlag(cultistFlag, 1);
			}
		}
	}
}
