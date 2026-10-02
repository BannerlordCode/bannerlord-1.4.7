using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F0 RID: 240
	[NullableContext(1)]
	internal interface IXmlDocument : IXmlNode
	{
		// Token: 0x06000C9A RID: 3226
		IXmlNode CreateComment([Nullable(2)] string text);

		// Token: 0x06000C9B RID: 3227
		IXmlNode CreateTextNode([Nullable(2)] string text);

		// Token: 0x06000C9C RID: 3228
		IXmlNode CreateCDataSection([Nullable(2)] string data);

		// Token: 0x06000C9D RID: 3229
		IXmlNode CreateWhitespace([Nullable(2)] string text);

		// Token: 0x06000C9E RID: 3230
		IXmlNode CreateSignificantWhitespace([Nullable(2)] string text);

		// Token: 0x06000C9F RID: 3231
		[NullableContext(2)]
		[return: Nullable(1)]
		IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone);

		// Token: 0x06000CA0 RID: 3232
		[NullableContext(2)]
		[return: Nullable(1)]
		IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset);

		// Token: 0x06000CA1 RID: 3233
		IXmlNode CreateProcessingInstruction(string target, [Nullable(2)] string data);

		// Token: 0x06000CA2 RID: 3234
		IXmlElement CreateElement(string elementName);

		// Token: 0x06000CA3 RID: 3235
		IXmlElement CreateElement(string qualifiedName, string namespaceUri);

		// Token: 0x06000CA4 RID: 3236
		IXmlNode CreateAttribute(string name, [Nullable(2)] string value);

		// Token: 0x06000CA5 RID: 3237
		IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, [Nullable(2)] string value);

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000CA6 RID: 3238
		[Nullable(2)]
		IXmlElement DocumentElement
		{
			[NullableContext(2)]
			get;
		}
	}
}
