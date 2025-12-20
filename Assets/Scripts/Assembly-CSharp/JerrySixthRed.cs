using System.Collections.Generic;

public class JerrySixthRed : JerryRedSwingBase
{
	protected override void Awake()
	{
		base.Awake();
		actions = new KeyValuePair<int, int>[10]
		{
			new KeyValuePair<int, int>(2, 25),
			new KeyValuePair<int, int>(2, 35),
			new KeyValuePair<int, int>(0, 10),
			new KeyValuePair<int, int>(0, 10),
			new KeyValuePair<int, int>(1, 10),
			new KeyValuePair<int, int>(1, 30),
			new KeyValuePair<int, int>(2, 25),
			new KeyValuePair<int, int>(2, 25),
			new KeyValuePair<int, int>(2, 50),
			new KeyValuePair<int, int>(3, 420)
		};
		bulletNumberPerSwing = 3;
		bulletSpeed = 1.5f;
	}
}
