using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200006B RID: 107
	public enum GameManagerLoadingSteps
	{
		// Token: 0x04000401 RID: 1025
		None = -1,
		// Token: 0x04000402 RID: 1026
		PreInitializeZerothStep,
		// Token: 0x04000403 RID: 1027
		FirstInitializeFirstStep,
		// Token: 0x04000404 RID: 1028
		WaitSecondStep,
		// Token: 0x04000405 RID: 1029
		SecondInitializeThirdState,
		// Token: 0x04000406 RID: 1030
		PostInitializeFourthState,
		// Token: 0x04000407 RID: 1031
		FinishLoadingFifthStep,
		// Token: 0x04000408 RID: 1032
		LoadingIsOver
	}
}
