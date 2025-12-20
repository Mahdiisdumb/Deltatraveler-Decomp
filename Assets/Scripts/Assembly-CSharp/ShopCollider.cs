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
		fade = Object.FindObjectOfType<Fade>();
	}

	private void Update()
	{
		if (!activated || fade.IsPlaying())
		{
			return;
		}
		Vector2 vector = (vertical ? Vector2.up : Vector2.right);
		if (downOrLeft)
		{
			vector *= -1f;
		}
		Object.FindObjectOfType<OverworldPlayer>().transform.position = newPos;
		Object.FindObjectOfType<OverworldPlayer>().ChangeDirection(vector);
		OverworldPartyMember[] array = Object.FindObjectsOfType<OverworldPartyMember>();
		foreach (OverworldPartyMember overworldPartyMember in array)
		{
			if ((overworldPartyMember.gameObject.name == "Susie" && Util.GameManager().SusieInParty()) || (overworldPartyMember.gameObject.name == "Noelle" && Util.GameManager().NoelleInParty()))
			{
				overworldPartyMember.transform.position = newPos + overworldPartyMember.GetPositionOffset();
				overworldPartyMember.ChangeDirection(vector);
			}
		}
		Object.Instantiate(shop, GameObject.Find("Canvas").transform);
		activated = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Player" && !activated)
		{
			if ((bool)Object.FindObjectOfType<MainMenu>())
			{
				Object.FindObjectOfType<MainMenu>().CancelControlReturn();
				Object.Destroy(Object.FindObjectOfType<MainMenu>().gameObject);
			}
			if ((bool)Object.FindObjectOfType<PunchCard>())
			{
				Object.Destroy(Object.FindObjectOfType<PunchCard>().gameObject);
			}
			Util.GameManager().DisablePlayerMovement(true);
			fade.FadeOut(7);
			Util.GameManager().StopMusic(7f);
			activated = true;
		}
	}
}
