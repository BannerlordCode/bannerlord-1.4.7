using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000F8 RID: 248
	[NullableContext(2)]
	[Nullable(0)]
	internal class XTextWrapper : XObjectWrapper
	{
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x00032719 File Offset: 0x00030919
		[Nullable(1)]
		private XText Text
		{
			[NullableContext(1)]
			get
			{
				return (XText)base.WrappedNode;
			}
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x00032726 File Offset: 0x00030926
		[NullableContext(1)]
		public XTextWrapper(XText text)
			: base(text)
		{
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x0003272F File Offset: 0x0003092F
		// (set) Token: 0x06000CE0 RID: 3296 RVA: 0x0003273C File Offset: 0x0003093C
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

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x0003274A File Offset: 0x0003094A
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
