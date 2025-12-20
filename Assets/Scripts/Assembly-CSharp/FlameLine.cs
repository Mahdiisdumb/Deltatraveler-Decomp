using UnityEngine;

public class FlameLine : MonoBehaviour
{
	[SerializeField]
	private int numFlames = 5;

	[SerializeField]
	private float maxWidth = 6f;

	[SerializeField]
	private float maxHeight = 0.5f;

	[SerializeField]
	private int maxWidthFrames = 180;

	[SerializeField]
	private int maxHeightFrames = 60;

	[SerializeField]
	private int frameDifference = 5;

	private int frames;

	private GameObject[] flames;

	private void Awake()
	{
		GameObject original = Resources.Load<GameObject>("battle/attacks/bullets/FlameBullet");
		flames = new GameObject[numFlames];
		for (int i = 0; i < numFlames; i++)
		{
			flames[i] = Object.Instantiate(original, base.transform);
		}
	}

	private void Update()
	{
		frames++;
		for (int i = 0; i < numFlames; i++)
		{
			if ((bool)flames[i])
			{
				int num = (frames + maxHeightFrames / 4 + i * frameDifference) % maxHeightFrames;
				float num2 = (float)num / ((float)maxHeightFrames / 2f);
				if ((float)num > (float)maxHeightFrames / 2f)
				{
					num2 = (float)(maxHeightFrames - num) / ((float)maxHeightFrames / 2f);
				}
				num2 = num2 * num2 * (3f - 2f * num2);
				float y = Mathf.Lerp(0f - maxHeight, maxHeight, num2);
				int num3 = (frames + i * frameDifference) % maxWidthFrames;
				float t = (float)num3 / ((float)maxWidthFrames / 2f);
				if ((float)num3 > (float)maxWidthFrames / 2f)
				{
					t = (float)(maxWidthFrames - num3) / ((float)maxWidthFrames / 2f);
				}
				float x = Mathf.Lerp(0f - maxWidth, maxWidth, t);
				flames[i].transform.localPosition = new Vector3(x, y);
			}
		}
	}
}
