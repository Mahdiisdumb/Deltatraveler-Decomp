using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

public class ClassWithMembersAndTypes : IndexedRecord
{
	public class ClassInfo
	{
		public readonly string name;

		public readonly List<string> memberNames;

		public ClassInfo(Reader reader)
		{
			name = reader.ReadString();
			int num = reader.buff.ReadInt32();
			List<string> list = new List<string>();
			for (int i = 0; i < num; i++)
			{
				list.Add(reader.ReadString());
			}
			memberNames = list;
		}
	}

	public class MemberTypeInfo
	{
		public class TypeInfoPair
		{
			public readonly BinaryType type;

			public readonly object additionalInfo;

			internal TypeInfoPair(BinaryType type, object additionalInfo)
			{
				this.additionalInfo = additionalInfo;
				this.type = type;
			}
		}

		public readonly List<TypeInfoPair> typeInfo;

		public MemberTypeInfo(Reader reader, int numRecords)
		{
			List<TypeInfoPair> list = new List<TypeInfoPair>();
			List<BinaryType> list2 = new List<BinaryType>();
			for (int i = 0; i < numRecords; i++)
			{
				list2.Add((BinaryType)reader.buff.ReadByte());
			}
			foreach (BinaryType item in list2)
			{
				object additionalInfo = null;
				switch (item)
				{
				case BinaryType.Primitive:
				case BinaryType.PrimitiveArray:
					additionalInfo = (PrimitiveType)reader.buff.ReadByte();
					break;
				case BinaryType.SystemClass:
					additionalInfo = reader.ReadString();
					break;
				default:
					throw new NotSupportedException("Unsupported BinaryType " + item);
				case BinaryType.String:
				case BinaryType.Object:
				case BinaryType.ObjectArray:
				case BinaryType.StringArray:
					break;
				}
				list.Add(new TypeInfoPair(item, additionalInfo));
			}
			typeInfo = list;
		}
	}

	public readonly ClassInfo classInfo;

	public readonly MemberTypeInfo memberTypeInfo;

	public readonly int libraryId;

	public Dictionary<string, object> values;

	public Dictionary<string, int> offsets;

	private BinaryLibrary library;

	public ClassWithMembersAndTypes(Reader reader)
		: this(reader, true)
	{
	}

	public ClassWithMembersAndTypes(Reader reader, bool hasLibrary)
		: base(reader.buff.ReadInt32())
	{
		classInfo = new ClassInfo(reader);
		memberTypeInfo = new MemberTypeInfo(reader, classInfo.memberNames.Count);
		offsets = new Dictionary<string, int>();
		values = new Dictionary<string, object>();
		if (hasLibrary)
		{
			libraryId = reader.buff.ReadInt32();
		}
		else
		{
			libraryId = -1;
		}
		for (int i = 0; i < classInfo.memberNames.Count; i++)
		{
			string key = classInfo.memberNames[i];
			MemberTypeInfo.TypeInfoPair typeInfoPair = memberTypeInfo.typeInfo[i];
			offsets[key] = (int)reader.stream.Position;
			object value;
			if (typeInfoPair.additionalInfo == null)
			{
				value = reader.ReadRecord();
			}
			else
			{
				switch (typeInfoPair.type)
				{
				case BinaryType.Primitive:
				{
					PrimitiveType primitiveType = (PrimitiveType)typeInfoPair.additionalInfo;
					value = reader.ReadPrimitive(primitiveType);
					break;
				}
				case BinaryType.SystemClass:
				case BinaryType.PrimitiveArray:
					value = reader.ReadRecord();
					break;
				default:
					throw new NotSupportedException("Unsupported BinaryType " + typeInfoPair.type);
				}
			}
			values[key] = value;
		}
	}

	public string GetClassName()
	{
		return classInfo.name;
	}

	public BinaryLibrary GetLibrary()
	{
		return library;
	}

	public override void PostProcess(Dictionary<int, Record> objects)
	{
		if (libraryId > -1)
		{
			library = (BinaryLibrary)objects[libraryId];
		}
	}

	public T GetAs<T>(bool deserializeNestedObjects = false)
	{
		return (T)GetAs(typeof(T), deserializeNestedObjects);
	}

