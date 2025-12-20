using UnityEngine;

public class InteractWanderingNPC : InteractTextBox
{
	[SerializeField]
	private Vector3 maxPos = Vector3.zero;

	[SerializeField]
	private Vector3 minPos = Vector3.zero;

	[SerializeField]
	private int waitLength = 45;

	[SerializeField]
	private int walkLength = 30;

	[SerializeField]
	private int walkSpeed = 4;

	private bool isWalking;

	private int frames;

	private Vector2 direction = Vector2.zero;

	[SerializeField]
	private int sortingOrderOffset;

	[SerializeField]
	private bool forceDisableWander;

	private bool wanderDisabled;

	[SerializeField]
	private Vector2 defaultLook = Vector2.zero;

	protected override void Awake()
	{
		base.Awake();
		MonoBehaviour.print("Your mom");
		GetComponent<Rigidbody2D>().isKinematic = true;
		if (forceDisableWander && defaultLook != Vector2.zero)
		{
			GetComponent<Animator>().SetFloat("dirX", defaultLook.x);
			GetComponent<Animator>().SetFloat("dirY", defaultLook.y);
		}
	}

	protected override void Update()
	{
		if (!forceDisableWander && !wanderDisabled && !txt)
		{
			frames++;
			if ((frames > waitLength && !isWalking) || (frames > walkLength && isWalking))
			{
				frames = 0;
				isWalking = !isWalking;
				GetComponent<Animator>().SetBool("isMoving", isWalking);
				GetComponent<Rigidbody2D>().isKinematic = !isWalking;
				if (isWalking)
				{
					int num = Random.Range(-1, 2);
					int num2 = 0;
					if (num == 0)
					{
						num2 = ((Random.Range(0, 2) != 0) ? 1 : (-1));
					}
					if ((base.transform.position.x > maxPos.x && num > 0) || (base.transform.position.x < minPos.x && num < 0))
					{
						num *= -1;
					}
					if ((base.transform.position.y > maxPos.y && num2 > 0) || (base.transform.position.y < minPos.y && num2 < 0))
					{
						num2 *= -1;
					}
					direction = new Vector2(num, num2);
					GetComponent<Animator>().SetFloat("dirX", num);
					GetComponent<Animator>().SetFloat("dirY", num2);
				}
			}
			if (isWalking)
			{
				GetComponent<Rigidbody2D>().MovePosition(base.transform.position + (Vector3)direction * ((float)walkSpeed / 48f));
			}
		}
		GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(base.transform.position.y * -5f) + sortingOrderOffset;
	}

	private void LateUpdate()
	{
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}

	public override void DoInteract()
	{
		if (!txt && enabled)
		{
			frames = 0;
			isWalking = false;
			GetComponent<Animator>().SetBool("isMoving", isWalking);
			wanderDisabled = true;
			GetComponent<Animator>().SetFloat("dirX", Object.FindObjectOfType<OverworldPlayer>().transform.position.x - base.transform.position.x);
			GetComponent<Animator>().SetFloat("dirY", Object.FindObjectOfType<OverworldPlayer>().transform.position.y - base.transform.position.y);
		}
		base.DoInteract();
	}

	public void StopMoving()
	{
		GetComponent<Animator>().SetBool("isMoving", false);
		forceDisableWander = true;
	}

	public void ChangeDirection(Vector2 dir)
	{
		GetComponent<Animator>().SetFloat("dirX", dir.x);
		GetComponent<Animator>().SetFloat("dirY", dir.y);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((bool)collision.gameObject.GetComponent<OverworldPlayer>())
		{
			GetComponent<Rigidbody2D>().isKinematic = true;
			frames = 0;
			isWalking = false;
			GetComponent<Animator>().SetBool("isMoving", isWalking);
			wanderDisabled = true;
		}
		else
		{
			frames = 0;
			isWalking = false;
			GetComponent<Animator>().SetBool("isMoving", isWalking);
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if ((bool)collision.gameObject.GetComponent<OverworldPlayer>())
		{
			wanderDisabled = false;
			if (forceDisableWander && defaultLook != Vector2.zero)
			{
				GetComponent<Animator>().SetFloat("dirX", defaultLook.x);
				GetComponent<Animator>().SetFloat("dirY", defaultLook.y);
			}
		}
	}
}
