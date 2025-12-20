public class ActionBulletBase : BulletBase
{
	protected int fadeFrames;

	protected bool activated;

	protected virtual void Update()
	{
		if (activated && fadeFrames < 12)
		{
			fadeFrames++;
		}
		else if (!activated && fadeFrames > 0)
		{
			fadeFrames--;
		}
	}

	public virtual void SetActivated(bool activated)
	{
		this.activated = activated;
	}
}
