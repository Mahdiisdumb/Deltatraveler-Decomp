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

	public static readonly int FORMAT_VERSION = 4;

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
		using BinaryWriter binaryWriter = new BinaryWriter(stream);
		binaryWriter.Write((short)FORMAT_VERSION);
		long position = stream.Position;
		binaryWriter.Write(HASH_SALT);
		binaryWriter.Write(file.name);
		binaryWriter.Write(file.exp);
		WriteList(file.items, binaryWriter);
		WriteList(file.equipItems, binaryWriter);
		WriteList(file.boxItems, binaryWriter);
		for (int i = 0; i < file.party.Length; i++)
		{
			binaryWriter.Write((sbyte)file.party[i]);
		}
		binaryWriter.Write((byte)PartyMembers.GetNumPartyMembers());
		for (int j = 0; j < PartyMembers.GetNumPartyMembers(); j++)
		{
			binaryWriter.Write((short)file.hp[j]);
			binaryWriter.Write((short)file.weapon[j]);
			binaryWriter.Write((short)file.armor[j]);
		}
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

	public static FileStatus ReadFile(ref SAVEFile file, FileStream fs)
	{
		long length = fs.Length;
		byte[] array = new byte[length];
		fs.Read(array, 0, (int)length);
		using MemoryStream memoryStream = new MemoryStream(array);
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
				using BinaryReader binaryReader = new BinaryReader(memoryStream);
				bool flag = false;
				byte[] array3 = null;
				byte[] array4 = null;
				int num = binaryReader.ReadInt16();
				Debug.Log("NEW SAVE (FORMAT VERSION " + num + ")");
				if (num > FORMAT_VERSION)
				{
					int fORMAT_VERSION = FORMAT_VERSION;
					Debug.Log("Not loading, save version is newer than max supported version (" + fORMAT_VERSION + ")");
					return FileStatus.Newer;
				}
				if (num > 0)
				{
					array3 = binaryReader.ReadBytes(32);
					memoryStream.Seek(-32L, SeekOrigin.Current);
					memoryStream.Write(HASH_SALT, 0, HASH_SALT.Length);
					Debug.Log(array.Length);
					using SHA256 sHA = SHA256.Create();
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
				file.name = binaryReader.ReadString();
				file.exp = binaryReader.ReadInt32();
				file.items = ReadList(binaryReader);
				if (num >= 3)
				{
					file.equipItems = ReadList(binaryReader);
					file.boxItems = ReadList(binaryReader);
				}
				file.party = new int[6];
				file.hp = new int[PartyMembers.GetNumPartyMembers()];
				file.weapon = new int[PartyMembers.GetNumPartyMembers()];
				file.armor = new int[PartyMembers.GetNumPartyMembers()];
				if (num >= 4)
				{
					for (int i = 0; i < file.party.Length; i++)
					{
						file.party[i] = binaryReader.ReadSByte();
					}
					int num2 = binaryReader.ReadByte();
					for (int j = 0; j < num2; j++)
					{
						file.hp[j] = binaryReader.ReadInt16();
						file.weapon[j] = binaryReader.ReadInt16();
						file.armor[j] = binaryReader.ReadInt16();
					}
				}
				else
				{
					for (int k = 0; k < 3; k++)
					{
						file.weapon[k] = binaryReader.ReadInt16();
						file.armor[k] = binaryReader.ReadInt16();
					}
					file.susieActive = binaryReader.ReadBoolean();
					file.noelleActive = binaryReader.ReadBoolean();
				}
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
					using SHA256 sHA2 = SHA256.Create();
					array4 = sHA2.ComputeHash(array, 0, (int)memoryStream.Position);
					if (!array4.SequenceEqual(array3))
					{
						Debug.Log("Hash mismatch (2): " + ToHexString(array4) + " != " + ToHexString(array3));
						return FileStatus.Corrupted;
					}
					Debug.Log("File hash discrepancy fixed (" + memoryStream.Length + " -> " + memoryStream.Position + ")");
					fs.SetLength(memoryStream.Position);
				}
				if (num < FORMAT_VERSION)
				{
					PatchFile(ref file);
					return FileStatus.Older;
				}
				return FileStatus.OK;
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
			for (int fORMAT_VERSION = 0; fORMAT_VERSION < values.Length; fORMAT_VERSION++)
			{
				if (values[fORMAT_VERSION] is ClassWithMembersAndTypes classWithMembersAndTypes)
				{
					Debug.LogError("Invalid SAVEFile: class " + classWithMembersAndTypes.GetClassName() + " contained in flags");
					return FileStatus.Corrupted;
				}
			}
			values = arraySingleObject2.GetValues();
			for (int fORMAT_VERSION = 0; fORMAT_VERSION < values.Length; fORMAT_VERSION++)
			{
				if (values[fORMAT_VERSION] is ClassWithMembersAndTypes classWithMembersAndTypes2)
				{
					Debug.LogError("Invalid SAVEFile: class " + classWithMembersAndTypes2.GetClassName() + " contained in flags");
					return FileStatus.Corrupted;
				}
			}
			file = rootObject.GetAs<SAVEFile>(deserializeNestedObjects: true);
			Debug.Log("LEGACY SAVE");
			PatchFile(ref file);
			return FileStatus.Older;
		}
		catch (Exception ex2)
		{
			Debug.LogError("Couldn't deserialize file\n" + ex2);
			return FileStatus.Corrupted;
		}
	}

	private static void WriteFlags(object[] flags, BinaryWriter writer)
	{
		writer.Write((short)flags.Length);
		foreach (object obj in flags)
		{
			if (obj != null)
			{
				if (!(obj is int value))
				{
					if (!(obj is string value2))
					{
						if (!(obj is bool value3))
						{
							if (obj is float value4)
							{
								writer.Write((byte)3);
								writer.Write(value4);
							}
							else
							{
								Debug.LogError("Invalid flag type " + obj.GetType());
							}
						}
						else
						{
							writer.Write((byte)2);
							writer.Write(value3);
						}
					}
					else
					{
						writer.Write((byte)1);
						writer.Write(value2);
					}
				}
				else
				{
					writer.Write((byte)0);
					writer.Write(value);
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

	private static void WriteList(List<int> list, BinaryWriter writer)
	{
		writer.Write((byte)list.Count);
		foreach (int item in list)
		{
			writer.Write((short)item);
		}
	}

	private static List<int> ReadList(BinaryReader reader)
	{
		List<int> list = new List<int>();
		byte b = reader.ReadByte();
		for (int i = 0; i < b; i++)
		{
			list.Add(reader.ReadInt16());
		}
		return list;
	}

	private static T UnwrapRef<T>(object obj) where T : Record
	{
		while (obj is MemberReference memberReference)
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
			stringBuilder.Append($"{b:X2}");
		}
		return stringBuilder.ToString();
	}

	private static void SetFlag<T>(ref SAVEFile file, int id, T flag)
	{
		file.flags[id] = flag;
	}

	private static T GetFlag<T>(ref SAVEFile file, int id)
	{
		object obj = file.flags[id];
		if (obj is T)
		{
			return (T)obj;
		}
		return default(T);
	}

	private static void PatchFile(ref SAVEFile save)
	{
		if (GetFlag<int>(ref save, 322) == 0)
		{
			SetFlag(ref save, 322, 1);
			save.equipItems = new List<int>();
			save.boxItems = new List<int>();
			foreach (int item in new List<int>(save.items))
			{
				if (Items.ItemType(item) == 1 || Items.ItemType(item) == 2)
				{
					save.items.Remove(item);
					save.items.Add(-1);
					save.equipItems.Add(item);
				}
				else if (item == 16)
				{
					save.items.Remove(item);
					save.items.Add(-1);
					SetFlag(ref save, 286, 1);
				}
			}
			while (save.equipItems.Count < 8)
			{
				save.equipItems.Add(-1);
			}
			if (GetFlag<int>(ref save, 156) == 1)
			{
				for (int i = 157; i < 167; i++)
				{
					int flag = GetFlag<int>(ref save, i);
					if (flag == 16)
					{
						SetFlag(ref save, 286, 1);
					}
					else if (flag > -1)
					{
						save.boxItems.Add(flag);
					}
					SetFlag(ref save, i, -1);
				}
			}
		}
		if (GetFlag<int>(ref save, 325) != 0)
		{
			return;
		}
		SetFlag(ref save, 325, 1);
		save.party = new int[6];
		for (int j = 0; j < save.party.Length; j++)
		{
			save.party[j] = -1;
		}
		save.hp = new int[PartyMembers.GetNumPartyMembers()];
		for (int k = 0; k < PartyMembers.GetNumPartyMembers(); k++)
		{
			save.hp[k] = PartyMembers.GetMaxHP(k, save.exp);
			if (k < 3)
			{
				save.hp[k] += GetFlag<int>(ref save, 319 + k);
				SetFlag(ref save, 319 + k, -1);
			}
		}
		int[] array = (int[])save.weapon.Clone();
		int[] array2 = (int[])save.armor.Clone();
		save.weapon = new int[PartyMembers.GetNumPartyMembers()];
		save.armor = new int[PartyMembers.GetNumPartyMembers()];
		for (int l = 0; l < PartyMembers.GetNumPartyMembers(); l++)
		{
			if (l < 3)
			{
				save.weapon[l] = array[l];
				save.armor[l] = array2[l];
			}
			else
			{
				save.weapon[l] = PartyMembers.GetMemberStarterWeapon(l);
				save.armor[l] = PartyMembers.GetMemberStarterArmor(l);
			}
		}
		save.party[0] = ((GetFlag<int>(ref save, 107) == 1) ? 6 : 0);
		if (save.susieActive && save.noelleActive)
		{
			save.party[1] = 1;
			save.party[2] = 2;
		}
		else if (save.susieActive)
		{
			save.party[1] = 1;
		}
		else if (save.noelleActive)
		{
			save.party[1] = 2;
		}
		save.party[3] = ((GetFlag<int>(ref save, 86) == 1) ? 3 : (-1));
		SetFlag(ref save, 86, -1);
		if (GetFlag<int>(ref save, 107) == 1)
		{
			save.hp[6] = save.hp[0];
			save.weapon[6] = save.weapon[0];
			save.armor[6] = save.armor[0];
			save.hp[0] = PartyMembers.GetMaxHP(0, save.exp);
			save.weapon[0] = PartyMembers.GetMemberStarterWeapon(0);
			save.armor[0] = PartyMembers.GetMemberStarterArmor(0);
			SetFlag(ref save, 326, GetFlag<string>(ref save, 0));
			SetFlag(ref save, 107, -1);
		}
	}
}
