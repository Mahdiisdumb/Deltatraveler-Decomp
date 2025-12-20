using UnityEngine;

public class WalkingBunnies : InteractTextBox
{
	[SerializeField]
	private Vector3[] positions;

	private int curPos;

	private int nextPos = 1;

	private Animator anim;

	private Vector3 turnDir = Vector2.left;

	private OverworldPlayer player;

	private bool currentlyMoving = true;

	private int idleFrames;

	protected override void Awake()
	{
		base.Awake();
		anim = GetComponent<Animator>();
		SetPosition(0);
		if ((int)Util.GameManager().GetFlag(205) == 1 || (int)Util.GameManager().GetFlag(206) == 1 || (int)Util.GameManager().GetFlag(207) == 1 || (int)Util.GameManager().GetFlag(209) == 1)
		{
			lines = new string[4] { "* It's nice to be out of the\n  house again,^05 but...", "* It feels like I'm being\n  haunted.", "* Like I can hear distant growls\n  or footsteps,^05 but nothing is\n  actually there.", "* It's probably just my\n  imagination." };
		}
	}

	private void Start()
	{
		player = Object.FindObjectOfType<OverworldPlayer>();
	}

	protected override void Update()
	{
		if (Vector3.Distance(base.transform.position, player.transform.position) <= 2f)
		{
			anim.Play("Idle", 0, 0f);
			currentlyMoving = false;
			idleFrames = 0;
		}
		else if (!currentlyMoving)
		{
			idleFrames++;
			if (idleFrames >= 30)
			{
				anim.SetFloat("dirX", turnDir.x);
				anim.SetFloat("dirY", turnDir.y);
				anim.Play("Walk", 0, 0f);
				currentlyMoving = true;
			}
		}
		if (currentlyMoving)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, positions[nextPos], 1f / 24f);
			if (base.transform.position == positions[nextPos])
			{
				SetPosition(nextPos);
			}
		}
		else
		{
			anim.SetFloat("dirX", player.transform.position.x - base.transform.position.x);
			anim.SetFloat("dirY", player.transform.position.y - base.transform.position.y);
		}
		GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(base.transform.position.y * -5f);
	}

	public void SetPosition(int newPos)
	{
		curPos = newPos % positions.Length;
		nextPos = (newPos + 1) % positions.Length;
		turnDir = positions[nextPos] - positions[curPos];
		base.transform.position = positions[curPos];
		anim.SetFloat("dirX", turnDir.x);
		anim.SetFloat("dirY", turnDir.y);
	}
}
