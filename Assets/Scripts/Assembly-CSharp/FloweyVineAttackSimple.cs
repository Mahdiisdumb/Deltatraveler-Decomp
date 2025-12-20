using UnityEngine;

public class FloweyVineAttackSimple : AttackBase
{
	private bool lastVertical;

	private bool lastOtherSide;

	private bool hardmode;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 270;
		bbSize = new Vector2(165f, 140f);
		bbPos = new Vector2(0f, 2f);
		soulPos = bbPos;
		hardmode = (int)Util.GameManager().GetFlag(108) == 1;
		Object.FindObjectOfType<Flowey>().SetHeadOffset(new Vector3(0f, 1.22f));
		attackAllTargets = false;
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		if (frames >= 13 && frames <= 18)
		{
			if (frames == 13)
			{
				Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_grab");
			}
			int num = 18 - frames;
			bb.transform.position = (Vector3)bbPos + new Vector3(Random.Range(0, 3) - 1, Random.Range(0, 3) - 1) * ((float)num / 48f);
		}
		if (frames >= 25 && frames <= 210 && frames % (hardmode ? 24 : 30) == (hardmode ? 4 : 0))
		{
			bool flag = Random.Range(0, 2) == 0;
			bool flag2 = Random.Range(0, 2) == 0;
			if (lastVertical == flag && flag2 == lastOtherSide)
			{
				if (Random.Range(0, 2) == 0)
				{
					flag = !flag;
				}
				else
				{
					flag2 = !flag2;
				}
			}
			lastVertical = flag;
			lastOtherSide = flag2;
			Vector3 position = Vector3.zero;
			Vector2 zero = Vector2.zero;
			if (flag)
			{
				position = new Vector3(Random.Range(-1.43f, 1.43f), flag2 ? 4 : 0);
				zero = (flag2 ? Vector2.down : Vector2.up);
			}
			else
			{
				position = new Vector3(flag2 ? (-2.26f) : 2.26f, Random.Range(0.76f, 3.33f));
				zero = (flag2 ? Vector2.right : Vector2.left);
			}
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/flowey/FloweyVineBullet"), position, Quaternion.identity, base.transform).GetComponent<FloweyVineBullet>().Activate(zero);
		}
		if (frames == 250)
		{
			Object.FindObjectOfType<Flowey>().GetPart("vineLeft").GetComponent<Animator>()
				.Play("Uninsert");
			Object.FindObjectOfType<Flowey>().GetPart("vineRight").GetComponent<Animator>()
				.Play("Uninsert");
		}
		if (frames == 258)
		{
			Object.FindObjectOfType<Flowey>().SetHeadOffset(Vector3.zero);
			Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_grab");
		}
		if (frames >= 259 && frames <= 265)
		{
			int num2 = 265 - frames;
			bb.transform.position = (Vector3)bbPos + new Vector3(Random.Range(0, 3) - 1, Random.Range(0, 3) - 1) * ((float)num2 / 48f);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.FindObjectOfType<Flowey>().GetPart("vineLeft").GetComponent<Animator>()
			.Play("Insert");
		Object.FindObjectOfType<Flowey>().GetPart("vineRight").GetComponent<Animator>()
			.Play("Insert");
	}
}
