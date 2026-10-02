using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FB RID: 251
	[NullableContext(1)]
	[Nullable(0)]
	internal class XContainerWrapper : XObjectWrapper
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x000327FB File Offset: 0x000309FB
		private XContainer Container
		{
			get
			{
				return (XContainer)base.WrappedNode;
			}
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00032808 File Offset: 0x00030A08
		public XContainerWrapper(XContainer container)
			: base(container)
		{
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00032814 File Offset: 0x00030A14
		public override List<IXmlNode> ChildNodes
		{
			get
			{
				if (this._childNodes == null)
				{
					if (!this.HasChildNodes)
					{
						this._childNodes = XmlNodeConverter.EmptyChildNodes;
					}
					else
					{
						this._childNodes = new List<IXmlNode>();
						foreach (XNode xnode in this.Container.Nodes())
						{
							this._childNodes.Add(XContainerWrapper.WrapNode(xnode));
						}
					}
				}
				return this._childNodes;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x000328A0 File Offset: 0x00030AA0
		protected virtual bool HasChildNodes
		{
			get
			{
				return this.Container.LastNode != null;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x000328B0 File Offset: 0x00030AB0
		[Nullable(2)]
		public override IXmlNode ParentNode
		{
			[NullableContext(2)]
			get
			{
				if (this.Container.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Container.Parent);
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x000328D4 File Offset: 0x00030AD4
		internal static IXmlNode WrapNode(XObject node)
		{
			XDocument xdocument = node as XDocument;
			if (xdocument != null)
			{
				return new XDocumentWrapper(xdocument);
			}
			XElement xelement = node as XElement;
			if (xelement != null)
			{
				return new XElementWrapper(xelement);
			}
			XContainer xcontainer = node as XContainer;
			if (xcontainer != null)
			{
				return new XContainerWrapper(xcontainer);
			}
			XProcessingInstruction xprocessingInstruction = node as XProcessingInstruction;
			if (xprocessingInstruction != null)
			{
				return new XProcessingInstructionWrapper(xprocessingInstruction);
			}
			XText xtext = node as XText;
			if (xtext != null)
			{
				return new XTextWrapper(xtext);
			}
			XComment xcomment = node as XComment;
			if (xcomment != null)
			{
				return new XCommentWrapper(xcomment);
			}
			XAttribute xattribute = node as XAttribute;
			if (xattribute != null)
			{
				return new XAttributeWrapper(xattribute);
			}
			XDocumentType xdocumentType = node as XDocumentType;
			if (xdocumentType != null)
			{
				return new XDocumentTypeWrapper(xdocumentType);
			}
			return new XObjectWrapper(node);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0003297B File Offset: 0x00030B7B
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			this.Container.Add(newChild.WrappedNode);
			this._childNodes = null;
			return newChild;
		}

		// Token: 0x040003ED RID: 1005
		[Nullable(new byte[] { 2, 1 })]
		private List<IXmlNode> _childNodes;
	}
}
