using System;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Definition;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x02000040 RID: 64
	internal class PropertyLoadData : MemberLoadData
	{
		// Token: 0x06000292 RID: 658 RVA: 0x0000C7B8 File Offset: 0x0000A9B8
		public PropertyLoadData(ObjectLoadData objectLoadData, IReader reader)
			: base(objectLoadData, reader)
		{
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000C7C4 File Offset: 0x0000A9C4
		public void FillObject()
		{
			PropertyDefinition propertyDefinitionWithId;
			if (base.ObjectLoadData.TypeDefinition == null || (propertyDefinitionWithId = base.ObjectLoadData.TypeDefinition.GetPropertyDefinitionWithId(this.GetMemberTypeId())) == null)
			{
				return;
			}
			MethodInfo setMethod = propertyDefinitionWithId.SetMethod;
			object target = base.ObjectLoadData.Target;
			object dataToUse = base.GetDataToUse();
			if (dataToUse != null && !propertyDefinitionWithId.PropertyInfo.PropertyType.IsInstanceOfType(dataToUse) && !LoadContext.TryConvertType(dataToUse.GetType(), propertyDefinitionWithId.PropertyInfo.PropertyType, ref dataToUse))
			{
				return;
			}
			setMethod.Invoke(target, new object[] { dataToUse });
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000C858 File Offset: 0x0000AA58
		private MemberTypeId GetMemberTypeId()
		{
			MemberTypeId memberSaveId = base.MemberSaveId;
			base.Context.DefinitionContext.GetConflictedPropertyMemberTypeId(base.ObjectLoadData.TypeDefinition, ref memberSaveId);
			return memberSaveId;
		}
	}
}
