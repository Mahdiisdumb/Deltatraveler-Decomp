using UnityEngine;

public class ActionBulletHandler : MonoBehaviour
{
	private int fadeFrames;

	private bool activated;

	private SpriteRenderer roomBorder;

	private ActionPartyPanels panels;

	private void Start()
	{
		if (!Util.FindObjectOfType<ActionSOUL>())
		{
			Object.Instantiate(Resources.Load<GameObject>("overworld/ActionSOUL"), base.transform.parent, worldPositionStays: true).name = "ActionSOUL";
		}
		if (!Util.FindObjectOfType<ActionPartyPanels>())
		{
			Object.Instantiate(Resources.Load<GameObject>("ui/ActionPartyPanels"), GameObject.Find("Canvas").transform).name = "ActionPartyPanels";
		}
		panels = Util.FindObjectOfType<ActionPartyPanels>();
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
		OverworldPartyMember[] array = Util.FindObjectsOfType<OverworldPartyMember>();
		foreach (OverworldPartyMember overworldPartyMember in array)
		{
			if ((bool)overworldPartyMember)
			{
				overworldPartyMember.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(0.5f, 0.5f, 0.5f, 1f), (float)fadeFrames / 12f);
			}
		}
		if ((bool)roomBorder)
		{
			roomBorder.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)fadeFrames / 12f);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision && (bool)collision.GetComponent<OverworldPlayer>() && base.enabled)
		{
			Util.GameManager().DisableMenu();
			activated = true;
			Util.FindObjectOfType<ActionSOUL>().SetActivated(activated: true);
			panels.SetActivated(activated: true);
			ActionBulletBase[] array = Util.FindObjectsOfType<ActionBulletBase>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActivated(activated: true);
			}
			ActionBulletGenerator[] array2 = Util.FindObjectsOfType<ActionBulletGenerator>();
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].SetActivated(activated: true);
			}
			if (Util.GameManager().GetCombinedHPNoOverheal() < Util.GameManager().GetCombinedMaxHP())
			{
				panels.Raise();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if ((bool)collision && (bool)collision.GetComponent<OverworldPlayer>() && base.enabled)
		{
			Util.GameManager().EnableMenu();
			activated = false;
			if ((bool)Util.FindObjectOfType<ActionSOUL>())
			{
				Util.FindObjectOfType<ActionSOUL>().SetActivated(activated: false);
			}
			panels.SetActivated(activated: false);
			ActionBulletBase[] array = Util.FindObjectsOfType<ActionBulletBase>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActivated(activated: false);
			}
			ActionBulletGenerator[] array2 = Util.FindObjectsOfType<ActionBulletGenerator>();
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].SetActivated(activated: false);
			}
		}
	}

	public bool IsActivated()
	{
		return activated;
	}
}
