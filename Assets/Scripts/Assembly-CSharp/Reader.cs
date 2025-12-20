using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class Reader : IDisposable
{
	public readonly BinaryReader buff;

	public readonly Stream stream;

	protected Dictionary<int, Record> objects;

	protected List<Record> records;

	private bool readHeader;

	private bool finished;

	public Reader(string path)
		: this(File.OpenRead(path))
	{
	}

	public Reader(Stream stream)
	{
		buff = new BinaryReader(stream);
		this.stream = stream;
		records = new List<Record>();
		objects = new Dictionary<int, Record>();
	}

	public Reader(byte[] bytes)
		: this(new MemoryStream(bytes))
	{
	}

	public Record ReadRecord()
	{
		if (finished)
		{
			return null;
		}
		int offset = (int)stream.Position;
		Record record = ReadRecordInternal();
		if (record != null)
		{
			records.Add(record);
			record.offset = offset;
		}
		return record;
	}

	private Record ReadRecordInternal()
	{
		if (stream.Length <= stream.Position)
		{
			return null;
		}
		int num = (int)stream.Position;
		RecordType? recordType = null;
		try
		{
			recordType = (RecordType)buff.ReadByte();
			switch (recordType.Value)
			{
			case RecordType.SerializedStreamHeader:
				if (readHeader)
				{
					throw new InvalidOperationException("Stream header has already been read for this file");
				}
				readHeader = true;
				return new StreamHeader(buff.ReadInt32(), buff.ReadInt32(), buff.ReadInt32(), buff.ReadInt32());
			case RecordType.BinaryLibrary:
				return new BinaryLibrary(buff.ReadInt32(), ReadString());
			case RecordType.SystemClassWithMembersAndTypes:
				return new SystemClassWithMembersAndTypes(this);
			case RecordType.ClassWithMembersAndTypes:
				return new ClassWithMembersAndTypes(this);
			case RecordType.MemberReference:
				return new MemberReference(buff.ReadInt32());
			case RecordType.BinaryObjectString:
				return new BinaryObject(recordType.Value, buff.ReadInt32(), ReadString());
			case RecordType.ArraySinglePrimitive:
				return new ArraySinglePrimitive(this);
			case RecordType.ArraySingleObject:
				return new ArraySingleObject(this);
			case RecordType.ArraySingleString:
				return new ArraySingleString(this);
			case RecordType.MemberPrimitiveTyped:
				return new MemberPrimitiveTyped(this);
			case RecordType.ObjectNullMultiple256:
				return ObjectNull.Get(buff.ReadByte());
			case RecordType.ObjectNullMultiple:
				return ObjectNull.Get(buff.ReadInt32());
			case RecordType.ObjectNull:
				return ObjectNull.Get();
			case RecordType.MessageEnd:
				finished = true;
				break;
			default:
				throw new NotSupportedException("Cannot deserialize record of type " + recordType);
			}
		}
		catch (Exception cause)
		{
			long position = stream.Position;
			throw new Deserializer.DeserializationException(string.Format("\nException while processing {0} at position 0x{1:X8} (error occured around 0x{2:X8})", (recordType.HasValue ? recordType.GetValueOrDefault().ToString() : null) ?? "<unknown type>", num, position), cause);
		}
		return null;
	}

	public BinaryObject ReadBinaryObject()
	{
		return (BinaryObject)ReadRecord();
	}

	public object ReadPrimitive(PrimitiveType type)
	{
		switch (type)
		{
		case PrimitiveType.Boolean:
			return buff.ReadByte() == 1;
		case PrimitiveType.Byte:
			return buff.ReadByte();
		case PrimitiveType.Char:
			return buff.ReadChar();
		case PrimitiveType.Int16:
			return buff.ReadInt16();
		case PrimitiveType.Int32:
			return buff.ReadInt32();
		case PrimitiveType.Int64:
			return buff.ReadInt64();
		case PrimitiveType.UInt16:
			return buff.ReadUInt16();
		case PrimitiveType.UInt32:
			return buff.ReadUInt32();
		case PrimitiveType.Single:
			return buff.ReadSingle();
		case PrimitiveType.Double:
			return buff.ReadDouble();
		default:
			throw new NotSupportedException("Unsupported PrimitiveType " + type);
		}
	}

	public string ReadString()
	{
		byte[] array = new byte[ReadStringLength()];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = buff.ReadByte();
		}
		return Encoding.UTF8.GetString(array);
	}

	private int ReadStringLength()
	{
		int num = 0;
		int num2 = 0;
		bool flag = false;
		do
		{
			byte b = buff.ReadByte();
			flag = (b & 0x80) > 0;
			num |= (b & 0x7F) << num2;
			num2 += 7;
		}
		while (flag && num2 < 32);
		if ((flag && num2 >= 32) || num < 0)
		{
			throw new InvalidOperationException("Invalid string length field");
		}
		return num;
	}

	public void Finish()
	{
		records.ForEach(delegate(Record r)
		{
			r.PreProcess(objects);
		});
		records.ForEach(delegate(Record r)
		{
			r.Process(objects);
		});
		records.ForEach(delegate(Record r)
		{
			r.PostProcess(objects);
		});
	}

	public void Dispose()
	{
		buff.Close();
		stream.Close();
	}
}
