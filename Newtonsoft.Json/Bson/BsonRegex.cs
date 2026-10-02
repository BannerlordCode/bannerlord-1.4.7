using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010C RID: 268
	internal class BsonRegex : BsonToken
	{
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x00035D9B File Offset: 0x00033F9B
		// (set) Token: 0x06000D8A RID: 3466 RVA: 0x00035DA3 File Offset: 0x00033FA3
		public BsonString Pattern { get; set; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00035DAC File Offset: 0x00033FAC
		// (set) Token: 0x06000D8C RID: 3468 RVA: 0x00035DB4 File Offset: 0x00033FB4
		public BsonString Options { get; set; }

		// Token: 0x06000D8D RID: 3469 RVA: 0x00035DBD File Offset: 0x00033FBD
		public BsonRegex(string pattern, string options)
		{
			this.Pattern = new BsonString(pattern, false);
			this.Options = new BsonString(options, false);
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x00035DDF File Offset: 0x00033FDF
		public override BsonType Type
		{
			get
			{
				return BsonType.Regex;
			}
		}
	}
}
