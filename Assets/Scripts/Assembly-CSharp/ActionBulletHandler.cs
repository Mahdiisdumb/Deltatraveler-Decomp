using UnityEngine;

public class ActionBulletHandler : MonoBehaviour
{
	private int fadeFrames;

	private bool activated;

	private SpriteRenderer roomBorder;

	private ActionPartyPanels panels;

	private void Start()
	{
		if (!Object.FindObjectOfType<ActionSOUL>())
		{
			Object.Instantiate(Resources.Load<GameObject>("overworld/ActionSOUL"), base.transform.parent, true).name = "ActionSOUL";
		}
		if (!Object.FindObjectOfType<ActionPartyPanels>())
		{
			Object.Instantiate(Resources.Load<GameObject>("ui/ActionPartyPanels"), GameObject.Find("Canvas").transform).name = "ActionPartyPanels";
		}
		panels = Object.FindObjectOfType<ActionPartyPanels>();
		if ((bool)GameObject.Find("RoomBorders"))
		{
			roomBorder = GameObject.Find("RoomBorders").GetComponent<SpriteRenderer>();
		}
		GetComponent<Collider2D>().enabled = true;
	}

	private void Update()
	{
		if (activated && fadeFrames < 12)
		{
			fadeFrames++;
		}
		else if (!activated && fadeFrames > 0)
		{
			fadeFrames--;
		}
		GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(0f, 0f, 0f, 0f), new Color(0f, 0f, 0f, 0.5f), (float)fadeFrames / 12f);
		OverworldPartyMember[] array = Object.FindObjectsOfType<OverworldPartyMember>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(0.5f, 0.5f, 0.5f, 1f), (float)fadeFrames / 12f);
		}
		if ((bool)roomBorder)
		{
			roomBorder.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)fadeFrames / 12f);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>() && base.enabled)
		{
			Object.FindObjectOfType<GameManager>().DisableMenu();
			activated = true;
			Object.FindObjectOfType<ActionSOUL>().SetActivated(true);
			panels.SetActivated(true);
			ActionBulletBase[] array = Object.FindObjectsOfType<ActionBulletBase>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActivated(true);
			}
			ActionBulletGenerator[] array2 = Object.FindObjectsOfType<ActionBulletGenerator>();
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].SetActivated(true);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>() && base.enabled)
		{
			Object.FindObjectOfType<GameManager>().EnableMenu();
			activated = false;
			Object.FindObjectOfType<ActionSOUL>().SetActivated(false);
			panels.SetActivated(false);
			ActionBulletBase[] array = Object.FindObjectsOfType<ActionBulletBase>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActivated(false);
			}
			ActionBulletGenerator[] array2 = Object.FindObjectsOfType<ActionBulletGenerator>();
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].SetActivated(false);
			}
		}
	}

	public bool IsActivated()
	{
		return activated;
	}
}
