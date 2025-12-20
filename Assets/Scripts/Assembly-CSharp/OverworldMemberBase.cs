using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class OverworldMemberBase : MonoBehaviour
{
	[SerializeField]
	protected int partyMember = -1;

	protected GameManager gm;

	protected SpriteRenderer sr;

	protected Animator anim;

	protected BoxCollider2D col;

	protected Rigidbody2D rigid2D;

	protected bool animControl;

	protected Vector2 faceDir = Vector2.down;

	protected string spritePath = "";

	protected string curSpriteName = "";

	protected string customPrefix = "";

	protected bool useRunAnim = true;

	protected virtual void Awake()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		sr = base.transform.GetComponent<SpriteRenderer>();
		anim = base.transform.GetComponent<Animator>();
		col = base.transform.GetComponent<BoxCollider2D>();
		col.offset = new Vector2(0f, -0.55f);
		col.size = new Vector2(0.8f, 0.4f);
		rigid2D = base.transform.GetComponent<Rigidbody2D>();
		rigid2D.bodyType = RigidbodyType2D.Dynamic;
		rigid2D.gravityScale = 0f;
		rigid2D.freezeRotation = true;
		animControl = true;
		useRunAnim = GameManager.GetOptions().runAnimations.value == 1;
	}

	protected virtual void LateUpdate()
	{
		if (anim.enabled)
		{
			curSpriteName = GetOverrideSprite(sr, curSpriteName);
		}
	}

	public string GetOverrideSprite(SpriteRenderer baseRenderer, string currentSprite)
	{
		string memberOWSpriteSuffix = customPrefix;
		if (partyMember > -1)
		{
			memberOWSpriteSuffix = PartyMembers.GetMemberOWSpriteSuffix(partyMember, customPrefix);
		}
		if (memberOWSpriteSuffix != "")
		{
			string text = spritePath;
			if (partyMember > -1)
			{
				text = text + memberOWSpriteSuffix + "/";
			}
			string text2 = baseRenderer.sprite.name + "_" + memberOWSpriteSuffix;
			string path = text + text2;
			if (baseRenderer.sprite.name != currentSprite || text2 != baseRenderer.sprite.name)
			{
				Sprite sprite = Resources.Load<Sprite>(path);
				if (sprite != null)
				{
					baseRenderer.sprite = sprite;
				}
			}
			currentSprite = baseRenderer.sprite.name;
		}
		return currentSprite;
	}

	protected virtual void Start()
	{
		if (partyMember > -1)
		{
			spritePath = PartyMembers.GetMemberSpritePath(partyMember);
			string memberName = PartyMembers.GetMemberName(partyMember);
			anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("player/" + memberName.ToLower() + "_ow");
			ChangeDirection(faceDir);
		}
	}

	public void SetSprite(string spriteName)
	{
		sr.sprite = Resources.Load<Sprite>(spritePath + spriteName);
	}

	public void SetSprite(Sprite sprite)
	{
		sr.sprite = sprite;
	}

	public void SetSpritePath(string path)
	{
		spritePath = path;
	}

	public string GetSpritePath()
	{
		return spritePath;
	}

	public void ChangeDirection(Vector2 faceDir)
	{
		this.faceDir = faceDir;
		anim.SetFloat("dirX", faceDir.x);
		anim.SetFloat("dirY", faceDir.y);
	}

	public Vector2 GetDirection()
	{
		return new Vector2(anim.GetFloat("dirX"), anim.GetFloat("dirY"));
	}

	public void SetSelfAnimControl(bool setAnimControl)
	{
		animControl = setAnimControl;
	}

	public void EnableAnimator()
	{
		anim.enabled = true;
	}

	public void DisableAnimator()
	{
		anim.enabled = false;
	}

	public void HideSprite()
	{
		sr.enabled = false;
	}

	public void ShowSprite()
	{
		sr.enabled = true;
	}

	public int GetSlotID()
	{
		return new List<int>(Util.GameManager().GetParty()).IndexOf(partyMember);
	}

	public int GetMemberID()
	{
		return partyMember;
	}
}
