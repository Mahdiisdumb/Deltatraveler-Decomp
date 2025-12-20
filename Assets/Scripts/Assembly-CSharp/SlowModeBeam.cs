using System;
using UnityEngine;

public class SlowModeBeam : MonoBehaviour
{
	private int frames;

	private Transform[] balls = new Transform[2];

	private Transform[] beams = new Transform[2];

	private Transform soulBall;

	private void Awake()
	{
		for (int i = 0; i < 2; i++)
		{
			balls[i] = base.transform.Find("ball" + i);
			beams[i] = base.transform.Find("beam" + i);
		}
		soulBall = base.transform.Find("soulball");
	}

	private void LateUpdate()
	{
		base.transform.position = UnityEngine.Object.FindObjectOfType<Porky>().GetPart("mech").position - new Vector3(0f, 2.122f);
		frames++;
		float num = Mathf.Cos((float)(frames * 24) * ((float)Math.PI / 180f));
		if (frames < 40)
		{
			for (int i = 0; i < 2; i++)
			{
				balls[i].transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(8f + num, 8f + num), (float)frames / 40f);
			}
		}
		else if (frames >= 40 && frames < 70)
		{
			if (frames == 40)
			{
				GetComponents<AudioSource>()[1].Play();
				UnityEngine.Object.FindObjectOfType<BattleCamera>().GiantBlastShake();
			}
			if (frames == 55)
			{
				UnityEngine.Object.FindObjectOfType<SOUL>().ChangeSOULMode(4);
			}
			for (int j = 0; j < 2; j++)
			{
				float y = Vector3.Distance(UnityEngine.Object.FindObjectOfType<SOUL>().transform.position, beams[j].transform.position) * 48f;
				balls[j].transform.localScale = Vector3.Lerp(new Vector3(8f + num, 8f + num), new Vector3(3f + num, 3f + num), (float)(frames - 40) / 5f);
				beams[j].transform.localScale = Vector3.Lerp(new Vector3(0f + num, y), new Vector3(15f + num * 10f, y), (float)(frames - 40) / 5f);
				beams[j].transform.up = UnityEngine.Object.FindObjectOfType<SOUL>().transform.position - beams[j].transform.position;
			}
			soulBall.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(3f + num, 3f + num), (float)(frames - 40) / 5f);
		}
		else if (frames >= 70)
		{
			for (int k = 0; k < 2; k++)
			{
				float y2 = Vector3.Distance(UnityEngine.Object.FindObjectOfType<SOUL>().transform.position, beams[k].transform.position) * 48f;
				balls[k].transform.localScale = Vector3.Lerp(new Vector3(3f + num, 3f + num), Vector3.zero, (float)(frames - 70) / 10f);
				beams[k].transform.localScale = Vector3.Lerp(new Vector3(15f + num * 10f, y2), new Vector3(0f, y2), (float)(frames - 70) / 10f);
				beams[k].transform.up = UnityEngine.Object.FindObjectOfType<SOUL>().transform.position - beams[k].transform.position;
			}
			soulBall.transform.localScale = Vector3.Lerp(new Vector3(3f + num, 3f + num), Vector3.zero, (float)(frames - 70) / 10f);
		}
		soulBall.position = UnityEngine.Object.FindObjectOfType<SOUL>().transform.position;
		if (frames == 100)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
