using System.Collections.Generic;

public class MemberReference : Record
{
	public readonly int refId;

	private Record reference;

	public MemberReference(int refId)
	{
		this.refId = refId;
	}

	public override void PostProcess(Dictionary<int, Record> objects)
	{
		reference = objects[refId];
	}

	public Record GetReference()
	{
		return reference;
	}

	public T GetReference<T>() where T : Record
	{
		return (T)reference;
	}

	public Record Unwrap()
	{
		Record record;
		for (record = reference; record is MemberReference memberReference; record = memberReference.reference)
		{
		}
		return record;
	}
}
