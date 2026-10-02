using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EC RID: 236
	[NullableContext(1)]
	[Nullable(0)]
	internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement, IXmlNode
	{
		// Token: 0x06000C7C RID: 3196 RVA: 0x00032119 File Offset: 0x00030319
		public XmlElementWrapper(XmlElement element)
			: base(element)
		{
			this._element = element;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0003212C File Offset: 0x0003032C
		public void SetAttributeNode(IXmlNode attribute)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)attribute;
			this._element.SetAttributeNode((XmlAttribute)xmlNodeWrapper.WrappedNode);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00032157 File Offset: 0x00030357
		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return this._element.GetPrefixOfNamespace(namespaceUri);
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x00032165 File Offset: 0x00030365
		public bool IsEmpty
		{
			get
			{
				return this._element.IsEmpty;
			}
		}

		// Token: 0x040003E5 RID: 997
		private readonly XmlElement _element;
	}
}
