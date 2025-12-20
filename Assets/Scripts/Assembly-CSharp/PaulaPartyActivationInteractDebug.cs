using UnityEngine;

public class PaulaPartyActivationInteractDebug : InteractSelectionBase
{
	public override void MakeDecision(Vector2 index, int id)
	{
		selectActivated = false;
		txt = new GameObject("InteractTextBoxItem", typeof(TextBox)).GetComponent<TextBox>();
		if (index == Vector2.left)
		{
			Object.FindObjectOfType<GameManager>().SetMiniPartyMember(1);
			Object.FindObjectOfType<GameManager>().HealAll(999);
			txt.CreateBox(new string[1] { "* Paula joins you." });
		}
		else if (index == Vector2.right)
		{
			Object.FindObjectOfType<GameManager>().SetMiniPartyMember(0);
			Object.FindObjectOfType<GameManager>().HealAll(999);
			txt.CreateBox(new string[1] { "* Paula doesn't join you." });
		}
	}
}