	public object GetAs(Type type, bool deserializeNestedObjects = false)
	{
		object uninitializedObject = FormatterServices.GetUninitializedObject(type);
		foreach (string key in values.Keys)
		{
			FieldInfo field = type.GetField(key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				throw new InvalidOperationException("Missing instance field '" + key + "' in given class " + type.FullName + " (serialized class: " + GetClassName() + ")");
			}
			object obj = values[key];
			MemberReference memberReference;
			if ((memberReference = obj as MemberReference) != null)
			{
				obj = memberReference.Unwrap();
			}
			if (!obj.GetType().IsPrimitive)
			{
				ClassWithMembersAndTypes classWithMembersAndTypes;
				BinaryObject binaryObject;
				ArraySingle arraySingle;
				if (obj is ObjectNull)
				{
					obj = null;
				}
				else if ((classWithMembersAndTypes = obj as ClassWithMembersAndTypes) != null)
				{
					if (!deserializeNestedObjects)
					{
						throw new InvalidOperationException("Nested object deserialization disabled, pass 'deserializeNestedObjects = true' to this method to enable it");
					}
					Type type2 = Type.GetType(classWithMembersAndTypes.classInfo.name);
					if (type2 == null)
					{
						throw new InvalidOperationException("Cannot instantiate missing class " + classWithMembersAndTypes.classInfo.name);
					}
					if (!type2.IsEquivalentTo(field.FieldType))
					{
						throw new InvalidOperationException("Mismatched types for field '" + key + "' (" + classWithMembersAndTypes.classInfo.name + " != " + field.FieldType.FullName + ")");
					}
					obj = classWithMembersAndTypes.GetAs(type2);
				}
				else if ((binaryObject = obj as BinaryObject) != null)
				{
					obj = binaryObject.value;
				}
				else if ((arraySingle = obj as ArraySingle) != null)
				{
					ArraySinglePrimitive arr;
					if ((arr = arraySingle as ArraySinglePrimitive) != null)
					{
						obj = ConvertToPrimitiveArray(arr);
					}
					else
					{
						object[] array = new object[arraySingle.length];
						object[] array2 = arraySingle.GetValues();
						for (int i = 0; i < arraySingle.length; i++)
						{
							object obj2 = array2[i];
							MemberReference memberReference2;
							if ((memberReference2 = obj2 as MemberReference) != null)
							{
								obj2 = memberReference2.Unwrap();
							}
							BinaryObject binaryObject2;
							MemberPrimitiveTyped memberPrimitiveTyped;
							if ((binaryObject2 = obj2 as BinaryObject) != null)
							{
								obj2 = binaryObject2.value;
							}
							else if ((memberPrimitiveTyped = obj2 as MemberPrimitiveTyped) != null)
							{
								obj2 = memberPrimitiveTyped.value;
							}
							array[i] = obj2;
						}
						obj = array;
					}
				}
			}
			field.SetValue(uninitializedObject, obj);
		}
		return uninitializedObject;
	}

	private static Array ConvertToPrimitiveArray(ArraySinglePrimitive arr)
	{
		object[] array = arr.GetValues();
		Type typeFromHandle;
		switch (arr.primitiveType)
		{
		case PrimitiveType.Boolean:
			typeFromHandle = typeof(bool);
			break;
		case PrimitiveType.Byte:
			typeFromHandle = typeof(byte);
			break;
		case PrimitiveType.Char:
			typeFromHandle = typeof(char);
			break;
		case PrimitiveType.Int16:
			typeFromHandle = typeof(short);
			break;
		case PrimitiveType.Int32:
			typeFromHandle = typeof(int);
			break;
		case PrimitiveType.Int64:
			typeFromHandle = typeof(long);
			break;
		case PrimitiveType.UInt16:
			typeFromHandle = typeof(ushort);
			break;
		case PrimitiveType.UInt32:
			typeFromHandle = typeof(uint);
			break;
		case PrimitiveType.Single:
			typeFromHandle = typeof(float);
			break;
		case PrimitiveType.Double:
			typeFromHandle = typeof(double);
			break;
		default:
			throw new NotSupportedException("no ArraySinglePrimitive type mapping exists for PrimitiveType " + arr.primitiveType);
		}
		Array array2 = Array.CreateInstance(typeFromHandle, array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			array2.SetValue(array[i], i);
		}
		return array2;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder("class ");
		stringBuilder.Append(classInfo.name);
		stringBuilder.Append(" (id ");
		stringBuilder.Append(index);
		stringBuilder.Append(string.Format(") @ 0x{0,X8} {\n", offset));
		for (int i = 0; i < classInfo.memberNames.Count; i++)
		{
			string text = classInfo.memberNames[i];
			MemberTypeInfo.TypeInfoPair typeInfoPair = memberTypeInfo.typeInfo[i];
			object value = values[text];
			stringBuilder.Append('\t');
			stringBuilder.Append(typeInfoPair.type);
			if (typeInfoPair.additionalInfo != null)
			{
				stringBuilder.Append('<');
				if (typeInfoPair.type == BinaryType.SystemClass)
				{
					object additionalInfo = typeInfoPair.additionalInfo;
					string text2 = ((additionalInfo != null) ? additionalInfo.ToString() : null) ?? "null";
					stringBuilder.Append(text2.Split('`')[0]);
				}
				else
				{
					stringBuilder.Append(typeInfoPair.additionalInfo);
				}
				stringBuilder.Append('>');
			}
			stringBuilder.Append(' ');
			stringBuilder.Append(text);
			stringBuilder.Append(" = ");
			stringBuilder.Append(value);
			stringBuilder.Append('\n');
		}
		stringBuilder.Append('}');
		return stringBuilder.ToString();
	}
}
