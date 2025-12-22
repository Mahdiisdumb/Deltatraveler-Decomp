using UnityEngine;

namespace MarioBrosMayhem
{
	public class PowBlock : MonoBehaviour
	{
		private readonly float[] LEVEL_HEIGHTS = new float[4]
		{
			0f,
			1f / 3f,
			0.5f,
			2f / 3f
		};

		[SerializeField]
		private int powId;

		[SerializeField]
		private Sprite[] sprites;

		private int level = 3;

		private AudioSource aud;

		private SpriteRenderer sr;

		private void Awake()
		{
			aud = GetComponent<AudioSource>();
			sr = GetComponent<SpriteRenderer>();
		}

		public void Hit(int playerId)
		{
			PowToLevel(playerId, level - 1, sendToServer: true);
		}

		public void PowToLevel(int playerId, int level, bool sendToServer = false, bool pickupBlock = false)
		{
			aud.Play();
			Util.FindObjectOfType<CameraController>().PowHit();
			this.level = level;
			if (level == 0)
			{
				GetComponent<BoxCollider2D>().enabled = false;
			}
			GetComponent<BoxCollider2D>().size = new Vector2(2f / 3f, LEVEL_HEIGHTS[level]);
			sr.sprite = sprites[level];
			Enemy[] array = Util.FindObjectsOfType<Enemy>();
			foreach (Enemy enemy in array)
			{
				if (enemy.Grounded())
				{
					enemy.Flip(playerId, 0, playAudio: false);
				}
			}
			Coin[] array2 = Util.FindObjectsOfType<Coin>();
			foreach (Coin coin in array2)
			{
				if (coin.Grounded() && (bool)Util.FindObjectOfType<MarioBrosManager>())
				{
					coin.CollectCoin(playerId);
				}
			}
			Fireball[] array3 = Util.FindObjectsOfType<Fireball>();
			foreach (Fireball fireball in array3)
			{
				if (fireball.CanBeKilled())
				{
					fireball.Kill(playerId, playSound: false);
				}
			}
			Freezie[] array4 = Util.FindObjectsOfType<Freezie>();
			foreach (Freezie freezie in array4)
			{
				if (freezie.Grounded())
				{
					freezie.Die(playerId);
				}
			}
			Icicle[] array5 = Util.FindObjectsOfType<Icicle>();
			foreach (Icicle icicle in array5)
			{
				if (icicle.CanBeKilled())
				{
					icicle.Kill(playerId, playSound: false);
				}
			}
			Mushroom[] array6 = Util.FindObjectsOfType<Mushroom>();
			foreach (Mushroom mushroom in array6)
			{
				if (mushroom.Grounded())
				{
					mushroom.HitFromBelow(playerId);
				}
			}
			Util.FindObjectOfType<MarioBrosNetworkManager>().HitPowBlock(powId);
		}

		public void VanishPowBlock()
		{
			level = 0;
			GetComponent<BoxCollider2D>().enabled = false;
			sr.sprite = sprites[level];
		}

		public void ResetBlock()
		{
			level = 3;
			sr.sprite = sprites[3];
			GetComponent<BoxCollider2D>().enabled = true;
			GetComponent<BoxCollider2D>().size = new Vector2(2f / 3f, LEVEL_HEIGHTS[level]);
		}

		public int GetPowId()
		{
			return powId;
		}

		public int GetPowLevel()
		{
			return level;
		}
	}
}
