using UnityEngine;

public class SnowPuddle : InteractItemBox
{
	private void Start()
	{
		if ((int)Util.GameManager().GetFlag(172) != 0 || !Util.GameManager().NoelleInParty())
		{
			purchaseLines = new string[1] { "* (You got the Carrot.)" };
		}
	}

	public override void DoInteract()
	{
		if (empty)
		{
			if ((int)Util.GameManager().GetPersistentFlag(1) == 1)
			{
				Util.GameManager().SetFlag(220, 1);
			}
			if ((int)Util.GameManager().GetFlag(220) == 0)
			{
				Util.GameManager().SetFlag(220, 1);
				if (Random.Range(0, 5) != 0)
				{
					ResetToNormal();
				}
			}
			else
			{
				ResetToNormal();
			}
		}
		base.DoInteract();
	}

	public void ResetToNormal()
	{
		emptyLines = new string[2]
		{
			emptyLines[0],
			emptyLines[1]
		};
	}
}
