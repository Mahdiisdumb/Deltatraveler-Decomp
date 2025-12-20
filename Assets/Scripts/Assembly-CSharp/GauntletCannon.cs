using System;
using UnityEngine;

public class GauntletCannon : MonoBehaviour
{
	private readonly float HIGHEST_Z_ANGLE = -45f;

	private readonly float LOWEST_Z_ANGLE = 37f;

	private readonly int AIM_TIMER = 30;

	private readonly int FIRE_SPEED = 6;

	private GameObject prefab;

	private float highestZAngle;

	private float lowestZAngle;

	private bool playAudio = true;

	private int state;

	private int frames;

	protected AudioSource aud;

	private Transform wheel;

	private Transform cannon;

	private Vector3 velocity;

	private float oocDir;

	private void Awake()
	{
		aud = GetComponent<AudioSource>();
		prefab = Resources.Load<GameObject>("overworld/bullets/GauntletCannonBall");
		base.transform.localScale = new Vector3((base.transform.localPosition.x > 0f) ? 1 : (-1), 1f, 1f);
		playAudio = Mathf.Sign(base.transform.localScale.x) == 1f;
		highestZAngle = HIGHEST_Z_ANGLE;
		lowestZAngle = LOWEST_Z_ANGLE;
		if (playAudio)
		{
			aud.Play();
		}
		wheel = base.transform.Find("Wheel");
		cannon = base.transform.Find("Cannon");
	}

	private void Update()
	{
		frames++;
		if (state == 0)
		{
			if (frames <= 20)
			{
				base.transform.localPosition = new Vector3(Mathf.Lerp(7.736f, 6.023f, (float)frames / 20f) * base.transform.localScale.x, -5.55f);
				wheel.localEulerAngles = new Vector3(0f, 0f, frames * 18);
				if (frames == 20)
				{
					aud.loop = false;
					PlaySFX("sounds/snd_cannonaim");
				}
			}
			else if (frames <= 40)
			{
				cannon.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0f, highestZAngle, (float)(frames - 20) / 20f) + 53f);
				if (frames == 40)
				{
					PlaySFX("sounds/snd_cannonready");
				}
			}
			if (frames == 60)
			{
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if (frames % FIRE_SPEED == 1)
			{
				float direction = cannon.localEulerAngles.z * base.transform.localScale.x;
				PlaySFX("sounds/snd_bomb");
				UnityEngine.Object.Instantiate(prefab, cannon.GetChild(0).position, Quaternion.identity, base.transform.parent).GetComponent<GauntletCannonBall>().SetDirection(direction);
			}
			float num = (float)(frames % FIRE_SPEED) / ((float)FIRE_SPEED / 2f);
			num = Mathf.Sin(num * (float)Math.PI * 0.5f);
			cannon.localScale = Vector3.Lerp(new Vector3(1.1f, 0.9f, 1f), new Vector3(1f, 1f, 1f), num);
			int num2 = frames % AIM_TIMER;
			bool num3 = frames / AIM_TIMER % 2 == 1;
			float num4 = (float)num2 / (float)AIM_TIMER;
			if (num3)
			{
				num4 = 1f - num4;
			}
			cannon.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(highestZAngle, lowestZAngle, num4) + 53f);
			if (frames >= AIM_TIMER * 4)
			{
				state = 2;
				frames = 0;
				velocity = new Vector3((float)UnityEngine.Random.Range(1, 3) / 48f * (0f - base.transform.localScale.x), 0f);
				oocDir = UnityEngine.Random.Range(2f, 4f) * (float)((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
			}
		}
		else if (state == 2)
		{
			base.transform.localEulerAngles += new Vector3(0f, 0f, oocDir);
			velocity.y += 1f / 64f;
			base.transform.position += velocity;
			if (base.transform.localPosition.y > 7f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public void PlaySFX(string sfx, float pitch = 1f)
	{
		if (playAudio)
		{
			aud.clip = Resources.Load<AudioClip>(sfx);
			aud.pitch = pitch;
			aud.Play();
		}
	}
}
