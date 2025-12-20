using UnityEngine;

public class MoleDigBullet : BulletBase
{
	[SerializeField]
	private Sprite[] emergeSprites;

	[SerializeField]
	private Sprite[] sprites;

	[SerializeField]
	private Sprite[] burrowSprites;

	private Vector3 dir;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 9;
		if ((bool)Object.FindObjectOfType<MondoMole>() && !Object.FindObjectOfType<MondoMole>().IsLectured())
		{
			baseDmg = 9;
		}
		destroyOnHit = false;
		if (Random.Range(0, 2) == 0)
		{
			dir = new Vector3(Random.Range(0, 2) * 2 - 1, 0f);
			base.transform.eulerAngles = new Vector3(0f, 0f, 90f * dir.x);
			base.transform.position = new Vector3(-1.94f * dir.x, Random.Range(-2.82f, -0.52f));
		}
		else
		{
			dir = new Vector3(0f, Random.Range(0, 2) * 2 - 1);
			if (dir.y == 1f)
			{
				base.transform.eulerAngles = new Vector3(0f, 0f, 180f);
			}
			base.transform.position = new Vector3(Random.Range(-1.18f, 1.18f), -1.66f + 1.66f * (0f - dir.y));
		}
		sr.sprite = emergeSprites[0];
	}

	private void Update()
	{
		frames++;
		if (state == 0)
		{
			int num = frames / 3;
			if (num >= 3)
			{
				frames = 0;
				state = 1;
			}
			else
			{
				sr.sprite = emergeSprites[num];
			}
		}
		if (state == 1)
		{
			int num2 = frames / 6 % 2;
			sr.sprite = sprites[num2];
			base.transform.position += dir / 48f * base.transform.localScale.x * 2f;
			if (Mathf.Abs(base.transform.position.x) >= 2f || base.transform.position.y <= -3.4f || base.transform.position.y >= 0.1f)
			{
				frames = 0;
				state = 2;
			}
		}
		if (state == 2)
		{
			int num3 = frames / 3;
			if (num3 >= 3)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				sr.sprite = burrowSprites[num3];
			}
		}
	}

	public override void PreSOULHit()
	{
		if ((bool)Object.FindObjectOfType<RoughMole>())
		{
			baseDmg = ((Random.Range(0, 8) == 0) ? 14 : 9);
			if (baseDmg > 9)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/dr/StatusNumber")).GetComponent<StatusNumber>().StartWord("smash_bad", Color.white, new Vector3(0f, -3.86f));
			}
		}
	}
}
