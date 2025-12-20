using System;
using UnityEngine;

public class FloweySpikeAttack : AttackBase
{
	private FloweySpikeInterior[] spikes;

	private float degrees;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 325;
		bbSize = new Vector2(165f, 140f);
		soulPos = bbPos;
	}

	public void OnDestroy()
	{
		for (int i = 0; i < 4; i++)
		{
			if ((bool)spikes[i])
			{
				UnityEngine.Object.Destroy(spikes[i].gameObject);
			}
		}
	}

	protected override void Update()
	{
		if (bb.IsPlaying())
		{
			return;
		}
		base.Update();
		if (!isStarted)
		{
			return;
		}
		if (frames < 25)
		{
			if (frames < 10)
			{
				bb.transform.position = bbPos + new Vector2(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2)) / 48f;
				return;
			}
			if (frames == 10)
			{
				bb.transform.position = bbPos;
				spikes = new FloweySpikeInterior[4];
				Util.GameManager().PlayGlobalSFX("sounds/snd_spearrise");
				for (int i = 0; i < 4; i++)
				{
					bool num = i > 1;
					float num2 = ((i % 2 != 0) ? 1 : (-1));
					spikes[i] = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/flowey/FloweySpikeInterior"), bb.transform).GetComponent<FloweySpikeInterior>();
					spikes[i].GetComponent<SpriteRenderer>().flipY = i % 2 == 1;
					spikes[i].GetComponent<BoxCollider2D>().offset *= 0f - num2;
					if (num)
					{
						spikes[i].transform.eulerAngles = new Vector3(0f, 0f, -90f);
						spikes[i].transform.localPosition = new Vector3(1.913f * num2, 0f);
					}
					else
					{
						spikes[i].transform.localPosition = new Vector3(0f, 1.662f * num2);
					}
				}
				return;
			}
			for (int j = 0; j < 4; j++)
			{
				bool num3 = j > 1;
				float num4 = ((j % 2 != 0) ? 1 : (-1));
				if (num3)
				{
					spikes[j].transform.localPosition = new Vector3(Mathf.Lerp(1.913f, 1.618f, (float)(frames - 10) / 5f) * num4, 0f);
				}
				else
				{
					spikes[j].transform.localPosition = new Vector3(0f, Mathf.Lerp(1.662f, 1.371f, (float)(frames - 10) / 5f) * num4);
				}
			}
		}
		else
		{
			float num5 = Mathf.Lerp(0f, 2f, (float)(frames - 25) / 120f);
			degrees += num5;
			float f = degrees * ((float)Math.PI / 180f);
			float f2 = degrees * 3f * ((float)Math.PI / 180f);
			bb.transform.position = new Vector3(Mathf.Sin(f) * 2f, Mathf.Sin(f2) / 2f - 1.66f);
			if (frames % 7 == 1)
			{
				f = (float)UnityEngine.Random.Range(-90, 90) * ((float)Math.PI / 180f);
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/flowey/FloweyPelletStandard"), new Vector3(Mathf.Sin(f) * 2f, Mathf.Cos(f) + 3f), Quaternion.identity, base.transform);
			}
		}
	}
}
