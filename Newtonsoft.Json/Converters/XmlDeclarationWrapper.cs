using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000ED RID: 237
	[NullableContext(1)]
	[Nullable(0)]
	internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x06000C80 RID: 3200 RVA: 0x00032172 File Offset: 0x00030372
		public XmlDeclarationWrapper(XmlDeclaration declaration)
			: base(declaration)
		{
			this._declaration = declaration;
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x00032182 File Offset: 0x00030382
		public string Version
		{
			get
			{
				return this._declaration.Version;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x0003218F File Offset: 0x0003038F
		// (set) Token: 0x06000C83 RID: 3203 RVA: 0x0003219C File Offset: 0x0003039C
		public string Encoding
		{
			get
			{
				return this._declaration.Encoding;
			}
			set
			{
				this._declaration.Encoding = value;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x000321AA File Offset: 0x000303AA
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x000321B7 File Offset: 0x000303B7
		public string Standalone
		{
			get
			{
				return this._declaration.Standalone;
			}
			set
			{
				this._declaration.Standalone = value;
			}
		}

		// Token: 0x040003E6 RID: 998
		private readonly XmlDeclaration _declaration;
	}
}
