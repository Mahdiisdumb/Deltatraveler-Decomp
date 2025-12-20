using System.Collections.Generic;

public class JerryFourthRed : JerryRedSwingBase
{
	protected override void Awake()
	{
		base.Awake();
		actions = new KeyValuePair<int, int>[8]
		{
			new KeyValuePair<int, int>(2, 45),
			new KeyValuePair<int, int>(2, 45),
			new KeyValuePair<int, int>(0, 15),
			new KeyValuePair<int, int>(0, 15),
			new KeyValuePair<int, int>(0, 15),
			new KeyValuePair<int, int>(1, 45),
			new KeyValuePair<int, int>(2, 30),
			new KeyValuePair<int, int>(2, 100)
		};
		bulletNumberPerSwing = 3;
		bulletSpeed = 1.35f;
	}
}
