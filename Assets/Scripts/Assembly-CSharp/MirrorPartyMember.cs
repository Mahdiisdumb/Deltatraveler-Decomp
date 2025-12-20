using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class MirrorPartyMember : MonoBehaviour
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

	[SerializeField]
	private OverworldPartyMember partyMember;

	private Animator anim;

	private SpriteRenderer sr;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
		anim.runtimeAnimatorController = partyMember.GetComponent<Animator>().runtimeAnimatorController;
		if ((bool)animatorOverride)
		{
			anim.runtimeAnimatorController = animatorOverride;
		}
		if (!reflectParent)
		{
			reflectParent = base.transform.parent;
		}
	}

	private void Update()
	{
		if (isEnabled)
		{
			Vector3 vector = -partyMember.transform.position;
			if (isReflection)
			{
				vector = new Vector3(0f - vector.x, vector.y);
			}
			if ((bool)reflectParent)
			{
				base.transform.localPosition = vector - reflectParent.position + new Vector3(0f, -0.85f) + partyMember.GetPositionOffset() * 2f;
			}
			else
			{
				base.transform.localPosition = vector + new Vector3(0f, -0.85f) + partyMember.GetPositionOffset() * 2f;
			}
			if (Mathf.Abs(base.transform.localPosition.x) >= xLimit || Mathf.Abs(base.transform.localPosition.y) >= yLimit)
			{
				sr.enabled = false;
			}
			else
			{
				sr.enabled = true;
			}
			anim.SetFloat("dirX", 0f - partyMember.GetDirection().x);
			if (isReflection)
			{
				anim.SetFloat("dirX", partyMember.GetDirection().x);
			}
			anim.SetFloat("dirY", 0f - partyMember.GetDirection().y);
			bool flag = partyMember.IsMoving();
			anim.SetBool("isMoving", flag);
			anim.SetFloat("speed", partyMember.GetComponent<Animator>().GetFloat("speed"));
			if (flag)
			{
				anim.Play(partyMember.IsRunning() ? "run" : "walk");
			}
			sr.sortingOrder = Mathf.RoundToInt(base.transform.position.y * -5f) + sortingOrderOffset;
		}
	}

	private void LateUpdate()
	{
		if (!partyMember.IsUnhappy())
		{
			return;
		}
		string text = "player/" + partyMember.gameObject.name + "/" + GetComponent<SpriteRenderer>().sprite.name;
		string text2 = text;
		if (partyMember.IsUnhappy())
		{
			for (int i = 0; i < 6; i++)
			{
				text2 = text2.Replace("_" + i, "_unhappy_" + i);
			}
		}
		Sprite sprite = Resources.Load<Sprite>(text2);
		if (sprite != null)
		{
			GetComponent<SpriteRenderer>().sprite = sprite;
			return;
		}
		sprite = Resources.Load<Sprite>(text);
		if (sprite != null)
		{
			GetComponent<SpriteRenderer>().sprite = sprite;
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
}
