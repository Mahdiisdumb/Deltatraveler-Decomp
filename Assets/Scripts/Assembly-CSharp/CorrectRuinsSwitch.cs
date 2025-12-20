using UnityEngine;

public class CorrectRuinsSwitch : Interactable
{
	[SerializeField]
	private string color = "red";

	[SerializeField]
	private int flag = -1;

	[SerializeField]
	private Sprite spikeSprite;

	private bool activated;

	private void Awake()
	{
		if (flag > -1 && (int)Util.GameManager().GetFlag(flag) == 1)
		{
			activated = true;
			GameObject.Find("Spikes").GetComponent<BoxCollider2D>().isTrigger = true;
			GameObject.Find("Spikes").GetComponent<SpriteRenderer>().sprite = spikeSprite;
		}
	}

	public override void DoInteract()
	{
		if ((bool)txt || !base.enabled)
		{
			return;
		}
		TextBox component = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
		if (activated)
		{
			component.CreateBox(new string[1] { "* You press the " + color + " switch.\n* Nothing happened." });
		}
		else
		{
			GameObject.Find("Spikes").GetComponent<BoxCollider2D>().isTrigger = true;
			GameObject.Find("Spikes").GetComponent<SpriteRenderer>().sprite = spikeSprite;
			activated = true;
			if (flag > -1)
			{
				Util.GameManager().SetFlag(flag, 1);
			}
			GetComponent<AudioSource>().Play();
			component.CreateBox(new string[1] { "* You press the " + color + " switch.\n* You hear a clicking sound." });
		}
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
	}

	public override int GetEventData()
	{
		return -1;
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		Debug.LogError("THis nuts");
	}
}
