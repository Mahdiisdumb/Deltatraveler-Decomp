using UnityEngine;

public class PaintBlobBullet : BulletBase
{
	private int speed = 3;

	[SerializeField]
	private Sprite[] sprites;

	private bool moveRight = true;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 9;
		destroyOnHit = true;
		ChangeType(Random.Range(0, 2));
		if (base.gameObject.tag == "BlueBullet" && !Object.FindObjectOfType<Carpainter>())
		{
			speed = 5;
		}
		moveRight = Random.Range(0, 2) == 0;
		int num = (moveRight ? (-3) : 3);
		float[] obj = new float[4] { -2.688f, -2.048f, -1.328f, -0.692f };
		int num2 = Random.Range(0, 4);
		if ((num2 <= 1 && Object.FindObjectOfType<SOUL>().transform.position.y > -1.685f) || (num2 > 1 && Object.FindObjectOfType<SOUL>().transform.position.y < -1.685f))
		{
			num2 = Random.Range(0, 4);
		}
		float y = obj[num2];
		base.transform.position = new Vector3(num, y);
		if (!moveRight)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
			speed *= -1;
		}
	}

	private void Update()
	{
		frames++;
		base.transform.position += new Vector3((float)speed / 48f, 0f);
		sr.sprite = sprites[frames / 3 % 4];
		if (Mathf.Abs(base.transform.position.x) > 3f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public override void SOULHit()
	{
		if ((bool)Object.FindObjectOfType<PaintBlobAttack>())
		{
			Object.FindObjectOfType<PaintBlobAttack>().GetHit();
		}
		if ((bool)Object.FindObjectOfType<CarPaintBlobAttack>())
		{
			Object.FindObjectOfType<CarPaintBlobAttack>().GetHit();
		}
		base.SOULHit();
	}
}
