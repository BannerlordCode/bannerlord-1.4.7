using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F7 RID: 247
	[NullableContext(1)]
	[Nullable(0)]
	internal class XDocumentWrapper : XContainerWrapper, IXmlDocument, IXmlNode
	{
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0003257C File Offset: 0x0003077C
		private XDocument Document
		{
			get
			{
				return (XDocument)base.WrappedNode;
			}
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x00032589 File Offset: 0x00030789
		public XDocumentWrapper(XDocument document)
			: base(document)
		{
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00032594 File Offset: 0x00030794
		public override List<IXmlNode> ChildNodes
		{
			get
			{
				List<IXmlNode> childNodes = base.ChildNodes;
				if (this.Document.Declaration != null && (childNodes.Count == 0 || childNodes[0].NodeType != XmlNodeType.XmlDeclaration))
				{
					childNodes.Insert(0, new XDeclarationWrapper(this.Document.Declaration));
				}
				return childNodes;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x000325E5 File Offset: 0x000307E5
		protected override bool HasChildNodes
		{
			get
			{
				return base.HasChildNodes || this.Document.Declaration != null;
			}
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x000325FF File Offset: 0x000307FF
		public IXmlNode CreateComment([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XComment(text));
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0003260C File Offset: 0x0003080C
		public IXmlNode CreateTextNode([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00032619 File Offset: 0x00030819
		public IXmlNode CreateCDataSection([Nullable(2)] string data)
		{
			return new XObjectWrapper(new XCData(data));
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00032626 File Offset: 0x00030826
		public IXmlNode CreateWhitespace([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00032633 File Offset: 0x00030833
		public IXmlNode CreateSignificantWhitespace([Nullable(2)] string text)
		{
			return new XObjectWrapper(new XText(text));
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00032640 File Offset: 0x00030840
		[NullableContext(2)]
		[return: Nullable(1)]
		public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new XDeclarationWrapper(new XDeclaration(version, encoding, standalone));
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0003264F File Offset: 0x0003084F
		[NullableContext(2)]
		[return: Nullable(1)]
		public IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new XDocumentTypeWrapper(new XDocumentType(name, publicId, systemId, internalSubset));
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00032660 File Offset: 0x00030860
		public IXmlNode CreateProcessingInstruction(string target, [Nullable(2)] string data)
		{
			return new XProcessingInstructionWrapper(new XProcessingInstruction(target, data));
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0003266E File Offset: 0x0003086E
		public IXmlElement CreateElement(string elementName)
		{
			return new XElementWrapper(new XElement(elementName));
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x00032680 File Offset: 0x00030880
		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			return new XElementWrapper(new XElement(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri)));
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00032698 File Offset: 0x00030898
		public IXmlNode CreateAttribute(string name, [Nullable(2)] string value)
		{
			return new XAttributeWrapper(new XAttribute(name, value));
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x000326AB File Offset: 0x000308AB
		public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, [Nullable(2)] string value)
		{
			return new XAttributeWrapper(new XAttribute(XName.Get(MiscellaneousUtils.GetLocalName(qualifiedName), namespaceUri), value));
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x000326C4 File Offset: 0x000308C4
		[Nullable(2)]
		public IXmlElement DocumentElement
		{
			[NullableContext(2)]
			get
			{
				if (this.Document.Root == null)
				{
					return null;
				}
				return new XElementWrapper(this.Document.Root);
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x000326E8 File Offset: 0x000308E8
		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			XDeclarationWrapper xdeclarationWrapper = newChild as XDeclarationWrapper;
			if (xdeclarationWrapper != null)
			{
				this.Document.Declaration = xdeclarationWrapper.Declaration;
				return xdeclarationWrapper;
			}
			return base.AppendChild(newChild);
		}
	}
}
