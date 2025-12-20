using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SAVEFileIO
{
	private enum FlagTypeID
	{
		Int = 0,
		String = 1,
		Bool = 2,
		Float = 3,
		Null = 255
	}

	public static readonly int FORMAT_VERSION = 2;

	public static readonly string MAGIC = "SAVE";

	public static readonly byte[] HASH_SALT = new byte[32]
	{
		83, 84, 79, 80, 46, 32, 80, 79, 83, 84,
		73, 78, 71, 46, 32, 65, 66, 79, 85, 84,
		46, 32, 65, 77, 79, 78, 71, 46, 32, 85,
		83, 46
	};

	public static void WriteFile(ref SAVEFile file, FileStream stream)
	{
		if (file == null || string.IsNullOrEmpty(file.name))
		{
			file = Util.GameManager().GetFile();
		}
		stream.Write(Encoding.ASCII.GetBytes(MAGIC), 0, MAGIC.Length);
		using (BinaryWriter binaryWriter = new BinaryWriter(stream))
		{
			binaryWriter.Write((short)FORMAT_VERSION);
			long position = stream.Position;
			binaryWriter.Write(HASH_SALT);
			binaryWriter.Write(file.name);
			binaryWriter.Write(file.exp);
			binaryWriter.Write((byte)file.items.Count);
			foreach (int item in file.items)
			{
				binaryWriter.Write((short)item);
			}
			for (int i = 0; i < 3; i++)
			{
				binaryWriter.Write((short)file.weapon[i]);
				binaryWriter.Write((short)file.armor[i]);
			}
			binaryWriter.Write(file.susieActive);
			binaryWriter.Write(file.noelleActive);
			binaryWriter.Write(file.playTime);
			binaryWriter.Write((short)file.zone);
			binaryWriter.Write(file.gold);
			binaryWriter.Write(file.deaths);
			WriteFlags(file.flags, binaryWriter);
			WriteFlags(file.persFlags, binaryWriter);
			binaryWriter.Flush();
			long position2 = stream.Position;
			byte[] array = new byte[position2];
			stream.Seek(0L, SeekOrigin.Begin);
			stream.Read(array, 0, (int)position2);
			stream.Seek(position, SeekOrigin.Begin);
			Debug.Log(array.Length);
			using (SHA256 sHA = SHA256.Create())
			{
				binaryWriter.Write(sHA.ComputeHash(array));
			}
			stream.SetLength(position2);
			binaryWriter.Flush();
		}
	}

	public static FileStatus ReadFile(ref SAVEFile file, FileStream fs)
	{
		long length = fs.Length;
		byte[] array = new byte[length];
		fs.Read(array, 0, (int)length);
		using (MemoryStream memoryStream = new MemoryStream(array))
		{
			byte[] array2 = new byte[MAGIC.Length];
			memoryStream.Read(array2, 0, MAGIC.Length);
			if (Encoding.ASCII.GetString(array2) == MAGIC)
			{
				if (file == null)
				{
					file = new SAVEFile();
				}
				try
				{
					using (BinaryReader binaryReader = new BinaryReader(memoryStream))
					{
						bool flag = false;
						byte[] array3 = null;
						byte[] array4 = null;
						int num = binaryReader.ReadInt16();
						Debug.Log("NEW SAVE (FORMAT VERSION " + num + ")");
						if (num > FORMAT_VERSION)
						{
							Debug.Log("Not loading, save version is newer than max supported version (" + FORMAT_VERSION + ")");
							return FileStatus.Newer;
						}
						if (num > 0)
						{
							array3 = binaryReader.ReadBytes(32);
							memoryStream.Seek(-32L, SeekOrigin.Current);
							memoryStream.Write(HASH_SALT, 0, HASH_SALT.Length);
							Debug.Log(array.Length);
							using (SHA256 sHA = SHA256.Create())
							{
								array4 = sHA.ComputeHash(array);
								if (!array4.SequenceEqual(array3))
								{
									Debug.Log("Hash mismatch: " + ToHexString(array4) + " != " + ToHexString(array3));
									if (num != 1)
									{
										return FileStatus.Corrupted;
									}
									flag = true;
								}
							}
						}
						file.name = binaryReader.ReadString();
						file.exp = binaryReader.ReadInt32();
						file.items = new List<int>();
						byte b = binaryReader.ReadByte();
						for (int i = 0; i < b; i++)
						{
							file.items.Add(binaryReader.ReadInt16());
						}
						file.weapon = new int[3];
						file.armor = new int[3];
						for (int j = 0; j < 3; j++)
						{
							file.weapon[j] = binaryReader.ReadInt16();
							file.armor[j] = binaryReader.ReadInt16();
						}
						file.susieActive = binaryReader.ReadBoolean();
						file.noelleActive = binaryReader.ReadBoolean();
						file.playTime = binaryReader.ReadInt32();
						file.zone = binaryReader.ReadInt16();
						file.gold = binaryReader.ReadInt32();
						file.deaths = binaryReader.ReadInt32();
						file.flags = ReadFlags(binaryReader);
						file.persFlags = ReadFlags(binaryReader);
						if (flag)
						{
							if (memoryStream.Length <= memoryStream.Position)
							{
								return FileStatus.Corrupted;
							}
							using (SHA256 sHA2 = SHA256.Create())
							{
								array4 = sHA2.ComputeHash(array, 0, (int)memoryStream.Position);
								if (!array4.SequenceEqual(array3))
								{
									Debug.Log("Hash mismatch (2): " + ToHexString(array4) + " != " + ToHexString(array3));
									return FileStatus.Corrupted;
								}
								Debug.Log("File hash discrepancy fixed (" + memoryStream.Length + " -> " + memoryStream.Position + ")");
								fs.SetLength(memoryStream.Position);
							}
						}
						if (num < FORMAT_VERSION)
						{
							return FileStatus.Older;
						}
						return FileStatus.OK;
					}
				}
				catch (Exception ex)
				{
					Debug.LogError("Error reading new format file\n" + ex);
					return FileStatus.Corrupted;
				}
			}
			try
			{
				memoryStream.Seek(0L, SeekOrigin.Begin);
				ClassWithMembersAndTypes rootObject = Deserializer.Deserialize(memoryStream).GetRootObject<ClassWithMembersAndTypes>();
				if (rootObject.GetClassName() != "SAVEFile")
				{
					Debug.LogError("Invalid SAVEFile: class is " + rootObject.GetClassName());
					return FileStatus.Corrupted;
				}
				ArraySingleObject arraySingleObject = UnwrapRef<ArraySingleObject>(rootObject.values["flags"]);
				ArraySingleObject arraySingleObject2 = UnwrapRef<ArraySingleObject>(rootObject.values["persFlags"]);
				if (arraySingleObject == null)
				{
					throw new SerializationException("flags array is missing");
				}
				if (arraySingleObject2 == null)
				{
					throw new SerializationException("persFlags array is missing");
				}
				object[] values = arraySingleObject.GetValues();
				for (int k = 0; k < values.Length; k++)
				{
					ClassWithMembersAndTypes classWithMembersAndTypes;
					if ((classWithMembersAndTypes = values[k] as ClassWithMembersAndTypes) != null)
					{
						Debug.LogError("Invalid SAVEFile: class " + classWithMembersAndTypes.GetClassName() + " contained in flags");
						return FileStatus.Corrupted;
					}
				}
				values = arraySingleObject2.GetValues();
				for (int k = 0; k < values.Length; k++)
				{
					ClassWithMembersAndTypes classWithMembersAndTypes2;
					if ((classWithMembersAndTypes2 = values[k] as ClassWithMembersAndTypes) != null)
					{
						Debug.LogError("Invalid SAVEFile: class " + classWithMembersAndTypes2.GetClassName() + " contained in flags");
						return FileStatus.Corrupted;
					}
				}
				file = rootObject.GetAs<SAVEFile>(true);
				Debug.Log("LEGACY SAVE");
				return FileStatus.Older;
			}
			catch (Exception ex2)
			{
				Debug.LogError("Couldn't deserialize file\n" + ex2);
				return FileStatus.Corrupted;
			}
		}
	}

	private static void WriteFlags(object[] flags, BinaryWriter writer)
	{
		writer.Write((short)flags.Length);
		foreach (object obj in flags)
		{
			object obj2 = obj;
			if (obj2 != null)
			{
				string text;
				object obj3;
				if ((obj3 = obj2) is int)
				{
					int num = (int)obj3;
					int value = num;
					writer.Write((byte)0);
					writer.Write(value);
				}
				else if ((text = obj2 as string) == null)
				{
					if ((obj3 = obj2) is bool)
					{
						bool flag = (bool)obj3;
						bool value2 = flag;
						writer.Write((byte)2);
						writer.Write(value2);
					}
					else if ((obj3 = obj2) is float)
					{
						float num2 = (float)obj3;
						float value3 = num2;
						writer.Write((byte)3);
						writer.Write(value3);
					}
					else
					{
						Debug.LogError("Invalid flag type " + obj.GetType());
					}
				}
				else
				{
					string value4 = text;
					writer.Write((byte)1);
					writer.Write(value4);
				}
			}
			else
			{
				writer.Write(byte.MaxValue);
			}
		}
	}

	private static object[] ReadFlags(BinaryReader reader)
	{
		object[] array = new object[reader.ReadInt16()];
		for (int i = 0; i < array.Length; i++)
		{
			switch (reader.ReadByte())
			{
			case byte.MaxValue:
				array[i] = null;
				break;
			case 0:
				array[i] = reader.ReadInt32();
				break;
			case 1:
				array[i] = reader.ReadString();
				break;
			case 2:
				array[i] = reader.ReadBoolean();
				break;
			case 3:
				array[i] = reader.ReadSingle();
				break;
			}
		}
		return array;
	}

	private static T UnwrapRef<T>(object obj) where T : Record
	{
		MemberReference memberReference;
		while ((memberReference = obj as MemberReference) != null)
		{
			obj = memberReference.GetReference();
		}
		if (obj is ObjectNull)
		{
			return null;
		}
		return (T)obj;
	}

	private static string ToHexString(byte[] bytes)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte b in bytes)
		{
			stringBuilder.Append(string.Format("{0:X2}", b));
		}
		return stringBuilder.ToString();
	}
}
