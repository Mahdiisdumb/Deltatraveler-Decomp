using System;
using UnityEngine;

public class OverworldBoneBullet : ActionBulletBase
{
	private bool spinning;

	private bool modifyColor = true;

	private int spinFrames;

	private bool playSound;

	private bool tracking;

	private Vector3 relativePosition = Vector3.zero;

	private bool turnLeft;

	private bool moving;

	private Vector3 direction;

	private float velocity;

	protected override void Awake()
	{
		base.Awake();
		turnLeft = UnityEngine.Object.FindObjectOfType<OverworldPlayer>().transform.position.x < base.transform.position.x;
		baseDmg = 6;
	}

	protected override void Update()
	{
		base.Update();
		if (!moving || spinning)
		{
			return;
		}
		if (spinFrames > 0)
		{
			spinFrames = 0;
			if (playSound)
			{
				PlaySFX("sounds/snd_spearthrow");
			}
		}
		velocity += 1f;
		base.transform.position += direction * velocity / 48f;
		if (Mathf.Abs(base.transform.position.x) > 63f || Mathf.Abs(base.transform.position.y) > 55f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void LateUpdate()
	{
		if (spinning)
		{
			spinFrames++;
			OverworldPlayer overworldPlayer = UnityEngine.Object.FindObjectOfType<OverworldPlayer>();
			float num = (float)spinFrames / 24f;
			num = Mathf.Sin(num * (float)Math.PI * 0.5f);
			if (tracking)
			{
				base.transform.position = overworldPlayer.transform.position + relativePosition;
			}
			if (modifyColor)
			{
				base.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, num);
			}
			base.transform.up = base.transform.position - overworldPlayer.transform.position;
			base.transform.rotation = Quaternion.Lerp(Quaternion.identity, base.transform.rotation, num);
			base.transform.GetChild(0).localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, turnLeft ? 360 : (-360), num));
			if (spinFrames == 24)
			{
				spinning = false;
				direction = new Vector3(Mathf.Sin(base.transform.rotation.eulerAngles.z * ((float)Math.PI / 180f)), 0f - Mathf.Cos(base.transform.rotation.eulerAngles.z * ((float)Math.PI / 180f)));
			}
		}
	}

	public void StartSpinning(bool modifyColor = true)
	{
		this.modifyColor = modifyColor;
		spinning = true;
		spinFrames = 0;
	}

	public void StartMoving(bool playSound = false)
	{
		this.playSound = playSound;
		moving = true;
	}

	public void StartTracking(Vector3 relativePosition)
	{
		tracking = true;
		this.relativePosition = relativePosition;
	}
}
