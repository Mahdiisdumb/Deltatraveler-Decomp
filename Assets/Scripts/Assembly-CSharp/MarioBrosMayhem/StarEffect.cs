using System;
using UnityEngine;

namespace MarioBrosMayhem
{
	public class StarEffect : MonoBehaviour
	{
		private bool activated;

		private Vector3 origin = Vector3.zero;

		private Vector3 tradjectory = Vector3.zero;

		private float distance;

		private float timer;

		private void Update()
		{
			if (!activated)
			{
				return;
			}
			timer += Time.deltaTime;
			if (timer >= 1f / 6f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			float num = timer / (2f / 15f);
			if (num > 1f)
			{
				num = 1f;
			}
			num = Mathf.Sin(num * (float)Math.PI * 0.5f);
			base.transform.position = origin + tradjectory * distance * num;
		}

		public void Activate(Vector3 tradjectory, float distance)
		{
			origin = base.transform.position;
			this.tradjectory = tradjectory;
			this.distance = distance;
			activated = true;
		}
	}
}
