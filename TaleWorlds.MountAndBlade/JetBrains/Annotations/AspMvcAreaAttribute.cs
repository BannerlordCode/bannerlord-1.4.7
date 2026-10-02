using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000F1 RID: 241
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class AspMvcAreaAttribute : PathReferenceAttribute
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x0000F784 File Offset: 0x0000D984
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x0000F78C File Offset: 0x0000D98C
		[UsedImplicitly]
		public string AnonymousProperty { get; private set; }

		// Token: 0x0600092D RID: 2349 RVA: 0x0000F795 File Offset: 0x0000D995
		[UsedImplicitly]
		public AspMvcAreaAttribute()
		{
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0000F79D File Offset: 0x0000D99D
		public AspMvcAreaAttribute(string anonymousProperty)
		{
			this.AnonymousProperty = anonymousProperty;
		}
	}
}
