using UnityEngine;

public class NoelleInventoryCheck : MonoBehaviour
{
	private void Awake()
	{
		if (Util.GameManager().GetFlagInt(313) == 1)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.GetComponent<OverworldPlayer>())
		{
			return;
		}
		bool flag = false;
		if (Util.GameManager().GetGold() <= 20)
		{
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				if (Items.ItemType(Util.GameManager().GetItem(i)) == 0)
				{
					num++;
				}
			}
			if (num <= 2)
			{
				flag = true;
			}
		}
		if (flag)
		{
			Util.GameManager().SetFlag(313, 1);
			new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[9] { "* Say,^05 Kris.", "* I noticed we aren't\n  carrying a lot of\n  stuff right now.", "* We don't have a lot\n  of food,^05 or money to\n  buy more.", "* Unless you have a\n  bunch of food in\n  the box...", "* Maybe we should try\n  to get more money\n  before continuing.", "* I...^10 think I have\n  some spare change,^05\n  actually.", "* (You got 62 cents from Noelle.)", "* (By process of an arbitrary\n  dimensional gold conversion\n  system,^05 this equals 35 G.)", "* Please use this wisely,^05\n  Kris." }, new string[9] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_text", "snd_text", "snd_txtnoe" }, new int[1], true, new string[9] { "no_curious", "no_thinking", "no_curious", "no_weird", "no_happy", "no_confused_side", "", "", "no_weird" });
			Util.GameManager().AddGold(35);
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
		}
		Object.Destroy(base.gameObject);
	}
}
