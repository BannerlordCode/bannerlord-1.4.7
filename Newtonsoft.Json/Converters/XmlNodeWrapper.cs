using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EF RID: 239
	[NullableContext(2)]
	[Nullable(0)]
	internal class XmlNodeWrapper : IXmlNode
	{
		// Token: 0x06000C8C RID: 3212 RVA: 0x00032210 File Offset: 0x00030410
		[NullableContext(1)]
		public XmlNodeWrapper(XmlNode node)
		{
			this._node = node;
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0003221F File Offset: 0x0003041F
		public object WrappedNode
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x00032227 File Offset: 0x00030427
		public XmlNodeType NodeType
		{
			get
			{
				return this._node.NodeType;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x00032234 File Offset: 0x00030434
		public virtual string LocalName
		{
			get
			{
				return this._node.LocalName;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x00032244 File Offset: 0x00030444
		[Nullable(1)]
		public List<IXmlNode> ChildNodes
		{
			[NullableContext(1)]
			get
			{
				if (this._childNodes == null)
				{
					if (!this._node.HasChildNodes)
					{
						this._childNodes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._childNodes = new List<IXmlNode>(this._node.ChildNodes.Count);
						foreach (object obj in this._node.ChildNodes)
						{
							XmlNode xmlNode = (XmlNode)obj;
							this._childNodes.Add(XmlNodeWrapper.WrapNode(xmlNode));
						}
					}
				}
				return this._childNodes;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x000322F4 File Offset: 0x000304F4
		protected virtual bool HasChildNodes
		{
			get
			{
				return this._node.HasChildNodes;
			}
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00032304 File Offset: 0x00030504
		[NullableContext(1)]
		internal static IXmlNode WrapNode(XmlNode node)
		{
			XmlNodeType nodeType = node.NodeType;
			if (nodeType == XmlNodeType.Element)
			{
				return new XmlElementWrapper((XmlElement)node);
			}
			if (nodeType == XmlNodeType.DocumentType)
			{
				return new XmlDocumentTypeWrapper((XmlDocumentType)node);
			}
			if (nodeType != XmlNodeType.XmlDeclaration)
			{
				return new XmlNodeWrapper(node);
			}
			return new XmlDeclarationWrapper((XmlDeclaration)node);
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x00032354 File Offset: 0x00030554
		[Nullable(1)]
		public List<IXmlNode> Attributes
		{
			[NullableContext(1)]
			get
			{
				if (this._attributes == null)
				{
					if (!this.HasAttributes)
					{
						this._attributes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._attributes = new List<IXmlNode>(this._node.Attributes.Count);
						foreach (object obj in this._node.Attributes)
						{
							XmlAttribute xmlAttribute = (XmlAttribute)obj;
							this._attributes.Add(XmlNodeWrapper.WrapNode(xmlAttribute));
						}
					}
				}
				return this._attributes;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x000323FC File Offset: 0x000305FC
		private bool HasAttributes
		{
			get
			{
				XmlElement xmlElement = this._node as XmlElement;
				if (xmlElement != null)
				{
					return xmlElement.HasAttributes;
				}
				XmlAttributeCollection attributes = this._node.Attributes;
				return attributes != null && attributes.Count > 0;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x00032438 File Offset: 0x00030638
		public IXmlNode ParentNode
		{
			get
			{
				XmlAttribute xmlAttribute = this._node as XmlAttribute;
				XmlNode xmlNode = ((xmlAttribute != null) ? xmlAttribute.OwnerElement : this._node.ParentNode);
				if (xmlNode == null)
				{
					return null;
				}
				return XmlNodeWrapper.WrapNode(xmlNode);
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x00032473 File Offset: 0x00030673
		// (set) Token: 0x06000C97 RID: 3223 RVA: 0x00032480 File Offset: 0x00030680
		public string Value
		{
			get
			{
				return this._node.Value;
			}
			set
			{
				this._node.Value = value;
			}
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x00032490 File Offset: 0x00030690
		[NullableContext(1)]
		public IXmlNode AppendChild(IXmlNode newChild)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)newChild;
			this._node.AppendChild(xmlNodeWrapper._node);
			this._childNodes = null;
			this._attributes = null;
			return newChild;
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x000324C5 File Offset: 0x000306C5
		public string NamespaceUri
		{
			get
			{
				return this._node.NamespaceURI;
			}
		}

		// Token: 0x040003E8 RID: 1000
		[Nullable(1)]
		private readonly XmlNode _node;

		// Token: 0x040003E9 RID: 1001
		[Nullable(new byte[] { 2, 1 })]
		private List<IXmlNode> _childNodes;

		// Token: 0x040003EA RID: 1002
		[Nullable(new byte[] { 2, 1 })]
		private List<IXmlNode> _attributes;
	}
}
