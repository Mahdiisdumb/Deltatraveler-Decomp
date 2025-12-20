using UnityEngine;

public class GlowstarAnim : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<Animator>().SetFloat("speed", 0.2f + Random.Range(0f, 0.1f));
		Vector3 vector = base.transform.position * 48f;
		vector.x = Mathf.RoundToInt(vector.x);
		if (vector.x % 2f == 0f)
		{
			vector.x += 1f;
		}
		vector.y = Mathf.RoundToInt(vector.y);
		if (vector.y % 2f == 0f)
		{
			vector.y += 1f;
		}
		base.transform.position = vector / 48f;
	}
}
