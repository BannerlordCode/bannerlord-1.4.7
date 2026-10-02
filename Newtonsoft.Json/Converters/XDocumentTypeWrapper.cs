using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F6 RID: 246
	[NullableContext(1)]
	[Nullable(0)]
	internal class XDocumentTypeWrapper : XObjectWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x06000CC5 RID: 3269 RVA: 0x00032531 File Offset: 0x00030731
		public XDocumentTypeWrapper(XDocumentType documentType)
			: base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x00032541 File Offset: 0x00030741
		public string Name
		{
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0003254E File Offset: 0x0003074E
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x0003255B File Offset: 0x0003075B
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x00032568 File Offset: 0x00030768
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x00032575 File Offset: 0x00030775
		[Nullable(2)]
		public override string LocalName
		{
			[NullableContext(2)]
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x040003EC RID: 1004
		private readonly XDocumentType _documentType;
	}
}
