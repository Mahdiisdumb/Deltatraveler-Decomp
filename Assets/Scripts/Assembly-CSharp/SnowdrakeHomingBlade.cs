using System;
using System.Collections.Generic;
using UnityEngine;

public class SnowdrakeHomingBlade : BulletBase
{
	[SerializeField]
	private GameObject bladePrefab;

	private Transform[] blades;

	private int[] bladeIndexes;

	private List<Vector3> positions = new List<Vector3>();

	private List<float> rotations = new List<float>();

	private int curBlades;

	private int maxBlades;

	private float curRotation;

	private float velocity;

	private bool head;

	private Vector3 origin = Vector3.zero;

	private float originalRotation;

	private readonly float chargeSpeed = 12f;

	private readonly float aimSpeed = 6.5f;

	private readonly float minSpeed = 6f;

	private readonly float multiplier = 0.75f;

	protected override void Awake()
	{
		base.Awake();
		destroyOnHit = false;
		baseDmg = 10;
		tpGrazeValue = 0.5f;
		tpGrazeValueReuse = 0.5f;
	}

	private void Update()
	{
		if (!head)
		{
			return;
		}
		bool num = Vector3.Distance(base.transform.position, UnityEngine.Object.FindObjectOfType<SOUL>().transform.position) <= 2.5f;
		MonoBehaviour.print(Vector3.Distance(base.transform.position, UnityEngine.Object.FindObjectOfType<SOUL>().transform.position));
		Quaternion rotation = base.transform.rotation;
		base.transform.right = base.transform.position - UnityEngine.Object.FindObjectOfType<SOUL>().transform.position;
		Quaternion b = base.transform.rotation;
		if (base.transform.rotation.eulerAngles.y == -180f)
		{
			b = Quaternion.Euler(0f, 0f, 180f);
		}
		float t = (num ? 0.02f : Mathf.Lerp(0.2f, 0f, (velocity - aimSpeed * multiplier) / ((chargeSpeed - aimSpeed) * multiplier)));
		if (velocity < minSpeed * multiplier)
		{
			t = 0.1f;
		}
		base.transform.rotation = Quaternion.Lerp(rotation, b, t);
		curRotation = base.transform.rotation.eulerAngles.z;
		float num2 = (num ? chargeSpeed : aimSpeed) * multiplier;
		float num3 = ((num || velocity < minSpeed * multiplier) ? 0.4f : 0.1f) * multiplier;
		if (velocity < num2)
		{
			velocity += num3;
		}
		else if (velocity > num2)
		{
			velocity -= num3;
		}
		base.transform.position += new Vector3(0f - Mathf.Cos(curRotation * ((float)Math.PI / 180f)), 0f - Mathf.Sin(curRotation * ((float)Math.PI / 180f))) * velocity / 48f;
		positions.Add(base.transform.position);
		rotations.Add(curRotation);
		bool flag = curBlades == maxBlades;
		for (int i = 0; i < curBlades; i++)
		{
			if ((bool)blades[i])
			{
				blades[i].transform.position = positions[bladeIndexes[i]];
				blades[i].transform.rotation = Quaternion.Euler(0f, 0f, rotations[bladeIndexes[i]]);
				if (!flag)
				{
					bladeIndexes[i]++;
				}
			}
		}
		if (flag)
		{
			positions.RemoveAt(0);
			rotations.RemoveAt(0);
		}
		else if (!flag && frames % 6 == 0 && frames != 0)
		{
			blades[curBlades] = UnityEngine.Object.Instantiate(bladePrefab, origin, Quaternion.Euler(0f, 0f, originalRotation), base.transform.parent).transform;
			curBlades++;
		}
		frames++;
	}

	public void ActivateHead(float startRotation, int childCount)
	{
		head = true;
		maxBlades = childCount;
		curRotation = startRotation;
		base.transform.rotation = Quaternion.Euler(0f, 0f, startRotation);
		blades = new Transform[maxBlades];
		bladeIndexes = new int[maxBlades];
		origin = base.transform.position;
		originalRotation = startRotation;
	}
}
