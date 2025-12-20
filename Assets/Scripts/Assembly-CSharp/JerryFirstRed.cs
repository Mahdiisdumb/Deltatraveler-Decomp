using System.Collections.Generic;

public class JerryFirstRed : JerryRedSwingBase
{
	protected override void Awake()
	{
		base.Awake();
		actions = new KeyValuePair<int, int>[3]
		{
			new KeyValuePair<int, int>(0, 45),
			new KeyValuePair<int, int>(1, 45),
			new KeyValuePair<int, int>(1, 85)
		};
		bulletNumberPerSwing = 4;
		bulletSpeed = 1.2f;
	}
}
