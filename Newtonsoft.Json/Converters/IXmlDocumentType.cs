using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F2 RID: 242
	[NullableContext(1)]
	internal interface IXmlDocumentType : IXmlNode
	{
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000CAC RID: 3244
		string Name { get; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000CAD RID: 3245
		string System { get; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000CAE RID: 3246
		string Public { get; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000CAF RID: 3247
		string InternalSubset { get; }
	}
}
