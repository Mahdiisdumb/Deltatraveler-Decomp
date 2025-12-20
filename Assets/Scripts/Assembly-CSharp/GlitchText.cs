using UnityEngine;

public class GlitchText : InteractTextBox
{
	public int interactsTaken = 2;

	public int timesInteracted;

	private int state;

	protected override void Awake()
	{
		interactsTaken = Random.Range(15, 25);
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(87) >= 5)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public override void DoInteract()
	{
		base.DoInteract();
		timesInteracted++;
		if (timesInteracted < interactsTaken)
		{
			return;
		}
		if (state < 1)
		{
			switch (Random.Range(1, 4))
			{
			case 1:
				ModifyContents(new string[1] { "*^90 qqqqqqqqqqqqqqqqqqqqqqqqqqqqqq\n  qqqqqqqqqqqqqqqqqqqqqqqqqqqqqq\n  qqqqqqqqqqqqqqqqqqqqqqqqqqqqqq" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
				break;
			case 2:
				ModifyContents(new string[1] { "*^90 hhhhhhhhhhhhhhhhhhhhhhhhhhhhhh\n  hhhhhhhhhhhhhhhhhhhhhhhhhhhhhh\n  hhhhhhhhhhhhhhhhhhhhhhhhhhhhhh" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
				break;
			default:
				ModifyContents(new string[1] { "*^90 ABCDEFGHIJKLMNOPQRSTUVWXYZ\n  abcdefghijklmnopqrstuvwxyz\n  0123456789!@#$%&*;:?/()[]" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
				break;
			}
			state = 1;
		}
		else if (state < 2)
		{
			txt.MakeUnskippable();
			ModifyContents(new string[1] { "* (No problem here.)" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
			state = 2;
		}
	}
}
