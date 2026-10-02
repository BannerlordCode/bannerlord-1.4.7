using System;

namespace Helpers
{
	// Token: 0x02000018 RID: 24
	public static class BoardGameHelper
	{
		// Token: 0x020004E4 RID: 1252
		public enum AIDifficulty
		{
			// Token: 0x04001500 RID: 5376
			Easy,
			// Token: 0x04001501 RID: 5377
			Normal,
			// Token: 0x04001502 RID: 5378
			Hard,
			// Token: 0x04001503 RID: 5379
			NumTypes
		}

		// Token: 0x020004E5 RID: 1253
		public enum BoardGameState
		{
			// Token: 0x04001505 RID: 5381
			None,
			// Token: 0x04001506 RID: 5382
			Win,
			// Token: 0x04001507 RID: 5383
			Loss,
			// Token: 0x04001508 RID: 5384
			Draw
		}
	}
}
