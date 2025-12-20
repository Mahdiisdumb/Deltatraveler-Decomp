using UnityEngine;

public class StarloTumbleweed : MonoBehaviour
{
	[SerializeField]
	private int sineTimer = 300;

	[SerializeField]
	private float xSpeed = 2f;

	[SerializeField]
	private float rotationSpeed = 1f;

	private float time;

	private void Update()
	{
		time += Time.deltaTime * 1000f;
		float z = 57.295784f * rotationSpeed * 1000f / 30f / (float)sineTimer;
		base.transform.eulerAngles += new Vector3(0f, 0f, z);
		float x = (1f + Mathf.Abs(Mathf.Sin(time / 2f / (float)sineTimer))) * 0.5f + xSpeed;
		float y = Mathf.Sin(time / (float)sineTimer);
		Vector3 vector = new Vector3(x, y) / 48f;
		base.transform.position -= vector;
		base.transform.localScale = new Vector3(1f, (1f + Mathf.Abs(Mathf.Sin(time / 2f / (float)sineTimer))) * 0.3f + 0.4f);
		if (base.transform.position.x < -7.225f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
