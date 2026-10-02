using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000E9 RID: 233
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class MeansImplicitUseAttribute : Attribute
	{
		// Token: 0x06000917 RID: 2327 RVA: 0x0000F6BE File Offset: 0x0000D8BE
		[UsedImplicitly]
		public MeansImplicitUseAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0000F6C8 File Offset: 0x0000D8C8
		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			this.UseKindFlags = useKindFlags;
			this.TargetFlags = targetFlags;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0000F6DE File Offset: 0x0000D8DE
		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0000F6E8 File Offset: 0x0000D8E8
		[UsedImplicitly]
		public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x0000F6F2 File Offset: 0x0000D8F2
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x0000F6FA File Offset: 0x0000D8FA
		[UsedImplicitly]
		public ImplicitUseKindFlags UseKindFlags { get; private set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x0000F703 File Offset: 0x0000D903
		// (set) Token: 0x0600091E RID: 2334 RVA: 0x0000F70B File Offset: 0x0000D90B
		[UsedImplicitly]
		public ImplicitUseTargetFlags TargetFlags { get; private set; }
	}
}
