using UnityEngine;

public class NessPaulaSouls : MonoBehaviour
{
	private int frames;

	private bool activated;

	private Vector3[] initialPositions = new Vector3[2]
	{
		new Vector3(11.981f, -3.521f),
		new Vector3(13.44f, -3.521f)
	};

	private Vector3[] finalPositions = new Vector3[2]
	{
		new Vector3(10.11f, -1.5f),
		new Vector3(15.11f, -1.5f)
	};

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		frames++;
		if (frames == 11)
		{
			GameObject.Find("Ness").GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("Paula").GetComponent<SpriteRenderer>().enabled = false;
		}
		for (int i = 0; i < 2; i++)
		{
			SpriteRenderer component = base.transform.GetChild(i).GetComponent<SpriteRenderer>();
			if (frames == 1 || frames == 5 || frames == 9)
			{
				component.enabled = true;
			}
			if (frames == 3 || frames == 7)
			{
				component.enabled = false;
			}
			if (frames >= 11)
			{
				component.transform.position = Vector3.Lerp(initialPositions[i], finalPositions[i], (float)(frames - 11) / 18f);
			}
		}
		if ((bool)Object.FindObjectOfType<BattleManager>())
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void Activate()
	{
		activated = true;
		GameObject.Find("Ness").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("Paula").GetComponent<SpriteRenderer>().enabled = true;
	}
}
