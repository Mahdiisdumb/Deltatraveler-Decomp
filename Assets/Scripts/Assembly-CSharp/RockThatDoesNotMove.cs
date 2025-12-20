using UnityEngine;

public class RockThatDoesNotMove : MonoBehaviour
{
	[SerializeField]
	private GameObject thrownObject;

	[SerializeField]
	private Sprite sprite;

	private bool initiated;

	private void Awake()
	{
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(21) == 1)
		{
			ThrowObject(false);
		}
	}

	public Transform ThrowObject(bool playAnim)
	{
		Animator component = Object.Instantiate(thrownObject, base.transform.parent).GetComponent<Animator>();
		if (playAnim)
		{
			component.enabled = true;
			component.Play("Throw", 0, 0f);
		}
		else
		{
			component.GetComponent<AudioSource>().mute = true;
		}
		GameObject.Find("Spikes").GetComponent<SpriteRenderer>().sprite = sprite;
		GameObject.Find("Spikes").GetComponent<BoxCollider2D>().isTrigger = true;
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(28) == 1 && (int)Object.FindObjectOfType<GameManager>().GetFlag(29) == 3)
		{
			component.GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* ...", "* You two are twisted,^10\n  y'know that?", "* (What...?)" }, new string[3] { "snd_text", "snd_text", "snd_txtsus" }, new int[3], new string[3] { "", "", "su_side_sweat" });
		}
		Object.Destroy(base.gameObject);
		return component.transform;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>() && !initiated)
		{
			initiated = true;
			CutsceneHandler.GetCutscene(10).StartCutscene();
		}
	}
}
