using UnityEngine;

public class SpamtonPortraitShake : MonoBehaviour
{
	private int frames;

	private Vector3 origPos;

	private void Start()
	{
		origPos = base.transform.localPosition;
	}

	private void Update()
	{
		frames++;
		if (frames % 2 == 0)
		{
			base.transform.localPosition = origPos + new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f));
		}
	}
}
