using System;
using UnityEngine;

public class GiantColorBlasterAttack : AttackBase
{
	private GameObject blastPrefab;

	private GameObject test;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 305;
		bbSize = new Vector2(500f, 140f);
		UnityEngine.Object.FindObjectOfType<SOUL>().ChangeSOULMode(0);
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % 16 == 1 && frames + 35 < maxFrames)
		{
			int id = UnityEngine.Random.Range(1, 3);
			bool num = UnityEngine.Random.Range(0, 2) == 0;
			int num2 = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
			GasterBlaster component = UnityEngine.Object.Instantiate(blastPrefab).GetComponent<GasterBlaster>();
			component.SetTPBuildRate(0.1f);
			component.SetTPGrazeValue(1f);
			Vector3 vector = ((!num) ? new Vector3(6 * num2, UnityEngine.Random.Range(-3.83f, 3.83f)) : new Vector3(UnityEngine.Random.Range(-6, 6), 3.83f * (float)num2));
			component.transform.position = vector;
			component.ChangeType(id);
			component.transform.up = UnityEngine.Object.FindObjectOfType<SOUL>().transform.position - component.transform.position;
			float num3 = component.transform.rotation.eulerAngles.z - 180f;
			Vector3 vector2 = new Vector3(Mathf.Cos((num3 + 90f) * ((float)Math.PI / 180f)), Mathf.Sin((num3 + 90f) * ((float)Math.PI / 180f))) * 5f;
			component.transform.position += vector2;
			component.Activate(4, 4, num3, vector, 12);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		bb.StartMovement(new Vector2(500f, 300f));
		blastPrefab = Resources.Load<GameObject>("battle/attacks/bullets/GasterBlaster");
	}
}
