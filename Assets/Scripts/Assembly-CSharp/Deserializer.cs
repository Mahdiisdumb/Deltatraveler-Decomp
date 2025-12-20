using System;
using System.IO;

public class Deserializer
{
	public class DeserializationException : Exception
	{
		public DeserializationException(string message, Exception cause)
			: base(message, cause)
		{
		}
	}

	public static StreamHeader Deserialize(string path)
	{
		return Deserialize(File.OpenRead(path));
	}

	public static StreamHeader Deserialize(Stream stream)
	{
		Reader reader = new Reader(stream);
		RecordType recordType = (RecordType)reader.buff.ReadByte();
		if (recordType != RecordType.SerializedStreamHeader)
		{
			throw new InvalidOperationException("Expected SerializedStreamHeader, got " + recordType);
		}
		reader.stream.Seek(0L, SeekOrigin.Begin);
		Record record = reader.ReadRecord();
		StreamHeader result = (StreamHeader)record;
		while (record != null)
		{
			record = reader.ReadRecord();
		}
		reader.Finish();
		return result;
	}
}
