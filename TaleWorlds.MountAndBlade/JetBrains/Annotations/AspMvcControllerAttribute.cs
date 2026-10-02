using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000F2 RID: 242
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
	public sealed class AspMvcControllerAttribute : Attribute
	{
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x0000F7AC File Offset: 0x0000D9AC
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x0000F7B4 File Offset: 0x0000D9B4
		[UsedImplicitly]
		public string AnonymousProperty { get; private set; }

		// Token: 0x06000931 RID: 2353 RVA: 0x0000F7BD File Offset: 0x0000D9BD
		public AspMvcControllerAttribute()
		{
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0000F7C5 File Offset: 0x0000D9C5
		public AspMvcControllerAttribute(string anonymousProperty)
		{
			this.AnonymousProperty = anonymousProperty;
		}
	}
}
