public class ObjectNull : Record
{
	public readonly int repeats;

	public static ObjectNull Get()
	{
		return new ObjectNull();
	}

	public static ObjectNull Get(int repeats)
	{
		if (repeats < 2)
		{
			return Get();
		}
		return new ObjectNull(repeats);
	}

	private ObjectNull()
		: this(1)
	{
	}

	private ObjectNull(int repeats)
	{
		this.repeats = repeats;
	}

	public override string ToString()
	{
		return ((repeats > 1) ? ("ObjectNull x " + repeats) : "ObjectNull") + string.Format(" @ 0x%08x", offset);
	}

	public override RecordType GetRecordType()
	{
		if (repeats > 1)
		{
			if (repeats >= 256)
			{
				return RecordType.ObjectNullMultiple;
			}
			return RecordType.ObjectNullMultiple256;
		}
		return RecordType.ObjectNull;
	}
}
