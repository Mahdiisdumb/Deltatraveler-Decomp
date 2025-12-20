using System;
using UnityEngine;

public class FallSwitch : Interactable
{
	private bool triggered;

	private int frames;

	private bool playCut;

	private int moveBody = -10;

	[SerializeField]
	private Sprite triggerSprite;

	[SerializeField]
	private Sprite spikeSprite;

	private void Awake()
	{
		if ((int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(34) == 1)
		{
			GetComponent<SpriteRenderer>().sprite = triggerSprite;
			triggered = true;
			GameObject.Find("Spikes").GetComponent<SpriteRenderer>().sprite = spikeSprite;
			GameObject.Find("Spikes").GetComponent<BoxCollider2D>().isTrigger = true;
		}
	}

	private void Update()
	{
		if (!playCut)
		{
			return;
		}
		frames++;
		if (frames % 2 == 0)
		{
			if (moveBody < 0)
			{
				moveBody *= -1;
			}
			else if (moveBody > 0)
			{
				moveBody -= 2;
				moveBody *= -1;
			}
			else
			{
				playCut = false;
				UnityEngine.Object.FindObjectOfType<GameManager>().EnablePlayerMovement();
				UnityEngine.Object.FindObjectOfType<CameraController>().SetFollowPlayer(true);
			}
		}
		UnityEngine.Object.FindObjectOfType<CameraController>().transform.position = UnityEngine.Object.FindObjectOfType<CameraController>().GetClampedPos() + new Vector3((float)moveBody / 48f, 0f);
	}

	public override void DoInteract()
	{
		if (!triggered)
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().SetFlag(34, 1);
			GetComponent<AudioSource>().Play();
			GetComponent<SpriteRenderer>().sprite = triggerSprite;
			triggered = true;
			GameObject.Find("Spikes").GetComponent<SpriteRenderer>().sprite = spikeSprite;
			GameObject.Find("Spikes").GetComponent<BoxCollider2D>().isTrigger = true;
			playCut = true;
			UnityEngine.Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			UnityEngine.Object.FindObjectOfType<CameraController>().SetFollowPlayer(false);
			UnityEngine.Object.FindObjectOfType<CameraController>().transform.position = UnityEngine.Object.FindObjectOfType<CameraController>().GetClampedPos() + new Vector3((float)moveBody / 48f, 0f);
		}
	}

	public override int GetEventData()
	{
		throw new NotImplementedException();
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		throw new NotImplementedException();
	}
}
