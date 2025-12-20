using System;
using System.IO;
using IniParser;
using IniParser.Model;
using UnityEngine;

public class Config
{
	private IniData configData;

	private FileIniDataParser configParser;

	private string configLocation;

	public Config(string file)
	{
		configLocation = Path.Combine(Application.persistentDataPath, file);
		configParser = new FileIniDataParser();
		Read();
	}

	public void Read()
	{
		if (File.Exists(configLocation))
		{
			try
			{
				configData = configParser.ReadFile(configLocation);
				return;
			}
			catch
			{
				configData = new IniData();
				return;
			}
		}
		configData = new IniData();
	}

	public int GetInt(string category, string value, int def, bool writeIfNotExist = false)
	{
		if (configData[category][value] != null)
		{
			int result = 0;
			if (int.TryParse(configData[category][value], out result))
			{
				return result;
			}
		}
		else if (writeIfNotExist)
		{
			SetInt(category, value, def);
			Write();
		}
		return def;
	}

	public void SetInt(string category, string value, int val)
	{
		configData[category][value] = val.ToString();
	}

	public void SetString(string category, string value, string val)
	{
		configData[category][value] = val;
	}

	public string GetString(string category, string value, string def, bool writeIfNotExist = false)
	{
		if (configData[category][value] != null)
		{
			return configData[category][value];
		}
		if (writeIfNotExist)
		{
			SetString(category, value, def);
			Write();
		}
		return def;
	}

	public void Write()
	{
		try
		{
			configParser.WriteFile(configLocation, configData);
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Couldn't write config file :(\n" + ex);
		}
	}

	public void ResetCategory(string category)
	{
		configData[category].RemoveAllKeys();
	}
}
