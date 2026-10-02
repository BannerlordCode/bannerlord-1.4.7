using System;
using System.Reflection;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000061 RID: 97
	public class FieldDefinition : MemberDefinition
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000E598 File Offset: 0x0000C798
		// (set) Token: 0x06000342 RID: 834 RVA: 0x0000E5A0 File Offset: 0x0000C7A0
		public FieldInfo FieldInfo { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000E5A9 File Offset: 0x0000C7A9
		// (set) Token: 0x06000344 RID: 836 RVA: 0x0000E5B1 File Offset: 0x0000C7B1
		public SaveableFieldAttribute SaveableFieldAttribute { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000E5BA File Offset: 0x0000C7BA
		// (set) Token: 0x06000346 RID: 838 RVA: 0x0000E5C2 File Offset: 0x0000C7C2
		public GetFieldValueDelegate GetFieldValueMethod { get; private set; }

		// Token: 0x06000347 RID: 839 RVA: 0x0000E5CB File Offset: 0x0000C7CB
		public FieldDefinition(FieldInfo fieldInfo, MemberTypeId id)
			: base(fieldInfo, id)
		{
			this.FieldInfo = fieldInfo;
			this.SaveableFieldAttribute = fieldInfo.GetCustomAttribute<SaveableFieldAttribute>();
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000E5E8 File Offset: 0x0000C7E8
		public override Type GetMemberType()
		{
			return this.FieldInfo.FieldType;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000E5F8 File Offset: 0x0000C7F8
		public override object GetValue(object target)
		{
			object obj;
			if (this.GetFieldValueMethod != null)
			{
				obj = this.GetFieldValueMethod(target);
			}
			else
			{
				obj = this.FieldInfo.GetValue(target);
			}
			return obj;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000E62A File Offset: 0x0000C82A
		public void InitializeForAutoGeneration(GetFieldValueDelegate getFieldValueMethod)
		{
			this.GetFieldValueMethod = getFieldValueMethod;
		}
	}
}
