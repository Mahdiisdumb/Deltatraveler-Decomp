using System;
using UnityEngine;

public class FireballPanBullet : MonoBehaviour
{
	private int frames;

	private bool right;

	private void Awake()
	{
		right = UnityEngine.Random.Range(0, 2) == 1;
		if (right)
		{
			GetComponent<SpriteRenderer>().flipX = true;
		}
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
		base.transform.position = new Vector3(UnityEngine.Random.Range(3.96f, 5.22f) * (float)(right ? 1 : (-1)), UnityEngine.Random.Range(0.81f, -2.56f));
		UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/FireballBullet"), base.transform.position + new Vector3(right ? (-1.37f) : 1.37f, 0f), Quaternion.identity, base.transform.parent);
	}

	private void Update()
	{
		frames++;
		base.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Sin((float)(frames * 9) * ((float)Math.PI / 180f)) * (float)(right ? (-136) : 136));
		if (frames <= 5)
		{
			GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)frames / 5f);
		}
		if (frames >= 30)
		{
			GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - (float)(frames - 30) / 5f);
		}
		if (frames == 35)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
