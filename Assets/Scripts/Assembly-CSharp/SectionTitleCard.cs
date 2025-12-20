using System;
using UnityEngine;
using UnityEngine.UI;

public class SectionTitleCard : MonoBehaviour
{
	private int frames;

	private bool activate;

	private void Awake()
	{
		if (GetComponent<Image>().sprite.name.EndsWith("1") && Util.GameManager().GetFlagInt(287) == 0)
		{
			Util.GameManager().SetFlag(287, 1);
			if (Util.GameManager().GetFlagInt(108) == 0)
			{
				Activate();
			}
		}
	}

	private void Update()
	{
		if (activate)
		{
			frames++;
			GetComponent<Image>().enabled = true;
			float num = (float)frames / 45f;
			if (num > 1f)
			{
				num = 1f;
			}
			num = Mathf.Sin(num * (float)Math.PI * 0.5f);
			base.transform.localPosition = new Vector3(Mathf.Lerp(140f, 0f, num), 0f);
			float a = (float)frames / 30f;
			if (frames > 150)
			{
				a = (float)(180 - frames) / 30f;
			}
			GetComponent<Image>().color = new Color(1f, 1f, 1f, a);
			if (frames == 180)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public void Activate()
	{
		activate = true;
		base.transform.parent = GameObject.Find("FadeCanvas").transform;
	}
}
