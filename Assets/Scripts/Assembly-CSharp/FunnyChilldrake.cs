using UnityEngine;

public class FunnyChilldrake : InteractTextBox
{
	public override void DoInteract()
	{
		if (!talkedToBefore)
		{
			FunnyChilldrake[] array = Object.FindObjectsOfType<FunnyChilldrake>();
			foreach (FunnyChilldrake funnyChilldrake in array)
			{
				if (funnyChilldrake != this)
				{
					funnyChilldrake.ModifySecondaryContents(new string[2] { "* He's lying.^05\n* I'm much cooler!", "* (You're both lame.)" }, new string[2] { "snd_text", "snd_txtsus" }, new int[1], new string[2] { "", "su_inquisitive" });
					funnyChilldrake.ForceTalkedToBefore();
				}
			}
		}
		base.DoInteract();
	}
}
