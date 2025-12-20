using UnityEngine;

public class PelletLinesAttack : AttackBase
{
	private int lineFrames;

	private float lineX;

	private float lineY;

	private bool horizontal;

	private bool hardmode;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 200;
		bbSize = new Vector2(165f, 140f);
		hardmode = (int)Util.GameManager().GetFlag(108) == 1;
		attackAllTargets = false;
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		if (frames % (hardmode ? 16 : 20) == 1)
		{
			lineFrames = 0;
		}
		if (lineFrames >= 5 || frames % 2 != 0)
		{
			return;
		}
		if (lineFrames == 0)
		{
			horizontal = Random.Range(0, 2) == 0;
			if (horizontal)
			{
				lineX = Random.Range(-3f, 1f);
				lineY = Random.Range(0f, 1f);
			}
			else
			{
				lineX = Random.Range(2.25f, 3f) * (float)((Random.Range(0, 2) == 0) ? 1 : (-1));
				lineY = Random.Range(-0.16f, -3f);
			}
		}
		Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/flowey/FloweyPelletStandard"), new Vector3(lineX, lineY), Quaternion.identity, base.transform).GetComponent<FloweyPelletStandard>().SetMovementOffset(-lineFrames * 2);
		lineFrames++;
		if (horizontal)
		{
			lineX += 0.5f;
		}
		else
		{
			lineY -= 0.5f;
		}
	}
}
