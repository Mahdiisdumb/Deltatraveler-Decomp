using UnityEngine;

namespace MarioBrosMayhem
{
	public class FreezeSpot : MonoBehaviour
	{
		[SerializeField]
		private Platform platform;

		[SerializeField]
		private int freezeId;

		private Freezie freezie;

		private Vector3 oldPos;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			OnTriggerStay2D(collision);
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if (!freezie && (bool)collision.GetComponent<Freezie>())
			{
				freezie = collision.GetComponent<Freezie>();
				oldPos = freezie.transform.position;
			}
			else if ((bool)freezie)
			{
				if (freezie.Grounded() && platform.CanFreeze() && !freezie.IsPerformingAction() && ((oldPos.x > base.transform.position.x && freezie.transform.position.x < base.transform.position.x) || (oldPos.x < base.transform.position.x && freezie.transform.position.x > base.transform.position.x)))
				{
					freezie.StartFreezing(new Vector3(base.transform.position.x, freezie.transform.position.y), freezeId);
				}
				oldPos = freezie.transform.position;
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if ((bool)freezie)
			{
				freezie = null;
			}
		}

		public int GetSpotID()
		{
			return freezeId;
		}

		public Platform GetPlatform()
		{
			return platform;
		}
	}
}
