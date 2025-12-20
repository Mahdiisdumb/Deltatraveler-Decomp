using System.Collections.Generic;
using UnityEngine;

public class SlideStart : OverworldManipulator
{
	private OverworldPlayer player;

	private OverworldPartyMember susie;

	private OverworldPartyMember noelle;

	private MoleFriend mole;

	private bool activated;

	private List<Vector3> path;

	private int susieIndex;

	private int noelleIndex;

	private int moleIndex;

	protected override void Awake()
	{
		base.Awake();
		path = new List<Vector3>();
	}

	private void LateUpdate()
	{
		if (!activated)
		{
			return;
		}
		if ((bool)player)
		{
			if (player.IsSliding())
			{
				path.Add(player.transform.position);
			}
			else
			{
				player = null;
			}
		}
		if ((bool)susie)
		{
			if (susieIndex < path.Count)
			{
				susie.transform.position = path[susieIndex] + susie.GetPositionOffset();
				susieIndex++;
			}
			else
			{
				susieIndex = 0;
				susie.StopSliding();
				susie = null;
			}
		}
		if ((bool)noelle)
		{
			if (noelleIndex < path.Count)
			{
				noelle.transform.position = path[noelleIndex] + noelle.GetPositionOffset();
				noelleIndex++;
			}
			else
			{
				noelleIndex = 0;
				noelle.StopSliding();
				noelle = null;
			}
		}
		if ((bool)mole)
		{
			if (moleIndex < path.Count)
			{
				mole.transform.position = path[moleIndex] + mole.GetPositionOffset();
				moleIndex++;
			}
			else
			{
				moleIndex = 0;
				mole.StopSliding();
				mole = null;
			}
		}
		if (player == null && susie == null && noelle == null && mole == null)
		{
			activated = false;
			path.Clear();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>() && !activated)
		{
			activated = true;
			player = collision.GetComponent<OverworldPlayer>();
			player.StartSliding();
		}
		else if ((bool)collision.GetComponent<OverworldPartyMember>() && activated)
		{
			collision.GetComponent<OverworldPartyMember>().StartSliding();
			if (collision.gameObject.name == "Susie" && susie == null)
			{
				susie = collision.GetComponent<OverworldPartyMember>();
				MonoBehaviour.print("susie now sliding");
			}
			else if (collision.gameObject.name == "Noelle" && noelle == null)
			{
				noelle = collision.GetComponent<OverworldPartyMember>();
				MonoBehaviour.print("noelle now sliding");
			}
			else if ((bool)collision.GetComponent<MoleFriend>() && mole == null)
			{
				mole = collision.GetComponent<MoleFriend>();
				MonoBehaviour.print("mole now sliding");
			}
		}
	}
}
