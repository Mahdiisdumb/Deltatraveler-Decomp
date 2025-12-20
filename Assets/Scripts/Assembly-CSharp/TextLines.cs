using System.Collections.Generic;
using UnityEngine;

public class TextLines
{
	public Dictionary<string, TextSet> myLines = new Dictionary<string, TextSet>();

	public void Add(string key, TextSet lines)
	{
		if (myLines.ContainsKey(key))
		{
			Debug.LogWarning("tried to add duplicate key in textlines");
		}
		else
		{
			myLines.Add(key, lines);
		}
	}

	public TextSet Get(string key)
	{
		if (myLines.ContainsKey(key))
		{
			return myLines[key];
		}
		return new TextSet(new string[1] { "* [INVALID_KEY]" });
	}
}
