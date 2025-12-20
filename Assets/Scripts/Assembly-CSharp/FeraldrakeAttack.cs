using UnityEngine;

public class FeraldrakeAttack : AttackBase
{
	private int attack;

	private bool diagonal;

	private int crossSpawnFrames;

	private int crossSpawnAt;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(185f, 140f);
		maxFrames = 200;
		attack = Random.Range(0, 2);
		if (attack == 1)
		{
			maxFrames = 180;
		}
	}

	protected override void Update()
	{
		if (isStarted)
		{
			base.Update();
			bool flag = frames % 7 == 1;
			if (attack == 1)
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			if (attack == 0)
			{
				Vector3 position = Vector3.zero;
				bool num = Random.Range(0, 2) == 0;
				bool flag2 = Random.Range(0, 2) == 0;
				position = ((!num) ? new Vector3(Random.Range(-2f, 2f), flag2 ? (-3.4f) : 1.1f) : new Vector3(flag2 ? (-2.2f) : 2.2f, Random.Range(-3.2f, 0.9f)));
				Vector2 zero = Vector2.zero;
				zero = ((!num) ? (flag2 ? Vector2.up : Vector2.down) : (flag2 ? Vector2.right : Vector2.left));
				SnowdrakeSpinBullet component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/SnowdrakeSpinBullet"), base.transform).GetComponent<SnowdrakeSpinBullet>();
				component.transform.position = position;
				component.Activate(Random.Range(3f, 5f), zero);
			}
			else if (crossSpawnFrames >= crossSpawnAt)
			{
				if (crossSpawnAt == 0)
				{
					crossSpawnAt = 25;
				}
				else if (crossSpawnAt > 17)
				{
					crossSpawnAt -= 2;
				}
				crossSpawnFrames = 0;
				Vector2[] array = new Vector2[4]
				{
					Vector2.down,
					Vector2.up,
					Vector2.left,
					Vector2.right
				};
				if (diagonal)
				{
					array[0] += Vector2.right;
					array[1] += Vector2.left;
					array[2] += Vector2.down;
					array[3] += Vector2.up;
				}
				if (diagonal)
				{
					float num2 = Mathf.Sqrt(2f) / 2f;
				}
				for (int i = 0; i < 4; i++)
				{
					Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/FeraldrakeCrossBullet"), base.transform).GetComponent<FeraldrakeCrossBullet>().Activate(Object.FindObjectOfType<SOUL>().transform.position, array[i], i == 0);
				}
				diagonal = !diagonal;
			}
			else
			{
				crossSpawnFrames++;
			}
		}
		else if (!Object.FindObjectOfType<Feraldrake>().Roaring() && !Object.FindObjectOfType<TextBubble>() && !bb.IsPlaying())
		{
			StartAttack();
		}
	}

	public override void StartAttack()
	{
		if (!Object.FindObjectOfType<Feraldrake>().Roaring() && !Object.FindObjectOfType<TextBubble>() && !bb.IsPlaying())
		{
			base.StartAttack();
			bb.StartMovement(new Vector2(185f, 190f));
		}
	}
}
