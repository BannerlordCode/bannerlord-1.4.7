using System;
using System.Collections;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x02000026 RID: 38
	internal class ContainerSaveData
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00006DAC File Offset: 0x00004FAC
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00006DB4 File Offset: 0x00004FB4
		public int ObjectId { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00006DBD File Offset: 0x00004FBD
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00006DC5 File Offset: 0x00004FC5
		public ISaveContext Context { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00006DCE File Offset: 0x00004FCE
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00006DD6 File Offset: 0x00004FD6
		public object Target { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00006DDF File Offset: 0x00004FDF
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00006DE7 File Offset: 0x00004FE7
		public Type Type { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00006DF0 File Offset: 0x00004FF0
		internal int ElementPropertyCount
		{
			get
			{
				if (this._childStructs.Count <= 0)
				{
					return 0;
				}
				return this._childStructs[0].PropertyCount;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00006E13 File Offset: 0x00005013
		internal int ElementFieldCount
		{
			get
			{
				if (this._childStructs.Count <= 0)
				{
					return 0;
				}
				return this._childStructs[0].FieldCount;
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00006E38 File Offset: 0x00005038
		public ContainerSaveData(ISaveContext context, int objectId, object target, ContainerType containerType)
		{
			this.ObjectId = objectId;
			this.Context = context;
			this.Target = target;
			this._containerType = containerType;
			this.Type = target.GetType();
			this._elementCount = this.GetElementCount();
			this._childStructs = new List<ObjectSaveData>();
			this._typeDefinition = context.DefinitionContext.GetContainerDefinition(this.Type);
			if (this._typeDefinition == null)
			{
				throw new Exception("Could not find type definition of container type: " + this.Type);
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006EC0 File Offset: 0x000050C0
		public void CollectChildren()
		{
			this._keys = new ElementSaveData[this._elementCount];
			this._values = new ElementSaveData[this._elementCount];
			if (this._containerType == ContainerType.Dictionary)
			{
				IDictionary dictionary = (IDictionary)this.Target;
				int num = 0;
				using (IDictionaryEnumerator enumerator = dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						object key = dictionaryEntry.Key;
						object value = dictionaryEntry.Value;
						ElementSaveData elementSaveData = new ElementSaveData(this, key, num);
						ElementSaveData elementSaveData2 = new ElementSaveData(this, value, this._elementCount + num);
						this._keys[num] = elementSaveData;
						this._values[num] = elementSaveData2;
						num++;
					}
					return;
				}
			}
			if (this._containerType == ContainerType.List || this._containerType == ContainerType.CustomList || this._containerType == ContainerType.CustomReadOnlyList)
			{
				IList list = (IList)this.Target;
				for (int i = 0; i < this._elementCount; i++)
				{
					object obj2 = list[i];
					ElementSaveData elementSaveData3 = new ElementSaveData(this, obj2, i);
					this._values[i] = elementSaveData3;
				}
				return;
			}
			if (this._containerType == ContainerType.Queue)
			{
				IEnumerable enumerable = (ICollection)this.Target;
				int num2 = 0;
				using (IEnumerator enumerator2 = enumerable.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						object obj3 = enumerator2.Current;
						ElementSaveData elementSaveData4 = new ElementSaveData(this, obj3, num2);
						this._values[num2] = elementSaveData4;
						num2++;
					}
					return;
				}
			}
			if (this._containerType == ContainerType.Array)
			{
				Array array = (Array)this.Target;
				for (int j = 0; j < this._elementCount; j++)
				{
					object value2 = array.GetValue(j);
					ElementSaveData elementSaveData5 = new ElementSaveData(this, value2, j);
					this._values[j] = elementSaveData5;
				}
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000070B4 File Offset: 0x000052B4
		public void SaveHeaderTo(SaveEntryFolder parentFolder, IArchiveContext archiveContext)
		{
			SaveEntryFolder saveEntryFolder = archiveContext.CreateFolder(parentFolder, new FolderId(this.ObjectId, SaveFolderExtension.Container), 1);
			BinaryWriter binaryWriter = BinaryWriterFactory.GetBinaryWriter();
			this._typeDefinition.SaveId.WriteTo(binaryWriter);
			binaryWriter.WriteByte((byte)this._containerType);
			binaryWriter.WriteInt(this.GetElementCount());
			saveEntryFolder.CreateEntry(new EntryId(-1, SaveEntryExtension.Object)).FillFrom(binaryWriter);
			BinaryWriterFactory.ReleaseBinaryWriter(binaryWriter);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007120 File Offset: 0x00005320
		public void SaveHeaderDataTo(BinaryWriter headerWriter, int folderId)
		{
			headerWriter.Write3ByteInt(folderId);
			headerWriter.Write3ByteInt(-1);
			headerWriter.WriteByte(9);
			headerWriter.WriteShort((short)this.GetHeaderDataSize());
			this._typeDefinition.SaveId.WriteTo(headerWriter);
			headerWriter.WriteByte((byte)this._containerType);
			headerWriter.WriteInt(this.GetElementCount());
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000717A File Offset: 0x0000537A
		public void SaveHeaderFolderTo(BinaryWriter headerWriter, int folderId)
		{
			headerWriter.Write3ByteInt(-1);
			headerWriter.Write3ByteInt(folderId);
			headerWriter.Write3ByteInt(this.ObjectId);
			headerWriter.WriteByte(3);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000719D File Offset: 0x0000539D
		private int GetHeaderDataSize()
		{
			return 5 + this._typeDefinition.SaveId.GetSizeInBytes();
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000071B1 File Offset: 0x000053B1
		public int GetHeaderSize()
		{
			return this.GetHeaderDataSize() + 19;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000071BC File Offset: 0x000053BC
		public int GetDataSize()
		{
			int num = 18;
			for (int i = 0; i < this._elementCount; i++)
			{
				num += this._values[i].GetDataSize();
				num += this.GetMemberEntrySize();
				if (this._containerType == ContainerType.Dictionary)
				{
					num += this._keys[i].GetDataSize();
					num += this.GetMemberEntrySize();
				}
			}
			foreach (ObjectSaveData objectSaveData in this._childStructs)
			{
				TypeDefinition structDefinition = this.Context.DefinitionContext.GetStructDefinition(objectSaveData.Type);
				if (this.ShouldSaveStruct(structDefinition, objectSaveData))
				{
					num += objectSaveData.GetDataSize() - 8;
				}
			}
			return num;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007288 File Offset: 0x00005488
		private bool ShouldSaveStruct(TypeDefinition structDefinition, ObjectSaveData objectSaveData)
		{
			ISavedStruct savedStruct;
			if (structDefinition == null || !(structDefinition is StructDefinition) || (savedStruct = objectSaveData.Target as ISavedStruct) == null || !savedStruct.IsDefault())
			{
				return true;
			}
			ContainerSaveId containerSaveId = (ContainerSaveId)this._typeDefinition.SaveId;
			if (this._containerType != ContainerType.Dictionary)
			{
				return containerSaveId.KeyId != structDefinition.SaveId;
			}
			SaveId keyId = containerSaveId.KeyId;
			SaveId valueId = containerSaveId.ValueId;
			if (this.IsKey(objectSaveData))
			{
				return keyId != structDefinition.SaveId;
			}
			return valueId != structDefinition.SaveId;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007318 File Offset: 0x00005518
		private bool IsKey(ObjectSaveData objectSaveData)
		{
			for (int i = 0; i < this._elementCount; i++)
			{
				ElementSaveData elementSaveData = this._keys[i];
				ElementSaveData elementSaveData2 = this._values[i];
				if (elementSaveData != null && objectSaveData.ObjectId == elementSaveData.ElementIndex)
				{
					return true;
				}
				if (elementSaveData2 != null && objectSaveData.ObjectId == elementSaveData2.ElementIndex)
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000736F File Offset: 0x0000556F
		private int GetMemberEntrySize()
		{
			return 9;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007374 File Offset: 0x00005574
		public int GetEntryCount()
		{
			int num = ((this._containerType == ContainerType.Dictionary) ? (this._elementCount * 2) : this._elementCount);
			foreach (ObjectSaveData objectSaveData in this._childStructs)
			{
				TypeDefinition structDefinition = this.Context.DefinitionContext.GetStructDefinition(objectSaveData.Type);
				if (this.ShouldSaveStruct(structDefinition, objectSaveData))
				{
					num += objectSaveData.GetEntryCount();
				}
			}
			return num;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007408 File Offset: 0x00005608
		public int GetFolderCount()
		{
			int num = 1;
			foreach (ObjectSaveData objectSaveData in this._childStructs)
			{
				TypeDefinition structDefinition = this.Context.DefinitionContext.GetStructDefinition(objectSaveData.Type);
				if (this.ShouldSaveStruct(structDefinition, objectSaveData))
				{
					num += objectSaveData.GetFolderCount();
				}
			}
			return num;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007484 File Offset: 0x00005684
		public void SaveDataFolder(BinaryWriter writer, ref int folderId)
		{
			writer.Write3ByteInt(-1);
			writer.Write3ByteInt(0);
			writer.Write3ByteInt(this.ObjectId);
			writer.WriteByte(3);
			folderId++;
			foreach (ObjectSaveData objectSaveData in this._childStructs)
			{
				TypeDefinition structDefinition = this.Context.DefinitionContext.GetStructDefinition(objectSaveData.Type);
				if (this.ShouldSaveStruct(structDefinition, objectSaveData))
				{
					objectSaveData.SaveDataFolder(writer, 0, ref folderId);
				}
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007524 File Offset: 0x00005724
		public void SaveTo(BinaryWriter writer, ref int folderId)
		{
			for (int i = 0; i < this._elementCount; i++)
			{
				this.WriteElementEntry(writer, this._values[i], folderId, i, SaveEntryExtension.Value);
				this._values[i].SaveTo(writer);
				if (this._containerType == ContainerType.Dictionary)
				{
					this.WriteElementEntry(writer, this._keys[i], folderId, i, SaveEntryExtension.Key);
					this._keys[i].SaveTo(writer);
				}
			}
			folderId++;
			foreach (ObjectSaveData objectSaveData in this._childStructs)
			{
				TypeDefinition structDefinition = this.Context.DefinitionContext.GetStructDefinition(objectSaveData.Type);
				if (this.ShouldSaveStruct(structDefinition, objectSaveData))
				{
					objectSaveData.SaveTo(writer, ref folderId);
				}
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000075FC File Offset: 0x000057FC
		public void SaveTo(SaveEntryFolder parentFolder, IArchiveContext archiveContext)
		{
			int num = ((this._containerType == ContainerType.Dictionary) ? (this._elementCount * 2) : this._elementCount);
			SaveEntryFolder saveEntryFolder = archiveContext.CreateFolder(parentFolder, new FolderId(this.ObjectId, SaveFolderExtension.Container), num);
			for (int i = 0; i < this._elementCount; i++)
			{
				VariableSaveData variableSaveData = this._values[i];
				BinaryWriter binaryWriter = BinaryWriterFactory.GetBinaryWriter();
				variableSaveData.SaveTo(binaryWriter);
				saveEntryFolder.CreateEntry(new EntryId(i, SaveEntryExtension.Value)).FillFrom(binaryWriter);
				BinaryWriterFactory.ReleaseBinaryWriter(binaryWriter);
				if (this._containerType == ContainerType.Dictionary)
				{
					VariableSaveData variableSaveData2 = this._keys[i];
					BinaryWriter binaryWriter2 = BinaryWriterFactory.GetBinaryWriter();
					variableSaveData2.SaveTo(binaryWriter2);
					saveEntryFolder.CreateEntry(new EntryId(i, SaveEntryExtension.Key)).FillFrom(binaryWriter2);
					BinaryWriterFactory.ReleaseBinaryWriter(binaryWriter2);
				}
			}
			foreach (ObjectSaveData objectSaveData in this._childStructs)
			{
				TypeDefinition structDefinition = this.Context.DefinitionContext.GetStructDefinition(objectSaveData.Type);
				if (this.ShouldSaveStruct(structDefinition, objectSaveData))
				{
					objectSaveData.SaveTo(saveEntryFolder, archiveContext);
				}
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007720 File Offset: 0x00005920
		private void WriteElementEntry(BinaryWriter writer, ElementSaveData data, int parentFolderId, int id, SaveEntryExtension extension)
		{
			writer.Write3ByteInt(parentFolderId);
			writer.Write3ByteInt(id);
			writer.WriteByte((byte)extension);
			writer.WriteShort((short)data.GetDataSize());
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007748 File Offset: 0x00005948
		internal int GetElementCount()
		{
			if (this._containerType == ContainerType.List || this._containerType == ContainerType.CustomList || this._containerType == ContainerType.CustomReadOnlyList)
			{
				return ((IList)this.Target).Count;
			}
			if (this._containerType == ContainerType.Queue)
			{
				return ((ICollection)this.Target).Count;
			}
			if (this._containerType == ContainerType.Dictionary)
			{
				return ((IDictionary)this.Target).Count;
			}
			if (this._containerType == ContainerType.Array)
			{
				return ((Array)this.Target).GetLength(0);
			}
			return 0;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000077D4 File Offset: 0x000059D4
		public void CollectStrings()
		{
			foreach (object obj in this.GetChildString())
			{
				string text = (string)obj;
				this.Context.AddOrGetStringId(text);
			}
			foreach (ObjectSaveData objectSaveData in this._childStructs)
			{
				objectSaveData.CollectStrings();
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000786C File Offset: 0x00005A6C
		public void CollectStringsInto(List<string> collection)
		{
			foreach (object obj in this.GetChildString())
			{
				string text = (string)obj;
				collection.Add(text);
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000078C0 File Offset: 0x00005AC0
		public void CollectStructs()
		{
			foreach (ElementSaveData elementSaveData in this.GetChildElementSaveDatas())
			{
				if (elementSaveData.MemberType == SavedMemberType.CustomStruct)
				{
					object elementValue = elementSaveData.ElementValue;
					ObjectSaveData objectSaveData = new ObjectSaveData(this.Context, elementSaveData.ElementIndex, elementValue, false);
					this._childStructs.Add(objectSaveData);
				}
			}
			foreach (ObjectSaveData objectSaveData2 in this._childStructs)
			{
				objectSaveData2.CollectStructs();
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00007978 File Offset: 0x00005B78
		public void CollectMembers()
		{
			foreach (ObjectSaveData objectSaveData in this._childStructs)
			{
				objectSaveData.CollectMembers();
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000079C8 File Offset: 0x00005BC8
		public IEnumerable<ElementSaveData> GetChildElementSaveDatas()
		{
			int num;
			for (int i = 0; i < this._elementCount; i = num + 1)
			{
				ElementSaveData elementSaveData = this._keys[i];
				ElementSaveData value = this._values[i];
				if (elementSaveData != null)
				{
					yield return elementSaveData;
				}
				if (value != null)
				{
					yield return value;
				}
				value = null;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000079D8 File Offset: 0x00005BD8
		public IEnumerable<object> GetChildElements()
		{
			return ContainerSaveData.GetChildElements(this._containerType, this.Target);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000079EB File Offset: 0x00005BEB
		public static IEnumerable<object> GetChildElements(ContainerType containerType, object target)
		{
			if (containerType == ContainerType.List || containerType == ContainerType.CustomList || containerType == ContainerType.CustomReadOnlyList)
			{
				IList list = (IList)target;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					object obj = list[i];
					if (obj != null)
					{
						yield return obj;
					}
					num = i;
				}
				list = null;
			}
			else if (containerType == ContainerType.Queue)
			{
				ICollection collection = (ICollection)target;
				foreach (object obj2 in collection)
				{
					if (obj2 != null)
					{
						yield return obj2;
					}
				}
				IEnumerator enumerator = null;
			}
			else if (containerType == ContainerType.Dictionary)
			{
				IDictionary dictionary = (IDictionary)target;
				foreach (object obj3 in dictionary)
				{
					DictionaryEntry entry = (DictionaryEntry)obj3;
					yield return entry.Key;
					object value = entry.Value;
					if (value != null)
					{
						yield return value;
					}
					entry = default(DictionaryEntry);
				}
				IDictionaryEnumerator dictionaryEnumerator = null;
			}
			else if (containerType == ContainerType.Array)
			{
				Array array = (Array)target;
				int num;
				for (int i = 0; i < array.Length; i = num + 1)
				{
					object value2 = array.GetValue(i);
					if (value2 != null)
					{
						yield return value2;
					}
					num = i;
				}
				array = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007A04 File Offset: 0x00005C04
		public IEnumerable<object> GetChildObjects(ISaveContext context)
		{
			List<object> list = new List<object>();
			ContainerSaveData.GetChildObjects(context, this._typeDefinition, this._containerType, this.Target, list);
			return list;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007A34 File Offset: 0x00005C34
		public static void GetChildObjects(ISaveContext context, ContainerDefinition containerDefinition, ContainerType containerType, object target, List<object> collectedObjects)
		{
			if (containerDefinition.CollectObjectsMethod != null)
			{
				if (!containerDefinition.HasNoChildObject)
				{
					containerDefinition.CollectObjectsMethod(target, collectedObjects);
					return;
				}
			}
			else
			{
				if (containerType == ContainerType.List || containerType == ContainerType.CustomList || containerType == ContainerType.CustomReadOnlyList)
				{
					IList list = (IList)target;
					for (int i = 0; i < list.Count; i++)
					{
						object obj = list[i];
						if (obj != null)
						{
							ContainerSaveData.ProcessChildObjectElement(obj, context, collectedObjects);
						}
					}
					return;
				}
				if (containerType == ContainerType.Queue)
				{
					using (IEnumerator enumerator = ((ICollection)target).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							if (obj2 != null)
							{
								ContainerSaveData.ProcessChildObjectElement(obj2, context, collectedObjects);
							}
						}
						return;
					}
				}
				if (containerType == ContainerType.Dictionary)
				{
					using (IDictionaryEnumerator enumerator2 = ((IDictionary)target).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj3 = enumerator2.Current;
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
							ContainerSaveData.ProcessChildObjectElement(dictionaryEntry.Key, context, collectedObjects);
							object value = dictionaryEntry.Value;
							if (value != null)
							{
								ContainerSaveData.ProcessChildObjectElement(value, context, collectedObjects);
							}
						}
						return;
					}
				}
				if (containerType == ContainerType.Array)
				{
					Array array = (Array)target;
					for (int j = 0; j < array.Length; j++)
					{
						object value2 = array.GetValue(j);
						if (value2 != null)
						{
							ContainerSaveData.ProcessChildObjectElement(value2, context, collectedObjects);
						}
					}
					return;
				}
				foreach (object obj4 in ContainerSaveData.GetChildElements(containerType, target))
				{
					ContainerSaveData.ProcessChildObjectElement(obj4, context, collectedObjects);
				}
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00007BEC File Offset: 0x00005DEC
		private static void ProcessChildObjectElement(object childElement, ISaveContext context, List<object> collectedObjects)
		{
			Type type = childElement.GetType();
			bool isClass = type.IsClass;
			if (isClass && type != typeof(string))
			{
				collectedObjects.Add(childElement);
				return;
			}
			if (!isClass)
			{
				TypeDefinition structDefinition = context.DefinitionContext.GetStructDefinition(type);
				if (structDefinition != null)
				{
					if (structDefinition.CollectObjectsMethod != null)
					{
						structDefinition.CollectObjectsMethod(childElement, collectedObjects);
						return;
					}
					for (int i = 0; i < structDefinition.MemberDefinitions.Count; i++)
					{
						MemberDefinition memberDefinition = structDefinition.MemberDefinitions[i];
						ObjectSaveData.GetChildObjectFrom(context, childElement, memberDefinition, collectedObjects);
					}
				}
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007C7B File Offset: 0x00005E7B
		private IEnumerable<object> GetChildString()
		{
			foreach (object obj in this.GetChildElements())
			{
				if (obj.GetType() == typeof(string))
				{
					yield return obj;
				}
			}
			IEnumerator<object> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04000058 RID: 88
		private ContainerType _containerType;

		// Token: 0x04000059 RID: 89
		private ElementSaveData[] _keys;

		// Token: 0x0400005A RID: 90
		private ElementSaveData[] _values;

		// Token: 0x0400005B RID: 91
		private ContainerDefinition _typeDefinition;

		// Token: 0x0400005C RID: 92
		private int _elementCount;

		// Token: 0x0400005D RID: 93
		private List<ObjectSaveData> _childStructs;
	}
}
