using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200008A RID: 138
	public enum SaveResult
	{
		// Token: 0x04000185 RID: 389
		Success,
		// Token: 0x04000186 RID: 390
		NoSpace,
		// Token: 0x04000187 RID: 391
		Corrupted,
		// Token: 0x04000188 RID: 392
		GeneralFailure,
		// Token: 0x04000189 RID: 393
		FileDriverFailure,
		// Token: 0x0400018A RID: 394
		PlatformFileHelperFailure,
		// Token: 0x0400018B RID: 395
		ConfigFileFailure,
		// Token: 0x0400018C RID: 396
		SaveLimitReached
	}
}
