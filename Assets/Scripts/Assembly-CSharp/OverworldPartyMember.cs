using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldPartyMember : Interactable
{
	[SerializeField]
	private int posDistance = 10;

	[SerializeField]
	private Vector3 posOffset = Vector3.zero;

	private List<Vector3> posList;

	private List<Vector2> dirList;

	private List<float> spdList;

	private List<int> moveStateList;

	protected Animator anim;

	private Vector2 faceDir = Vector2.zero;

	private bool isMoving;

	private bool activated;

	private bool doLastMove;

	private bool inSamePos;

	private bool animControl = true;

	private bool unhappy;

	protected string curSpriteName = "";

	private string customPrefix = "";

	protected bool isPlayer = true;

	private bool sliding;

	private bool forceMove;

	private bool locked;

	private int lastMoveState;

	private bool activateAfterLastMove;

	private int ignoreFrames;

	private bool acceptingIgnores;

	private bool useRunAnim = true;

	private bool isRunning;

	protected virtual void Awake()
	{
		anim = GetComponent<Animator>();
		ResetPathLists();
		if (base.gameObject.name == "Noelle" && !UnityEngine.Object.FindObjectOfType<GameManager>().SusieInParty())
		{
			posDistance = 10;
		}
		if (SceneManager.GetActiveScene().buildIndex == 123)
		{
			customPrefix = "hd";
		}
		if ((bool)UnityEngine.Object.FindObjectOfType<UndyneShadow>())
		{
			customPrefix = "undynes";
		}
		useRunAnim = GameManager.GetOptions().runAnimations.value == 1;
	}

	protected virtual void Update()
	{
		if ((isMoving || forceMove) && (activated || doLastMove) && !UnityEngine.Object.FindObjectOfType<OverworldPlayer>().CannotMoveBattleSpecial())
		{
			try
			{
				if (locked && forceMove && lastMoveState == 1 && moveStateList.Count > 0 && moveStateList[0] == 0)
				{
					if (HasUnlockableStateList())
					{
						forceMove = false;
						locked = false;
						activateAfterLastMove = false;
						if (posList.Count > posDistance)
						{
							ResetPathLists();
						}
					}
					HandleMoveStateChange(0);
					if (posList.Count < posDistance && acceptingIgnores)
					{
						acceptingIgnores = false;
						ignoreFrames = posDistance - posList.Count;
					}
				}
				if (ignoreFrames > 0)
				{
					GetComponent<Rigidbody2D>().velocity = Vector2.zero;
					ignoreFrames--;
					anim.SetFloat("speed", 0f);
				}
				else if (posList.Count > 0)
				{
					int num = Mathf.RoundToInt(Mathf.Abs(base.transform.position.x - posList[0].x) * 48f);
					if (num == 0)
					{
						num = Mathf.RoundToInt(Mathf.Abs(base.transform.position.y - posList[0].y) * 48f);
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
					base.transform.position = posList[0];
					if (animControl)
					{
						faceDir = dirList[0];
						anim.SetFloat("dirX", dirList[0].x);
						anim.SetFloat("dirY", dirList[0].y);
						anim.SetFloat("speed", spdList[0]);
					}
					if (moveStateList[0] != lastMoveState)
					{
						HandleMoveStateChange(moveStateList[0]);
					}
					lastMoveState = moveStateList[0];
					posList.RemoveAt(0);
					dirList.RemoveAt(0);
					spdList.RemoveAt(0);
					moveStateList.RemoveAt(0);
				}
				if (doLastMove)
				{
					doLastMove = false;
					ResetPathLists();
				}
				if (posList.Count == 0 && activateAfterLastMove)
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
				if (lastMoveState == 1 && !UnityEngine.Object.FindObjectOfType<OverworldPlayer>().CannotMoveBattleSpecial())
				{
					HandleMoveStateChange(0);
				}
				anim.SetBool("isMoving", false);
				anim.Play("idle");
			}
		}
		GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt((base.transform.position.y - posOffset.y) * -5f);
	}

	private void LateUpdate()
	{
		if (!anim.enabled || (!(customPrefix != "") && !unhappy))
		{
			return;
		}
		if (GetComponent<SpriteRenderer>().sprite.name != curSpriteName)
		{
			string text = "player/" + base.gameObject.name + "/" + GetComponent<SpriteRenderer>().sprite.name;
			if (customPrefix != "")
			{
				text = "player/" + base.gameObject.name + "/" + customPrefix + "/" + GetComponent<SpriteRenderer>().sprite.name + "_" + customPrefix;
			}
			string text2 = text;
			if (unhappy)
			{
				for (int i = 0; i < 6; i++)
				{
					text2 = text2.Replace("_" + i, "_unhappy_" + i);
				}
				if ((int)Util.GameManager().GetFlag(172) == 2 || ((int)Util.GameManager().GetFlag(172) == 1 && base.gameObject.name == "Noelle"))
				{
					text2 = text2.Replace("unhappy", "depressed");
				}
			}
			Sprite sprite = Resources.Load<Sprite>(text2);
			if (sprite != null)
			{
				GetComponent<SpriteRenderer>().sprite = sprite;
			}
			else
			{
				sprite = Resources.Load<Sprite>(text);
				if (sprite != null)
				{
					GetComponent<SpriteRenderer>().sprite = sprite;
				}
			}
		}
		curSpriteName = GetComponent<SpriteRenderer>().sprite.name;
	}

	public void SetCustomSpritesetPrefix(string customPrefix)
	{
		this.customPrefix = customPrefix;
		if (customPrefix == "kr")
		{
			useRunAnim = false;
		}
		else
		{
			useRunAnim = GameManager.GetOptions().runAnimations.value == 1;
		}
	}

	public void AddNewPosition(Vector3 newPos, Vector2 dir, int moveState, float speed)
	{
		newPos += posOffset;
		bool flag = false;
		if (posList.Count != 0)
		{
			flag = posList[posList.Count - 1] != newPos;
		}
		if (posList.Count == 0)
		{
			Vector2 item = new Vector2(newPos.x - base.transform.position.x, newPos.y - base.transform.position.y);
			if (inSamePos)
			{
				item = faceDir;
			}
			for (int i = 1; i <= posDistance; i++)
			{
				posList.Add(Vector3.Lerp(base.transform.position, newPos, (float)i / (float)posDistance));
				if (i == posDistance)
				{
					dirList.Add(dir);
				}
				else
				{
					dirList.Add(item);
				}
				if (inSamePos)
				{
					spdList.Add(0f);
				}
				else
				{
					spdList.Add(1f);
				}
				moveStateList.Add(0);
			}
			flag = true;
		}
		else if (flag)
		{
			posList.Add(newPos);
			dirList.Add(dir);
			spdList.Add(speed);
			moveStateList.Add(moveState);
			if (moveState == 1)
			{
				ForceMove(true);
				Lock();
			}
		}
		if (flag && animControl)
		{
			anim.SetBool("isMoving", true);
		}
		isMoving = flag;
		inSamePos = false;
	}

	public void UseUnhappySprites()
	{
		unhappy = true;
	}

	public void UseHappySprites()
	{
		unhappy = false;
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

	public void ForceMove(bool activateAfterLastMove = false)
	{
		forceMove = true;
		this.activateAfterLastMove = activateAfterLastMove;
		anim.SetBool("isMoving", true);
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

	public void ResetPathLists()
	{
		posList = new List<Vector3>();
		dirList = new List<Vector2>();
		spdList = new List<float>();
		moveStateList = new List<int>();
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
			if (base.gameObject.name == "Susie")
			{
				SetSprite("spr_su_iceslide_" + text);
			}
			else if (base.gameObject.name == "Noelle" && (int)Util.GameManager().GetFlag(172) == 0)
			{
				SetSprite("spr_no_iceslide_" + text);
			}
			break;
		}
		}
	}

	private bool HasUnlockableStateList()
	{
		foreach (int moveState in moveStateList)
		{
			if (moveState == 1)
			{
				return false;
			}
		}
		return true;
	}

	public void SetSprite(string spriteName)
	{
		if (GetComponent<SpriteRenderer>().sprite.name != spriteName)
		{
			GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("player/" + base.gameObject.name + "/" + spriteName);
		}
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
		return isPlayer;
	}

	public bool IsLocked()
	{
		return locked;
	}

	public bool IsUnhappy()
	{
		return unhappy;
	}

	public bool IsRunning()
	{
		return isRunning;
	}

	public override void DoInteract()
	{
	}

	public override int GetEventData()
	{
		return -1;
	}

	public override void MakeDecision(Vector2 index, int id)
	{
	}
}
