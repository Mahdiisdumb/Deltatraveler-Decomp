using UnityEngine;

public class IceSpot : MonoBehaviour
{
	private OverworldPlayer player;

	private float velocity = 6f;

	private Vector3 direction = Vector3.zero;

	private bool inEncounter;

	private void Update()
	{
		if ((bool)player)
		{
			inEncounter = player.CannotMoveBattleSpecial();
			if (!inEncounter)
			{
				player.GetComponent<Rigidbody2D>().MovePosition(player.transform.position + direction * velocity / 48f);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!inEncounter && (bool)collision.GetComponent<OverworldPlayer>() && !player)
		{
			MonoBehaviour.print("icespot: player entered");
			player = collision.GetComponent<OverworldPlayer>();
			if (Util.GameManager().SusieInParty())
			{
				player.ForceSendPositions();
			}
			Util.GameManager().DisableMenu();
			player.SetMoveState(1);
			direction = new Vector3(UTInput.GetAxis("Horizontal"), UTInput.GetAxis("Vertical"));
			if (direction == Vector3.zero)
			{
				direction = player.GetDirection();
			}
			if (Mathf.Abs(direction.x) > 0f && Mathf.Abs(direction.x) < 1f)
			{
				direction.x = Mathf.Sign(direction.x);
			}
			velocity = player.GetSpeed();
			if (velocity < 6f)
			{
				velocity = 6f;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!inEncounter && (bool)collision.GetComponent<OverworldPlayer>() && (bool)player)
		{
			MonoBehaviour.print("icespot: player left");
			if (Util.GameManager().SusieInParty())
			{
				player.FreeSendPositions();
			}
			player.SetMoveState(0);
			Util.GameManager().EnablePlayerMovement();
			Util.GameManager().EnableMenu();
			player = null;
		}
	}
}
