using System;
using UnityEngine;

public class WaterfallGlowParticle : Interactable
{
	[SerializeField]
	private Sprite[] sprites;

	private int sprIndex;

	private int delay = 20;

	private int sprFrames;

	private Vector3 startPos;

	private float speed = 1f;

	private Vector3 direction;

	private Vector3 gravDirection;

	private bool gravEnactment;

	private Vector3 prevPos;

	private Rigidbody rigid;

	private bool speedingUp;

	private void Awake()
	{
		startPos = base.transform.position;
		rigid = GetComponent<Rigidbody>();
		direction = new Vector3(Mathf.Sin(UnityEngine.Random.Range(0f, (float)Math.PI * 2f)), Mathf.Cos(UnityEngine.Random.Range(0f, (float)Math.PI * 2f)));
		base.transform.GetChild(0).GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("sounds/snd_oddtalk" + UnityEngine.Random.Range(1, 3));
	}

	private void Update()
	{
		delay--;
		sprFrames++;
		if (sprFrames == 2)
		{
			sprIndex++;
			if (sprIndex >= sprites.Length)
			{
				sprIndex = 0;
			}
			GetComponent<SpriteRenderer>().sprite = sprites[sprIndex];
			sprFrames = 0;
		}
		if (delay <= 0)
		{
			if (Mathf.Abs(speed) < 0.5f)
			{
				speed += 0.1f;
				direction = Vector3.MoveTowards(base.transform.position, startPos + new Vector3(UnityEngine.Random.Range(0, 41), UnityEngine.Random.Range(0, 41)) / 24f, 1f) - base.transform.position;
				speedingUp = true;
			}
			else
			{
				speedingUp = false;
			}
			gravDirection = new Vector3(Mathf.Sin(UnityEngine.Random.Range(0f, (float)Math.PI * 2f)), Mathf.Cos(UnityEngine.Random.Range(0f, (float)Math.PI * 2f)));
			gravEnactment = true;
			delay = 30 + UnityEngine.Random.Range(0, 21);
		}
		prevPos = base.transform.position;
		rigid.MovePosition(base.transform.position + direction * speed / 24f);
		if (gravEnactment)
		{
			rigid.AddForce(gravDirection * 0.03f * 9.8f, ForceMode.Acceleration);
		}
		speed = Mathf.Abs(speed) + (speedingUp ? 0.04f : (-0.04f));
		if (speed < 0f)
		{
			speed = 0f;
		}
	}

	public override void DoInteract()
	{
		GetComponent<AudioSource>().Play();
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
