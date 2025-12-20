using System.Collections.Generic;

[BinaryRecordType(RecordType.SerializedStreamHeader)]
public class StreamHeader : Record
{
	public readonly int rootId;

	public readonly int headerId;

	public readonly int majorVersion;

	public readonly int minorVersion;

	private Record rootObject;

	public StreamHeader(int rootId, int headerId, int majorVersion, int minorVersion)
	{
		this.minorVersion = minorVersion;
		this.majorVersion = majorVersion;
		this.headerId = headerId;
		this.rootId = rootId;
	}

	public override void PostProcess(Dictionary<int, Record> objects)
	{
		rootObject = objects[rootId];
	}

	public Record GetRootObject()
	{
		return rootObject;
	}

	public T GetRootObject<T>() where T : Record
	{
		return (T)rootObject;
	}
}
