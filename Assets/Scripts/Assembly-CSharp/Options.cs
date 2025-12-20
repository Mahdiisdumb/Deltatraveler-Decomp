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

		public void LoadFromConfig(ref Config c)
		{
			value = c.GetInt("General", name, value, true);
		}

		public void SaveToConfig(ref Config c)
		{
			c.SetInt("General", name, value);
		}

		public void Increase()
		{
			value = (value + 1) % limit;
		}
	}

	public Option contentSetting;

	public Option lowGraphics;

	public Option buttonStyle;

	public Option autoButton;

	public Option startingFlavor;

	public Option autoRun;

	public Option runAnimations;

	public Option easyMode;

	public Options()
	{
		contentSetting = new Option(0, "ContentSetting", 2);
		lowGraphics = new Option(GameManager.autoLowGraphics ? 1 : 0, "LowGraphics", 2);
		buttonStyle = new Option(0, "ButtonStyle", 3);
		autoButton = new Option(0, "AutoButton", 2);
		startingFlavor = new Option(0, "StartingFlavor", 11);
		autoRun = new Option(0, "AutoRun", 2);
		runAnimations = new Option(1, "RunAnimations", 2);
		easyMode = new Option(0, "EasyMode", 2);
	}

	public void LoadFromConfig(ref Config c)
	{
		contentSetting.LoadFromConfig(ref c);
		lowGraphics.LoadFromConfig(ref c);
		buttonStyle.LoadFromConfig(ref c);
		autoButton.LoadFromConfig(ref c);
		startingFlavor.LoadFromConfig(ref c);
		autoRun.LoadFromConfig(ref c);
		runAnimations.LoadFromConfig(ref c);
		easyMode.LoadFromConfig(ref c);
	}

	public void SaveToConfig(ref Config c)
	{
		contentSetting.SaveToConfig(ref c);
		lowGraphics.SaveToConfig(ref c);
		buttonStyle.SaveToConfig(ref c);
		autoButton.SaveToConfig(ref c);
		startingFlavor.SaveToConfig(ref c);
		autoRun.SaveToConfig(ref c);
		runAnimations.SaveToConfig(ref c);
		easyMode.SaveToConfig(ref c);
	}
}
