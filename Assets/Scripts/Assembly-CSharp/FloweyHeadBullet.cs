using System;
using UnityEngine;

public class FloweyHeadBullet : BulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private Transform top;

	private Transform cen;

	private Transform bot;

	private Vector3 topBite;

	private Vector3 botBite;

	private int biteAttempts;

	private bool hard;

	private bool hardmode;

	private float aimSpeed = 20f;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 7;
		destroyOnHit = false;
		GetComponent<BoxCollider2D>().enabled = false;
		base.transform.position = UnityEngine.Object.FindObjectOfType<Flowey>().GetPart("head").transform.position;
		UnityEngine.Object.FindObjectOfType<Flowey>().GetPart("head").GetComponent<SpriteRenderer>()
			.enabled = false;
		PlaySFX("sounds/snd_spearappear");
		top = base.transform.Find("Top");
		cen = base.transform.Find("Center");
		bot = base.transform.Find("Bottom");
		tpGrazeValueReuse = 1f;
		hardmode = (int)Util.GameManager().GetFlag(108) == 1;
		if (hardmode)
		{
			aimSpeed = 16f;
		}
	}

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(0f, 1.67f), 0.35f);
			if (frames >= 10)
			{
				GetComponent<SpriteRenderer>().sprite = sprites[(frames - 10) / 3 % 3];
				base.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(2f, 2f, 1f), (float)(frames - 10) / 8f);
				if (frames == 18)
				{
					frames = 0;
					state = 1;
					GetComponent<SpriteRenderer>().enabled = false;
					top.GetComponent<SpriteRenderer>().enabled = true;
					bot.GetComponent<SpriteRenderer>().enabled = true;
					cen.GetComponent<SpriteRenderer>().enabled = true;
				}
			}
		}
		if (state == 1)
		{
			frames++;
			top.localPosition = new Vector3(Mathf.Lerp(top.localPosition.x, UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.x / 2f, (float)frames / aimSpeed), Mathf.Lerp(top.localPosition.y, 0f, 0.35f));
			bot.localPosition = new Vector3(Mathf.Lerp(bot.localPosition.x, UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.x / 2f, (float)frames / aimSpeed), Mathf.Lerp(bot.localPosition.y, -1.76f, 0.35f));
			if (bot.localPosition.y < -1.376f)
			{
				bot.GetComponent<SpriteRenderer>().sortingOrder = 199;
			}
			if (frames == 20)
			{
				PlaySFX("sounds/snd_ehurt1");
				state = 2;
				frames = 0;
				top.GetComponent<SpriteRenderer>().sortingOrder = 199;
				top.GetComponent<BoxCollider2D>().enabled = true;
				bot.GetComponent<BoxCollider2D>().enabled = true;
			}
		}
		if (state == 2)
		{
			frames++;
			if (frames <= 10)
			{
				top.localPosition += new Vector3(0f, 1f / 48f);
				bot.localPosition -= new Vector3(0f, 1f / 48f);
				if (frames == 10)
				{
					topBite = top.localPosition;
					botBite = bot.localPosition;
				}
			}
			else if (frames <= 17)
			{
				top.localPosition = new Vector3(top.localPosition.x, Mathf.Lerp(topBite.y, -0.9f, (float)(frames - 10) / 7f));
				bot.localPosition = new Vector3(bot.localPosition.x, Mathf.Lerp(botBite.y, -0.9f, (float)(frames - 10) / 7f));
				if (frames == 17)
				{
					UnityEngine.Object.FindObjectOfType<BattleCamera>().BlastShake();
					PlaySFX("sounds/snd_crash");
					topBite = top.localPosition;
					botBite = bot.localPosition;
					if (hard)
					{
						if (hardmode)
						{
							for (int i = 0; i < 30; i++)
							{
								FloweyPelletStandard component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/flowey/FloweyPelletStandard"), new Vector3(topBite.x * 2f, UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.y), Quaternion.identity, base.transform.parent).GetComponent<FloweyPelletStandard>();
								component.GetComponent<AudioSource>().volume = 0f;
								component.SetPremadeVelocity(new Vector3(Mathf.Cos((float)(i * 12) * ((float)Math.PI / 180f)) / 9.6f, Mathf.Sin((float)(i * 12) * ((float)Math.PI / 180f)) / 9.6f));
								component.GetComponent<SpriteRenderer>().sortingOrder = 202;
								component.SetBaseDamage(4);
							}
						}
						else
						{
							for (int j = 0; j < 12; j++)
							{
								FloweyPelletStandard component2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/flowey/FloweyPelletStandard"), new Vector3(topBite.x * 2f, UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.y), Quaternion.identity, base.transform.parent).GetComponent<FloweyPelletStandard>();
								component2.GetComponent<AudioSource>().volume = 0f;
								component2.SetPremadeVelocity(new Vector3(Mathf.Cos((float)(j * 30) * ((float)Math.PI / 180f)) / 9.6f, Mathf.Sin((float)(j * 30) * ((float)Math.PI / 180f)) / 9.6f));
								component2.GetComponent<SpriteRenderer>().sortingOrder = 202;
								component2.SetBaseDamage(4);
							}
						}
					}
				}
			}
			else
			{
				int num = 25 - frames;
				if (num < 0)
				{
					num = 0;
				}
				Vector3 vector = new Vector3(UnityEngine.Random.Range(0, 3) - 1, UnityEngine.Random.Range(0, 3) - 1) * ((float)num / 96f);
				Vector3 vector2 = new Vector3(UnityEngine.Random.Range(0, 3) - 1, UnityEngine.Random.Range(0, 3) - 1) * ((float)num / 96f);
				top.localPosition = topBite + vector;
				bot.localPosition = botBite + vector2;
			}
			if ((frames == 25 && !hard) || (frames == 35 && hard))
			{
				biteAttempts++;
				frames = 0;
				if (biteAttempts == 5)
				{
					top.GetComponent<BoxCollider2D>().enabled = false;
					bot.GetComponent<BoxCollider2D>().enabled = false;
					state = 3;
				}
				else
				{
					state = 1;
				}
			}
		}
		if (state == 3)
		{
			frames++;
			top.localPosition = Vector3.Lerp(top.localPosition, Vector3.zero, 0.35f);
			bot.localPosition = Vector3.Lerp(bot.localPosition, Vector3.zero, 0.35f);
			base.transform.localScale = Vector3.Lerp(new Vector3(2f, 2f, 1f), new Vector3(1f, 1f, 1f), (float)frames / 10f);
			base.transform.position = Vector3.Lerp(base.transform.position, UnityEngine.Object.FindObjectOfType<Flowey>().GetPart("head").transform.position, (float)frames / 10f);
			if (frames == 10)
			{
				UnityEngine.Object.FindObjectOfType<Flowey>().GetPart("head").GetComponent<SpriteRenderer>()
					.enabled = true;
				UnityEngine.Object.Destroy(base.transform.parent.gameObject);
			}
		}
		cen.localPosition = new Vector3(top.localPosition.x, top.localPosition.y - 0.229f);
		cen.localScale = new Vector3(1f, Mathf.Abs(top.position.y - bot.position.y) * 12f, 1f);
	}

	public void SetToHard()
	{
		hard = true;
	}
}
