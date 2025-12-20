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
		if (!Object.FindObjectOfType<OverworldPlayer>().IsInitiatingBattle())
		{
			frames++;
			Object.FindObjectOfType<OverworldPlayer>().transform.position = Vector3.Lerp(origPos, newPos, (float)frames / 40f);
			if (frames == 40)
			{
				Object.FindObjectOfType<OverworldPlayer>().GetComponent<SpriteRenderer>().enabled = true;
				Object.FindObjectOfType<OverworldPlayer>().SetCollision(true);
				OverworldPartyMember component = GameObject.Find("Susie").GetComponent<OverworldPartyMember>();
				component.GetComponent<SpriteRenderer>().enabled = true;
				component.transform.position = Object.FindObjectOfType<OverworldPlayer>().transform.position + component.GetPositionOffset();
				component.ChangeDirection(Vector2.down);
				component.Activate();
				Object.FindObjectOfType<GameManager>().EnablePlayerMovement();
				isPlaying = false;
			}
		}
		else
		{
			isPlaying = false;
			Object.FindObjectOfType<OverworldPlayer>().transform.position = newPos;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>() && !isPlaying && collision.GetComponent<OverworldPlayer>().CanMove())
		{
			origPos = collision.transform.position;
			newPos = origPos + new Vector3(0f, 16.68f + yOffset);
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(true);
			Object.FindObjectOfType<OverworldPlayer>().SetCollision(false);
			isPlaying = true;
			frames = 0;
			collision.GetComponent<SpriteRenderer>().enabled = false;
			collision.GetComponent<BoxCollider2D>().enabled = false;
			collision.GetComponent<OverworldPlayer>().ChangeDirection(Vector2.down);
			OverworldPartyMember component = GameObject.Find("Susie").GetComponent<OverworldPartyMember>();
			component.Deactivate();
			component.ResetPathLists();
			component.GetComponent<SpriteRenderer>().enabled = false;
		}
	}
}
