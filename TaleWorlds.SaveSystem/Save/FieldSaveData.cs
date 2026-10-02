using System;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x02000028 RID: 40
	internal class FieldSaveData : MemberSaveData
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00007D30 File Offset: 0x00005F30
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00007D38 File Offset: 0x00005F38
		public FieldDefinition FieldDefinition { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00007D41 File Offset: 0x00005F41
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00007D49 File Offset: 0x00005F49
		public MemberTypeId SaveId { get; private set; }

		// Token: 0x0600017C RID: 380 RVA: 0x00007D52 File Offset: 0x00005F52
		public FieldSaveData(ObjectSaveData objectSaveData, FieldDefinition fieldDefinition, MemberTypeId saveId)
			: base(objectSaveData)
		{
			this.FieldDefinition = fieldDefinition;
			this.SaveId = saveId;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007D6C File Offset: 0x00005F6C
		public override void Initialize(TypeDefinitionBase typeDefinition)
		{
			object value = this.FieldDefinition.GetValue(base.ObjectSaveData.Target);
			Type fieldType = this.FieldDefinition.FieldInfo.FieldType;
			base.InitializeData(this.SaveId, fieldType, typeDefinition, value);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007DB0 File Offset: 0x00005FB0
		public override void InitializeAsCustomStruct(int structId)
		{
			base.InitializeDataAsCustomStruct(this.SaveId, structId, base.TypeDefinition);
		}
	}
}
