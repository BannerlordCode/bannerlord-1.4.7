using System;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x02000038 RID: 56
	internal class FieldLoadData : MemberLoadData
	{
		// Token: 0x0600023A RID: 570 RVA: 0x0000B252 File Offset: 0x00009452
		public FieldLoadData(ObjectLoadData objectLoadData, IReader reader)
			: base(objectLoadData, reader)
		{
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000B25C File Offset: 0x0000945C
		public void FillObject()
		{
			FieldDefinition fieldDefinitionWithId;
			if (base.ObjectLoadData.TypeDefinition == null || (fieldDefinitionWithId = base.ObjectLoadData.TypeDefinition.GetFieldDefinitionWithId(this.GetMemberTypeId())) == null)
			{
				return;
			}
			FieldInfo fieldInfo = fieldDefinitionWithId.FieldInfo;
			object target = base.ObjectLoadData.Target;
			object dataToUse = base.GetDataToUse();
			if (dataToUse != null && !fieldInfo.FieldType.IsInstanceOfType(dataToUse) && !LoadContext.TryConvertType(dataToUse.GetType(), fieldInfo.FieldType, ref dataToUse))
			{
				return;
			}
			fieldInfo.SetValue(target, dataToUse);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000B2DC File Offset: 0x000094DC
		private MemberTypeId GetMemberTypeId()
		{
			MemberTypeId memberSaveId = base.MemberSaveId;
			base.Context.DefinitionContext.GetConflictedFieldMemberTypeId(base.ObjectLoadData.TypeDefinition, ref memberSaveId);
			return memberSaveId;
		}
	}
}
