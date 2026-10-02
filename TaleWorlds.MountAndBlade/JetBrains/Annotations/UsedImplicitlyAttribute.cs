using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000E8 RID: 232
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public sealed class UsedImplicitlyAttribute : Attribute
	{
		// Token: 0x0600090F RID: 2319 RVA: 0x0000F668 File Offset: 0x0000D868
		[UsedImplicitly]
		public UsedImplicitlyAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0000F672 File Offset: 0x0000D872
		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0000F688 File Offset: 0x0000D888
		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0000F692 File Offset: 0x0000D892
		[UsedImplicitly]
		public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x0000F69C File Offset: 0x0000D89C
		// (set) Token: 0x06000914 RID: 2324 RVA: 0x0000F6A4 File Offset: 0x0000D8A4
		[UsedImplicitly]
		public ImplicitUseKindFlags UseKindFlags { get; private set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x0000F6AD File Offset: 0x0000D8AD
		// (set) Token: 0x06000916 RID: 2326 RVA: 0x0000F6B5 File Offset: 0x0000D8B5
		[UsedImplicitly]
		public ImplicitUseTargetFlags TargetFlags { get; private set; }
	}
}
