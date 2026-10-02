using System;

namespace TaleWorlds.Engine.Options
{
	// Token: 0x020000A4 RID: 164
	public interface INumericOptionData : IOptionData
	{
		// Token: 0x06000F39 RID: 3897
		float GetMinValue();

		// Token: 0x06000F3A RID: 3898
		float GetMaxValue();

		// Token: 0x06000F3B RID: 3899
		bool GetIsDiscrete();

		// Token: 0x06000F3C RID: 3900
		int GetDiscreteIncrementInterval();

		// Token: 0x06000F3D RID: 3901
		bool GetShouldUpdateContinuously();
	}
}
