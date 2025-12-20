using System;
using UnityEngine;

namespace MarioBrosMayhem
{
	public class KickEffect : MonoBehaviour
	{
		private void Update()
		{
			if (!GetComponent<SpriteRenderer>().sprite)
			{
				for (int i = 0; i < 4; i++)
				{
					float num = ((i / 2 != 0) ? 1 : (-1));
					float num2 = ((i % 2 != 0) ? 1 : (-1));
					float num3 = UnityEngine.Random.Range(30f, 60f);
					Vector3 tradjectory = new Vector3(Mathf.Cos(num3 * ((float)Math.PI / 180f)) * num, Mathf.Sin(num3 * ((float)Math.PI / 180f)) * num2);
					float distance = UnityEngine.Random.Range(0.5f, 1f);
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/fx/StarEffect"), base.transform.position + new Vector3(0.133f * num, 0.1f * num2), Quaternion.identity).GetComponent<StarEffect>().Activate(tradjectory, distance);
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
