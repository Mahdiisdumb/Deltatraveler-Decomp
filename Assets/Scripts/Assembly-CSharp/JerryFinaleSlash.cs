using System;
using UnityEngine;

public class JerryFinaleSlash : BulletBase
{
	private float velocity = 1f / 6f;

	private GameObject echo;

	protected override void Awake()
	{
		base.Awake();
		echo = Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySlashEcho");
		baseDmg = UnityEngine.Object.FindObjectOfType<Jerry>().GetDamageValue();
		aud.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		sr.color = Color.red;
	}

	private void Update()
	{
		frames++;
		sr.color = Color.Lerp(Color.red, Color.white, (float)frames / 10f);
		float t = Mathf.Abs(Mathf.Sin((float)(frames * 36) * ((float)Math.PI / 180f)));
		base.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1.2f, 0.8f, 1f), t);
		float z = base.transform.eulerAngles.z;
		base.transform.eulerAngles = new Vector3(0f, 0f, z);
		Vector3 vector = new Vector3(Mathf.Cos(z * ((float)Math.PI / 180f)), Mathf.Sin(z * ((float)Math.PI / 180f)));
		base.transform.position += vector * velocity;
		if (frames % 10 == 0)
		{
			UnityEngine.Object.Instantiate(echo, base.transform.position, base.transform.rotation).GetComponent<JerrySlashEcho>().Activate(velocity, vector, base.transform.localScale, base.transform.GetChild(0).GetComponent<SpriteRenderer>().color);
		}
		if ((Mathf.Sign(base.transform.position.x) == Mathf.Sign(vector.x) && Mathf.Abs(base.transform.position.x) > 8f) || Mathf.Abs(base.transform.position.y) > 8f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void SetSpeed(float speed)
	{
		velocity = speed / 48f;
	}

	public override void PreSOULHit()
	{
		base.PreSOULHit();
		baseDmg = UnityEngine.Object.FindObjectOfType<Jerry>().GetDamageValue();
	}
}
