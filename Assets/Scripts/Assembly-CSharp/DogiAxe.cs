using System;
using UnityEngine;

public class DogiAxe : BulletBase
{
	private bool rage;

	private int beginFrames;

	private int beginLimit;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 8;
		destroyOnHit = false;
		rage = UnityEngine.Object.FindObjectOfType<Dogamy>().IsKilled();
		if (rage)
		{
			baseDmg = 12;
		}
		beginLimit = UnityEngine.Random.Range(20, 26);
	}

	private void Update()
	{
		if (beginFrames >= beginLimit)
		{
			int num = (rage ? 35 : 55);
			int num2 = (rage ? 30 : 37);
			float b = (rage ? (-1.81f) : (-1.6f));
			frames = (frames + 1) % num;
			float t = Mathf.Sin((float)frames / (float)(num2 - 1) * 180f * ((float)Math.PI / 180f));
			if (frames < num2)
			{
				base.transform.position = new Vector3(base.transform.position.x + 0.037727274f * base.transform.localScale.x, Mathf.Lerp(-2.264f, b, t));
			}
			else if (frames == num2)
			{
				UnityEngine.Object.FindObjectOfType<BattleCamera>().BlastShake();
				aud.Play();
			}
		}
		else
		{
			beginFrames++;
		}
	}
}
