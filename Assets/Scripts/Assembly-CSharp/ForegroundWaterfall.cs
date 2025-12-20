using UnityEngine;

public class ForegroundWaterfall : MonoBehaviour
{
	private void Update()
	{
		base.transform.position -= new Vector3(0f, 0.125f);
		if (base.transform.position.y < -7.5f)
		{
			base.transform.position += new Vector3(0f, 7.5f);
		}
	}
}
