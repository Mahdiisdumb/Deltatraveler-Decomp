using System.IO;
using UnityEngine;

public class LanguagePack
{
	private PackInfo info;

	private string file;

	public LanguagePack(AssetBundle bund, string fileName)
	{
		file = fileName;
		if ((bool)bund)
		{
			TextAsset textAsset = (TextAsset)PackManager.LoadBundleAsset(bund, "Assets/pack/lang.json");
			if (!textAsset)
			{
				info = new PackInfo(TruncateFileName(fileName) + ".pack", "No description given");
			}
			info = (PackInfo)Serializer.Deserialize(typeof(PackInfo), textAsset.ToString());
			bund.Unload(true);
		}
		else
		{
			info = new PackInfo("Default (English)", "The default, original version\nof the script.");
		}
	}

	private string TruncateFileName(string value)
	{
		int num = 20;
		if (value.Length > num)
		{
			return value.Substring(0, num) + "...";
		}
		return value;
	}

	public string GetPath()
	{
		return file;
	}

	public string GetFileName()
	{
		return Path.GetFileName(file);
	}

	public string GetFileNameWithoutExtension()
	{
		return Path.GetFileNameWithoutExtension(file);
	}

	public PackInfo GetPackInfo()
	{
		return info;
	}
}
