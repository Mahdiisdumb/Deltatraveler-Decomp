using UnityEngine;

public class ShopCollider : OverworldManipulator
{
	[SerializeField]
	private GameObject shop;

	[SerializeField]
	private Vector3 newPos = Vector3.zero;

	[SerializeField]
	private bool vertical = true;

	[SerializeField]
	private bool downOrLeft = true;

	private Fade fade;

	private bool activated;

	private void Start()
	{
		fade = Util.FindObjectOfType<Fade>();
	}

	private void Update()
	{
		if (!activated || fade.IsPlaying())
		{
			return;
		}
		Vector2 faceDir = (vertical ? Vector2.up : Vector2.right);
		if (downOrLeft)
		{
			faceDir *= -1f;
		}
		Util.OverworldPlayer().transform.position = newPos;
		Util.OverworldPlayer().ChangeDirection(faceDir);
		OverworldPartyMember[] array = Util.FindObjectsOfType<OverworldPartyMember>();
		foreach (OverworldPartyMember overworldPartyMember in array)
		{
			if ((overworldPartyMember.GetMemberID() == 1 && Util.GameManager().SusieInParty()) || (overworldPartyMember.GetMemberID() == 2 && Util.GameManager().NoelleInParty()))
			{
				overworldPartyMember.transform.position = newPos + overworldPartyMember.GetPositionOffset();
				overworldPartyMember.ChangeDirection(faceDir);
			}
		}
		Object.Instantiate(shop, GameObject.Find("Canvas").transform);
		activated = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Player" && !activated)
		{
			if ((bool)Util.FindObjectOfType<OverworldMenu>())
			{
				Util.FindObjectOfType<OverworldMenu>().CancelControlReturn();
				Object.Destroy(Util.FindObjectOfType<OverworldMenu>().gameObject);
			}
			if ((bool)Util.FindObjectOfType<PunchCard>())
			{
				Object.Destroy(Util.FindObjectOfType<PunchCard>().gameObject);
			}
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: true);
			fade.FadeOut(7);
			Util.GameManager().StopMusic(7f);
			activated = true;
		}
	}
}
