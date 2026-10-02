using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000F0 RID: 240
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
	public sealed class AspMvcActionAttribute : Attribute
	{
		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x0000F75C File Offset: 0x0000D95C
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x0000F764 File Offset: 0x0000D964
		[UsedImplicitly]
		public string AnonymousProperty { get; private set; }

		// Token: 0x06000929 RID: 2345 RVA: 0x0000F76D File Offset: 0x0000D96D
		public AspMvcActionAttribute()
		{
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0000F775 File Offset: 0x0000D975
		public AspMvcActionAttribute(string anonymousProperty)
		{
			this.AnonymousProperty = anonymousProperty;
		}
	}
}
