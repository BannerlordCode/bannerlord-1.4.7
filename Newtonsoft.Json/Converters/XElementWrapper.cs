using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FE RID: 254
	[NullableContext(1)]
	[Nullable(0)]
	internal class XElementWrapper : XContainerWrapper, IXmlElement, IXmlNode
	{
		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x00032A5E File Offset: 0x00030C5E
		private XElement Element
		{
			get
			{
				return (XElement)base.WrappedNode;
			}
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00032A6B File Offset: 0x00030C6B
		public XElementWrapper(XElement element)
			: base(element)
		{
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00032A74 File Offset: 0x00030C74
		public void SetAttributeNode(IXmlNode attribute)
		{
			XObjectWrapper xobjectWrapper = (XObjectWrapper)attribute;
			this.Element.Add(xobjectWrapper.WrappedNode);
			this._attributes = null;
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00032AA0 File Offset: 0x00030CA0
		public override List<IXmlNode> Attributes
		{
			get
			{
				if (this._attributes == null)
				{
					if (!this.Element.HasAttributes && !this.HasImplicitNamespaceAttribute(this.NamespaceUri))
					{
						this._attributes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._attributes = new List<IXmlNode>();
						foreach (XAttribute xattribute in this.Element.Attributes())
						{
							this._attributes.Add(new XAttributeWrapper(xattribute));
						}
						string namespaceUri = this.NamespaceUri;
						if (this.HasImplicitNamespaceAttribute(namespaceUri))
						{
							this._attributes.Insert(0, new XAttributeWrapper(new XAttribute("xmlns", namespaceUri)));
						}
					}
				}
				return this._attributes;
			}
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00032B74 File Offset: 0x00030D74
		private bool HasImplicitNamespaceAttribute(string namespaceUri)
		{
			if (!StringUtils.IsNullOrEmpty(namespaceUri))
			{
				IXmlNode parentNode = this.ParentNode;
				if (namespaceUri != ((parentNode != null) ? parentNode.NamespaceUri : null) && StringUtils.IsNullOrEmpty(this.GetPrefixOfNamespace(namespaceUri)))
				{
					bool flag = false;
					if (this.Element.HasAttributes)
					{
						foreach (XAttribute xattribute in this.Element.Attributes())
						{
							if (xattribute.Name.LocalName == "xmlns" && StringUtils.IsNullOrEmpty(xattribute.Name.NamespaceName) && xattribute.Value == namespaceUri)
							{
								flag = true;
							}
						}
					}
					if (!flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00032C44 File Offset: 0x00030E44
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			IXmlNode xmlNode = base.AppendChild(newChild);
			this._attributes = null;
			return xmlNode;
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00032C54 File Offset: 0x00030E54
		// (set) Token: 0x06000D0C RID: 3340 RVA: 0x00032C61 File Offset: 0x00030E61
		[Nullable(2)]
		public override string Value
		{
			[NullableContext(2)]
			get
			{
				return this.Element.Value;
			}
			[NullableContext(2)]
			set
			{
				this.Element.Value = value;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x00032C6F File Offset: 0x00030E6F
		[Nullable(2)]
		public override string LocalName
		{
			[NullableContext(2)]
			get
			{
				return this.Element.Name.LocalName;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x00032C81 File Offset: 0x00030E81
		[Nullable(2)]
		public override string NamespaceUri
		{
			[NullableContext(2)]
			get
			{
				return this.Element.Name.NamespaceName;
			}
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00032C93 File Offset: 0x00030E93
		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return this.Element.GetPrefixOfNamespace(namespaceUri);
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x00032CA6 File Offset: 0x00030EA6
		public bool IsEmpty
		{
			get
			{
				return this.Element.IsEmpty;
			}
		}

		// Token: 0x040003EF RID: 1007
		[Nullable(new byte[] { 2, 1 })]
		private List<IXmlNode> _attributes;
	}
}
