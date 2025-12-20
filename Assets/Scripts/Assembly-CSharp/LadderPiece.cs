using System;
using UnityEngine;

public class LadderPiece : Interactable
{
	[SerializeField]
	private int flag;

	private bool pickup;

	private void Awake()
	{
		if ((int)Util.GameManager().GetFlag(flag) >= 1)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (!txt && pickup)
		{
			if (flag == 227)
			{
				OverworldSnowy component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/enemies/Feraldrake2"), GameObject.Find("NPC").transform).GetComponent<OverworldSnowy>();
				component.transform.localScale = new Vector3(1f, 1f, 1f);
				component.DetectPlayer();
			}
			else if (flag == 208)
			{
				Util.GameManager().LockMenu();
				CutsceneHandler.GetCutscene(77).StartCutscene();
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public override void DoInteract()
	{
		txt = new GameObject("LadderPieceTextbox").AddComponent<TextBox>();
		if ((int)Util.GameManager().GetFlag(210) == 0)
		{
			bool flag = !Util.GameManager().NoelleInParty();
			txt.CreateBox(new string[2]
			{
				"* Hey,^05 let's avoid\n  grabbing these for now.",
				flag ? "* We should really go\n  to that bunny house." : "* Not really in the mood\n  to fight more of them\n  right now."
			}, new string[1] { "snd_txtsus" }, new int[1], giveBackControl: true, new string[2] { "su_annoyed", "su_side" });
		}
		else if (this.flag == 208 && ((int)Util.GameManager().GetFlag(227) == 0 || (int)Util.GameManager().GetFlag(228) == 0))
		{
			txt.CreateBox(new string[1] { "* (For some reason,^05 you felt that\n  you should get this one\n  after getting the rest.)" }, giveBackControl: true);
		}
		else if ((int)Util.GameManager().GetFlag(178) == 0)
		{
			txt.CreateBox(new string[1] { "* (For some reason,^05 you felt that\n  you should come back here\n  with a torch.)" }, giveBackControl: true);
		}
		else
		{
			if (this.flag == 208)
			{
				Util.GameManager().SetCheckpoint(90, new Vector3(-5.87f, 0.656f));
			}
			txt.CreateBox(new string[1] { "* (Susie got a ladder piece.)" }, this.flag != 208);
			pickup = true;
			Util.GameManager().SetFlag(this.flag, 1);
		}
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
	}

	public override int GetEventData()
	{
		throw new NotImplementedException();
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		throw new NotImplementedException();
	}
}
