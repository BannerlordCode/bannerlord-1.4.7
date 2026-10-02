using System;

namespace TaleWorlds.Localization.TextProcessor
{
	// Token: 0x02000028 RID: 40
	[Serializable]
	internal class MBTextToken
	{
		// Token: 0x06000115 RID: 277 RVA: 0x00006050 File Offset: 0x00004250
		internal MBTextToken(TokenType tokenType)
		{
			this.TokenType = tokenType;
			this.Value = string.Empty;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000606A File Offset: 0x0000426A
		internal MBTextToken(TokenType tokenType, string value)
		{
			this.TokenType = tokenType;
			this.Value = value;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00006080 File Offset: 0x00004280
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00006088 File Offset: 0x00004288
		internal TokenType TokenType { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00006091 File Offset: 0x00004291
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00006099 File Offset: 0x00004299
		public string Value { get; set; }

		// Token: 0x0600011B RID: 283 RVA: 0x000060A2 File Offset: 0x000042A2
		public MBTextToken Clone()
		{
			return new MBTextToken(this.TokenType, this.Value);
		}
	}
}
