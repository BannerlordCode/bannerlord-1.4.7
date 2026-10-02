using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F4 RID: 244
	[NullableContext(2)]
	internal interface IXmlNode
	{
		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000CB3 RID: 3251
		XmlNodeType NodeType { get; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000CB4 RID: 3252
		string LocalName { get; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000CB5 RID: 3253
		[Nullable(1)]
		List<IXmlNode> ChildNodes
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000CB6 RID: 3254
		[Nullable(1)]
		List<IXmlNode> Attributes
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000CB7 RID: 3255
		IXmlNode ParentNode { get; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000CB8 RID: 3256
		// (set) Token: 0x06000CB9 RID: 3257
		string Value { get; set; }

		// Token: 0x06000CBA RID: 3258
		[NullableContext(1)]
		IXmlNode AppendChild(IXmlNode newChild);

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000CBB RID: 3259
		string NamespaceUri { get; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000CBC RID: 3260
		object WrappedNode { get; }
	}
}
