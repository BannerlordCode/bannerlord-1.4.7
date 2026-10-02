using System;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x02000027 RID: 39
	internal class ElementSaveData : VariableSaveData
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00007C8B File Offset: 0x00005E8B
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00007C93 File Offset: 0x00005E93
		public object ElementValue { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00007C9C File Offset: 0x00005E9C
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00007CA4 File Offset: 0x00005EA4
		public int ElementIndex { get; private set; }

		// Token: 0x06000177 RID: 375 RVA: 0x00007CB0 File Offset: 0x00005EB0
		public ElementSaveData(ContainerSaveData containerSaveData, object value, int index)
			: base(containerSaveData.Context)
		{
			this.ElementValue = value;
			this.ElementIndex = index;
			if (value == null)
			{
				base.InitializeDataAsNullObject(MemberTypeId.Invalid);
				return;
			}
			TypeDefinitionBase typeDefinition = containerSaveData.Context.DefinitionContext.GetTypeDefinition(value.GetType());
			TypeDefinition typeDefinition2 = typeDefinition as TypeDefinition;
			if (typeDefinition2 != null && !typeDefinition2.IsClassDefinition)
			{
				base.InitializeDataAsCustomStruct(MemberTypeId.Invalid, index, typeDefinition);
				return;
			}
			base.InitializeData(MemberTypeId.Invalid, value.GetType(), typeDefinition, value);
		}
	}
}
