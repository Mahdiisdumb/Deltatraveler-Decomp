using System.Collections.Generic;

public class JerryFifthRed : JerryRedSwingBase
{
	protected override void Awake()
	{
		base.Awake();
		actions = new KeyValuePair<int, int>[8]
		{
			new KeyValuePair<int, int>(0, 10),
			new KeyValuePair<int, int>(0, 10),
			new KeyValuePair<int, int>(1, 30),
			new KeyValuePair<int, int>(1, 10),
			new KeyValuePair<int, int>(1, 10),
			new KeyValuePair<int, int>(0, 30),
			new KeyValuePair<int, int>(2, 60),
			new KeyValuePair<int, int>(3, 420)
		};
		bulletNumberPerSwing = 4;
		bulletSpeed = 1.5f;
	}
}
