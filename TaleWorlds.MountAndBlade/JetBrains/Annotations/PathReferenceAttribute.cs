using System;

namespace JetBrains.Annotations
{
	// Token: 0x020000EF RID: 239
	[AttributeUsage(AttributeTargets.Parameter)]
	public class PathReferenceAttribute : Attribute
	{
		// Token: 0x06000923 RID: 2339 RVA: 0x0000F734 File Offset: 0x0000D934
		public PathReferenceAttribute()
		{
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0000F73C File Offset: 0x0000D93C
		[UsedImplicitly]
		public PathReferenceAttribute([PathReference] string basePath)
		{
			this.BasePath = basePath;
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x0000F74B File Offset: 0x0000D94B
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x0000F753 File Offset: 0x0000D953
		[UsedImplicitly]
		public string BasePath { get; private set; }
	}
}
