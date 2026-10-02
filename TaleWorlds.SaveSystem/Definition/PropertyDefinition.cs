using System;
using System.Reflection;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x02000067 RID: 103
	public class PropertyDefinition : MemberDefinition
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000E971 File Offset: 0x0000CB71
		// (set) Token: 0x06000368 RID: 872 RVA: 0x0000E979 File Offset: 0x0000CB79
		public PropertyInfo PropertyInfo { get; private set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000369 RID: 873 RVA: 0x0000E982 File Offset: 0x0000CB82
		// (set) Token: 0x0600036A RID: 874 RVA: 0x0000E98A File Offset: 0x0000CB8A
		public SaveablePropertyAttribute SaveablePropertyAttribute { get; private set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0000E993 File Offset: 0x0000CB93
		// (set) Token: 0x0600036C RID: 876 RVA: 0x0000E99B File Offset: 0x0000CB9B
		public MethodInfo GetMethod { get; private set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0000E9A4 File Offset: 0x0000CBA4
		// (set) Token: 0x0600036E RID: 878 RVA: 0x0000E9AC File Offset: 0x0000CBAC
		public MethodInfo SetMethod { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0000E9B5 File Offset: 0x0000CBB5
		// (set) Token: 0x06000370 RID: 880 RVA: 0x0000E9BD File Offset: 0x0000CBBD
		public GetPropertyValueDelegate GetPropertyValueMethod { get; private set; }

		// Token: 0x06000371 RID: 881 RVA: 0x0000E9C8 File Offset: 0x0000CBC8
		public PropertyDefinition(PropertyInfo propertyInfo, MemberTypeId id)
			: base(propertyInfo, id)
		{
			this.PropertyInfo = propertyInfo;
			this.SaveablePropertyAttribute = propertyInfo.GetCustomAttribute<SaveablePropertyAttribute>();
			this.SetMethod = this.PropertyInfo.GetSetMethod(true);
			if (this.SetMethod == null && this.PropertyInfo.DeclaringType != null)
			{
				PropertyInfo property = this.PropertyInfo.DeclaringType.GetProperty(this.PropertyInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null)
				{
					this.SetMethod = property.GetSetMethod(true);
				}
			}
			if (this.SetMethod == null)
			{
				Debug.FailedAssert(string.Concat(new string[]
				{
					"Property ",
					this.PropertyInfo.Name,
					" at Type ",
					this.PropertyInfo.DeclaringType.FullName,
					" does not have setter method."
				}), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Definition\\PropertyDefinition.cs", ".ctor", 39);
				throw new Exception(string.Concat(new string[]
				{
					"Property ",
					this.PropertyInfo.Name,
					" at Type ",
					this.PropertyInfo.DeclaringType.FullName,
					" does not have setter method."
				}));
			}
			this.GetMethod = this.PropertyInfo.GetGetMethod(true);
			if (this.GetMethod == null && this.PropertyInfo.DeclaringType != null)
			{
				PropertyInfo property2 = this.PropertyInfo.DeclaringType.GetProperty(this.PropertyInfo.Name);
				if (property2 != null)
				{
					this.GetMethod = property2.GetGetMethod(true);
				}
			}
			if (this.GetMethod == null)
			{
				throw new Exception(string.Concat(new string[]
				{
					"Property ",
					this.PropertyInfo.Name,
					" at Type ",
					this.PropertyInfo.DeclaringType.FullName,
					" does not have getter method."
				}));
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000EBC4 File Offset: 0x0000CDC4
		public override Type GetMemberType()
		{
			return this.PropertyInfo.PropertyType;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000EBD4 File Offset: 0x0000CDD4
		public override object GetValue(object target)
		{
			object obj;
			if (this.GetPropertyValueMethod != null)
			{
				obj = this.GetPropertyValueMethod(target);
			}
			else
			{
				obj = this.GetMethod.Invoke(target, new object[0]);
			}
			return obj;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000EC0C File Offset: 0x0000CE0C
		public void InitializeForAutoGeneration(GetPropertyValueDelegate getPropertyValueMethod)
		{
			this.GetPropertyValueMethod = getPropertyValueMethod;
		}
	}
}
