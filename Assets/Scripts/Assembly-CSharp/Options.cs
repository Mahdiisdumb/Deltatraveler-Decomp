public class Options
{
	public struct Option
	{
		public int value;

		public string name;

		public int limit;

		public Option(int value, string name, int limit)
		{
			this.value = value;
			this.name = name;
			this.limit = limit;
		}

		public void LoadFromConfig(ref Config c, string category = "General")
		{
			value = c.GetInt(category, name, value, writeIfNotExist: true);
		}

		public void SaveToConfig(ref Config c, string category = "General")
		{
			c.SetInt(category, name, value);
		}

		public void Increase()
		{
			value = (value + 1) % limit;
		}
	}

	public static readonly string[] FLAVORS = new string[11]
	{
		"Vanilla", "Mint", "Strawberry", "Banana", "ButtsPie", "Blueberry", "Cinnamon", "Moss", "Nerds", "Cotton Candy",
		"Eggplant"
	};

	public Option contentSetting;

	public Option lowGraphics;

	public Option buttonStyle;

	public Option autoButton;

	public Option startingFlavor;

	public Option autoRun;

	public Option runAnimations;

	public Option easyMode;

	public Option vSync;

	public Option monitorInfo;

	public Options()
	{
		contentSetting = new Option(0, "ContentSetting", 2);
		lowGraphics = new Option(0, "LowGraphics", 2);
		buttonStyle = new Option(0, "ButtonStyle", 5);
		autoButton = new Option(1, "AutoButton", 2);
		startingFlavor = new Option(0, "StartingFlavor", 11);
		autoRun = new Option(0, "AutoRun", 2);
		runAnimations = new Option(1, "RunAnimations", 2);
		easyMode = new Option(0, "EasyMode", 2);
		vSync = new Option(0, "vSync", 2);
		monitorInfo = new Option(0, "MonitorInfo", 2);
	}

	public void LoadFromConfig(ref Config c)
	{
		contentSetting.LoadFromConfig(ref c);
		buttonStyle.LoadFromConfig(ref c);
		autoButton.LoadFromConfig(ref c);
		startingFlavor.LoadFromConfig(ref c);
		autoRun.LoadFromConfig(ref c);
		runAnimations.LoadFromConfig(ref c);
		easyMode.LoadFromConfig(ref c);
		vSync.LoadFromConfig(ref c);
		monitorInfo.LoadFromConfig(ref c, "Debug");
	}

	public void SaveToConfig(ref Config c)
	{
		contentSetting.SaveToConfig(ref c);
		buttonStyle.SaveToConfig(ref c);
		autoButton.SaveToConfig(ref c);
		startingFlavor.SaveToConfig(ref c);
		autoRun.SaveToConfig(ref c);
		runAnimations.SaveToConfig(ref c);
		easyMode.SaveToConfig(ref c);
		vSync.SaveToConfig(ref c);
		monitorInfo.SaveToConfig(ref c, "Debug");
	}
}
