public class SystemClassWithMembersAndTypes : ClassWithMembersAndTypes
{
	public SystemClassWithMembersAndTypes(Reader reader)
		: base(reader, false)
	{
	}

	public new string GetClassName()
	{
		return classInfo.name.Split('`')[0];
	}
}
