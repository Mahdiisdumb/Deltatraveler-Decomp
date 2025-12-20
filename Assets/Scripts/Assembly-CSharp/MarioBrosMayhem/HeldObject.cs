using UnityEngine;

namespace MarioBrosMayhem
{
	public class HeldObject : MonoBehaviour
	{
		public virtual Vector3 GetPositionOffset()
		{
			return Vector3.zero;
		}

		public virtual void Throw(bool facingRight, Vector3 velocity)
		{
		}
	}
}
