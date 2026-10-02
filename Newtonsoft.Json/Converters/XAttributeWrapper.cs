using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FD RID: 253
	[NullableContext(2)]
	[Nullable(0)]
	internal class XAttributeWrapper : XObjectWrapper
	{
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x000329E8 File Offset: 0x00030BE8
		[Nullable(1)]
		private XAttribute Attribute
		{
			[NullableContext(1)]
			get
			{
				return (XAttribute)base.WrappedNode;
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x000329F5 File Offset: 0x00030BF5
		[NullableContext(1)]
		public XAttributeWrapper(XAttribute attribute)
			: base(attribute)
		{
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x000329FE File Offset: 0x00030BFE
		// (set) Token: 0x06000D01 RID: 3329 RVA: 0x00032A0B File Offset: 0x00030C0B
		public override string Value
		{
			get
			{
				return this.Attribute.Value;
			}
			set
			{
				this.Attribute.Value = value;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00032A19 File Offset: 0x00030C19
		public override string LocalName
		{
			get
			{
				return this.Attribute.Name.LocalName;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x00032A2B File Offset: 0x00030C2B
		public override string NamespaceUri
		{
			get
			{
				return this.Attribute.Name.NamespaceName;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x00032A3D File Offset: 0x00030C3D
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Attribute.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Attribute.Parent);
			}
		}
	}
}
