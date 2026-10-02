using System;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x02000031 RID: 49
	internal abstract class VariableSaveData
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000A590 File Offset: 0x00008790
		// (set) Token: 0x06000200 RID: 512 RVA: 0x0000A598 File Offset: 0x00008798
		public ISaveContext Context { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000A5A1 File Offset: 0x000087A1
		// (set) Token: 0x06000202 RID: 514 RVA: 0x0000A5A9 File Offset: 0x000087A9
		public SavedMemberType MemberType { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000A5B2 File Offset: 0x000087B2
		// (set) Token: 0x06000204 RID: 516 RVA: 0x0000A5BA File Offset: 0x000087BA
		public object Value { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000A5C3 File Offset: 0x000087C3
		// (set) Token: 0x06000206 RID: 518 RVA: 0x0000A5CB File Offset: 0x000087CB
		public MemberTypeId MemberSaveId { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000A5D4 File Offset: 0x000087D4
		// (set) Token: 0x06000208 RID: 520 RVA: 0x0000A5DC File Offset: 0x000087DC
		public TypeDefinitionBase TypeDefinition { get; private set; }

		// Token: 0x06000209 RID: 521 RVA: 0x0000A5E5 File Offset: 0x000087E5
		protected VariableSaveData(ISaveContext context)
		{
			this.Context = context;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000A5F4 File Offset: 0x000087F4
		protected void InitializeDataAsNullObject(MemberTypeId memberSaveId)
		{
			this.MemberSaveId = memberSaveId;
			this.MemberType = SavedMemberType.Object;
			this.Value = -1;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000A610 File Offset: 0x00008810
		protected void InitializeDataAsCustomStruct(MemberTypeId memberSaveId, int structId, TypeDefinitionBase typeDefinition)
		{
			this.MemberSaveId = memberSaveId;
			this.MemberType = SavedMemberType.CustomStruct;
			this.Value = structId;
			this.TypeDefinition = typeDefinition;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000A634 File Offset: 0x00008834
		protected void InitializeData(MemberTypeId memberSaveId, Type memberType, TypeDefinitionBase definition, object data)
		{
			this.MemberSaveId = memberSaveId;
			this.TypeDefinition = definition;
			TypeDefinition typeDefinition = this.TypeDefinition as TypeDefinition;
			if (this.TypeDefinition is ContainerDefinition)
			{
				int num = -1;
				if (data != null)
				{
					num = this.Context.GetContainerId(data);
				}
				this.MemberType = SavedMemberType.Container;
				this.Value = num;
			}
			else if (typeof(string) == memberType)
			{
				this.MemberType = SavedMemberType.String;
				this.Value = data;
			}
			else if ((typeDefinition != null && typeDefinition.IsClassDefinition) || this.TypeDefinition is InterfaceDefinition || (this.TypeDefinition == null && memberType.IsInterface))
			{
				int num2 = -1;
				if (data != null)
				{
					num2 = this.Context.GetObjectId(data);
				}
				this.MemberType = SavedMemberType.Object;
				this.Value = num2;
			}
			else if (this.TypeDefinition is EnumDefinition)
			{
				this.MemberType = SavedMemberType.Enum;
				this.Value = data;
			}
			else if (this.TypeDefinition is BasicTypeDefinition)
			{
				this.MemberType = SavedMemberType.BasicType;
				this.Value = data;
			}
			else
			{
				this.MemberType = SavedMemberType.CustomStruct;
				this.Value = data;
			}
			if (this.TypeDefinition == null && !memberType.IsInterface)
			{
				string text = string.Format("Cant find definition for: {0}. Save id: {1}", memberType.Name, this.MemberSaveId);
				Debug.Print(text, 0, Debug.DebugColor.Red, 17592186044416UL);
				Debug.FailedAssert(text, "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Save\\VariableSaveData.cs", "InitializeData", 98);
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000A7A4 File Offset: 0x000089A4
		public void SaveTo(IWriter writer)
		{
			writer.WriteByte((byte)this.MemberType);
			writer.WriteByte(this.MemberSaveId.TypeLevel);
			writer.WriteShort(this.MemberSaveId.LocalSaveId);
			if (this.MemberType == SavedMemberType.Object)
			{
				writer.WriteInt((int)this.Value);
				return;
			}
			if (this.MemberType == SavedMemberType.Container)
			{
				writer.WriteInt((int)this.Value);
				return;
			}
			if (this.MemberType == SavedMemberType.String)
			{
				int stringId = this.Context.GetStringId((string)this.Value);
				writer.WriteInt(stringId);
				return;
			}
			if (this.MemberType == SavedMemberType.Enum)
			{
				this.TypeDefinition.SaveId.WriteTo(writer);
				writer.WriteString(this.Value.ToString());
				return;
			}
			if (this.MemberType == SavedMemberType.BasicType)
			{
				this.TypeDefinition.SaveId.WriteTo(writer);
				if (this.Context.DefinitionContext.TryGetTypeDefinition(this.TypeDefinition.SaveId) == null)
				{
					Debug.FailedAssert("Basic type definition cant be found: " + this.TypeDefinition.SaveId.GetStringId(), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Save\\VariableSaveData.cs", "SaveTo", 132);
				}
				((BasicTypeDefinition)this.TypeDefinition).Serializer.Serialize(writer, this.Value);
				return;
			}
			if (this.MemberType == SavedMemberType.CustomStruct)
			{
				writer.WriteInt((int)this.Value);
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000A908 File Offset: 0x00008B08
		public int GetDataSize()
		{
			int num = 4;
			if (this.MemberType == SavedMemberType.Object || this.MemberType == SavedMemberType.Container || this.MemberType == SavedMemberType.String || this.MemberType == SavedMemberType.CustomStruct)
			{
				num += 4;
			}
			else if (this.MemberType == SavedMemberType.Enum)
			{
				num += this.TypeDefinition.SaveId.GetSizeInBytes() + SaveContext.GetStringSizeInBytes(this.Value.ToString());
			}
			else if (this.MemberType == SavedMemberType.BasicType)
			{
				num += this.TypeDefinition.SaveId.GetSizeInBytes();
				BasicTypeDefinition basicTypeDefinition = (BasicTypeDefinition)this.TypeDefinition;
				num += basicTypeDefinition.Serializer.GetSizeInBytes();
			}
			return num;
		}
	}
}
