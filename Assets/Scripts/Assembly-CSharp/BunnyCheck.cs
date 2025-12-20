using System;
using UnityEngine;

public class BunnyCheck : MonoBehaviour
{
	private SpriteRenderer bnuuy;

	private Sprite[] bunSprites;

	private int whatTheBunDoin;

	private AudioSource bunnyMusic;

	private Vector2 bunInitialPos;

	private float songBPM = 140f;

	private int lastStep = -1;

	private void Start()
	{
		whatTheBunDoin = UnityEngine.Random.Range(0, 2);
		bnuuy = GetComponent<SpriteRenderer>();
		bunnyMusic = GetComponent<AudioSource>();
		bunnyMusic.clip = Resources.Load<AudioClip>((whatTheBunDoin == 1) ? "music/mus_vibe_of_bunny" : "music/mus_dogcheck");
		songBPM = ((whatTheBunDoin == 1) ? 118f : 140f);
		bunnyMusic.Play();
		bunInitialPos = bnuuy.transform.position;
		if (whatTheBunDoin == 1)
		{
			bunSprites = new Sprite[2]
			{
				Resources.Load<Sprite>("ui/spr_bunny_dance_0"),
				Resources.Load<Sprite>("ui/spr_bunny_dance_1")
			};
		}
		else
		{
			bunSprites = new Sprite[2]
			{
				Resources.Load<Sprite>("ui/spr_bunny_sleep_check_0"),
				Resources.Load<Sprite>("ui/spr_bunny_sleep_check_1")
			};
		}
	}

	private void Update()
	{
		float num = 60f / songBPM;
		float num2 = bunnyMusic.time / num;
		if (whatTheBunDoin == 1)
		{
			bnuuy.sprite = bunSprites[(int)Mathf.Floor(num2 * 2f) % 2];
			bnuuy.GetComponent<SpriteRenderer>().flipX = Mathf.Floor(num2) % 4f > 1f;
			bnuuy.transform.position = new Vector2(0f, bunInitialPos.y + Mathf.Abs(Mathf.Sin(num2 * (float)Math.PI)) / 2f);
			return;
		}
		bnuuy.sprite = bunSprites[(int)Mathf.Floor(num2 / 2f) % 2];
		bnuuy.transform.localScale = new Vector2(2f, 2f + Mathf.Sin(num2 * (float)Math.PI / 2f) / 4f);
		int num3 = (int)Mathf.Floor(num2 / 2f);
		if (num3 != lastStep)
		{
			new GameObject("SnoozeZ").AddComponent<SnoreParticle>().CreateSnore(base.transform.position + new Vector3(-1f, 1f, 0f), 0.5f);
		}
		lastStep = num3;
	}
}
