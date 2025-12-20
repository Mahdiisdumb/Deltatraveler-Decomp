using System;
using UnityEngine;

public class FakeOverworldBoneBullet : MonoBehaviour
{
	private Vector3 dir;

	private void Awake()
	{
		base.transform.position = new Vector3(UnityEngine.Random.Range(-1.25f, 1.25f), -2.61f);
		base.transform.up = base.transform.position - UnityEngine.Object.FindObjectOfType<UFLetter>().transform.position;
		dir = new Vector3(Mathf.Sin(base.transform.rotation.eulerAngles.z * ((float)Math.PI / 180f)), 0f - Mathf.Cos(base.transform.rotation.eulerAngles.z * ((float)Math.PI / 180f))) * 24f / 48f;
	}

	private void Update()
	{
		base.transform.position += dir;
		if (base.transform.position.y > 6f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
