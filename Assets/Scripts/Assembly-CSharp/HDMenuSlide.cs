using UnityEngine;

public class HDMenuSlide : MonoBehaviour
{
	private bool sliding;

	private float slidePosition;

	private void Awake()
	{
		sliding = false;
	}

	private void Update()
	{
		if (sliding)
		{
			Vector3 b = new Vector3(slidePosition, base.transform.localPosition.y);
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, b, 0.1f);
			if (Vector3.Distance(base.transform.localPosition, b) < 6.6666665f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public void Slide(float pos)
	{
		sliding = true;
		slidePosition = pos;
	}
}
