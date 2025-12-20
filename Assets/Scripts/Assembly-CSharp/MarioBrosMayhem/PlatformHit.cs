using UnityEngine;

namespace MarioBrosMayhem
{
	public class PlatformHit : MonoBehaviour
	{
		private int platformType;

		private int playerId = -1;

		private int hitX;

		private void LateUpdate()
		{
			SpriteRenderer component = GetComponent<SpriteRenderer>();
			if (platformType > 0 && component.sprite.name.Contains("standard"))
			{
				component.sprite = Resources.Load<Sprite>("mariobros/sprites/objects/platforms/" + component.sprite.name.Replace("standard", GlobalVariables.PLATFORM_NAMES[platformType]));
			}
		}

		public void SetValues(int playerId, int hitX, bool edge, bool rightEdge, int platformType, int alpha)
		{
			this.playerId = playerId;
			this.hitX = hitX;
			this.platformType = platformType;
			GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
			if (edge)
			{
				GetComponent<SpriteMask>().sprite = Resources.Load<Sprite>("mariobros/sprites/objects/platforms/spr_platform_hit_mask_" + (rightEdge ? "rightedge" : "leftedge"));
			}
			GetComponent<BoxCollider2D>().enabled = true;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			OnTriggerStay2D(collision);
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if (playerId == -1)
			{
				return;
			}
			if ((bool)collision.GetComponent<Enemy>() && collision.GetComponent<Enemy>().Grounded())
			{
				collision.GetComponent<Enemy>().Flip(playerId, base.transform.position);
				if (collision.GetComponent<Enemy>().IsFlipped() && (bool)Object.FindObjectOfType<MarioBrosNetworkManager>())
				{
					Object.FindObjectOfType<MarioBrosNetworkManager>().FlipOverEnemyFromPlatform(Object.FindObjectOfType<Player>());
				}
			}
			else if ((bool)collision.GetComponent<Coin>())
			{
				collision.GetComponent<Coin>().CollectCoin(playerId);
			}
			else if ((bool)collision.GetComponent<Fireball>() && collision.GetComponent<Fireball>().CanBeKilled())
			{
				collision.GetComponent<Fireball>().Kill(playerId);
				Object.FindObjectOfType<MarioBrosNetworkManager>().KillFireball(Object.FindObjectOfType<Player>(), collision.GetComponent<Fireball>().IsRed(), collision.transform.position);
			}
			else if ((bool)collision.GetComponent<Freezie>() && collision.GetComponent<Freezie>().Grounded())
			{
				collision.GetComponent<Freezie>().Die(playerId);
			}
			else if ((bool)collision.GetComponent<Mushroom>() && collision.GetComponent<Mushroom>().Grounded())
			{
				collision.GetComponent<Mushroom>().HitFromBelow(playerId);
			}
		}

		private void DestroyHit()
		{
			Object.Destroy(base.gameObject);
		}

		public int GetPlayerId()
		{
			return playerId;
		}

		public int GetHitX()
		{
			return hitX;
		}
	}
}
