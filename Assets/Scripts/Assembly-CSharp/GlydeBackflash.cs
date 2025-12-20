using System;
using UnityEngine;

public class GlydeBackflash : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	private int frames = 1;

	private Glyde glyde;

	private void Start()
	{
		glyde = UnityEngine.Object.FindObjectOfType<Glyde>();
		SetPosition();
		GetComponent<SpriteRenderer>().enabled = true;
	}

	private void LateUpdate()
	{
		if (!UnityEngine.Object.FindObjectOfType<GlydeAttack>())
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SetPosition();
		frames++;
		int num = frames / 2;
		if (num >= sprites.Length)
		{
			base.transform.parent = null;
			for (int i = 0; i < 6; i++)
			{
				float num2 = (float)(-20 + i * 8) + UnityEngine.Random.Range(-5f, 5f);
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/GlydeBullet"), base.transform.position, base.transform.rotation, UnityEngine.Object.FindObjectOfType<GlydeAttack>().transform).GetComponent<GlydeBullet>().Activate(new Vector3(Mathf.Sin(num2 * ((float)Math.PI / 180f)), 0f - Mathf.Cos(num2 * ((float)Math.PI / 180f))));
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			GetComponent<SpriteRenderer>().sprite = sprites[num];
			if (frames == 5)
			{
				base.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
			}
		}
	}

	private void SetPosition()
	{
		base.transform.localPosition = new Vector3(0.4728f, Mathf.Lerp(0.209f, 0.048f, (glyde.GetGGValue() + 1f) / 2f));
	}
}
