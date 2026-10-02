using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F1 RID: 241
	[NullableContext(1)]
	internal interface IXmlDeclaration : IXmlNode
	{
		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000CA7 RID: 3239
		string Version { get; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000CA8 RID: 3240
		// (set) Token: 0x06000CA9 RID: 3241
		string Encoding { get; set; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000CAA RID: 3242
		// (set) Token: 0x06000CAB RID: 3243
		string Standalone { get; set; }
	}
}
