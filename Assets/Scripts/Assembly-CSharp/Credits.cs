using UnityEngine;
using UnityEngine.UI;

public class Credits : CreditsBase
{
	private Transform bg;

	private bool doSonaStuff = true;

	private AudioClip castletown;

	protected override void Awake()
	{
		base.Awake();
		length = 5833;
		startInterval = 150;
		endInterval = 90;
		seenFlag = "seen-credits";
		bg = GameObject.Find("BG").transform;
		if (PersistentSAVE.GetInt("mario-unlocked", 0) == 0)
		{
			Text component = credits.Find("CreditPage4").Find("SpecialThanksCredits").GetComponent<Text>();
			component.text = component.text.Replace("Mario Bros. Title Background", "***** **** Title Background");
		}
		castletown = Resources.Load<AudioClip>("music/mus_castletown");
	}

	private void Start()
	{
		if (Util.GameManager().GetFlagInt(12) == 1 && Util.GameManager().GetFlagInt(13) == GameManager.FULL_MURDER_LEVEL)
		{
			doSonaStuff = false;
			Object.Destroy(sonas.gameObject);
			music.pitch = 0.6f;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!music.isPlaying)
		{
			music.clip = castletown;
			music.Play();
			music.loop = true;
		}
		bg.transform.position = new Vector3(Mathf.Lerp(-5.9f, 6.7f, (float)(frames - 1800) / 2220f), 0f);
		if (doSonaStuff)
		{
			if (frames <= 1900)
			{
				bg.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 0.17f, (float)(frames - 1800) / 100f));
			}
			else if (frames <= 4020)
			{
				bg.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Lerp(0.17f, 0f, (float)(frames - 3920) / 100f));
			}
			if (frames == 505)
			{
				credits.Find("Sonas").Find("Sarah").GetComponent<Animator>()
					.enabled = true;
			}
			else if (frames == 1050)
			{
				credits.Find("Sonas").Find("Gabbo").GetComponent<Animator>()
					.enabled = true;
			}
			else if (frames == 1142)
			{
				credits.Find("Sonas").Find("Cubic").GetComponent<Animator>()
					.enabled = true;
			}
			else if (frames == 1144)
			{
				credits.Find("Sonas").Find("Cyber").GetComponent<Animator>()
					.enabled = false;
				credits.Find("Sonas").Find("Cyber").GetComponent<SpriteRenderer>()
					.sprite = Resources.Load<Sprite>("overworld/npcs/staff/spr_cyber_surprise");
			}
			else if (frames == 1481)
			{
				credits.Find("Sonas").Find("Jevilhumor").GetComponent<Animator>()
					.enabled = true;
			}
			else if (frames == 1496)
			{
				credits.Find("Sonas").Find("Shaunt").GetComponent<Animator>()
					.Play("shaunt_wake", 0, 0f);
			}
			else if (frames == 1729)
			{
				credits.Find("Sonas").Find("Scoot").GetComponent<Animator>()
					.enabled = true;
			}
			else if (frames == 1990)
			{
				credits.Find("Sonas").Find("RealisticExplosion").GetComponent<Animator>()
					.enabled = true;
			}
			else if (frames == 2000)
			{
				credits.Find("Sonas").Find("Beethovenus").GetComponent<Animator>()
					.enabled = true;
			}
			else if (frames == 2075)
			{
				credits.Find("Sonas").Find("Hue").GetComponent<Animator>()
					.enabled = true;
			}
			else if (frames == 2360)
			{
				credits.Find("Sonas").Find("Sonnakai").GetComponent<Animator>()
					.SetFloat("speed", 2f);
			}
			else if (frames == 2390)
			{
				credits.Find("Sonas").Find("Sonnakai").GetComponent<Animator>()
					.SetFloat("speed", 3f);
			}
			else if (frames == 2420)
			{
				credits.Find("Sonas").Find("Sonnakai").GetComponent<Animator>()
					.SetFloat("speed", 4f);
			}
			else if (frames == 2450)
			{
				credits.Find("Sonas").Find("Diddy").GetComponent<SpriteRenderer>()
					.sprite = Resources.Load<Sprite>("overworld/npcs/staff/spr_diddy_1");
			}
			else if (frames == 2878)
			{
				credits.Find("Sonas").Find("Lexi").GetComponent<Animator>()
					.Play("lexi_shoot", 0, 0f);
			}
			else if (frames == 2572)
			{
				credits.Find("Sonas").Find("Valor").GetComponent<Animator>()
					.Play("Human", 0, 0f);
			}
			else if (frames == 3150)
			{
				credits.Find("Sonas").Find("Sophie").GetComponent<Animator>()
					.enabled = true;
			}
			else if (frames == 3396)
			{
				credits.Find("Sonas").Find("Mari").GetComponent<Animator>()
					.Play("Silly", 0, 0f);
			}
			else if (frames == 3738)
			{
				credits.Find("Sonas").Find("Marxvee").GetComponent<Animator>()
					.enabled = true;
			}
		}
	}

	protected override void OnCreditsEnd()
	{
		base.OnCreditsEnd();
		if (Util.GameManager().GetEnding() == -1)
		{
			if (Util.GameManager().GetFlagInt(58) == 1)
			{
				PersistentSAVE.SetInt("flowey-killed-last-time", GameManager.FULL_COMPLETION);
				if (Util.GameManager().GetFlagInt(12) == 1)
				{
					Util.GameManager().ForceLoadArea(129);
				}
				else
				{
					Util.GameManager().ForceLoadArea(6);
				}
			}
			else
			{
				Util.GameManager().ForceLoadArea(77);
			}
		}
		else
		{
			Util.GameManager().ForceLoadArea(6);
		}
	}
}
