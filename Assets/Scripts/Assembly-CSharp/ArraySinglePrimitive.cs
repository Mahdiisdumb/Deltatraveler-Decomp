public class ArraySinglePrimitive : ArraySingle
{
	public readonly PrimitiveType primitiveType;

	public readonly object[] values;

	public ArraySinglePrimitive(Reader reader)
		: base(reader)
	{
		values = new object[length];
		primitiveType = (PrimitiveType)reader.buff.ReadByte();
		for (int i = 0; i < length; i++)
		{
			offsets[i] = (int)reader.stream.Position;
			values[i] = reader.ReadPrimitive(primitiveType);
		}
	}

	public override object[] GetValues()
	{
		return values;
	}
}
