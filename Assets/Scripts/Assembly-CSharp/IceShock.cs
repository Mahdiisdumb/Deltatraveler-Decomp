using System;
using UnityEngine;

public class IceShock : SpecialAttackEffect
{
	private int frames;

	private EnemyBase enemy;

	private void Update()
	{
		if (isPlaying)
		{
			frames++;
			if (frames == 1)
			{
				base.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
			}
			if (frames == 4)
			{
				base.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
			}
			if (frames == 7)
			{
				base.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
			}
			if (frames >= 10)
			{
				if (frames == 10)
				{
					DamageEnemy();
				}
				for (int i = 0; i < 3; i++)
				{
					if (frames == 10)
					{
						base.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
					}
					for (int j = 0; j < 6; j++)
					{
						if (frames == 10)
						{
							base.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>()
								.enabled = true;
						}
						float num = (float)(frames - 10) / 15f;
						num = Mathf.Sin(num * (float)Math.PI * 0.5f);
						float num2 = Mathf.Lerp(15f, 40f, num) / 48f;
						base.transform.GetChild(i).GetChild(j).transform.localPosition = new Vector3(Mathf.Cos((float)(frames * 12 + j * 60) * ((float)Math.PI / 180f)), Mathf.Sin((float)(frames * 12 + j * 60) * ((float)Math.PI / 180f))) * num2;
						base.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>()
							.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), (float)(frames - 18) / 9f);
					}
				}
			}
			if (frames == 27)
			{
				isPlaying = false;
			}
		}
		if (!isPlaying && !GetComponent<AudioSource>().isPlaying)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public override bool IsPlaying()
	{
		return frames < 10;
	}

	public void AssignEnemy(EnemyBase enemy)
	{
		this.enemy = enemy;
		base.transform.position = new Vector3(enemy.transform.position.x, base.transform.position.y);
	}

	public void DamageEnemy()
	{
		if ((bool)enemy)
		{
			int num = 30;
			enemy.Hit(2, num, false);
		}
	}
}
