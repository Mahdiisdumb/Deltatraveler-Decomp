using UnityEngine;

public class PapyrusFog : MonoBehaviour
{
	private void Update()
	{
		float num = (Object.FindObjectOfType<OverworldPlayer>().transform.position.x + 6.667f) / 18.333334f;
		num = 1f - Mathf.Abs(1f - num);
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, num);
		base.transform.position += new Vector3(1f / 24f, 0f);
		if (base.transform.position.x >= 3.3333333f)
		{
			base.transform.position -= new Vector3(3.3333333f, 0f);
		}
	}
}
