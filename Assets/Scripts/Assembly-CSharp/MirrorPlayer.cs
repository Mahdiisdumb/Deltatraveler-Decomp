using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class MirrorPlayer : MonoBehaviour
{
	[SerializeField]
	private bool isReflection = true;

	[SerializeField]
	private float xLimit = 10f;

	[SerializeField]
	private float yLimit = 10f;

	[SerializeField]
	private RuntimeAnimatorController animatorOverride;

	[SerializeField]
	private bool isEnabled = true;

	[SerializeField]
	private Transform reflectParent;

	[SerializeField]
	private int sortingOrderOffset;

	private OverworldPlayer player;

	private Animator anim;

	private SpriteRenderer sr;

	private bool canRunAnim;

	private void Awake()
	{
		player = Object.FindObjectOfType<OverworldPlayer>();
		anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
		if (!reflectParent)
		{
			reflectParent = base.transform.parent;
		}
		canRunAnim = GameManager.GetOptions().runAnimations.value == 1;
	}

	private void Start()
	{
		anim.runtimeAnimatorController = player.GetComponent<Animator>().runtimeAnimatorController;
		if ((bool)animatorOverride)
		{
			anim.runtimeAnimatorController = animatorOverride;
		}
	}

	private void Update()
	{
		if (isEnabled)
		{
			Vector3 vector = -player.transform.position;
			if (isReflection)
			{
				vector = new Vector3(0f - vector.x, vector.y);
			}
			if ((bool)reflectParent)
			{
				base.transform.localPosition = vector - reflectParent.position + new Vector3(0f, -0.85f);
			}
			else
			{
				base.transform.localPosition = vector + new Vector3(0f, -0.85f);
			}
			if (Mathf.Abs(base.transform.localPosition.x) >= xLimit || Mathf.Abs(base.transform.localPosition.y) >= yLimit)
			{
				sr.enabled = false;
			}
			else
			{
				sr.enabled = true;
			}
			anim.SetFloat("dirX", 0f - player.GetDirection().x);
			if (isReflection)
			{
				anim.SetFloat("dirX", player.GetDirection().x);
			}
			anim.SetFloat("dirY", 0f - player.GetDirection().y);
			bool flag = player.ProperlyMovedLastFrame();
			anim.SetBool("isMoving", flag);
			anim.SetFloat("speed", player.GetComponent<Animator>().GetFloat("speed"));
			bool flag2 = player.GetSpeed() >= 10f && canRunAnim;
			if (flag)
			{
				anim.Play(flag2 ? "run" : "walk");
			}
			sr.sortingOrder = Mathf.RoundToInt(base.transform.position.y * -5f) + sortingOrderOffset;
		}
	}

	private void LateUpdate()
	{
		if (Util.GameManager().GetFlagInt(108) != 1 || Util.GameManager().GetFlagInt(13) < 2 || Util.GameManager().GetFlagInt(127) != 1)
		{
			return;
		}
		string text = "g";
		string text2 = sr.sprite.name + "_" + text;
		if (text2 != sr.sprite.name)
		{
			Sprite sprite = Resources.Load<Sprite>("player/Frisk/" + text + "/" + text2);
			if (sprite != null)
			{
				sr.sprite = sprite;
			}
		}
	}

	public void Enable()
	{
		isEnabled = true;
	}

	public void Disable()
	{
		isEnabled = false;
	}

	public void SetParent(Transform newParent)
	{
		reflectParent = newParent;
	}

	public void PlayStepSound()
	{
	}
}
