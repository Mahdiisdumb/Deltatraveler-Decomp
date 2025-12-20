using UnityEngine;

namespace MarioBrosMayhem
{
	public class FakePlatform : MonoBehaviour
	{
		private bool activated;

		private float timer;

		private int startSize;

		private void Update()
		{
			if (activated)
			{
				timer += Time.deltaTime;
				int num = startSize - (int)(timer * 60f) * 8;
				if (num > 0)
				{
					GetComponent<SpriteRenderer>().size = new Vector2((float)num / 24f, 1f / 3f);
				}
				else
				{
					Object.Destroy(base.gameObject);
				}
			}
		}

		public void Activate(float startSize)
		{
			this.startSize = Mathf.RoundToInt(startSize * 24f);
			GetComponent<SpriteRenderer>().size = new Vector2(startSize, 1f / 3f);
			activated = true;
		}
	}
}
