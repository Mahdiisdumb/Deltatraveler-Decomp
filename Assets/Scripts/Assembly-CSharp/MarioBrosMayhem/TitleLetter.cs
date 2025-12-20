using UnityEngine;
using UnityEngine.UI;

namespace MarioBrosMayhem
{
	public class TitleLetter : MonoBehaviour
	{
		[SerializeField]
		private int index;

		private Image sprite;

		private Sprite[] sprites;

		private Coroutine bumpRoutine;

		private bool bumping;

		private float timer;

		private void Awake()
		{
			sprite = GetComponent<Image>();
			sprites = new Sprite[5];
			Sprite[] array = Resources.LoadAll<Sprite>("mariobros/sprites/ui/title/spr_title_logo");
			for (int i = 0; i < 5; i++)
			{
				sprites[i] = array[i + index * 5];
			}
			sprite.sprite = sprites[0];
		}

		private void Update()
		{
			if (!bumping)
			{
				return;
			}
			timer += Time.deltaTime;
			int num = Mathf.RoundToInt(timer * 60f);
			float num2 = timer / 0.1f;
			if (timer >= 13f / 30f)
			{
				num2 = (0.55f - timer) / (7f / 60f);
			}
			if (num2 <= 0f)
			{
				bumping = false;
			}
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.RoundToInt(Mathf.Lerp(0f, 24f, num2)));
			if (num >= 6)
			{
				if (num < 10)
				{
					sprite.sprite = sprites[1];
				}
				else if (num < 21)
				{
					sprite.sprite = sprites[2];
				}
				else if (num < 25)
				{
					sprite.sprite = sprites[3];
				}
				else
				{
					sprite.sprite = sprites[4];
				}
			}
		}

		public void Bump()
		{
			bumping = true;
		}

		public void Skip()
		{
			bumping = false;
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, 0f, 0f);
			sprite.sprite = sprites[4];
		}
	}
}
