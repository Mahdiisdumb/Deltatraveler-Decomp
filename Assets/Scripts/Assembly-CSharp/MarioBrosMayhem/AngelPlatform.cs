using UnityEngine;

namespace MarioBrosMayhem
{
	public class AngelPlatform : MonoBehaviour
	{
		private Sprite[] sprites;

		private SpriteRenderer sr;

		private void Awake()
		{
			sr = GetComponent<SpriteRenderer>();
			sprites = Resources.LoadAll<Sprite>("mariobros/sprites/objects/spr_angel_platform");
		}

		public void SetVisual(float timer)
		{
			sr.enabled = true;
			if (timer < 3.2333333f)
			{
				sr.sprite = sprites[0];
			}
			else if (timer < 4.3f)
			{
				sr.sprite = sprites[1];
			}
			else
			{
				sr.sprite = sprites[2];
			}
		}

		public void Hide()
		{
			sr.enabled = false;
		}
	}
}
