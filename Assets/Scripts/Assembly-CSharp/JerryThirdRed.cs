using System.Collections.Generic;

public class JerryThirdRed : JerryRedSwingBase
{
	protected override void Awake()
	{
		base.Awake();
		actions = new KeyValuePair<int, int>[9]
		{
			new KeyValuePair<int, int>(0, 15),
			new KeyValuePair<int, int>(0, 30),
			new KeyValuePair<int, int>(1, 15),
			new KeyValuePair<int, int>(1, 30),
			new KeyValuePair<int, int>(0, 15),
			new KeyValuePair<int, int>(0, 30),
			new KeyValuePair<int, int>(1, 15),
			new KeyValuePair<int, int>(1, 45),
			new KeyValuePair<int, int>(2, 100)
		};
		bulletNumberPerSwing = 4;
	}
}
