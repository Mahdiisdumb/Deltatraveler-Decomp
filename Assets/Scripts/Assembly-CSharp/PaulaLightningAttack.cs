using UnityEngine;

public class PaulaLightningAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 250;
		bbSize = new Vector2(320f, 140f);
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		if (frames < 80)
		{
			int num = frames % 20;
			bool flag = frames % 40 < 20;
			if (num <= 7)
			{
				float x = -6.25f + 1.66f * (float)(num - 1);
				if (!flag)
				{
					x = 2.88f - 1.66f * (float)(num - 1);
				}
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/LightningBullet"), new Vector3(x, -3.018f), Quaternion.identity, base.transform).GetComponent<AudioSource>().volume = 0.375f;
			}
			return;
		}
		if (frames < 128)
		{
			int num2 = (frames - 80) % 16;
			bool flag2 = (frames - 80) % 32 > 16;
			if (num2 != 1)
			{
				return;
			}
			for (int i = 0; i < 5; i++)
			{
				if ((i % 2 == 0 && !flag2) || (i % 2 == 1 && flag2))
				{
					Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/LightningBullet"), new Vector3(-2.48f + 1.245f * (float)i, -3.018f), Quaternion.identity, base.transform).transform.localScale = new Vector3(1.5f, 1.5f, 1f);
				}
			}
			return;
		}
		int num3 = (frames - 128) % 16;
		int num4 = (frames - 128) / 16;
		if (num3 != 1)
		{
			return;
		}
		if (num4 < 3)
		{
			for (int j = 0; j < 2; j++)
			{
				int num5 = ((j == 1) ? (6 - num4) : num4);
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/LightningBullet"), new Vector3(-2.52f + 0.83f * (float)num5, -3.018f), Quaternion.identity, base.transform);
			}
			return;
		}
		switch (num4)
		{
		case 3:
		{
			for (int l = 0; l < 7; l++)
			{
				if (l % 2 == 1)
				{
					Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/LightningBullet"), new Vector3(-2.52f + 0.83f * (float)l, -3.018f), Quaternion.identity, base.transform);
				}
			}
			break;
		}
		case 4:
		{
			for (int k = 0; k < 7; k++)
			{
				if (k % 2 == 0)
				{
					Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/LightningBullet"), new Vector3(-2.52f + 0.83f * (float)k, -3.018f), Quaternion.identity, base.transform);
				}
			}
			break;
		}
		}
	}
}
