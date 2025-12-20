using UnityEngine;

namespace MarioBrosMayhem
{
	public class Score : MonoBehaviour
	{
		[SerializeField]
		private bool big;

		[SerializeField]
		private bool stationary;

		private float life;

		private float lifespan = 0.5f;

		private void Awake()
		{
			if (big)
			{
				SpriteRenderer component = GetComponent<SpriteRenderer>();
				int num = int.Parse(component.sprite.name.Substring(component.sprite.name.LastIndexOf("_") + 1));
				if (num > 1)
				{
					num = 3;
				}
				lifespan = 1.5f + (float)num / 3f;
			}
		}

		private void Update()
		{
			if (!stationary)
			{
				base.transform.position += new Vector3(0f, 1.25f * Time.deltaTime);
			}
			life += Time.deltaTime;
			if (life >= lifespan)
			{
				Object.Destroy(base.gameObject);
			}
		}

		public void SetPalette(int skin, int palette)
		{
			GetComponent<SpriteRenderer>().material = GlobalVariables.GetHUDPaletteMaterial(skin, palette);
		}
	}
}
