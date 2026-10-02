using System;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x0200002D RID: 45
	internal class PropertySaveData : MemberSaveData
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00009567 File Offset: 0x00007767
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x0000956F File Offset: 0x0000776F
		public PropertyDefinition PropertyDefinition { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00009578 File Offset: 0x00007778
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x00009580 File Offset: 0x00007780
		public MemberTypeId SaveId { get; private set; }

		// Token: 0x060001C8 RID: 456 RVA: 0x00009589 File Offset: 0x00007789
		public PropertySaveData(ObjectSaveData objectSaveData, PropertyDefinition propertyDefinition, MemberTypeId saveId)
			: base(objectSaveData)
		{
			this.PropertyDefinition = propertyDefinition;
			this.SaveId = saveId;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000095A0 File Offset: 0x000077A0
		public override void Initialize(TypeDefinitionBase typeDefinition)
		{
			object value = this.PropertyDefinition.GetValue(base.ObjectSaveData.Target);
			base.InitializeData(this.SaveId, this.PropertyDefinition.PropertyInfo.PropertyType, typeDefinition, value);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000095E2 File Offset: 0x000077E2
		public override void InitializeAsCustomStruct(int structId)
		{
			base.InitializeDataAsCustomStruct(this.SaveId, structId, base.TypeDefinition);
		}
	}
}
