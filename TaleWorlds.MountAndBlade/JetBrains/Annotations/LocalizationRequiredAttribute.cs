using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000DD RID: 221
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class LocalizationRequiredAttribute : Attribute
	{
		// Token: 0x060008FB RID: 2299 RVA: 0x0000F57E File Offset: 0x0000D77E
		public LocalizationRequiredAttribute(bool required)
		{
			this.Required = required;
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x0000F58D File Offset: 0x0000D78D
		// (set) Token: 0x060008FD RID: 2301 RVA: 0x0000F595 File Offset: 0x0000D795
		[UsedImplicitly]
		public bool Required { get; set; }

		// Token: 0x060008FE RID: 2302 RVA: 0x0000F5A0 File Offset: 0x0000D7A0
		public override bool Equals(object obj)
		{
			LocalizationRequiredAttribute localizationRequiredAttribute = obj as LocalizationRequiredAttribute;
			return localizationRequiredAttribute != null && localizationRequiredAttribute.Required == this.Required;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0000F5C7 File Offset: 0x0000D7C7
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
