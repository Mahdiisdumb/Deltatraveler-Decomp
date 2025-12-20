using System;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPartyMember : OverworldMemberBase
{
	private struct Position
	{
		public Vector3 position;

		public Vector2 direction;

		public float speed;

		public int moveState;

		public Position(Vector3 position, Vector2 direction, float speed, int moveState)
		{
			this.position = position;
			this.direction = direction;
			this.speed = speed;
			this.moveState = moveState;
		}
	}

	[SerializeField]
	private int posDistance = 10;

	[SerializeField]
	private Vector3 posOffset = Vector3.zero;

	private List<Position> positions = new List<Position>();

	private bool isMoving;

	private bool activated;

	private bool doLastMove;

	private bool inSamePos;

	private bool sliding;

	private bool forceMove;

	private bool locked;

	private int lastMoveState;

	private bool activateAfterLastMove;

	private int ignoreFrames;

	private bool acceptingIgnores;

	private bool isRunning;

	private bool isUnhappy;

	protected override void Awake()
	{
		base.Awake();
		ResetPathLists();
	}

	protected virtual void Update()
	{
		if ((isMoving || forceMove) && (activated || doLastMove) && !Util.OverworldPlayer().CannotMoveBattleSpecial())
		{
			try
			{
				if (locked && forceMove && lastMoveState == 1 && positions[0].moveState == 0)
				{
					if (HasUnlockableStateList())
					{
						forceMove = false;
						locked = false;
						activateAfterLastMove = false;
						if (positions.Count > posDistance)
						{
							ResetPathLists();
						}
					}
					HandleMoveStateChange(0);
					if (positions.Count < posDistance && acceptingIgnores)
					{
						acceptingIgnores = false;
						ignoreFrames = posDistance - positions.Count;
					}
				}
				if (ignoreFrames > 0)
				{
					GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
					ignoreFrames--;
					anim.SetFloat("speed", 0f);
				}
				else if (positions.Count > 0)
				{
					int num = Mathf.RoundToInt(Mathf.Abs(base.transform.position.x - positions[0].position.x) * 48f);
					if (num == 0)
					{
						num = Mathf.RoundToInt(Mathf.Abs(base.transform.position.y - positions[0].position.y) * 48f);
					}
					isRunning = num >= 10 && useRunAnim;
					if (isRunning)
					{
						anim.Play("run");
					}
					else
					{
						anim.Play("walk");
					}
					base.transform.position = positions[0].position;
					if (animControl)
					{
						faceDir = positions[0].direction;
						anim.SetFloat("dirX", positions[0].direction.x);
						anim.SetFloat("dirY", positions[0].direction.y);
						anim.SetFloat("speed", positions[0].speed);
					}
					if (positions[0].moveState != lastMoveState)
					{
						HandleMoveStateChange(positions[0].moveState);
					}
					lastMoveState = positions[0].moveState;
					positions.RemoveAt(0);
				}
				if (doLastMove)
				{
					doLastMove = false;
					ResetPathLists();
				}
				if (positions.Count == 0 && activateAfterLastMove)
				{
					FreeMove();
					Activate();
				}
			}
			catch (Exception message)
			{
				Debug.LogError("Something broke when handling party member " + base.gameObject.name);
				Debug.LogError(message);
			}
			isMoving = false;
		}
		else
		{
			isMoving = false;
			if (animControl)
			{
				if (lastMoveState == 1 && !Util.OverworldPlayer().CannotMoveBattleSpecial())
				{
					HandleMoveStateChange(0);
				}
				anim.SetBool("isMoving", value: false);
				anim.Play("idle");
			}
		}
		GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt((base.transform.position.y - posOffset.y) * -5f);
	}

	public void SetCustomSpritesetPrefix(string customPrefix)
	{
		if (customPrefix == "" && isUnhappy)
		{
			customPrefix = "unhappy";
		}
		base.customPrefix = customPrefix;
		if (customPrefix == "kr")
		{
			useRunAnim = false;
		}
		else
		{
			useRunAnim = GameManager.GetOptions().runAnimations.value == 1;
		}
	}

	public void ResetCustomSpritesetPrefix()
	{
		SetCustomSpritesetPrefix("");
	}

	public void AddNewPosition(Vector3 newPos, Vector2 dir, int moveState, float speed)
	{
		newPos += posOffset;
		bool flag = false;
		if (positions.Count != 0)
		{
			flag = positions[positions.Count - 1].position != newPos;
		}
		if (positions.Count == 0)
		{
			Vector2 vector = new Vector2(newPos.x - base.transform.position.x, newPos.y - base.transform.position.y);
			if (inSamePos)
			{
				vector = faceDir;
			}
			for (int i = 1; i <= posDistance; i++)
			{
				positions.Add(new Position(Vector3.Lerp(base.transform.position, newPos, (float)i / (float)posDistance), (i == posDistance) ? dir : vector, (!inSamePos) ? 1 : 0, 0));
			}
			flag = true;
		}
		else if (flag)
		{
			positions.Add(new Position(newPos, dir, speed, moveState));
			if (moveState == 1)
			{
				ForceMove(activateAfterLastMove: true);
				Lock();
			}
		}
		if (flag && animControl)
		{
			anim.SetBool("isMoving", value: true);
		}
		isMoving = flag;
		inSamePos = false;
	}

	public void UseUnhappySprites()
	{
		isUnhappy = true;
		ResetCustomSpritesetPrefix();
	}

	public void UseHappySprites()
	{
		isUnhappy = false;
		ResetCustomSpritesetPrefix();
	}

	public void ForceMove(bool activateAfterLastMove = false)
	{
		forceMove = true;
		this.activateAfterLastMove = activateAfterLastMove;
		anim.SetBool("isMoving", value: true);
	}

	public void FreeMove()
	{
		forceMove = false;
		activateAfterLastMove = false;
		ResetPathLists();
	}

	public void Lock()
	{
		locked = true;
	}

	public void Unlock()
	{
		locked = false;
	}

	public bool IsMoving()
	{
		return anim.GetBool("isMoving");
	}

	public void StartSliding()
	{
		anim.Play("Slide", 0, 0f);
		sliding = true;
		animControl = false;
		Deactivate();
	}

	public void StopSliding()
	{
		sliding = false;
		animControl = true;
		anim.Play("idle");
		activated = true;
	}

	public void Activate()
	{
		if (!sliding && !locked)
		{
			activated = true;
			if (doLastMove)
			{
				doLastMove = false;
				ResetPathLists();
			}
		}
	}

	public void SetDistanceBySlotID()
	{
		int num = posDistance;
		SetDistance(GetSlotID() * 10);
		if (num != posDistance)
		{
			ResetPathLists(ignoreSlotChange: true);
		}
	}

	public void SetDistance(int posDistance)
	{
		this.posDistance = posDistance;
	}

	public void SpawnInSamePos()
	{
		inSamePos = true;
	}

	public void Deactivate()
	{
		if (!locked)
		{
			activated = false;
			doLastMove = true;
		}
	}

	public void ResetPathLists(bool ignoreSlotChange = false)
	{
		positions.Clear();
		if (GetSlotID() > -1 && !ignoreSlotChange)
		{
			SetDistance(GetSlotID() * 10);
		}
	}

	private void HandleMoveStateChange(int moveState)
	{
		switch (moveState)
		{
		case 0:
			EnableAnimator();
			if ((bool)GetComponentInChildren<SnowSculpture>())
			{
				GetComponentInChildren<SnowSculpture>().Break();
			}
			break;
		case 1:
		{
			DisableAnimator();
			acceptingIgnores = true;
			string text = "";
			text = ((faceDir.x != 0f) ? ((faceDir.x > 0f) ? "right" : "left") : ((faceDir.y > 0f) ? "up" : "down"));
			if (GetMemberID() == 1)
			{
				SetSprite("spr_su_iceslide_" + text);
			}
			else if (GetMemberID() == 2 && (int)Util.GameManager().GetFlag(172) == 0)
			{
				SetSprite("spr_no_iceslide_" + text);
			}
			break;
		}
		}
	}

	private bool HasUnlockableStateList()
	{
		foreach (Position position in positions)
		{
			if (position.moveState == 1)
			{
				return false;
			}
		}
		return true;
	}

	public void SetPositionOffset(Vector3 posOffset)
	{
		this.posOffset = posOffset;
	}

	public Vector3 GetPositionOffset()
	{
		return posOffset;
	}

	public bool IsActivated()
	{
		return activated;
	}

	public bool IsPlayer()
	{
		return partyMember > -1;
	}

	public bool IsLocked()
	{
		return locked;
	}

	public bool IsUnhappy()
	{
		return isUnhappy;
	}

	public bool IsRunning()
	{
		return isRunning;
	}
}
