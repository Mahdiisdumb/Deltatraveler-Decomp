using UnityEngine;

namespace MarioBrosMayhem
{
	public class EnterPipe : MonoBehaviour
	{
		public void PlayEnterAnimation()
		{
			GetComponent<Animator>().Play("Enter", 0, 0f);
		}
	}
}
