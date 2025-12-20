using System;
using UnityEngine;

public class KrisFire : MonoBehaviour
{
	private int frames;

	private bool activated;

	private bool attachToPartyPanel;

	private Vector3 pivot = Vector3.zero;

	private Transform origin;

	private void LateUpdate()
	{
		if (!activated)
		{
			return;
		}
		frames++;
		if (frames % 5 == 0)
		{
			SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer obj in componentsInChildren)
			{
				obj.flipX = !obj.flipX;
			}
		}
		float num = (Mathf.Cos(12f * (float)frames * ((float)Math.PI / 180f)) + 1f) / 2f;
		float num2 = (Mathf.Sin(24f * (float)frames * ((float)Math.PI / 180f)) + 1f) / 2f;
		for (int j = 0; j < 2; j++)
		{
			float x = Mathf.Lerp(1f, 1.15f, (j == 1) ? (1f - num2) : num2);
			float y = Mathf.Lerp(0.9f, 1f, (j == 1) ? (1f - num) : num);
			base.transform.GetChild(j).localScale = new Vector3(x, y, 1f);
		}
		if (attachToPartyPanel)
		{
			base.transform.position = origin.position - pivot;
			if (base.transform.position.y < -2.2f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public void Activate()
	{
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		activated = true;
	}

	public void AttachToPartyPanel()
	{
		for (int i = 0; i < 2; i++)
		{
			base.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 97 + i;
		}
		attachToPartyPanel = true;
		origin = UnityEngine.Object.FindObjectOfType<PartyPanels>().transform.Find("KrisSprite");
		pivot = origin.position;
	}
}
