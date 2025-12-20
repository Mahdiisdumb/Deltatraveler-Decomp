using UnityEngine;

public class Grillby : InteractTextBox
{
	protected override void Update()
	{
		base.Update();
		if ((bool)txt && txt.GetCurrentStringNum() == 2)
		{
			GameObject.Find("RedBird").GetComponent<InteractTextBox>().SetTalkable(txt);
			txt = null;
		}
	}
}
