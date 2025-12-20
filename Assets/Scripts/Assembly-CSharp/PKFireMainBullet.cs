using System;
using UnityEngine;

public class PKFireMainBullet : MonoBehaviour
{
	private int frames;

	private bool misaligned;

	private void Update()
	{
		frames++;
		if (frames <= 8)
		{
			base.transform.GetChild(0).localScale = new Vector3(1360f, Mathf.Lerp(1f, 15f, (float)frames / 8f));
			base.transform.GetChild(1).localScale = new Vector3(Mathf.Lerp(1f, 5f, (float)frames / 8f), Mathf.Lerp(150f, 200f, (float)frames / 8f));
		}
		else if (frames <= 20)
		{
			float num = (float)(frames - 8) / 12f;
			num = num * num * num;
			base.transform.GetChild(0).localScale = new Vector3(Mathf.Lerp(1360f, 0f, num), Mathf.Lerp(15f, 0f, (float)(frames - 8) / 12f));
			base.transform.GetChild(1).localScale = new Vector3(Mathf.Lerp(5f, 0f, (float)(frames - 8) / 6f), Mathf.Lerp(200f, 0f, num));
			base.transform.GetChild(2).localScale = Vector3.Lerp(Vector3.zero, new Vector3(1.5f, 1.5f, 1f), (float)(frames - 8) / 12f);
			base.transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.Lerp(new Color32(160, 0, 0, byte.MaxValue), Color.white, num);
		}
		else if (!GetComponents<AudioSource>()[1].isPlaying)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (frames != 20)
		{
			return;
		}
		base.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
		GetComponents<AudioSource>()[1].Play();
		float z = base.transform.eulerAngles.z;
		for (int i = 0; i < (misaligned ? 12 : 10); i++)
		{
			Vector2 vector = Vector2.left;
			Vector2 vector2 = Vector2.right;
			if (misaligned)
			{
				float num2 = z + UnityEngine.Random.Range(-2f, 2f);
				float num3 = z + UnityEngine.Random.Range(-2f, 2f);
				vector = new Vector2(Mathf.Cos(num2 * ((float)Math.PI / 180f)), Mathf.Sin(num2 * ((float)Math.PI / 180f)));
				vector2 = new Vector2(0f - Mathf.Cos(num3 * ((float)Math.PI / 180f)), 0f - Mathf.Sin(num3 * ((float)Math.PI / 180f)));
			}
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PKFireFlame"), base.transform.position + new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f)) / 48f, Quaternion.identity, base.transform.parent).GetComponent<PKFireFlame>().Activate(UnityEngine.Random.Range(0.9f, 1.1f) * (float)i, vector);
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PKFireFlame"), base.transform.position + new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f)) / 48f, Quaternion.identity, base.transform.parent).GetComponent<PKFireFlame>().Activate(UnityEngine.Random.Range(0.9f, 1.1f) * (float)i, vector2);
		}
	}

	public void MisalignFire()
	{
		misaligned = true;
	}
}
