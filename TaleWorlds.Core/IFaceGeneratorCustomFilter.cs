using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000085 RID: 133
	public interface IFaceGeneratorCustomFilter
	{
		// Token: 0x0600089A RID: 2202
		int[] GetHaircutIndices(BasicCharacterObject character);

		// Token: 0x0600089B RID: 2203
		int[] GetFacialHairIndices(BasicCharacterObject character);

		// Token: 0x0600089C RID: 2204
		FaceGeneratorStage[] GetAvailableStages();
	}
}
