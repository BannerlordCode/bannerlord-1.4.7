using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F9 RID: 249
	[NullableContext(2)]
	[Nullable(0)]
	internal class XCommentWrapper : XObjectWrapper
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x0003276B File Offset: 0x0003096B
		[Nullable(1)]
		private XComment Text
		{
			[NullableContext(1)]
			get
			{
				return (XComment)base.WrappedNode;
			}
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00032778 File Offset: 0x00030978
		[NullableContext(1)]
		public XCommentWrapper(XComment text)
			: base(text)
		{
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x00032781 File Offset: 0x00030981
		// (set) Token: 0x06000CE5 RID: 3301 RVA: 0x0003278E File Offset: 0x0003098E
		public override string Value
		{
			get
			{
				return this.Text.Value;
			}
			set
			{
				this.Text.Value = value;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x0003279C File Offset: 0x0003099C
		public override IXmlNode ParentNode
		{
			get
			{
				if (this.Text.Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode(this.Text.Parent);
			}
		}
	}
}
