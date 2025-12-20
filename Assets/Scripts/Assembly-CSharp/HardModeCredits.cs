using System;
using UnityEngine;

public class HardModeCredits : MonoBehaviour
{
	private int frames;

	private Transform credits;

	private SpriteRenderer sarah;

	private Sprite[] sarahSprites;

	private Sprite[] scootSprites;

	private Sprite[] sophieSprites;

	private Sprite[] chloeSprites;

	private Sprite[] eriSprites;

	private Sprite[] veeSprites;

	private float jevilSpin = 1f;

	private bool diddyFlip;

	private Animator tune;

	private Sprite[] beethSprites;

	private Animator beeth;

	private Transform sonas;

	private float tuneVelocity;

	private AudioSource audio;

	private readonly int CREDITS_LENGTH = 3056;

	private void Awake()
	{
		credits = base.transform.GetChild(0);
		sonas = credits.Find("SonasHM");
		sarah = sonas.Find("Sarah").GetComponent<SpriteRenderer>();
		tune = sonas.Find("TuneHero").GetComponent<Animator>();
		beeth = sonas.Find("BeethDog").GetComponent<Animator>();
		sarahSprites = LoadSprites("ui/spr_bunny_dance", 2);
		scootSprites = LoadSprites("overworld/npcs/staff/sillies/spr_scoot_fops", 2);
		sophieSprites = LoadSprites("overworld/npcs/staff/sillies/spr_sophie_fox", 2);
		beethSprites = LoadSprites("overworld/npcs/staff/sillies/spr_beeth_dog", 2);
		eriSprites = LoadSprites("overworld/npcs/staff/sillies/spr_eri_fops", 6);
		chloeSprites = Resources.LoadAll<Sprite>("overworld/npcs/staff/sillies/spr_luma_doggie");
		veeSprites = Resources.LoadAll<Sprite>("overworld/npcs/staff/sillies/spr_vee_sleep");
		audio = GetComponent<AudioSource>();
	}

	private Sprite[] LoadSprites(string baseName, int num)
	{
		Sprite[] array = new Sprite[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = Resources.Load<Sprite>(baseName + "_" + i);
		}
		return array;
	}

	private void Update()
	{
		frames++;
		if (frames == 1)
		{
			audio.Play();
		}
		credits.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0f, CREDITS_LENGTH), (float)(frames - 60) / (float)CREDITS_LENGTH * 2f);
		float num = 0.5084746f;
		float num2 = audio.time / num;
		sarah.sprite = sarahSprites[Mathf.FloorToInt(num2 * 2f) % 2];
		sarah.GetComponent<SpriteRenderer>().flipX = Mathf.Floor(num2) % 4f > 1f;
		sarah.transform.localPosition = new Vector2(-212f, -420f + Mathf.Abs(Mathf.Sin(num2 * (float)Math.PI)) * 12f);
		sonas.Find("Scoot").GetComponent<SpriteRenderer>().sprite = scootSprites[Mathf.FloorToInt(num2) % 2];
		sonas.Find("Sophie").GetComponent<SpriteRenderer>().sprite = sophieSprites[Mathf.FloorToInt(num2) / 2 % 2];
		float num3 = audio.clip.length / num;
		sonas.Find("Eribetra").GetComponent<SpriteRenderer>().sprite = eriSprites[(!(num3 - num2 < 3f) && !(num2 * 2f % 12f >= 6f)) ? ((uint)(num2 * 2f % 6f)) : 0];
		if (credits.localPosition.y >= 900f)
		{
			jevilSpin += 0.1f;
			sonas.Find("Jevilhumor").GetComponent<Animator>().SetFloat("speed", jevilSpin);
		}
		sonas.Find("Chloe").GetComponent<SpriteRenderer>().sprite = chloeSprites[Mathf.FloorToInt(num2 * 2f) % 4];
		sonas.Find("Marxvee").GetComponent<SpriteRenderer>().sprite = veeSprites[Mathf.FloorToInt(num2 / 2f) % 2];
		if (credits.localPosition.y >= 1300f)
		{
			tune.enabled = true;
		}
		if (tune.enabled)
		{
			tuneVelocity += 0.5f;
			tune.transform.localPosition += new Vector3(0f - tuneVelocity, 0f);
			if (tune.transform.localPosition.x <= -300f)
			{
				tune.transform.localPosition = new Vector3(-306f, tune.transform.localPosition.y);
				tune.transform.localScale = new Vector3(32f, 66f);
				tune.enabled = false;
			}
		}
		if (!diddyFlip && credits.localPosition.y >= 1919f)
		{
			sonas.Find("Autumn").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/staff/sillies/spr_autumn_silly_1");
			diddyFlip = true;
		}
		if (!beeth.enabled)
		{
			beeth.GetComponent<SpriteRenderer>().sprite = beethSprites[Mathf.FloorToInt(num2 * 2f) % 2];
			beeth.GetComponent<SpriteRenderer>().flipX = Mathf.Floor(num2) % 4f > 1f;
			if (credits.localPosition.y >= 1519f)
			{
				sonas.Find("RealisticExplosion").GetComponent<Animator>().enabled = true;
			}
			if (credits.localPosition.y >= 1529f)
			{
				beeth.enabled = true;
			}
		}
		if (frames >= CREDITS_LENGTH / 2 + 60)
		{
			audio.volume = Mathf.Lerp(1f, 0f, (float)(frames - (CREDITS_LENGTH / 2 + 60)) / 60f);
		}
		if (frames == CREDITS_LENGTH / 2 + 120)
		{
			Util.GameManager().ForceLoadArea(6);
		}
	}
}
