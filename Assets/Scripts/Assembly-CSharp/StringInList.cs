using System;
using System.Reflection;
using UnityEngine;

public class StringInList : PropertyAttribute
{
	public delegate string[] GetStringList();

	public string[] List { get; private set; }

	public StringInList(params string[] list)
	{
		List = list;
	}

	public StringInList(Type type, string methodName)
	{
		MethodInfo method = type.GetMethod(methodName);
		if (method != null)
		{
			List = method.Invoke(null, null) as string[];
			return;
		}
		Debug.LogError("NO SUCH METHOD " + methodName + " FOR " + type);
	}
}
