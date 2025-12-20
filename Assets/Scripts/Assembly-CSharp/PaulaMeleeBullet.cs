using UnityEngine;

public class PaulaMeleeBullet : MonoBehaviour
{
	private int frames;

	[SerializeField]
	private Sprite[] sprites;

	private bool animating;

	private Color color;

	private bool spinReverse;

	private bool hard;

	private void Awake()
	{
		color = GetComponent<SpriteRenderer>().color;
		animating = true;
	}

	private void Update()
	{
		if (animating)
		{
			Color b = color;
			b.a = 0f;
			base.transform.eulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0f, 0f, spinReverse ? 30 : (-30)), (float)frames / 7f);
			GetComponent<SpriteRenderer>().sprite = sprites[frames % 2];
			GetComponent<SpriteRenderer>().color = Color.Lerp(color, b, (float)(frames - 4) / 4f);
			if (frames <= 4)
			{
				base.transform.localScale += new Vector3(0.1f, 0.1f);
			}
			else
			{
				if (frames == 5)
				{
					Object.FindObjectOfType<BattleCamera>().HurtShake();
					GetComponent<SpriteRenderer>().sortingOrder = 197;
					for (int i = 0; i < 8; i++)
					{
						Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PaulaMeleeStarBullet"), base.transform.position, Quaternion.identity, base.transform.parent).GetComponent<PaulaMeleeStarBullet>().SetValues(i, base.transform.position, spinReverse, hard);
					}
				}
				base.transform.localScale = Vector3.Lerp(new Vector3(2.5f, 2.5f), new Vector3(1f, 1f), (float)(frames - 4) / 3f);
			}
			if (frames == 25)
			{
				animating = false;
			}
			frames++;
		}
		if (!GetComponent<AudioSource>().isPlaying && !animating)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void ToggleHard()
	{
		color = new Color(1f, 1f, 0.5f) * 0.625f;
		hard = true;
	}
}
