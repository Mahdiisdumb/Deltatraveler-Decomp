using UnityEngine;

public class PaulaPartyActivationInteractDebug : InteractSelectionBase
{
	public override void MakeDecision(Vector2 index, int id)
	{
		selectActivated = false;
		txt = new GameObject("InteractTextBoxItem", typeof(TextBox)).GetComponent<TextBox>();
		if (index == Vector2.left)
		{
			Util.GameManager().SetPartyMember(3, 3);
			txt.CreateBox(new string[1] { "* Paula joins you." });
		}
		else if (index == Vector2.right)
		{
			Util.GameManager().SetPartyMember(3, -1);
			txt.CreateBox(new string[1] { "* Paula doesn't join you." });
		}
	}
}
