using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class OverworldPlayer : MonoBehaviour
{
	private GameManager gm;

	private SpriteRenderer sr;

	private Animator anim;

	private BoxCollider2D col;

	private Rigidbody2D rigid2D;

	private bool canMove;

	private bool movePM;

	private Vector3 lastPos;

	private Vector3 moveLastPos;

	private Vector3 posEffect = Vector3.zero;

	private float spd;

	private float spdMultiplier = 1f;

	private Vector2 faceDir = Vector2.down;

	private Vector3 moveDir = Vector2.zero;

	private int runTimer;

	private int battleId;

	private bool initiating;

	private int iFrame;

	private int iFrameMax;

	private int moveFrames;

	private GameObject soul;

	private Vector2 oldSoulPos;

	private Vector2 soulPos;

	private bool specialBattleFreeze;

	private string curSpriteName = "";

	private bool useCustomSprites;

	private bool sliding;

	private int slideFrames;

	private bool forceSendPositions;

	private bool animControl;

	public bool cellphoneCall;

	public bool noclip;

	private bool isFrisk;

	private bool canWallDance;

	private bool usingStepSounds;

	private string customFootStep = "";

	private AudioSource[] aud;

	private int footstep;

	private float stepEncCounter;

	private int moveState;

	private bool depressed;

	private bool autoRun;

	private bool useRunAnim = true;

	private void Awake()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		sr = base.transform.GetComponent<SpriteRenderer>();
		anim = base.transform.GetComponent<Animator>();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("player/kris_ow");
		col = base.transform.GetComponent<BoxCollider2D>();
		col.offset = new Vector2(0f, -0.55f);
		col.size = new Vector2(0.8f, 0.4f);
		rigid2D = base.transform.GetComponent<Rigidbody2D>();
		rigid2D.bodyType = RigidbodyType2D.Dynamic;
		rigid2D.gravityScale = 0f;
		rigid2D.freezeRotation = true;
		spd = 6f;
		canMove = true;
		initiating = false;
		iFrame = 0;
		iFrameMax = 0;
		moveFrames = 0;
		animControl = true;
		if ((int)gm.GetFlag(107) == 1)
		{
			isFrisk = true;
			canWallDance = true;
			anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("player/frisk_ow");
			ChangeDirection(faceDir);
			GameObject.Find("Susie").GetComponent<OverworldPartyMember>().SetPositionOffset(new Vector3(0f, 0.352f));
		}
		OverworldPartyMember component = GameObject.Find("Susie").GetComponent<OverworldPartyMember>();
		OverworldPartyMember component2 = GameObject.Find("Noelle").GetComponent<OverworldPartyMember>();
		if (gm.SusieInParty())
		{
			component.Activate();
		}
		if (gm.NoelleInParty())
		{
			component2.Activate();
		}
		if (SceneManager.GetActiveScene().buildIndex == 100)
		{
			canWallDance = true;
		}
		sr.enabled = true;
		SetCollision(true);
		aud = GetComponents<AudioSource>();
		autoRun = GameManager.GetOptions().autoRun.value == 1;
		useRunAnim = GameManager.GetOptions().runAnimations.value == 1;
	}

	private void Start()
	{
		if (SceneManager.GetActiveScene().buildIndex == 101 || SceneManager.GetActiveScene().buildIndex == 102)
		{
			EnableStepSounds();
		}
	}

	private void Update()
	{
		depressed = (int)Util.GameManager().GetSessionFlag(6) == 1;
		if (animControl)
		{
			if (!isFrisk)
			{
				anim.SetFloat("speed", (CheckRun() && !depressed) ? 1.5f : 0.75f);
			}
			else
			{
				anim.SetFloat("speed", spd / 6f);
			}
		}
		if (!canMove && animControl)
		{
			spd = 6f;
			runTimer = 0;
			anim.SetBool("isMoving", false);
		}
		else if ((HoldingMoveButtons() || sliding) && canMove)
		{
			rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;
			HandleRun();
			if (sliding)
			{
				spd = 6f;
				runTimer = 0;
			}
			else if (CheckRun())
			{
				if (runTimer <= 60)
				{
					runTimer++;
				}
				if (!isFrisk && runTimer > 10)
				{
					spd = ((runTimer > 60) ? 12 : 10);
				}
				else
				{
					spd = (isFrisk ? 10 : 8);
				}
			}
			else if (!CheckRun())
			{
				spd = 6f;
				runTimer = 0;
			}
		}
		else if (animControl)
		{
			spd = 6f;
			runTimer = 0;
			anim.SetBool("isMoving", false);
			rigid2D.constraints = RigidbodyConstraints2D.FreezeAll;
		}
		if (sliding)
		{
			if (slideFrames % 3 == 0)
			{
				Object.Instantiate(Resources.Load<GameObject>("vfx/SlideDust"), base.transform.position + new Vector3(0f, 0.1f), Quaternion.identity);
			}
			slideFrames++;
		}
		sr.sortingOrder = Mathf.RoundToInt(base.transform.position.y * -5f);
		if (initiating)
		{
			canMove = false;
			iFrame++;
			if (iFrame == 1 || iFrame == 5 || iFrame == 9)
			{
				gm.PlayGlobalSFX("sounds/snd_noise");
				soul.GetComponent<SpriteRenderer>().enabled = true;
			}
			if (iFrame == 3 || iFrame == 7)
			{
				soul.GetComponent<SpriteRenderer>().enabled = false;
			}
			if (iFrame == 11)
			{
				sr.enabled = false;
				gm.PlayGlobalSFX("sounds/snd_battlestart");
			}
			if (iFrame >= 11)
			{
				soul.transform.position = Vector3.Lerp(oldSoulPos, soulPos, ((float)iFrame - 11f) / (float)moveFrames);
			}
			if (iFrame > iFrameMax)
			{
				SetCollision(false);
				initiating = false;
				iFrame = 0;
				gm.StartBattle(battleId);
			}
		}
	}

	private bool CheckRun()
	{
		return UTInput.GetButton("X") ^ autoRun;
	}

	private void LateUpdate()
	{
		if ((((ProperlyMoved() && HoldingMoveButtons()) || sliding) && movePM) || (forceSendPositions && !specialBattleFreeze))
		{
			OverworldPartyMember[] array = Object.FindObjectsOfType<OverworldPartyMember>();
			foreach (OverworldPartyMember overworldPartyMember in array)
			{
				if (overworldPartyMember.IsActivated())
				{
					overworldPartyMember.AddNewPosition(base.transform.position, faceDir, moveState, anim.GetFloat("speed"));
				}
			}
		}
		if (canMove)
		{
			movePM = true;
		}
		else
		{
			movePM = false;
		}
		if (gm.GetMiniPartyMember() > 0 || (int)gm.GetFlag(102) == 1 || (int)gm.GetFlag(178) == 1 || (int)gm.GetFlag(204) == 1 || depressed || (isFrisk && gm.GetFlagInt(108) == 1 && gm.GetFlagInt(13) >= 2 && gm.GetFlagInt(127) == 1))
		{
			useCustomSprites = true;
		}
		else
		{
			useCustomSprites = false;
		}
		if (anim.enabled && useCustomSprites)
		{
			string text = "";
			if (isFrisk)
			{
				if (gm.GetFlagInt(108) == 1 && gm.GetFlagInt(13) >= 2 && gm.GetFlagInt(127) == 1)
				{
					text = "g";
				}
			}
			else if (gm.GetMiniPartyMember() == 1)
			{
				text = "pau";
			}
			else if (depressed)
			{
				text = "depressed";
			}
			else if ((int)gm.GetFlag(102) == 1)
			{
				text = "injured";
			}
			else if ((int)gm.GetFlag(204) == 1 && (int)gm.GetFlag(178) == 1)
			{
				text = "eyehold";
			}
			else if (SceneManager.GetActiveScene().buildIndex == 123)
			{
				text = "hd";
			}
			else if ((bool)Object.FindObjectOfType<UndyneShadow>())
			{
				text = "undynes";
			}
			else if ((int)gm.GetFlag(204) == 1)
			{
				text = "eye";
			}
			else if ((int)gm.GetFlag(178) == 1)
			{
				text = "hold";
			}
			string text2 = "Kris";
			if (isFrisk)
			{
				text2 = "Frisk";
			}
			string text3 = sr.sprite.name + "_" + text;
			if (sr.sprite.name != curSpriteName || text3 != sr.sprite.name)
			{
				Sprite sprite = Resources.Load<Sprite>("player/" + text2 + "/" + text + "/" + text3);
				if (sprite != null)
				{
					sr.sprite = sprite;
				}
			}
			curSpriteName = sr.sprite.name;
		}
		if ((bool)Object.FindObjectOfType<ActionSOUL>())
		{
			string text4 = sr.sprite.name;
			if (isFrisk && gm.GetFlagInt(108) == 1 && text4.EndsWith("_g") && !text4.StartsWith("spr_fr_run"))
			{
				text4 = text4.Substring(0, text4.Length - 2);
			}
			Object.FindObjectOfType<ActionSOUL>().UpdateSprite(text4);
		}
		lastPos = base.transform.position;
		if (IsMoving())
		{
			moveLastPos = base.transform.position;
		}
		rigid2D.velocity = Vector2.zero;
	}

	private void HandleRun()
	{
		moveDir = new Vector3(UTInput.GetAxis("Horizontal"), UTInput.GetAxis("Vertical"));
		if (sliding)
		{
			moveDir.y = -1.75f;
		}
		if (moveDir != Vector3.zero)
		{
			movePM = true;
		}
		if (canWallDance && UTInput.GetButton("Down") && moveDir.y == 1f && col.enabled)
		{
			Physics2D.queriesHitTriggers = false;
			RaycastHit2D raycastHit2D = Physics2D.BoxCast(new Vector2(base.transform.position.x, base.transform.position.y) + col.offset, col.size, 0f, Vector2.up, 0.1f, ~LayerMask.GetMask("Player", "Ignore Raycast"));
			if ((bool)raycastHit2D && raycastHit2D.collider.transform.parent != null && raycastHit2D.collider.transform.parent.name != "OBJ" && raycastHit2D.collider.transform.parent.name != "NPC")
			{
				if (faceDir == Vector2.left || faceDir == Vector2.right)
				{
					faceDir = Vector2.up;
				}
				moveDir = new Vector3(UTInput.GetAxis("Horizontal"), -1f);
			}
			Physics2D.queriesHitTriggers = true;
		}
		if (depressed && spd != 0f)
		{
			spd = 4f;
		}
		rigid2D.MovePosition(base.transform.position + moveDir * spd * spdMultiplier / 48f + posEffect);
		if (sliding)
		{
			faceDir = Vector2.down;
		}
		else if (new List<Vector2>
		{
			Vector2.up,
			Vector2.left,
			Vector2.down,
			Vector2.right
		}.Contains(moveDir))
		{
			faceDir = moveDir;
		}
		else if (0f - moveDir.x == faceDir.x || 0f - moveDir.y == faceDir.y)
		{
			faceDir = new Vector3(0f, moveDir.y);
		}
		ChangeDirection(faceDir);
		if (animControl)
		{
			anim.SetBool("isMoving", ProperlyMovedLastFrame());
			if (ProperlyMoved())
			{
				string text = ((SceneManager.GetActiveScene().buildIndex == 123) ? "runb" : "run");
				if (useRunAnim)
				{
					anim.Play((spd >= (float)(isFrisk ? 8 : 10)) ? text : "walk");
				}
				else
				{
					anim.Play("walk");
				}
			}
		}
		if (!sliding && (bool)Object.FindObjectOfType<StepEncounterer>() && ProperlyMoved())
		{
			stepEncCounter += 0.1f * (CheckRun() ? 1.5f : 0.75f);
			if (stepEncCounter >= 1f)
			{
				stepEncCounter -= 1f;
				Object.FindObjectOfType<StepEncounterer>().AddStep();
			}
		}
	}

	public bool ProperlyMovedLastFrame()
	{
		if (!(Mathf.Round(Mathf.Abs(base.transform.position.x - lastPos.x) * 48f) > 1f))
		{
			return Mathf.Round(Mathf.Abs(base.transform.position.y - lastPos.y) * 48f) > 1f;
		}
		return true;
	}

	public bool ProperlyMoved()
	{
		if (!(Mathf.Round(Mathf.Abs(base.transform.position.x - moveLastPos.x) * 48f) > 1f))
		{
			return Mathf.Round(Mathf.Abs(base.transform.position.y - moveLastPos.y) * 48f) > 1f;
		}
		return true;
	}

	public void StartSliding()
	{
		if (!sliding)
		{
			base.gameObject.layer = 14;
			gm.DisableMenu();
			sliding = true;
			slideFrames = 0;
			anim.enabled = false;
			string text = "";
			if (gm.GetMiniPartyMember() == 1)
			{
				text = "pau";
			}
			else if ((int)gm.GetFlag(102) == 1)
			{
				text = "injured";
			}
			else if ((int)gm.GetFlag(204) == 1)
			{
				text = "eye";
			}
			if (text != "")
			{
				SetSprite(text + "/spr_kr_slide_" + text);
			}
			else
			{
				SetSprite("spr_kr_slide");
			}
			gm.PlayGlobalSFX("sounds/snd_noise");
			GetComponent<AudioSource>().loop = true;
			GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("sounds/snd_paper_surf");
			GetComponent<AudioSource>().Play();
		}
	}

	public void StopSliding()
	{
		if (sliding)
		{
			base.gameObject.layer = 13;
			gm.EnableMenu();
			sliding = false;
			anim.enabled = true;
			GetComponent<AudioSource>().Stop();
			GetComponent<AudioSource>().loop = false;
		}
	}

	public void ForceSendPositions()
	{
		forceSendPositions = true;
	}

	public void FreeSendPositions()
	{
		forceSendPositions = false;
	}

	public bool IsSliding()
	{
		return sliding;
	}

	public void SetSpeedMultiplier(float spdMultiplier)
	{
		this.spdMultiplier = spdMultiplier;
	}

	public void ChangeDirection(Vector2 dir)
	{
		anim.SetFloat("dirX", dir[0]);
		anim.SetFloat("dirY", dir[1]);
	}

	public void SetMovement(bool newMove)
	{
		MonoBehaviour.print(newMove ? "owplayer: Movement Restore" : "owplayer: Movement Revoke");
		if (moveState == 1)
		{
			if (!canMove && !newMove)
			{
				specialBattleFreeze = true;
				MonoBehaviour.print("Special Freeze");
			}
			else if (newMove && specialBattleFreeze)
			{
				specialBattleFreeze = false;
				MonoBehaviour.print("Special UnFreeze");
			}
			else
			{
				MonoBehaviour.print("player sliding: SetMovement() cancelled");
			}
			return;
		}
		if (!col.enabled && newMove)
		{
			SetCollision(true);
		}
		if (canMove && !newMove && IsMoving())
		{
			movePM = false;
		}
		else if (!canMove && newMove && HoldingMoveButtons())
		{
			movePM = true;
		}
		canMove = newMove;
	}

	public void SetMoveState(int moveState)
	{
		if (specialBattleFreeze)
		{
			return;
		}
		this.moveState = moveState;
		switch (moveState)
		{
		case 0:
			EnableAnimator();
			SetSelfAnimControl(true);
			anim.SetFloat("speed", 1f);
			if ((bool)GetComponentInChildren<SnowSculpture>())
			{
				GetComponentInChildren<SnowSculpture>().Break();
			}
			break;
		case 1:
			canMove = false;
			if ((int)gm.GetFlag(204) == 1)
			{
				DisableAnimator();
				string text = "";
				text = ((faceDir.x != 0f) ? ((faceDir.x > 0f) ? "right" : "left") : ((faceDir.y > 0f) ? "up" : "down"));
				string sprite = "spr_kr_iceslide_" + text;
				SetSprite(sprite);
			}
			else
			{
				SetSelfAnimControl(false);
				anim.SetFloat("speed", 0f);
			}
			break;
		}
	}

	public bool CannotMoveBattleSpecial()
	{
		return specialBattleFreeze;
	}

	public void SetPosEffect(Vector3 posEffect)
	{
		this.posEffect = posEffect;
	}

	public bool CanMove()
	{
		return canMove;
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

	public Vector2 GetDirection()
	{
		return new Vector2(anim.GetFloat("dirX"), anim.GetFloat("dirY"));
	}

	public bool IsMoving()
	{
		if (HoldingMoveButtons())
		{
			return canMove;
		}
		return false;
	}

	private bool HoldingMoveButtons()
	{
		if (UTInput.GetAxis("Horizontal") == 0f)
		{
			return UTInput.GetAxis("Vertical") != 0f;
		}
		return true;
	}

	public void InitiateBattle(Vector2 toSoulPos, int frames)
	{
		if (!gm.GetPlayingMusic().Contains("core"))
		{
			gm.PauseMusic();
		}
		gm.DisablePlayerMovement(false);
		GetComponentInChildren<InteractionTrigger>().GetComponent<BoxCollider2D>().enabled = false;
		GameObject.Find("Susie").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("Noelle").GetComponent<SpriteRenderer>().enabled = false;
		SpriteRenderer[] componentsInChildren = GameObject.Find("MAP").GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		Collider2D[] componentsInChildren2 = GameObject.Find("MAP").GetComponentsInChildren<Collider2D>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].enabled = false;
		}
		AudioSource[] componentsInChildren3 = GameObject.Find("MAP").GetComponentsInChildren<AudioSource>();
		foreach (AudioSource audioSource in componentsInChildren3)
		{
			if (audioSource.isPlaying)
			{
				audioSource.enabled = false;
			}
		}
		TilemapRenderer[] componentsInChildren4 = GameObject.Find("MAP").GetComponentsInChildren<TilemapRenderer>();
		for (int i = 0; i < componentsInChildren4.Length; i++)
		{
			componentsInChildren4[i].enabled = false;
		}
		SpriteMask[] componentsInChildren5 = GameObject.Find("MAP").GetComponentsInChildren<SpriteMask>();
		for (int i = 0; i < componentsInChildren5.Length; i++)
		{
			componentsInChildren5[i].enabled = false;
		}
		moveFrames = frames;
		soulPos = toSoulPos + new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
		iFrameMax = 11 + moveFrames + 5;
		soul = Object.Instantiate(Resources.Load<GameObject>("overworld/OWSoul"), base.transform);
		if (isFrisk)
		{
			soul.transform.localPosition = new Vector3(0f, -0.254f);
		}
		oldSoulPos = soul.transform.position;
		soul.transform.localScale = new Vector2(0.5f, 0.5f);
		soul.GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder + 300;
		soul.GetComponent<SpriteRenderer>().enabled = false;
		initiating = true;
	}

	public void InitiateBattle()
	{
		InitiateBattle(new Vector2(-5.646f, -4.48f), 18);
	}

	public void InitiateBattle(int btl)
	{
		battleId = btl;
		InitiateBattle();
		SetCustomSoulColor(btl);
	}

	public void InitiateBattle(int btl, Vector2 toSoulPos, int frames)
	{
		battleId = btl;
		InitiateBattle(toSoulPos, frames);
		SetCustomSoulColor(btl);
	}

	private void SetCustomSoulColor(int bt)
	{
		if (bt != 53)
		{
			soul.GetComponent<SpriteRenderer>().color = SOUL.GetSOULColorByID(Util.GameManager().GetFlagInt(312));
		}
	}

	public void HandleSpawn(Vector3 spawnPos, Vector2 spawnDir)
	{
		base.transform.position = spawnPos;
		ChangeDirection(spawnDir);
		OverworldPartyMember component = GameObject.Find("Susie").GetComponent<OverworldPartyMember>();
		OverworldPartyMember component2 = GameObject.Find("Noelle").GetComponent<OverworldPartyMember>();
		if (gm.SusieInParty())
		{
			component.transform.position = spawnPos + component.GetPositionOffset();
			component.ChangeDirection(spawnDir);
			component.SpawnInSamePos();
		}
		if (gm.NoelleInParty())
		{
			component2.transform.position = spawnPos + component2.GetPositionOffset();
			component2.ChangeDirection(spawnDir);
			component2.SpawnInSamePos();
		}
		if ((bool)Object.FindObjectOfType<MoleFriend>())
		{
			Object.FindObjectOfType<MoleFriend>().transform.position = spawnPos + Object.FindObjectOfType<MoleFriend>().GetPositionOffset();
			Object.FindObjectOfType<MoleFriend>().ChangeDirection(spawnDir);
			Object.FindObjectOfType<MoleFriend>().SpawnInSamePos();
		}
	}

	public void SetSprite(string spriteName)
	{
		if (isFrisk)
		{
			sr.sprite = Resources.Load<Sprite>("player/Frisk/" + spriteName);
		}
		else
		{
			sr.sprite = Resources.Load<Sprite>("player/Kris/" + spriteName);
		}
	}

	public void SetSprite(Sprite sprite)
	{
		sr.sprite = sprite;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (CheckRun())
		{
			spd = (isFrisk ? 10 : 8);
			runTimer = 0;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (CheckRun() && Mathf.Abs(moveDir.x) + Mathf.Abs(moveDir.y) == 2f)
		{
			spd = (isFrisk ? 10 : 8);
			runTimer = 0;
		}
	}

	public float GetSpeed()
	{
		return spd;
	}

	public void ToggleNoclip()
	{
		noclip = !noclip;
		col.enabled = !noclip;
	}

	public bool GetNoclip()
	{
		return noclip;
	}

	public void SetCollision(bool onoff)
	{
		GetComponentInChildren<InteractionTrigger>().GetComponent<BoxCollider2D>().enabled = onoff;
		if (!noclip)
		{
			col.enabled = onoff;
		}
	}

	public bool IsInitiatingBattle()
	{
		return initiating;
	}

	public void EnableStepSounds(string customFootStep = "")
	{
		this.customFootStep = customFootStep;
		usingStepSounds = true;
	}

	public void DisableStepSounds()
	{
		usingStepSounds = false;
	}

	public void PlayStepSound()
	{
		if (!usingStepSounds)
		{
			return;
		}
		if (customFootStep == "")
		{
			if (aud[footstep].clip == null || aud[footstep].clip.name != "snd_step" + (footstep + 1))
			{
				aud[footstep].clip = Resources.Load<AudioClip>("sounds/snd_step" + (footstep + 1));
			}
		}
		else
		{
			aud[footstep].clip = Resources.Load<AudioClip>(customFootStep);
		}
		aud[footstep].Play();
		footstep = (footstep + 1) % 2;
	}
}
