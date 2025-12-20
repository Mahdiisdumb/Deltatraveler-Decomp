using UnityEngine;

public class IceCapPipe : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	private int frames;

	private void Update()
	{
		frames++;
		if (frames <= 20)
		{
			base.transform.position = new Vector3(base.transform.position.x, Mathf.Lerp(-3.52f, -3.05f, (float)frames / 20f));
		}
		else if (frames <= 40)
		{
			GetComponent<SpriteRenderer>().sprite = sprites[(frames + 4) / 4 % 2];
			if (frames == 40)
			{
				GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/IcicleGroup"), new Vector3(base.transform.position.x, -3.44f), Quaternion.identity);
				for (int i = 0; i < 3; i++)
				{
					gameObject.transform.GetChild(0).parent = base.transform.parent;
				}
				Object.Destroy(gameObject);
			}
		}
		else if (frames >= 58)
		{
			base.transform.position = new Vector3(base.transform.position.x, Mathf.Lerp(-3.05f, -3.52f, (float)(frames - 58) / 7f));
			if (frames == 65)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
