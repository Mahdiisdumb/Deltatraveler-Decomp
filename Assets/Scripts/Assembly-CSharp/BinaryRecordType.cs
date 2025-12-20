using System;

public class BinaryRecordType : Attribute
{
	public RecordType type;

	public BinaryRecordType(RecordType type)
	{
		this.type = type;
	}
}
