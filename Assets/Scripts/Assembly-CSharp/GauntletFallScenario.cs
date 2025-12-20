using UnityEngine;

public class GauntletFallScenario : MonoBehaviour
{
	private OverworldPlayer kris;

	private bool activated;

	private Transform bg;

	private float bgYMulti = 0.05f;

	private int frames;

	private GameObject spearPrefab;

	private void Awake()
	{
		spearPrefab = Resources.Load<GameObject>("overworld/bullets/GauntletSpearBullet");
	}

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		Vector3 vector = new Vector3(UTInput.GetAxis("Horizontal"), UTInput.GetAxis("Vertical")) * 0.125f;
		kris.GetComponent<Rigidbody2D>().MovePosition(kris.transform.position + vector);
		float t = (float)frames / 350f;
		float num = (float)(frames - 50) / 350f / 1.5f;
		float num2 = Mathf.Lerp(600f, 20f, num);
		if (num >= 1f)
		{
			num2 = Mathf.Lerp(20f, 10f, num - 1f);
			t = Mathf.Lerp(1f, 0f, num - 1f);
		}
		bg.parent.position += new Vector3(0f, 10f * bgYMulti / Mathf.Lerp(48f, 90f, t));
		bg.parent.localScale += new Vector3(bgYMulti, bgYMulti) / num2;
		frames++;
		if (frames % 15 == 0 && frames <= 165)
		{
			if (frames <= 90)
			{
				int num3 = frames / 15;
				for (int i = 0; i < 2; i++)
				{
					int num4 = ((i % 2 == 0) ? 1 : (-1));
					GauntletSpearBullet component = Object.Instantiate(spearPrefab, new Vector3((7.4f - (float)num3 * 1.4f) * (float)num4, 7.44f * (float)num4) + base.transform.position, Quaternion.identity, base.transform).GetComponent<GauntletSpearBullet>();
					component.transform.localScale = new Vector3(num4, num4, 1f);
					component.SetDirection();
					if (i == 0)
					{
						component.PlaySFX("sounds/snd_spearappear");
					}
					else
					{
						component.GetComponent<AudioSource>().volume = 0f;
					}
				}
			}
			if (frames >= 90 && frames <= 180)
			{
				int num5 = (frames - 90) / 15;
				for (int j = 0; j < 2; j++)
				{
					int num6 = ((j % 2 == 0) ? 1 : (-1));
					GauntletSpearBullet component2 = Object.Instantiate(spearPrefab, new Vector3((7.4f - (float)num5 * 1.4f) * (float)num6, 7.44f * (float)(-num6)) + base.transform.position, Quaternion.identity, base.transform).GetComponent<GauntletSpearBullet>();
					component2.transform.localScale = new Vector3(num6, -num6, 1f);
					component2.SetDirection();
					if (j == 0 && num5 > 0)
					{
						component2.PlaySFX("sounds/snd_spearappear");
					}
					else
					{
						component2.GetComponent<AudioSource>().volume = 0f;
					}
				}
			}
		}
		if (frames == 195)
		{
			Object.Instantiate(Resources.Load<GameObject>("overworld/bullets/GauntletBallBullet"), new Vector3(0f, 18.54f) + base.transform.position, Quaternion.identity, base.transform);
		}
		if (frames == 280 || frames == 370)
		{
			for (int k = 0; k < 2; k++)
			{
				int num7 = ((k % 2 == 0) ? 1 : (-1));
				int num8 = ((frames == 280) ? 1 : (-1));
				GauntletSpearBullet component3 = Object.Instantiate(spearPrefab, new Vector3(5.5f * (float)num7, 7.44f * (float)num8) + base.transform.position, Quaternion.identity, base.transform).GetComponent<GauntletSpearBullet>();
				component3.transform.localScale = new Vector3(num7, num8, 1f);
				component3.SetDirection();
				if (k == 0)
				{
					component3.PlaySFX("sounds/snd_spearappear");
				}
				else
				{
					component3.GetComponent<AudioSource>().volume = 0f;
				}
			}
		}
		if (frames == 440)
		{
			for (int l = 0; l < 2; l++)
			{
				int num9 = ((l == 0) ? 1 : (-1));
				Object.Instantiate(Resources.Load<GameObject>("overworld/bullets/GauntletCannon"), new Vector3(7.736f * (float)num9, -5.55f) + base.transform.position, Quaternion.identity, base.transform);
			}
		}
		if (frames == 630)
		{
			Object.Instantiate(Resources.Load<GameObject>("overworld/bullets/GauntletFire"), Vector3.zero, Quaternion.identity, base.transform);
		}
		if (frames == 970)
		{
			Object.FindObjectOfType<ActionBulletHandler>().transform.position = Vector3.zero;
			Object.FindObjectOfType<ActionPartyPanels>().Lower();
		}
		if (frames == 982)
		{
			Util.GameManager().StopMusic(30f);
			Object.FindObjectOfType<Fade>().FadeOut(60, Color.white);
		}
		if (frames == 1070)
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_splash");
			Object.FindObjectOfType<Fade>().FadeOut(0, Color.black);
		}
		if (frames == 1100)
		{
			Util.GameManager().SetPartyMembers(true, true);
			Util.GameManager().LoadArea(107, true, new Vector3(1.2f, -1.965f), Vector2.down);
		}
	}

	public void Activate()
	{
		kris = Object.FindObjectOfType<OverworldPlayer>();
		base.transform.position = kris.transform.position;
		kris.SetCollision(true);
		bg = GameObject.Find("Paralax").transform;
		bgYMulti = bg.GetComponent<ParallaxEffect>().GetYMultiplier();
		Object.Destroy(bg.GetComponent<ParallaxEffect>());
		bg.position -= new Vector3(0f, 18f * bgYMulti * 40f / 48f);
		Transform transform = new GameObject("bgScaler").transform;
		transform.position = base.transform.position;
		bg.parent = transform;
		activated = true;
		Vector3 vector = new Vector3(320f, 240f) / 48f;
		Object.FindObjectOfType<CameraController>().SetClamps(base.transform.position + vector, base.transform.position - vector);
		Object.FindObjectOfType<CameraController>().SetFollowPlayer(true);
		Object.FindObjectOfType<ActionPartyPanels>().UpdatePanels();
	}
}
