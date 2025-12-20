public class MemberPrimitiveTyped : Record
{
	public readonly PrimitiveType primitiveType;

	public readonly object value;

	public MemberPrimitiveTyped(Reader reader)
	{
		primitiveType = (PrimitiveType)reader.buff.ReadByte();
		value = reader.ReadPrimitive(primitiveType);
	}
}
