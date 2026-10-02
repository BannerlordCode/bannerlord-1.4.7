using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F3 RID: 243
	[NullableContext(1)]
	internal interface IXmlElement : IXmlNode
	{
		// Token: 0x06000CB0 RID: 3248
		void SetAttributeNode(IXmlNode attribute);

		// Token: 0x06000CB1 RID: 3249
		string GetPrefixOfNamespace(string namespaceUri);

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000CB2 RID: 3250
		bool IsEmpty { get; }
	}
}
