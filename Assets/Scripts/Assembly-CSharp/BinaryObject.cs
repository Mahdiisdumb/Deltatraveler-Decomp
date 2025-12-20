using System.Collections.Generic;

public class BinaryObject : IndexedRecord
{
	public readonly object value;

	public BinaryObject(RecordType type, int id, object value)
		: base(type, id)
	{
		this.value = value;
	}

	public override void PreProcess(Dictionary<int, Record> objects)
	{
		if (index > -1)
		{
			objects[index] = this;
		}
	}
}
