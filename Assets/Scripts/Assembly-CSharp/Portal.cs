using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
	[SerializeField]
	private bool isBlue = true;

	[SerializeField]
	private bool isActivated;

	[SerializeField]
	private int flag = 8;

	private int curFlagVal;

	private string colorEnd;

	private int frames;

	private int rFrames;

	private bool isOpen;

	private bool isConnected;

	private Portal otherPortal;

	private MirrorPlayer mPlayer;

	private bool isTeleporting;

	private Vector3 origPos;

	private AudioSource aud;

	private AudioSource amb;

	private Animator anim;

	private void Awake()
	{
		curFlagVal = (int)Util.GameManager().GetFlag(flag);
		frames = 0;
		rFrames = 0;
		isOpen = false;
		isConnected = false;
		isTeleporting = false;
		otherPortal = null;
		aud = GetComponents<AudioSource>()[0];
		amb = GetComponents<AudioSource>()[1];
		anim = GetComponent<Animator>();
		anim.SetBool("isBlue", isBlue);
		colorEnd = "blue";
		if (!isBlue)
		{
			colorEnd = "orange";
		}
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			spriteRenderer.sortingOrder += GetComponent<SpriteRenderer>().sortingOrder;
			if (spriteRenderer.gameObject.name == "MirrorPlayer")
			{
				spriteRenderer.sortingOrder--;
				mPlayer = spriteRenderer.GetComponent<MirrorPlayer>();
			}
			if (spriteRenderer.gameObject.name == "Portal")
			{
				spriteRenderer.sprite = Resources.Load<Sprite>("overworld/spr_portal_out_" + colorEnd);
			}
			if (spriteRenderer.gameObject.name == "Glow")
			{
				spriteRenderer.sprite = Resources.Load<Sprite>("overworld/spr_portal_glow_" + colorEnd);
			}
			if (spriteRenderer.gameObject.name == "Inner")
			{
				spriteRenderer.GetComponent<Animator>().Play(colorEnd);
			}
		}
		if (curFlagVal == 1 && !isActivated)
		{
			isActivated = true;
		}
		else if (curFlagVal == 1 && isActivated)
		{
			isActivated = false;
		}
		mPlayer.Disable();
	}

	private void Update()
	{
		if (curFlagVal != (int)Util.GameManager().GetFlag(flag))
		{
			curFlagVal = (int)Util.GameManager().GetFlag(flag);
			isActivated = !isActivated;
		}
		if (!isActivated && anim.GetBool("activated"))
		{
			anim.Play("Off");
			anim.SetBool("activated", value: false);
		}
		if (!isActivated && isOpen)
		{
			isOpen = false;
			isConnected = false;
			frames = 0;
			rFrames = 0;
			base.transform.GetChild(0).localScale = Vector2.zero;
			aud.clip = Resources.Load<AudioClip>("sounds/snd_portal_close");
			aud.Play();
			amb.Stop();
		}
		if ((bool)otherPortal && (!isConnected || (!otherPortal.IsOpen() && isConnected)))
		{
			isConnected = false;
			rFrames = 0;
			mPlayer.Disable();
			mPlayer.GetComponent<SpriteRenderer>().enabled = false;
			base.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>()
				.color = new Color(1f, 1f, 1f, 1f);
			otherPortal = null;
		}
		if (isActivated && !anim.GetBool("activated"))
		{
			anim.Play("Activate");
		}
		if (isActivated && anim.GetBool("activated") && (anim.GetCurrentAnimatorStateInfo(0).IsName("On (Blue)") || anim.GetCurrentAnimatorStateInfo(0).IsName("On (Orange)")) && !isOpen)
		{
			frames++;
			if (frames == 1)
			{
				aud.clip = Resources.Load<AudioClip>("sounds/snd_portal_open_" + colorEnd);
				aud.Play();
				amb.Play();
			}
			Vector2 vector = new Vector2(1.1f, 1.1f);
			if (frames < 5)
			{
				base.transform.GetChild(0).localScale = Vector2.Lerp(Vector2.zero, vector, (float)frames / 5f);
			}
			else
			{
				float num = (float)(frames - 5) / 10f;
				base.transform.GetChild(0).localScale = Vector2.Lerp(vector, new Vector2(1f, 1f), Mathf.Sin(num * MathF.PI * 0.5f));
			}
			if (frames == 15)
			{
				isOpen = true;
				frames = 0;
			}
		}
		if (isOpen)
		{
			frames++;
			if (frames < 30)
			{
				base.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>()
					.color = new Color(1f, 1f, 1f, (float)frames / 30f);
			}
			else
			{
				base.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>()
					.color = new Color(1f, 1f, 1f, (float)(60 - frames) / 30f);
			}
			if (frames == 60)
			{
				frames = 0;
			}
			if (!isConnected)
			{
				Portal[] array = Util.FindObjectsOfType<Portal>();
				foreach (Portal portal in array)
				{
					if ((isBlue && portal.IsOpen() && portal.IsOrange()) || (!isBlue && portal.IsOpen() && portal.IsBlue()))
					{
						isConnected = true;
						otherPortal = portal;
						mPlayer.SetParent(otherPortal.transform);
						mPlayer.Enable();
						break;
					}
				}
			}
		}
		if (isConnected && !isTeleporting && rFrames < 10)
		{
			rFrames++;
			base.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>()
				.color = new Color(1f, 1f, 1f, (float)(10 - rFrames) / 10f);
		}
		if (isConnected && isTeleporting)
		{
			rFrames++;
			if (rFrames == 1)
			{
				aud.clip = Resources.Load<AudioClip>("sounds/snd_portal_enter");
				aud.Play();
			}
			Util.FindObjectOfType<CameraController>().transform.position = Vector3.Lerp(origPos, Util.FindObjectOfType<CameraController>().GetClampedPos(), (float)rFrames / 10f);
			if (rFrames == 10)
			{
				Util.FindObjectOfType<CameraController>().SetFollowPlayer(follow: true);
				Util.GameManager().EnablePlayerMovement();
				isTeleporting = false;
			}
		}
	}

	public bool IsActivated()
	{
		return isActivated;
	}

	public bool IsOpen()
	{
		return isOpen;
	}

	public bool IsBlue()
	{
		return isBlue;
	}

	public bool IsOrange()
	{
		return !isBlue;
	}

	public bool IsTeleporting()
	{
		return isTeleporting;
	}

	public void Activate()
	{
		isActivated = true;
	}

	public void Deactivate()
	{
		isActivated = false;
	}

	public void TurnOff()
	{
		if (isActivated)
		{
			isOpen = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision && (bool)collision.gameObject.GetComponent<OverworldPlayer>())
		{
			collision.gameObject.layer = 2;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if ((bool)collision)
		{
			OverworldPlayer component = collision.gameObject.GetComponent<OverworldPlayer>();
			if ((bool)component && isConnected && !otherPortal.IsTeleporting() && component.transform.position.y - base.transform.position.y >= -0.063f)
			{
				Util.FindObjectOfType<CameraController>().SetFollowPlayer(follow: false);
				Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: true);
				origPos = Util.FindObjectOfType<CameraController>().transform.position;
				component.transform.position = mPlayer.transform.position;
				component.ChangeDirection(Vector2.down);
				isTeleporting = true;
				rFrames = 0;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if ((bool)collision && (bool)collision.gameObject.GetComponent<OverworldPlayer>() && !isTeleporting)
		{
			collision.gameObject.layer = 0;
		}
	}
}
