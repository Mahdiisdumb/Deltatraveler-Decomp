using System.Collections.Generic;

public class JerrySecondRed : JerryRedSwingBase
{
	protected override void Awake()
	{
		base.Awake();
		actions = new KeyValuePair<int, int>[4]
		{
			new KeyValuePair<int, int>(0, 35),
			new KeyValuePair<int, int>(1, 35),
			new KeyValuePair<int, int>(0, 50),
			new KeyValuePair<int, int>(2, 100)
		};
	}
}
