using UnityEngine;

public class HoleRecoveryDoor : MonoBehaviour
{
	private int frames;

	private bool isPlaying;

	private Vector3 origPos;

	private Vector3 newPos;

	[SerializeField]
	private float yOffset;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (!Util.OverworldPlayer().IsInitiatingBattle())
		{
			frames++;
			Util.OverworldPlayer().transform.position = Vector3.Lerp(origPos, newPos, (float)frames / 40f);
			if (frames == 40)
			{
				Util.OverworldPlayer().GetComponent<SpriteRenderer>().enabled = true;
				Util.OverworldPlayer().SetCollision(onoff: true);
				OverworldPartyMember partyMemberByID = Util.OverworldPlayer().GetPartyMemberByID(1);
				partyMemberByID.GetComponent<SpriteRenderer>().enabled = true;
				partyMemberByID.transform.position = Util.OverworldPlayer().transform.position + partyMemberByID.GetPositionOffset();
				partyMemberByID.ChangeDirection(Vector2.down);
				partyMemberByID.Activate();
				Util.GameManager().EnablePlayerMovement();
				isPlaying = false;
			}
		}
		else
		{
			isPlaying = false;
			Util.OverworldPlayer().transform.position = newPos;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision && (bool)collision.GetComponent<OverworldPlayer>() && !isPlaying && collision.GetComponent<OverworldPlayer>().CanMove())
		{
			origPos = collision.transform.position;
			newPos = origPos + new Vector3(0f, 16.68f + yOffset);
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: true);
			Util.OverworldPlayer().SetCollision(onoff: false);
			isPlaying = true;
			frames = 0;
			collision.GetComponent<SpriteRenderer>().enabled = false;
			collision.GetComponent<BoxCollider2D>().enabled = false;
			collision.GetComponent<OverworldPlayer>().ChangeDirection(Vector2.down);
			OverworldPartyMember partyMemberByID = Util.OverworldPlayer().GetPartyMemberByID(1);
			partyMemberByID.Deactivate();
			partyMemberByID.ResetPathLists();
			partyMemberByID.GetComponent<SpriteRenderer>().enabled = false;
		}
	}
}
