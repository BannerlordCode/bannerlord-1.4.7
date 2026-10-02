using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EE RID: 238
	[NullableContext(1)]
	[Nullable(0)]
	internal class XmlDocumentTypeWrapper : XmlNodeWrapper, IXmlDocumentType, IXmlNode
	{
		// Token: 0x06000C86 RID: 3206 RVA: 0x000321C5 File Offset: 0x000303C5
		public XmlDocumentTypeWrapper(XmlDocumentType documentType)
			: base(documentType)
		{
			this._documentType = documentType;
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x000321D5 File Offset: 0x000303D5
		public string Name
		{
			get
			{
				return this._documentType.Name;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x000321E2 File Offset: 0x000303E2
		public string System
		{
			get
			{
				return this._documentType.SystemId;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x000321EF File Offset: 0x000303EF
		public string Public
		{
			get
			{
				return this._documentType.PublicId;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x000321FC File Offset: 0x000303FC
		public string InternalSubset
		{
			get
			{
				return this._documentType.InternalSubset;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x00032209 File Offset: 0x00030409
		[Nullable(2)]
		public override string LocalName
		{
			[NullableContext(2)]
			get
			{
				return "DOCTYPE";
			}
		}

		// Token: 0x040003E7 RID: 999
		private readonly XmlDocumentType _documentType;
	}
}
