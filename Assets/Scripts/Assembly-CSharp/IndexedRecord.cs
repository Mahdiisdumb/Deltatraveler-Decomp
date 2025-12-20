using System.Collections.Generic;

public abstract class IndexedRecord : Record
{
	protected int index;

	public IndexedRecord(int index)
	{
		this.index = index;
	}

	public IndexedRecord(RecordType type, int index)
		: base(type)
	{
		this.index = index;
	}

	public override void PreProcess(Dictionary<int, Record> objects)
	{
		objects[index] = this;
	}
}
