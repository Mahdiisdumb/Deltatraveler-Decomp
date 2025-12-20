using UnityEngine;

public class XOSpot : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	private int steppedOn;

	private bool disabled;

	public void ResetSpot()
	{
		steppedOn = 0;
		GetComponent<SpriteRenderer>().sprite = sprites[0];
	}

	public void DisableSpot()
	{
		disabled = true;
		GetComponent<SpriteRenderer>().sprite = sprites[3];
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>() && !disabled)
		{
			steppedOn++;
			if (steppedOn > 2)
			{
				steppedOn = 2;
			}
			else
			{
				GetComponent<AudioSource>().Play();
			}
			GetComponent<SpriteRenderer>().sprite = sprites[steppedOn];
		}
	}

	public bool IsActivated()
	{
		if (steppedOn != 1)
		{
			return disabled;
		}
		return true;
	}
}
