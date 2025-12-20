using System;
using System.Collections.Generic;

public class Record
{
	private readonly RecordType type;

	public int offset = -1;

	public Record()
	{
		BinaryRecordType binaryRecordType = (BinaryRecordType)Attribute.GetCustomAttribute(GetType(), typeof(BinaryRecordType));
		if (binaryRecordType != null)
		{
			type = binaryRecordType.type;
			return;
		}
		foreach (RecordType value in Enum.GetValues(typeof(RecordType)))
		{
			if (value.ToString().Equals(GetType().Name, StringComparison.OrdinalIgnoreCase))
			{
				type = value;
				return;
			}
		}
		throw new InvalidOperationException("Could not automatically find type for " + GetType());
	}

	public Record(RecordType type)
	{
		this.type = type;
	}

	public virtual RecordType GetRecordType()
	{
		return type;
	}

	public virtual void PreProcess(Dictionary<int, Record> objects)
	{
	}

	public virtual void Process(Dictionary<int, Record> objects)
	{
	}

	public virtual void PostProcess(Dictionary<int, Record> objects)
	{
	}
}
