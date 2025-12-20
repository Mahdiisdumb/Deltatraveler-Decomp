public class ArraySingleObject : ArraySingle
{
	public readonly object[] values;

	public ArraySingleObject(Reader reader)
		: base(reader)
	{
		values = new object[length];
		for (int i = 0; i < length; i++)
		{
			Record record = reader.ReadRecord();
			offsets[i] = record.offset;
			if (record is ObjectNull objectNull)
			{
				int num = 0;
				while (num < objectNull.repeats)
				{
					offsets[i] = record.offset;
					values[i] = null;
					num++;
					i++;
				}
				i--;
			}
			else
			{
				values[i] = record;
			}
		}
	}

	public override object[] GetValues()
	{
		return values;
	}
}
