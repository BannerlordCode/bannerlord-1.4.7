using System;
using System.Globalization;
using System.Text;

namespace TaleWorlds.Localization.TextProcessor
{
	// Token: 0x02000025 RID: 37
	public class DefaultTextProcessor : LanguageSpecificTextProcessor
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x00005233 File Offset: 0x00003433
		public override void ProcessToken(string sourceText, ref int cursorPos, string token, StringBuilder outputString)
		{
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00005235 File Offset: 0x00003435
		public override CultureInfo CultureInfoForLanguage
		{
			get
			{
				return CultureInfo.InvariantCulture;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000523C File Offset: 0x0000343C
		public override void ClearTemporaryData()
		{
		}
	}
}
