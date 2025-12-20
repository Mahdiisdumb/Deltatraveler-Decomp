using System;
using UnityEngine;

public class PeacefulRestStandardAttack : AttackBase
{
	private int sproutCount;

	private int ufoCount;

	private bool robot;

	private bool tree;

	private int totalCount;

	private bool[] sproutAttack;

	private bool[] ufoLaserAttack = new bool[3];

	private bool slowMode;

	private bool magnet;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 140;
		bbSize = new Vector2(165f, 140f);
		attackAllTargets = false;
		if ((bool)UnityEngine.Object.FindObjectOfType<SpinRobo>())
		{
			robot = !UnityEngine.Object.FindObjectOfType<SpinRobo>().IsDone();
			if (robot)
			{
				totalCount++;
			}
		}
		if ((bool)UnityEngine.Object.FindObjectOfType<ExplosiveOak>())
		{
			tree = !UnityEngine.Object.FindObjectOfType<ExplosiveOak>().IsDone() && !UnityEngine.Object.FindObjectOfType<ExplosiveOak>().IsGonnaExplode();
			if (tree)
			{
				totalCount++;
			}
		}
		MobileSprout[] array = UnityEngine.Object.FindObjectsOfType<MobileSprout>();
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].IsDone())
			{
				sproutCount++;
				totalCount++;
			}
		}
		LilUFO[] array2 = UnityEngine.Object.FindObjectsOfType<LilUFO>();
		foreach (LilUFO lilUFO in array2)
		{
			if (!lilUFO.IsDone())
			{
				ufoLaserAttack[ufoCount] = UnityEngine.Random.Range(0, 2) == 0;
				if (ufoLaserAttack[ufoCount])
				{
					lilUFO.ExposeLaser();
				}
				if (lilUFO.IsCheckingForSlow() && !slowMode)
				{
					slowMode = true;
					UnityEngine.Object.FindObjectOfType<SOUL>().ChangeSOULMode(4);
				}
				ufoCount++;
				totalCount++;
			}
		}
		sproutAttack = new bool[sproutCount];
		for (int j = 0; j < sproutCount; j++)
		{
			if (tree || magnet || slowMode)
			{
				sproutAttack[j] = true;
			}
			else
			{
				sproutAttack[j] = UnityEngine.Random.Range(0, 2) == 0;
			}
			if (!sproutAttack[j])
			{
				magnet = true;
			}
		}
		if (magnet)
		{
			bbSize = new Vector3(181f, 140f);
		}
	}

	private void OnDestroy()
	{
		LilUFO[] array = UnityEngine.Object.FindObjectsOfType<LilUFO>();
		foreach (LilUFO lilUFO in array)
		{
			if (!lilUFO.CanSpare() && !lilUFO.IsDone() && lilUFO.IsCheckingForSlow())
			{
				lilUFO.UncheckForSlow();
				lilUFO.AddActPoints(100);
			}
			if (!lilUFO.IsDone())
			{
				lilUFO.UnexposeLaser();
				lilUFO.SetMovement(true);
			}
		}
		if ((bool)UnityEngine.Object.FindObjectOfType<SpinRobo>() && !UnityEngine.Object.FindObjectOfType<SpinRobo>().IsDone())
		{
			UnityEngine.Object.FindObjectOfType<SpinRobo>().GetPart("body").GetComponent<SpriteRenderer>()
				.sprite = Resources.Load<Sprite>("battle/enemies/Spinning Robo/spr_b_robo_body");
		}
		if ((bool)UnityEngine.Object.FindObjectOfType<SOUL>())
		{
			UnityEngine.Object.FindObjectOfType<SOUL>().ChangeSOULMode(0);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		for (int i = 0; i < sproutCount; i++)
		{
			if (sproutAttack[i])
			{
				float y = (magnet ? 0.67f : (-0.21f));
				if ((float)frames % (6f * ((float)(totalCount + 1) / 2f)) == 1f)
				{
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/SproutFallPelletBullet"), new Vector3(UnityEngine.Random.Range(-1.53f, 1.53f) + bb.transform.position.x, y), Quaternion.identity, base.transform);
				}
			}
		}
		int num = 10 + 10 * totalCount;
		if (frames % num == 1)
		{
			LilUFO[] array = UnityEngine.Object.FindObjectsOfType<LilUFO>();
			foreach (LilUFO lilUFO in array)
			{
				if (!lilUFO.IsDone() && lilUFO.LaserExposed())
				{
					RoboLaserBullet component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/RoboLaserBullet"), base.transform).GetComponent<RoboLaserBullet>();
					component.transform.position += lilUFO.GetPart("body").transform.parent.localPosition + new Vector3(lilUFO.GetEnemyObject().transform.position.x, 0f);
					component.Activate(lilUFO);
					lilUFO.SetMovement(false);
				}
			}
		}
		if (robot && ((frames % num == num / 2 && totalCount > 1) || (frames % num == 1 && totalCount == 1)))
		{
			SpinRobo spinRobo = UnityEngine.Object.FindObjectOfType<SpinRobo>();
			if (!spinRobo.IsDone())
			{
				RoboLaserBullet component2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/RoboLaserBullet"), base.transform).GetComponent<RoboLaserBullet>();
				component2.GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0.8f);
				component2.transform.position = new Vector3(spinRobo.GetEnemyObject().transform.position.x - 0.213f, 2.948f);
				component2.Activate(spinRobo);
				spinRobo.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Spinning Robo/spr_b_robo_body_shoot");
			}
		}
		for (int k = 0; k < ufoCount; k++)
		{
			if (!ufoLaserAttack[k] && frames % (8 + (totalCount - 1) * 6) == 1)
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/UFOBullet"), base.transform);
			}
		}
		if (tree)
		{
			bb.transform.position = new Vector3(Mathf.Sin((float)(frames * 4) * ((float)Math.PI / 180f)), bb.transform.position.y);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		if (magnet)
		{
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/PSIMagnetBullet"), base.transform);
			bb.StartMovement(new Vector3(181f, 180f));
			if (totalCount == 1)
			{
				UnityEngine.Object.FindObjectOfType<PSIMagnetBullet>().ToggleStrongMode();
				maxFrames = 60;
			}
		}
		if (!UnityEngine.Object.FindObjectOfType<ExplosiveOak>())
		{
			return;
		}
		if (UnityEngine.Object.FindObjectOfType<ExplosiveOak>().IsGonnaExplode())
		{
			UnityEngine.Object.FindObjectOfType<ExplosiveOak>().Explode();
			if (totalCount == 0)
			{
				maxFrames = 80;
			}
		}
		else if (tree && totalCount == 1)
		{
			maxFrames = 60;
		}
	}
}
