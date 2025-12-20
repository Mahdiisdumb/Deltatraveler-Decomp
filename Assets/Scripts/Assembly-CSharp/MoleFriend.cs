using UnityEngine;

public class MoleFriend : OverworldPartyMember
{
	private void Start()
	{
		if ((int)Util.GameManager().GetFlag(151) == 0)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			base.Awake();
		}
	}
}
