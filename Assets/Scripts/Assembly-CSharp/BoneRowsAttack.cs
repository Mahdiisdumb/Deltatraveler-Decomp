using UnityEngine;

public class BoneRowsAttack : AttackBase
{
	private GameObject blasterPrefab;

	private Transform leftRow;

	private Transform rightRow;

	private int rate;

	private int maxRate = 37;

	private bool harder;

	protected override void Awake()
	{
		base.Awake();
		blasterPrefab = Resources.Load<GameObject>("battle/attacks/bullets/GasterBlaster");
		bbSize = new Vector2(280f, 140f);
		Util.FindObjectOfType<SOUL>().ChangeSOULMode(0);
		harder = Util.FindObjectOfType<KarmaHandler>();
		if (harder)
		{
			maxRate = 34;
		}
		rate = maxRate - 10;
		maxFrames = 310;
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		rate++;
		if (rate >= maxRate && maxFrames - frames >= 30)
		{
			rate = 0;
			if (maxRate > 20)
			{
				maxRate--;
			}
			SOUL sOUL = Util.FindObjectOfType<SOUL>();
			float x = 0f;
			if (Mathf.Abs(sOUL.transform.position.x) >= 1.15f)
			{
				x = 2.14f * Mathf.Sign(sOUL.transform.position.x);
			}
			Object.Instantiate(blasterPrefab, new Vector3(x, 7.5f), Quaternion.identity).GetComponent<GasterBlaster>().Activate(3, 3, 0f, new Vector2(x, 2.5f), 20);
		}
		leftRow.position += new Vector3(0f, (float)(harder ? 4 : 3) / 48f);
		if (leftRow.position.y >= 1.5f)
		{
			leftRow.position -= new Vector3(0f, 1.5f);
		}
		rightRow.position -= new Vector3(0f, (float)(harder ? 4 : 3) / 48f);
		if (rightRow.position.y <= -1.5f)
		{
			rightRow.position += new Vector3(0f, 1.5f);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Transform transform = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/BoneRows"), base.transform).transform;
		BoneBullet[] componentsInChildren = transform.GetComponentsInChildren<BoneBullet>();
		foreach (BoneBullet obj in componentsInChildren)
		{
			obj.ChangeHeight(harder ? 4 : 3);
			obj.SetTPGrazeValue(0.5f, setReuse: true);
		}
		leftRow = transform.GetChild(0);
		rightRow = transform.GetChild(1);
	}
}
