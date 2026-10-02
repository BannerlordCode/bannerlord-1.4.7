using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x0200002E RID: 46
	public class SaveContext : ISaveContext
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000095F7 File Offset: 0x000077F7
		// (set) Token: 0x060001CC RID: 460 RVA: 0x000095FF File Offset: 0x000077FF
		public object RootObject { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00009608 File Offset: 0x00007808
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00009610 File Offset: 0x00007810
		public GameData SaveData { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00009619 File Offset: 0x00007819
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00009621 File Offset: 0x00007821
		public DefinitionContext DefinitionContext { get; private set; }

		// Token: 0x060001D1 RID: 465 RVA: 0x0000962A File Offset: 0x0000782A
		public static SaveContext.SaveStatistics GetStatistics()
		{
			return new SaveContext.SaveStatistics(SaveContext._typeStatistics, SaveContext._containerStatistics);
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000963B File Offset: 0x0000783B
		public static bool EnableSaveStatistics
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00009640 File Offset: 0x00007840
		public SaveContext(DefinitionContext definitionContext)
		{
			this.DefinitionContext = definitionContext;
			this._childObjects = new List<object>(131072);
			this._idsOfChildObjects = new Dictionary<object, int>(131072);
			this._strings = new List<string>(131072);
			this._idsOfStrings = new Dictionary<string, int>(131072);
			this._childContainers = new List<object>(131072);
			this._idsOfChildContainers = new Dictionary<object, int>(131072);
			this._temporaryCollectedObjects = new List<object>(4096);
			this._locker = new object();
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000096D8 File Offset: 0x000078D8
		private void CollectSaveDatas()
		{
			SaveContext.SizeRecord.HeaderSize = SaveContext.GetConfigEntrySize();
			SaveContext.SizeRecord.StringSize = SaveContext.GetStringFolderSize();
			SaveContext.SizeRecord.ObjectSize = 0;
			SaveContext.SizeRecord.ContainerSize = 0;
			using (new PerformanceTestBlock("SaveContext::CollectSaveDataForObject::Objects"))
			{
				if (!SaveContext.EnableSaveStatistics)
				{
					TWParallel.ForWithoutRenderThread(0, this._childObjects.Count, delegate(int startInclusive, int endExclusive)
					{
						for (int k = startInclusive; k < endExclusive; k++)
						{
							this.CollectSaveDataForObject(k, ref SaveContext.SizeRecord);
						}
					}, 16);
				}
				else
				{
					for (int i = 0; i < this._childObjects.Count; i++)
					{
						this.CollectSaveDataForObject(i, ref SaveContext.SizeRecord);
					}
				}
			}
			using (new PerformanceTestBlock("SaveContext::CollectSaveDataForObject::Containers"))
			{
				if (!SaveContext.EnableSaveStatistics)
				{
					TWParallel.ForWithoutRenderThread(0, this._childContainers.Count, delegate(int startInclusive, int endExclusive)
					{
						for (int l = startInclusive; l < endExclusive; l++)
						{
							this.CollectSaveDataForContainer(l, ref SaveContext.SizeRecord);
						}
					}, 16);
				}
				else
				{
					for (int j = 0; j < this._childContainers.Count; j++)
					{
						this.CollectSaveDataForContainer(j, ref SaveContext.SizeRecord);
					}
				}
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000097F8 File Offset: 0x000079F8
		private void CollectObjects()
		{
			using (new PerformanceTestBlock("SaveContext::CollectObjects"))
			{
				this._objectsToIterate = new Queue<object>(1024);
				this._objectsToIterate.Enqueue(this.RootObject);
				while (this._objectsToIterate.Count > 0)
				{
					object obj = this._objectsToIterate.Dequeue();
					ContainerType containerType;
					if (obj.GetType().IsContainer(out containerType))
					{
						this.CollectContainerObjects(containerType, obj);
					}
					else
					{
						this.CollectObjects(obj);
					}
				}
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000988C File Offset: 0x00007A8C
		private void CollectContainerObjects(ContainerType containerType, object parent)
		{
			if (!this._idsOfChildContainers.ContainsKey(parent))
			{
				int count = this._childContainers.Count;
				this._childContainers.Add(parent);
				this._idsOfChildContainers.Add(parent, count);
				Type type = parent.GetType();
				ContainerDefinition containerDefinition = this.DefinitionContext.GetContainerDefinition(type);
				if (containerDefinition == null)
				{
					string text = "Cant find definition for " + type.FullName;
					Debug.Print(text, 0, Debug.DebugColor.Red, 17592186044416UL);
					Debug.FailedAssert(text, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Save\\SaveContext.cs", "CollectContainerObjects", 222);
				}
				ContainerSaveData.GetChildObjects(this, containerDefinition, containerType, parent, this._temporaryCollectedObjects);
				for (int i = 0; i < this._temporaryCollectedObjects.Count; i++)
				{
					object obj = this._temporaryCollectedObjects[i];
					if (obj != null)
					{
						this._objectsToIterate.Enqueue(obj);
					}
				}
				this._temporaryCollectedObjects.Clear();
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000996C File Offset: 0x00007B6C
		private void CollectObjects(object parent)
		{
			if (!this._idsOfChildObjects.ContainsKey(parent))
			{
				int count = this._childObjects.Count;
				this._childObjects.Add(parent);
				this._idsOfChildObjects.Add(parent, count);
				Type type = parent.GetType();
				TypeDefinition classDefinition = this.DefinitionContext.GetClassDefinition(type);
				if (classDefinition == null)
				{
					throw new Exception("Could not find type definition of type: " + type);
				}
				ObjectSaveData.GetChildObjects(this, classDefinition, parent, this._temporaryCollectedObjects);
				for (int i = 0; i < this._temporaryCollectedObjects.Count; i++)
				{
					object obj = this._temporaryCollectedObjects[i];
					if (obj != null)
					{
						this._objectsToIterate.Enqueue(obj);
					}
				}
				this._temporaryCollectedObjects.Clear();
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00009A28 File Offset: 0x00007C28
		public int AddOrGetStringId(string text)
		{
			int num = -1;
			if (text == null)
			{
				num = -1;
			}
			else
			{
				object locker = this._locker;
				lock (locker)
				{
					int num2;
					if (this._idsOfStrings.TryGetValue(text, out num2))
					{
						num = num2;
					}
					else
					{
						num = this._strings.Count;
						this._idsOfStrings.Add(text, num);
						this._strings.Add(text);
						int stringSizeWithOverhead = SaveContext.GetStringSizeWithOverhead(text);
						Interlocked.Add(ref SaveContext.SizeRecord.StringSize, stringSizeWithOverhead);
					}
				}
			}
			return num;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009AC0 File Offset: 0x00007CC0
		public int GetObjectId(object target)
		{
			int num;
			if (!this._idsOfChildObjects.TryGetValue(target, out num))
			{
				Debug.Print(string.Format("SAVE ERROR. Cant find {0} with type {1}", target, target.GetType()), 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.FailedAssert("SAVE ERROR. Cant find target object on save", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Save\\SaveContext.cs", "GetObjectId", 310);
			}
			return num;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00009B19 File Offset: 0x00007D19
		public int GetContainerId(object target)
		{
			return this._idsOfChildContainers[target];
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00009B28 File Offset: 0x00007D28
		public int GetStringId(string target)
		{
			if (target == null)
			{
				return -1;
			}
			int num = -1;
			object locker = this._locker;
			lock (locker)
			{
				num = this._idsOfStrings[target];
			}
			return num;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00009B78 File Offset: 0x00007D78
		private static void SaveStringTo(BinaryWriter stringWriter, int id, string text)
		{
			stringWriter.Write3ByteInt(0);
			stringWriter.Write3ByteInt(id);
			stringWriter.WriteByte(10);
			int stringSizeInBytes = SaveContext.GetStringSizeInBytes(text);
			stringWriter.WriteShort((short)stringSizeInBytes);
			stringWriter.WriteString(text);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009BB1 File Offset: 0x00007DB1
		public static int GetStringSizeInBytes(string text)
		{
			return 4 + Encoding.UTF8.GetByteCount(text);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00009BC0 File Offset: 0x00007DC0
		private static int GetStringSizeWithOverhead(string text)
		{
			return SaveContext.GetStringSizeInBytes(text) + 9;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00009BCC File Offset: 0x00007DCC
		public bool Save(object target, MetaData metaData, out string errorMessage)
		{
			errorMessage = "";
			bool flag = false;
			if (SaveContext.EnableSaveStatistics)
			{
				SaveContext._typeStatistics = new Dictionary<string, ValueTuple<int, int, int, long>>();
				SaveContext._containerStatistics = new Dictionary<string, ValueTuple<int, int, int, int, long>>();
			}
			try
			{
				this.RootObject = target;
				using (new PerformanceTestBlock("SaveContext::Save"))
				{
					this.CollectObjects();
					this._objectSaveDataList = new ObjectSaveData[this._childObjects.Count];
					this._containerSaveDataList = new ContainerSaveData[this._childContainers.Count];
					SaveContext.SizeRecord = default(SaveContext.SaveDataSizeRecord);
					this.CollectSaveDatas();
					byte[][] array = this.WriteObjects();
					byte[][] array2 = this.WriteContainers();
					new List<int>();
					byte[] array3 = SaveContext.WriteHeaders(this._objectSaveDataList, this._containerSaveDataList, SaveContext.SizeRecord.HeaderSize, this._strings.Count);
					byte[] array4 = SaveContext.WriteAllStrings(this._strings, SaveContext.SizeRecord.StringSize);
					this.SaveData = new GameData(array3, array4, array, array2);
					Debug.Print(SaveContext.SizeRecord.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				flag = true;
			}
			catch (Exception ex)
			{
				errorMessage = "SaveContext Error\n";
				errorMessage += ex.Message;
				flag = false;
			}
			return flag;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009D24 File Offset: 0x00007F24
		private byte[][] WriteObjects()
		{
			byte[][] objectData = new byte[this._childObjects.Count][];
			using (new PerformanceTestBlock("SaveContext::Saving Objects"))
			{
				if (!SaveContext.EnableSaveStatistics)
				{
					TWParallel.ForWithoutRenderThread(0, this._childObjects.Count, delegate(int startInclusive, int endExclusive)
					{
						for (int j = startInclusive; j < endExclusive; j++)
						{
							this.SaveSingleObject(objectData, j);
						}
					}, 16);
				}
				else
				{
					for (int i = 0; i < this._childObjects.Count; i++)
					{
						this.SaveSingleObject(objectData, i);
					}
				}
			}
			return objectData;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009DCC File Offset: 0x00007FCC
		private byte[][] WriteContainers()
		{
			byte[][] containerData = new byte[this._childContainers.Count][];
			using (new PerformanceTestBlock("SaveContext::Saving Containers"))
			{
				if (!SaveContext.EnableSaveStatistics)
				{
					TWParallel.ForWithoutRenderThread(0, this._childContainers.Count, delegate(int startInclusive, int endExclusive)
					{
						for (int j = startInclusive; j < endExclusive; j++)
						{
							this.SaveSingleContainer(containerData, j);
						}
					}, 16);
				}
				else
				{
					for (int i = 0; i < this._childContainers.Count; i++)
					{
						this.SaveSingleContainer(containerData, i);
					}
				}
			}
			return containerData;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009E74 File Offset: 0x00008074
		private static byte[] WriteHeaders(ObjectSaveData[] objects, ContainerSaveData[] containers, int headerSize, int stringCount)
		{
			BinaryWriter binaryWriter = new BinaryWriter(SaveContext.SizeRecord.HeaderSize);
			binaryWriter.WriteInt(objects.Length + containers.Length);
			for (int i = 0; i < objects.Length; i++)
			{
				objects[i].SaveHeaderFolderTo(binaryWriter, i);
			}
			for (int j = 0; j < containers.Length; j++)
			{
				containers[j].SaveHeaderFolderTo(binaryWriter, objects.Length + j);
			}
			binaryWriter.WriteInt(objects.Length + containers.Length + 1);
			for (int k = 0; k < objects.Length; k++)
			{
				objects[k].SaveHeaderDataTo(binaryWriter, k);
			}
			for (int l = 0; l < containers.Length; l++)
			{
				containers[l].SaveHeaderDataTo(binaryWriter, objects.Length + l);
			}
			SaveContext.WriteConfigEntry(binaryWriter, objects.Length, stringCount, containers.Length);
			return binaryWriter.Data;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009F30 File Offset: 0x00008130
		private static byte[] WriteAllStrings(List<string> strings, int stringSize)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stringSize);
			SaveContext.WriteStringsEntry(binaryWriter, strings.Count);
			for (int i = 0; i < strings.Count; i++)
			{
				string text = strings[i];
				SaveContext.SaveStringTo(binaryWriter, i, text);
			}
			return binaryWriter.Data;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009F77 File Offset: 0x00008177
		private static void WriteConfigEntry(BinaryWriter headerWriter, int objects, int strings, int containers)
		{
			headerWriter.Write3ByteInt(-1);
			headerWriter.Write3ByteInt(-1);
			headerWriter.WriteByte(7);
			headerWriter.WriteShort(12);
			headerWriter.WriteInt(objects);
			headerWriter.WriteInt(strings);
			headerWriter.WriteInt(containers);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00009FAB File Offset: 0x000081AB
		private static void WriteStringsEntry(BinaryWriter headerWriter, int strings)
		{
			headerWriter.WriteInt(1);
			headerWriter.Write3ByteInt(-1);
			headerWriter.Write3ByteInt(0);
			headerWriter.Write3ByteInt(-1);
			headerWriter.WriteByte(4);
			headerWriter.WriteInt(strings);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00009FD7 File Offset: 0x000081D7
		private static int GetConfigEntrySize()
		{
			return 29;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00009FDB File Offset: 0x000081DB
		private static int GetStringFolderSize()
		{
			return 18;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00009FE0 File Offset: 0x000081E0
		private void CollectSaveDataForObject(int id, ref SaveContext.SaveDataSizeRecord headerSize)
		{
			object obj = this._childObjects[id];
			ObjectSaveData objectSaveData = new ObjectSaveData(this, id, obj, true);
			objectSaveData.CollectStructs();
			objectSaveData.CollectMembers();
			objectSaveData.CollectStrings();
			this._objectSaveDataList[id] = objectSaveData;
			Interlocked.Add(ref headerSize.HeaderSize, objectSaveData.GetHeaderSize());
			Interlocked.Add(ref headerSize.ObjectSize, objectSaveData.GetDataSize());
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000A044 File Offset: 0x00008244
		private void CollectSaveDataForContainer(int id, ref SaveContext.SaveDataSizeRecord headerSize)
		{
			object obj = this._childContainers[id];
			ContainerType containerType;
			obj.GetType().IsContainer(out containerType);
			ContainerSaveData containerSaveData = new ContainerSaveData(this, id, obj, containerType);
			containerSaveData.CollectChildren();
			containerSaveData.CollectStructs();
			containerSaveData.CollectMembers();
			containerSaveData.CollectStrings();
			this._containerSaveDataList[id] = containerSaveData;
			Interlocked.Add(ref headerSize.HeaderSize, containerSaveData.GetHeaderSize());
			Interlocked.Add(ref headerSize.ContainerSize, containerSaveData.GetDataSize());
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000A0BC File Offset: 0x000082BC
		private void SaveSingleObject(byte[][] objectData, int id)
		{
			object obj = this._childObjects[id];
			ObjectSaveData objectSaveData = this._objectSaveDataList[id];
			int dataSize = objectSaveData.GetDataSize();
			BinaryWriter binaryWriter = new BinaryWriter(dataSize);
			int folderCount = objectSaveData.GetFolderCount();
			binaryWriter.WriteInt(folderCount);
			int num = 0;
			objectSaveData.SaveDataFolder(binaryWriter, -1, ref num);
			int entryCount = objectSaveData.GetEntryCount();
			binaryWriter.WriteInt(entryCount);
			num = 0;
			objectSaveData.SaveTo(binaryWriter, ref num);
			objectData[id] = binaryWriter.Data;
			if (SaveContext.EnableSaveStatistics)
			{
				string name = objectSaveData.Type.Name;
				ValueTuple<int, int, int, long> valueTuple;
				if (SaveContext._typeStatistics.TryGetValue(name, out valueTuple))
				{
					SaveContext._typeStatistics[name] = new ValueTuple<int, int, int, long>(valueTuple.Item1 + 1, valueTuple.Item2, valueTuple.Item3, valueTuple.Item4 + (long)dataSize);
					return;
				}
				SaveContext._typeStatistics[name] = new ValueTuple<int, int, int, long>(1, objectSaveData.FieldCount, objectSaveData.PropertyCount, (long)dataSize);
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000A1A4 File Offset: 0x000083A4
		private void SaveSingleContainer(byte[][] containerData, int id)
		{
			object obj = this._childContainers[id];
			ContainerSaveData containerSaveData = this._containerSaveDataList[id];
			int dataSize = containerSaveData.GetDataSize();
			BinaryWriter binaryWriter = new BinaryWriter(dataSize);
			binaryWriter.WriteInt(containerSaveData.GetFolderCount());
			int num = 0;
			containerSaveData.SaveDataFolder(binaryWriter, ref num);
			int entryCount = containerSaveData.GetEntryCount();
			binaryWriter.WriteInt(entryCount);
			num = 0;
			containerSaveData.SaveTo(binaryWriter, ref num);
			containerData[id] = binaryWriter.Data;
			if (SaveContext.EnableSaveStatistics)
			{
				string containerName = this.GetContainerName(containerSaveData.Type);
				ValueTuple<int, int, int, int, long> valueTuple;
				if (SaveContext._containerStatistics.TryGetValue(containerName, out valueTuple))
				{
					SaveContext._containerStatistics[containerName] = new ValueTuple<int, int, int, int, long>(valueTuple.Item1 + 1, valueTuple.Item2 + containerSaveData.GetElementCount(), valueTuple.Item3, valueTuple.Item4, SaveContext._containerStatistics[containerName].Item5 + (long)dataSize);
					return;
				}
				SaveContext._containerStatistics[containerName] = new ValueTuple<int, int, int, int, long>(1, containerSaveData.GetElementCount(), containerSaveData.ElementFieldCount, containerSaveData.ElementPropertyCount, (long)dataSize);
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000A2AC File Offset: 0x000084AC
		private string GetContainerName(Type t)
		{
			string text = t.Name;
			foreach (Type type in t.GetGenericArguments())
			{
				if (t.IsContainer())
				{
					text += this.GetContainerName(type);
				}
				else
				{
					text = text + type.Name + ".";
				}
			}
			return text.TrimEnd(new char[] { '.' });
		}

		// Token: 0x0400007D RID: 125
		private static SaveContext.SaveDataSizeRecord SizeRecord;

		// Token: 0x0400007F RID: 127
		private List<object> _childObjects;

		// Token: 0x04000080 RID: 128
		private Dictionary<object, int> _idsOfChildObjects;

		// Token: 0x04000081 RID: 129
		private List<object> _childContainers;

		// Token: 0x04000082 RID: 130
		private Dictionary<object, int> _idsOfChildContainers;

		// Token: 0x04000083 RID: 131
		private List<string> _strings;

		// Token: 0x04000084 RID: 132
		private Dictionary<string, int> _idsOfStrings;

		// Token: 0x04000085 RID: 133
		private List<object> _temporaryCollectedObjects;

		// Token: 0x04000086 RID: 134
		private ObjectSaveData[] _objectSaveDataList;

		// Token: 0x04000087 RID: 135
		private ContainerSaveData[] _containerSaveDataList;

		// Token: 0x04000089 RID: 137
		private object _locker;

		// Token: 0x0400008B RID: 139
		private static Dictionary<string, ValueTuple<int, int, int, long>> _typeStatistics;

		// Token: 0x0400008C RID: 140
		private static Dictionary<string, ValueTuple<int, int, int, int, long>> _containerStatistics;

		// Token: 0x0400008D RID: 141
		private Queue<object> _objectsToIterate;

		// Token: 0x02000077 RID: 119
		private struct SaveDataSizeRecord
		{
			// Token: 0x060003F9 RID: 1017 RVA: 0x00011978 File Offset: 0x0000FB78
			public override string ToString()
			{
				float num = (float)(this.HeaderSize + this.StringSize + this.ObjectSize + this.ContainerSize) / 1048576f;
				string.Format("Total size: {0:##.00} MB", num);
				return string.Format("[SaveDataSizeRecord] Header size: {0}, String Size: {1}, Object Size: {2}, Container Size: {3}.\nTotal Size: {4}", new object[] { this.HeaderSize, this.StringSize, this.ObjectSize, this.ContainerSize, num });
			}

			// Token: 0x0400014E RID: 334
			public int HeaderSize;

			// Token: 0x0400014F RID: 335
			public int StringSize;

			// Token: 0x04000150 RID: 336
			public int ObjectSize;

			// Token: 0x04000151 RID: 337
			public int ContainerSize;
		}

		// Token: 0x02000078 RID: 120
		public struct SaveStatistics
		{
			// Token: 0x060003FA RID: 1018 RVA: 0x00011A0A File Offset: 0x0000FC0A
			public SaveStatistics(Dictionary<string, ValueTuple<int, int, int, long>> typeStatistics, Dictionary<string, ValueTuple<int, int, int, int, long>> containerStatistics)
			{
				this._typeStatistics = typeStatistics;
				this._containerStatistics = containerStatistics;
			}

			// Token: 0x060003FB RID: 1019 RVA: 0x00011A1C File Offset: 0x0000FC1C
			public ValueTuple<int, int, int, long> GetObjectCounts(string key)
			{
				if (this._typeStatistics.ContainsKey(key))
				{
					return this._typeStatistics[key];
				}
				return default(ValueTuple<int, int, int, long>);
			}

			// Token: 0x060003FC RID: 1020 RVA: 0x00011A4D File Offset: 0x0000FC4D
			public ValueTuple<int, int, int, int, long> GetContainerCounts(string key)
			{
				return this._containerStatistics[key];
			}

			// Token: 0x060003FD RID: 1021 RVA: 0x00011A5B File Offset: 0x0000FC5B
			public long GetContainerSize(string key)
			{
				return this._containerStatistics[key].Item5;
			}

			// Token: 0x060003FE RID: 1022 RVA: 0x00011A6E File Offset: 0x0000FC6E
			public List<string> GetTypeKeys()
			{
				return this._typeStatistics.Keys.ToList<string>();
			}

			// Token: 0x060003FF RID: 1023 RVA: 0x00011A80 File Offset: 0x0000FC80
			public List<string> GetContainerKeys()
			{
				return this._containerStatistics.Keys.ToList<string>();
			}

			// Token: 0x04000152 RID: 338
			private Dictionary<string, ValueTuple<int, int, int, long>> _typeStatistics;

			// Token: 0x04000153 RID: 339
			private Dictionary<string, ValueTuple<int, int, int, int, long>> _containerStatistics;
		}
	}
}
