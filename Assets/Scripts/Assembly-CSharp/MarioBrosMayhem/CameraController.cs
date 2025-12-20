using UnityEngine;

namespace MarioBrosMayhem
{
	public class CameraController : MonoBehaviour
	{
		private bool powHit;

		private float timer;

		private void Update()
		{
			if (powHit)
			{
				timer += Time.deltaTime;
				float num = timer / 0.1f;
				if (timer > 2f / 15f)
				{
					num = (7f / 30f - timer) / 0.1f;
				}
				if (num < 0f)
				{
					powHit = false;
				}
				base.transform.position = new Vector3(0f, (float)Mathf.RoundToInt(Mathf.Lerp(0f, -1f / 3f, num) * 24f) / 24f, -10f);
			}
		}

		public void PowHit()
		{
			powHit = true;
			timer = 0f;
		}
	}
}
