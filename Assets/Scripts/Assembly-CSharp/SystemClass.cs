using System.Collections.Generic;

public class SystemClass : BinaryMember
{
	public string name;

	public Dictionary<string, object> values;

	public SystemClass(string name, Dictionary<string, object> values)
		: base(BinaryType.SystemClass, values)
	{
		this.values = values;
		this.name = name;
	}
}
