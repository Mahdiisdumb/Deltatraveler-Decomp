using UnityEngine;

public class StepSwitch : MonoBehaviour
{
	[SerializeField]
	private Sprite steppedOnSprite;

	protected bool stepped;

	public virtual void StepOn(bool sound = true)
	{
		if (sound)
		{
			GetComponent<AudioSource>().Play();
		}
		GetComponent<SpriteRenderer>().sprite = steppedOnSprite;
		stepped = true;
	}

	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>() && !stepped)
		{
			StepOn();
		}
	}
}
