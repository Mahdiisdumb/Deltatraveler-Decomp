using UnityEngine;

public class PaulaMeleeTarget : MonoBehaviour
{
	private bool activated;

	private bool setToDestroy;

	private FakeTargetBar[] bars;

	private int frames;

	private int times;

	private int maxTimes = 3;

	private bool superFast;

	private bool doImperfectNext;

	private bool perfect;

	private bool hasHitLastBar;

	private void Awake()
	{
		int childCount = base.transform.childCount;
		bars = new FakeTargetBar[childCount];
		superFast = Util.FindObjectOfType<NessDeathAttack>();
		for (int i = 0; i < childCount; i++)
		{
			bars[i] = base.transform.GetChild(i).GetComponent<FakeTargetBar>();
		}
	}

	private void Update()
	{
		if (activated)
		{
			frames++;
			if (frames <= 8)
			{
				GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)frames / 8f);
			}
			bool flag = true;
			if (!hasHitLastBar && bars[3].IsDoingHitAnim())
			{
				if (perfect)
				{
					GetComponent<AudioSource>().Play();
				}
				if ((bool)Util.FindObjectOfType<AttackBase>())
				{
					PaulaMeleeBullet component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PaulaMeleeBullet"), Util.FindObjectOfType<SOUL>().transform.position, Quaternion.identity, Util.FindObjectOfType<AttackBase>().transform).GetComponent<PaulaMeleeBullet>();
					if (perfect)
					{
						component.ToggleHard();
					}
				}
				hasHitLastBar = true;
			}
			for (int i = 0; i < 4; i++)
			{
				if (bars[i].IsActivated() || bars[i].IsDoingHitAnim())
				{
					flag = false;
				}
			}
			if (flag)
			{
				StartBarMovement();
			}
		}
		else if (setToDestroy)
		{
			frames++;
			base.transform.localScale = Vector2.Lerp(new Vector2(1f, 1f), new Vector2(0.278125f, 1f), (float)frames / 11f);
			GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - (float)frames / 11f);
			if (frames == 11)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public void Activate(int maxTimes, bool hard = false)
	{
		this.maxTimes = maxTimes;
		perfect = hard;
		activated = true;
		StartBarMovement();
	}

	private void StartBarMovement()
	{
		if (doImperfectNext)
		{
			doImperfectNext = false;
			perfect = false;
		}
		if (times >= maxTimes)
		{
			return;
		}
		times++;
		hasHitLastBar = false;
		bool flag = false;
		float num = 7f;
		int num2 = 0;
		for (int i = 0; i < bars.Length; i++)
		{
			int num3 = Random.Range(-1, 3);
			if (num3 == 0 && flag)
			{
				num3 = 1;
			}
			else
			{
				flag = true;
			}
			float speed = 0.2f;
			if (!Util.FindObjectOfType<PaulaRandomPatternsAttack>())
			{
				if (perfect)
				{
					speed = 0.25f;
				}
				else if (superFast)
				{
					speed = 0.3f;
				}
			}
			else
			{
				speed = 0.175f;
			}
			bars[i].Activate(perfect ? 0f : ((float)num3 * 0.2f), num, quadSounds: true, speed);
			int num4 = Random.Range(3, 6) * 2;
			if (num4 == 8 && num2 < 2)
			{
				num2++;
			}
			else
			{
				num4 = 6;
			}
			num += (float)num4 * 0.2f;
		}
	}

	public void SetToDestroy()
	{
		activated = false;
		setToDestroy = true;
		frames = 0;
		for (int i = 0; i < bars.Length; i++)
		{
			Object.Destroy(bars[i].gameObject);
		}
	}

	public void SetToImperfect()
	{
		doImperfectNext = true;
	}
}
