using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x0200003F RID: 63
	public class ObjectLoadData
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000C126 File Offset: 0x0000A326
		// (set) Token: 0x0600027D RID: 637 RVA: 0x0000C12E File Offset: 0x0000A32E
		public int Id { get; private set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000C137 File Offset: 0x0000A337
		// (set) Token: 0x0600027F RID: 639 RVA: 0x0000C13F File Offset: 0x0000A33F
		public object Target { get; private set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000C148 File Offset: 0x0000A348
		// (set) Token: 0x06000281 RID: 641 RVA: 0x0000C150 File Offset: 0x0000A350
		public LoadContext Context { get; private set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000C159 File Offset: 0x0000A359
		// (set) Token: 0x06000283 RID: 643 RVA: 0x0000C161 File Offset: 0x0000A361
		public TypeDefinition TypeDefinition { get; private set; }

		// Token: 0x06000284 RID: 644 RVA: 0x0000C16C File Offset: 0x0000A36C
		public object GetDataBySaveId(int localSaveId)
		{
			MemberLoadData memberLoadData = this._memberValues.SingleOrDefault<MemberLoadData>((MemberLoadData value) => (int)value.MemberSaveId.LocalSaveId == localSaveId);
			if (memberLoadData != null)
			{
				return memberLoadData.GetDataToUse();
			}
			return null;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000C1AC File Offset: 0x0000A3AC
		public object GetMemberValueBySaveId(int localSaveId, int typeLevel)
		{
			MemberLoadData memberLoadData = this._memberValues.SingleOrDefault<MemberLoadData>((MemberLoadData value) => (int)value.MemberSaveId.LocalSaveId == localSaveId && (int)value.MemberSaveId.TypeLevel == typeLevel);
			if (memberLoadData == null)
			{
				return null;
			}
			return memberLoadData.GetDataToUse();
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000C1F0 File Offset: 0x0000A3F0
		public object GetMemberValueBySaveId(int localSaveId)
		{
			MemberLoadData memberLoadData = this._memberValues.SingleOrDefault<MemberLoadData>((MemberLoadData value) => (int)value.MemberSaveId.LocalSaveId == localSaveId);
			if (memberLoadData == null)
			{
				return null;
			}
			return memberLoadData.GetDataToUse();
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000C22C File Offset: 0x0000A42C
		public object GetFieldValueBySaveId(int localSaveId)
		{
			FieldLoadData fieldLoadData = this._fieldValues.SingleOrDefault<FieldLoadData>((FieldLoadData value) => (int)value.MemberSaveId.LocalSaveId == localSaveId);
			if (fieldLoadData == null)
			{
				return null;
			}
			return fieldLoadData.GetDataToUse();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000C268 File Offset: 0x0000A468
		public object GetPropertyValueBySaveId(int localSaveId)
		{
			PropertyLoadData propertyLoadData = this._propertyValues.SingleOrDefault<PropertyLoadData>((PropertyLoadData value) => (int)value.MemberSaveId.LocalSaveId == localSaveId);
			if (propertyLoadData == null)
			{
				return null;
			}
			return propertyLoadData.GetDataToUse();
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000C2A4 File Offset: 0x0000A4A4
		public bool HasMember(int localSaveId)
		{
			return this._memberValues.Any<MemberLoadData>((MemberLoadData x) => (int)x.MemberSaveId.LocalSaveId == localSaveId);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000C2D8 File Offset: 0x0000A4D8
		public bool HasMember(int localSaveId, int typeLevel)
		{
			return this._memberValues.Any<MemberLoadData>((MemberLoadData x) => (int)x.MemberSaveId.LocalSaveId == localSaveId && (int)x.MemberSaveId.TypeLevel == typeLevel);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000C310 File Offset: 0x0000A510
		public ObjectLoadData(LoadContext context, int id)
		{
			this.Context = context;
			this.Id = id;
			this._propertyValues = new List<PropertyLoadData>();
			this._fieldValues = new List<FieldLoadData>();
			this._memberValues = new List<MemberLoadData>();
			this._childStructs = new List<ObjectLoadData>();
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000C360 File Offset: 0x0000A560
		public ObjectLoadData(ObjectHeaderLoadData headerLoadData)
		{
			this.Id = headerLoadData.Id;
			this.Target = headerLoadData.Target;
			this.Context = headerLoadData.Context;
			this.TypeDefinition = headerLoadData.TypeDefinition;
			this._propertyValues = new List<PropertyLoadData>();
			this._fieldValues = new List<FieldLoadData>();
			this._memberValues = new List<MemberLoadData>();
			this._childStructs = new List<ObjectLoadData>();
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000C3D0 File Offset: 0x0000A5D0
		public void InitializeReaders(SaveEntryFolder saveEntryFolder)
		{
			BinaryReader binaryReader = saveEntryFolder.GetEntry(new EntryId(-1, SaveEntryExtension.Basics)).GetBinaryReader();
			this._saveId = SaveId.ReadSaveIdFrom(binaryReader);
			this._propertyCount = binaryReader.ReadShort();
			this._childStructCount = binaryReader.ReadShort();
			for (int i = 0; i < (int)this._childStructCount; i++)
			{
				ObjectLoadData objectLoadData = new ObjectLoadData(this.Context, i);
				this._childStructs.Add(objectLoadData);
			}
			foreach (SaveEntry saveEntry in saveEntryFolder.ChildEntries)
			{
				if (saveEntry.Id.Extension == SaveEntryExtension.Property)
				{
					BinaryReader binaryReader2 = saveEntry.GetBinaryReader();
					PropertyLoadData propertyLoadData = new PropertyLoadData(this, binaryReader2);
					this._propertyValues.Add(propertyLoadData);
					this._memberValues.Add(propertyLoadData);
				}
				else if (saveEntry.Id.Extension == SaveEntryExtension.Field)
				{
					BinaryReader binaryReader3 = saveEntry.GetBinaryReader();
					FieldLoadData fieldLoadData = new FieldLoadData(this, binaryReader3);
					this._fieldValues.Add(fieldLoadData);
					this._memberValues.Add(fieldLoadData);
				}
			}
			for (int j = 0; j < (int)this._childStructCount; j++)
			{
				ObjectLoadData objectLoadData2 = this._childStructs[j];
				SaveEntryFolder childFolder = saveEntryFolder.GetChildFolder(new FolderId(j, SaveFolderExtension.Struct));
				objectLoadData2.InitializeReaders(childFolder);
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000C540 File Offset: 0x0000A740
		public void CreateStruct()
		{
			this.TypeDefinition = this.Context.DefinitionContext.TryGetTypeDefinition(this._saveId) as TypeDefinition;
			if (this.TypeDefinition != null)
			{
				Type type = this.TypeDefinition.Type;
				this.Target = FormatterServices.GetUninitializedObject(type);
			}
			foreach (ObjectLoadData objectLoadData in this._childStructs)
			{
				objectLoadData.CreateStruct();
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000C5D4 File Offset: 0x0000A7D4
		public void FillCreatedObject()
		{
			foreach (ObjectLoadData objectLoadData in this._childStructs)
			{
				objectLoadData.CreateStruct();
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000C624 File Offset: 0x0000A824
		public void Read()
		{
			foreach (ObjectLoadData objectLoadData in this._childStructs)
			{
				objectLoadData.Read();
			}
			foreach (MemberLoadData memberLoadData in this._memberValues)
			{
				memberLoadData.Read();
				if (memberLoadData.SavedMemberType == SavedMemberType.CustomStruct)
				{
					int num = (int)memberLoadData.Data;
					object target = this._childStructs[num].Target;
					memberLoadData.SetCustomStructData(target);
				}
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000C6E8 File Offset: 0x0000A8E8
		public void FillObject()
		{
			foreach (ObjectLoadData objectLoadData in this._childStructs)
			{
				objectLoadData.FillObject();
			}
			foreach (FieldLoadData fieldLoadData in this._fieldValues)
			{
				fieldLoadData.FillObject();
			}
			foreach (PropertyLoadData propertyLoadData in this._propertyValues)
			{
				propertyLoadData.FillObject();
			}
		}

		// Token: 0x040000C7 RID: 199
		private short _propertyCount;

		// Token: 0x040000C8 RID: 200
		private List<PropertyLoadData> _propertyValues;

		// Token: 0x040000C9 RID: 201
		private List<FieldLoadData> _fieldValues;

		// Token: 0x040000CA RID: 202
		private List<MemberLoadData> _memberValues;

		// Token: 0x040000CB RID: 203
		private SaveId _saveId;

		// Token: 0x040000CC RID: 204
		private List<ObjectLoadData> _childStructs;

		// Token: 0x040000CD RID: 205
		private short _childStructCount;
	}
}
