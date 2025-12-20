using UnityEngine;

namespace MarioBrosMayhem
{
	public class ScreenWrap : MonoBehaviour
	{
		[SerializeField]
		private bool onLeft;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if ((bool)collision.GetComponent<StageObject>() || (bool)collision.GetComponent<HeldObject>())
			{
				collision.transform.position += new Vector3(onLeft ? 11.5f : (-11.5f), 0f);
			}
		}
	}
}
