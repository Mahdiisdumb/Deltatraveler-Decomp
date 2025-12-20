using UnityEngine;

public class ActionBulletGenerator : MonoBehaviour
{
	protected bool activated;

	public virtual void SetActivated(bool activated)
	{
		this.activated = activated;
	}
}
