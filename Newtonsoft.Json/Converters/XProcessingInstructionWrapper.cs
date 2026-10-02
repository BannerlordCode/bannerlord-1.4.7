using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000FA RID: 250
	[NullableContext(2)]
	[Nullable(0)]
	internal class XProcessingInstructionWrapper : XObjectWrapper
	{
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x000327BD File Offset: 0x000309BD
		[Nullable(1)]
		private XProcessingInstruction ProcessingInstruction
		{
			[NullableContext(1)]
			get
			{
				return (XProcessingInstruction)base.WrappedNode;
			}
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x000327CA File Offset: 0x000309CA
		[NullableContext(1)]
		public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction)
			: base(processingInstruction)
		{
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x000327D3 File Offset: 0x000309D3
		public override string LocalName
		{
			get
			{
				return this.ProcessingInstruction.Target;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x000327E0 File Offset: 0x000309E0
		// (set) Token: 0x06000CEB RID: 3307 RVA: 0x000327ED File Offset: 0x000309ED
		public override string Value
		{
			get
			{
				return this.ProcessingInstruction.Data;
			}
			set
			{
				this.ProcessingInstruction.Data = value;
			}
		}
	}
}
