using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x0200000E RID: 14
	internal class SimpleNumberExpression : TextExpression
	{
		// Token: 0x0600009F RID: 159 RVA: 0x0000467C File Offset: 0x0000287C
		public SimpleNumberExpression(string value)
		{
			base.RawValue = value;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x0000468B File Offset: 0x0000288B
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.Number;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000468F File Offset: 0x0000288F
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			return base.RawValue;
		}
	}
}
