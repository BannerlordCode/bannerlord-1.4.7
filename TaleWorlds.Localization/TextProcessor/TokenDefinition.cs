using System;
using System.Text.RegularExpressions;

namespace TaleWorlds.Localization.TextProcessor
{
	// Token: 0x02000029 RID: 41
	internal class TokenDefinition
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000060B5 File Offset: 0x000042B5
		// (set) Token: 0x0600011D RID: 285 RVA: 0x000060BD File Offset: 0x000042BD
		public TokenType TokenType { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000060C6 File Offset: 0x000042C6
		// (set) Token: 0x0600011F RID: 287 RVA: 0x000060CE File Offset: 0x000042CE
		public int Precedence { get; private set; }

		// Token: 0x06000120 RID: 288 RVA: 0x000060D7 File Offset: 0x000042D7
		public TokenDefinition(TokenType tokenType, string regexPattern, int precedence)
		{
			this._regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			this.TokenType = tokenType;
			this.Precedence = precedence;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000060FC File Offset: 0x000042FC
		internal Match CheckMatch(string str, int beginIndex)
		{
			beginIndex = this.SkipWhiteSpace(str, beginIndex);
			Match match = this._regex.Match(str, beginIndex);
			if (match.Success && match.Index == beginIndex)
			{
				return match;
			}
			return null;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006138 File Offset: 0x00004338
		private int SkipWhiteSpace(string str, int beginIndex)
		{
			int num = beginIndex;
			int length = str.Length;
			while (num < length && char.IsWhiteSpace(str[num]))
			{
				num++;
			}
			return num;
		}

		// Token: 0x04000062 RID: 98
		private readonly Regex _regex;
	}
}
