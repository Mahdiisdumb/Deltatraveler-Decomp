using System;
using UnityEngine;

public class SusieLDBigRudeBuster : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	private int frames;

	private float velocity = 24f;

	private readonly float aimSpeed = 6.5f;

	private readonly float multiplier = 0.75f;

	private void Update()
	{
		frames++;
		GetComponent<SpriteRenderer>().sprite = sprites[frames % sprites.Length];
		if (frames <= 4)
		{
			GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)frames / 4f);
		}
		Quaternion rotation = base.transform.rotation;
		base.transform.right = UnityEngine.Object.FindObjectOfType<SOUL>().transform.position - base.transform.position;
		Quaternion b = base.transform.rotation;
		if (base.transform.rotation.eulerAngles.y == -180f)
		{
			b = Quaternion.Euler(0f, 0f, 180f);
		}
		base.transform.rotation = Quaternion.Lerp(rotation, b, 0.12f);
		float z = base.transform.rotation.eulerAngles.z;
		base.transform.position += new Vector3(Mathf.Cos(z * ((float)Math.PI / 180f)), Mathf.Sin(z * ((float)Math.PI / 180f))) * velocity / 48f;
		UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/SusieLDRudeBusterAfterImage"), base.transform.position, base.transform.rotation, base.transform.parent).GetComponent<SusieLDRudeBusterAfterImage>().Activate(GetComponent<SpriteRenderer>().color.a);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponentInParent<BulletBoard>())
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_rudebuster_hit");
			for (int i = 0; i < 8; i++)
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/SusieLDRudeBusterHit"), base.transform.position, Quaternion.Euler(0f, 0f, 45 + i * 90), base.transform.parent).GetComponent<SusieLDRudeBusterHit>().SetDecayRate((i >= 4) ? 0.8f : 0.75f);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
