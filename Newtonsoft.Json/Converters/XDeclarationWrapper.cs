using System;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F5 RID: 245
	[NullableContext(1)]
	[Nullable(0)]
	internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
	{
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x000324D2 File Offset: 0x000306D2
		internal XDeclaration Declaration { get; }

		// Token: 0x06000CBE RID: 3262 RVA: 0x000324DA File Offset: 0x000306DA
		public XDeclarationWrapper(XDeclaration declaration)
			: base(null)
		{
			this.Declaration = declaration;
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x000324EA File Offset: 0x000306EA
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.XmlDeclaration;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x000324EE File Offset: 0x000306EE
		public string Version
		{
			get
			{
				return this.Declaration.Version;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x000324FB File Offset: 0x000306FB
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x00032508 File Offset: 0x00030708
		public string Encoding
		{
			get
			{
				return this.Declaration.Encoding;
			}
			set
			{
				this.Declaration.Encoding = value;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x00032516 File Offset: 0x00030716
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x00032523 File Offset: 0x00030723
		public string Standalone
		{
			get
			{
				return this.Declaration.Standalone;
			}
			set
			{
				this.Declaration.Standalone = value;
			}
		}
	}
}
