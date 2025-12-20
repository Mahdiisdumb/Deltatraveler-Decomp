public class ArraySingleString : ArraySingle
{
	public readonly string[] values;

	public ArraySingleString(Reader reader)
		: base(reader)
	{
		values = new string[length];
		for (int i = 0; i < length; i++)
		{
			offsets[i] = (int)reader.stream.Position;
			values[i] = reader.ReadString();
		}
	}

	public override object[] GetValues()
	{
		return values;
	}
}
