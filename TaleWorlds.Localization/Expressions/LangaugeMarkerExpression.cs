using System;
using TaleWorlds.Localization.TextProcessor;

namespace TaleWorlds.Localization.Expressions
{
	// Token: 0x0200000F RID: 15
	internal class LangaugeMarkerExpression : TextExpression
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x00004697 File Offset: 0x00002897
		public LangaugeMarkerExpression(string innerText)
		{
			base.RawValue = innerText;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000046A6 File Offset: 0x000028A6
		internal override string EvaluateString(TextProcessingContext context, TextObject parent)
		{
			return base.RawValue;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000046AE File Offset: 0x000028AE
		internal override TokenType TokenType
		{
			get
			{
				return TokenType.LanguageMarker;
			}
		}
	}
}
