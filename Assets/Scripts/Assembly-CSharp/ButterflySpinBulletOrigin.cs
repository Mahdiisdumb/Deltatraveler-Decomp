using System;
using UnityEngine;

public class ButterflySpinBulletOrigin : MonoBehaviour
{
	private bool rotate;

	private int frames;

	private void Awake()
	{
		base.transform.position = new Vector3(0f, -1.5f);
	}

	private void Update()
	{
		if (rotate)
		{
			frames++;
			base.transform.position = new Vector3(Mathf.Sin((float)frames * 4.8f * (MathF.PI / 180f)), 0f - Mathf.Cos((float)frames * 4.8f * (MathF.PI / 180f))) * 0.34f + new Vector3(0f, -1.67f);
		}
	}

	public void Begin(bool rotate, float speed, bool pray)
	{
		this.rotate = rotate;
		if (rotate)
		{
			base.transform.position = new Vector3(0f, -1.84f);
		}
		for (int i = 0; i < 18; i++)
		{
			ButterflySpinBulletHard component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/hardmode/ButterflySpinBulletHard"), base.transform, worldPositionStays: true).GetComponent<ButterflySpinBulletHard>();
			if (i < 2 && pray)
			{
				component.Activate(20 * i, rotate, speed, isHealing: true);
			}
			else
			{
				component.Activate(20 * i, rotate, speed, isHealing: false);
			}
		}
	}
}
