using System;
using UnityEngine;

public class BasicBlueHopSlamAttack : AttackBase
{
	private GameObject boneGroup1;

	private GameObject boneGroup2;

	private GameObject boneGroup3;

	private GameObject boneGroup4;

	private GameObject boneGroup5;

	private GameObject boneGroup6;

	private GameObject boneSideLineLeft;

	private GameObject boneSideLineRight;

	private bool sansWeaponOut;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 340;
		bbSize = new Vector2(375f, 140f);
		soulPos = new Vector2(-0.055f, -2.83f);
		UnityEngine.Object.FindObjectOfType<SOUL>().ChangeSOULMode(1);
	}

	protected override void Update()
	{
		if (!isStarted)
		{
			return;
		}
		base.Update();
		boneSideLineLeft.transform.position = new Vector3(boneSideLineLeft.transform.position.x, Mathf.Lerp(0.21f, 0.47f, (float)(frames % 6) / 6f));
		boneSideLineRight.transform.position = new Vector3(boneSideLineRight.transform.position.x, Mathf.Lerp(0.47f, 0.21f, (float)(frames % 6) / 6f));
		if (frames == 30 || frames == 90 || frames == 184)
		{
			UnityEngine.Object.FindObjectOfType<Sans>().ResetBreatheAnimation();
		}
		if (frames == 1)
		{
			UnityEngine.Object.FindObjectOfType<SansGravityManager>().Slam(Vector2.up, 12f);
		}
		if (frames == 7)
		{
			UnityEngine.Object.FindObjectOfType<SansGravityManager>().Slam(Vector2.right);
		}
		if (frames == 11)
		{
			UnityEngine.Object.FindObjectOfType<SOUL>().SlamToDirection(Vector2.right, 20f);
			boneGroup1.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
			boneGroup1.transform.localScale = new Vector3(-1f, 1f, 1f);
			boneGroup2.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
			boneGroup2.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		if (frames >= 11 && frames <= 16)
		{
			boneSideLineRight.transform.position = new Vector3(Mathf.Lerp(0f, 2f, (float)(frames - 11) / 4f), boneSideLineRight.transform.position.y);
		}
		if (frames >= 15 && frames <= 95)
		{
			boneGroup1.transform.position = Vector3.Lerp(new Vector3(0.77f, -4.07f), new Vector3(0.77f, 1.03f), (float)(frames - 25) / 30f);
			boneGroup2.transform.position = Vector3.Lerp(new Vector3(0.77f, -4.07f), new Vector3(0.77f, 1.03f), (float)(frames - 50) / 30f);
		}
		if (frames == 80)
		{
			UnityEngine.Object.FindObjectOfType<SansGravityManager>().Slam(Vector2.left);
		}
		if (frames == 95)
		{
			boneGroup1.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
			boneGroup1.transform.localScale = new Vector3(1f, 1f, 1f);
			boneGroup2.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
			boneGroup2.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		if (frames >= 88 && frames <= 93)
		{
			boneSideLineLeft.transform.position = new Vector3(Mathf.Lerp(0f, -2f, (float)(frames - 88) / 4f), boneSideLineLeft.transform.position.y);
		}
		if (frames >= 90 && frames <= 105)
		{
			float num = (float)(frames - 90) / 15f;
			boneSideLineRight.transform.position = new Vector3(Mathf.Lerp(1.62f, 0f, Mathf.Sin(num * (float)Math.PI * 0.5f)), boneSideLineRight.transform.position.y);
		}
		if (frames >= 100)
		{
			boneGroup1.transform.position = Vector3.Lerp(new Vector3(-0.77f, -4.07f), new Vector3(-0.77f, 1.03f), (float)(frames - 100) / 30f);
			boneGroup2.transform.position = Vector3.Lerp(new Vector3(-0.77f, -4.07f), new Vector3(-0.77f, 1.03f), (float)(frames - 125) / 30f);
		}
		if (frames == 156)
		{
			UnityEngine.Object.FindObjectOfType<SansGravityManager>().Slam(Vector2.right);
		}
		if (frames == 164)
		{
			UnityEngine.Object.FindObjectOfType<SansGravityManager>().Slam(Vector2.down);
		}
		if (frames == 168)
		{
			UnityEngine.Object.FindObjectOfType<SOUL>().SlamToDirection(Vector2.down, 20f);
		}
		if (frames >= 165 && frames <= 180)
		{
			float num2 = (float)(frames - 165) / 15f;
			boneSideLineLeft.transform.position = new Vector3(Mathf.Lerp(-1.62f, 0f, Mathf.Sin(num2 * (float)Math.PI * 0.5f)), boneSideLineLeft.transform.position.y);
		}
		if (frames >= 170)
		{
			if (frames == 170)
			{
				boneGroup1.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				boneGroup1.transform.localScale = new Vector3(1f, 1f, 1f);
				boneGroup2.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				boneGroup2.transform.localScale = new Vector3(-1f, 1f, 1f);
				boneGroup3 = UnityEngine.Object.Instantiate(boneGroup1);
				boneGroup4 = UnityEngine.Object.Instantiate(boneGroup2);
				boneGroup5 = UnityEngine.Object.Instantiate(boneGroup1);
				boneGroup6 = UnityEngine.Object.Instantiate(boneGroup2);
			}
			boneGroup1.transform.position = Vector3.Lerp(new Vector3(5f, 0.01f), new Vector3(-5f, 0.01f), (float)(frames - 170) / 45f);
			boneGroup3.transform.position = Vector3.Lerp(new Vector3(5f, 0.01f), new Vector3(-5f, 0.01f), (float)(frames - 195) / 45f);
			boneGroup5.transform.position = Vector3.Lerp(new Vector3(5f, 0.01f), new Vector3(-5f, 0.01f), (float)(frames - 220) / 45f);
			boneGroup2.transform.position = Vector3.Lerp(new Vector3(-5f, 0.01f), new Vector3(5f, 0.01f), (float)(frames - 250) / 45f);
			boneGroup4.transform.position = Vector3.Lerp(new Vector3(-5f, 0.01f), new Vector3(5f, 0.01f), (float)(frames - 275) / 45f);
			boneGroup6.transform.position = Vector3.Lerp(new Vector3(-5f, 0.01f), new Vector3(5f, 0.01f), (float)(frames - 300) / 45f);
		}
	}

	public void OnDestroy()
	{
		if ((bool)boneGroup1)
		{
			UnityEngine.Object.Destroy(boneGroup1);
		}
		if ((bool)boneGroup2)
		{
			UnityEngine.Object.Destroy(boneGroup2);
		}
		if ((bool)boneGroup3)
		{
			UnityEngine.Object.Destroy(boneGroup3);
		}
		if ((bool)boneGroup4)
		{
			UnityEngine.Object.Destroy(boneGroup4);
		}
		if ((bool)boneGroup5)
		{
			UnityEngine.Object.Destroy(boneGroup5);
		}
		if ((bool)boneGroup6)
		{
			UnityEngine.Object.Destroy(boneGroup6);
		}
		if ((bool)boneSideLineLeft)
		{
			UnityEngine.Object.Destroy(boneSideLineLeft);
		}
		if ((bool)boneSideLineRight)
		{
			UnityEngine.Object.Destroy(boneSideLineRight);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		GameObject original = Resources.Load<GameObject>("battle/attacks/bullets/sans/BlueHopGroup");
		boneGroup1 = UnityEngine.Object.Instantiate(original, new Vector3(5f, 5f), Quaternion.identity);
		boneGroup2 = UnityEngine.Object.Instantiate(original, new Vector3(5f, 5f), Quaternion.identity);
		GameObject original2 = Resources.Load<GameObject>("battle/attacks/bullets/sans/BoneLineSide");
		boneSideLineLeft = UnityEngine.Object.Instantiate(original2);
		boneSideLineLeft.transform.position = new Vector3(0f, 0.21f);
		boneSideLineRight = UnityEngine.Object.Instantiate(original2);
		boneSideLineRight.transform.position = new Vector3(0f, 0.21f);
		boneSideLineRight.transform.localScale = new Vector3(-1f, 1f, 1f);
	}
}
