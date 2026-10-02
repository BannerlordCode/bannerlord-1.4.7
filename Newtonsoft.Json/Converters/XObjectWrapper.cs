using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FC RID: 252
	[NullableContext(2)]
	[Nullable(0)]
	internal class XObjectWrapper : IXmlNode
	{
		// Token: 0x06000CF3 RID: 3315 RVA: 0x00032996 File Offset: 0x00030B96
		public XObjectWrapper(XObject xmlObject)
		{
			this._xmlObject = xmlObject;
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x000329A5 File Offset: 0x00030BA5
		public object WrappedNode
		{
			get
			{
				return this._xmlObject;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x000329AD File Offset: 0x00030BAD
		public virtual XmlNodeType NodeType
		{
			get
			{
				XObject xmlObject = this._xmlObject;
				if (xmlObject == null)
				{
					return XmlNodeType.None;
				}
				return xmlObject.NodeType;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x000329C0 File Offset: 0x00030BC0
		public virtual string LocalName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x000329C3 File Offset: 0x00030BC3
		[Nullable(1)]
		public virtual List<IXmlNode> ChildNodes
		{
			[NullableContext(1)]
			get
			{
				return XmlNodeConverter.EmptyChildNodes;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x000329CA File Offset: 0x00030BCA
		[Nullable(1)]
		public virtual List<IXmlNode> Attributes
		{
			[NullableContext(1)]
			get
			{
				return XmlNodeConverter.EmptyChildNodes;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x000329D1 File Offset: 0x00030BD1
		public virtual IXmlNode ParentNode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x000329D4 File Offset: 0x00030BD4
		// (set) Token: 0x06000CFB RID: 3323 RVA: 0x000329D7 File Offset: 0x00030BD7
		public virtual string Value
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x000329DE File Offset: 0x00030BDE
		[NullableContext(1)]
		public virtual IXmlNode AppendChild(IXmlNode newChild)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x000329E5 File Offset: 0x00030BE5
		public virtual string NamespaceUri
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040003EE RID: 1006
		private readonly XObject _xmlObject;
	}
}
