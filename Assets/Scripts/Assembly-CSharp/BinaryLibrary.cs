public class BinaryLibrary : IndexedRecord
{
	public readonly string libraryName;

	public BinaryLibrary(int libraryId, string libraryName)
		: base(libraryId)
	{
		this.libraryName = libraryName;
	}
}
