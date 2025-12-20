using UnityEngine;

namespace MarioBrosMayhem
{
	public class AnimatorEffect : MonoBehaviour
	{
		private void Update()
		{
			if (!GetComponent<SpriteRenderer>().sprite)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
