using UnityEngine;

namespace MarioBrosMayhem
{
	public class ForceMove : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if ((bool)collision && (bool)collision.GetComponent<Player>())
			{
				collision.GetComponent<Player>().SetForceMove(forceMove: true);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if ((bool)collision && (bool)collision.GetComponent<Player>() && Mathf.Sign(collision.transform.position.x) == Mathf.Sign(base.transform.position.x))
			{
				collision.GetComponent<Player>().SetForceMove(forceMove: false);
			}
		}
	}
}
