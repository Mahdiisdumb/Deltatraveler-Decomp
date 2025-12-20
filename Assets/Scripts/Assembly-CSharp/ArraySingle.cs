public abstract class ArraySingle : IndexedRecord
{
	public readonly int length;

	public readonly int[] offsets;

	public ArraySingle(Reader reader)
		: base(reader.buff.ReadInt32())
	{
		length = reader.buff.ReadInt32();
		offsets = new int[length];
	}

	public abstract object[] GetValues();
}
