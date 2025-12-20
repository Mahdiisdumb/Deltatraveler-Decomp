using UnityEngine;

public class JerrySlashEcho : MonoBehaviour
{
	private float velocity = 1f / 6f;

	private Vector3 direction = Vector3.right;

	private int frames;

	private bool activated;

	private void Update()
	{
		if (activated)
		{
			frames++;
			float num = (float)frames / 15f;
			float num2 = 1f - num;
			num2 = 1f - num2 * num2;
			float num3 = Mathf.Lerp(1f, 0.5f, num2);
			Color color = GetComponent<SpriteRenderer>().color;
			color.a = Mathf.Lerp(0.75f, 0f, num);
			base.transform.position += direction * velocity * num3;
			GetComponent<SpriteRenderer>().color = color;
			base.transform.localScale = new Vector3(1.1f * base.transform.localScale.x, 0.9f * base.transform.localScale.y, 1f);
			if (num >= 1f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public void Activate(float velocity, Vector3 direction, Vector3 scale, Color color)
	{
		this.velocity = velocity;
		this.direction = direction;
		base.transform.localScale = scale;
		GetComponent<SpriteRenderer>().color = color;
		activated = true;
	}
}
