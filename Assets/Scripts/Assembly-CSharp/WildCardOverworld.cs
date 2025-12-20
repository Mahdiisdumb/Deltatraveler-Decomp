using UnityEngine;

public class WildCardOverworld : InteractSelectionBase
{
	private bool doBit;

	private void Awake()
	{
		DoInteract();
	}

	protected override void Update()
	{
		base.Update();
		if (!txt && doBit)
		{
			Object.Instantiate(Resources.Load<GameObject>("ui/PunchCardBit"), GameObject.Find("Canvas").transform, worldPositionStays: false);
			Object.Destroy(base.gameObject);
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		string text = "* (You feel a bepis.)";
		string text2 = "pipis";
		if (index == Vector2.left)
		{
			text = "* (You feel a sense of calm\n  rise.)";
			Util.GameManager().SetFlag(312, 2);
			text2 = "green";
		}
		else if (index == Vector2.right)
		{
			text = "* (You feel a sense of integrity\n  and self-worth.)";
			Util.GameManager().SetFlag(312, 1);
			text2 = "blue";
		}
		else if (index == Vector2.down)
		{
			text = "* (A sense of justice bubbles\n  from within your heart.)";
			Util.GameManager().SetFlag(312, 5);
			text2 = "yellow";
		}
		else
		{
			text = "* (You feel a sense of self\n  perpetuating within your\n  SOUL.)";
			Util.GameManager().SetFlag(312, 0);
			text2 = "red";
		}
		string text3 = $"* (The card emanates with a\n  {text2} glow.)";
		doBit = Util.GameManager().GetFlagInt(301) == 0 && Util.GameManager().GetPlayerName() == "SHAYY";
		txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
		if (doBit)
		{
			txt.CreateBox(new string[4] { text3, text, "* (...)", "* (Something is appearing on\n  the card...!)" });
		}
		else
		{
			txt.CreateBox(new string[2] { text3, text });
			Object.Destroy(base.gameObject);
		}
	}
}
