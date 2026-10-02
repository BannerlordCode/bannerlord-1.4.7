using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x02000010 RID: 16
	internal class TextIdExpression : TextExpression
	{
		// Token: 0x060000A5 RID: 165 RVA: 0x000046B2 File Offset: 0x000028B2
		public TextIdExpression(string innerText)
		{
			base.RawValue = innerText;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000046C1 File Offset: 0x000028C1
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			return "";
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000046C8 File Offset: 0x000028C8
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.TextId;
			}
		}
	}
}
