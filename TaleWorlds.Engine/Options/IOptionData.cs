using System;

namespace TaleWorlds.Engine.Options
{
	// Token: 0x020000A5 RID: 165
	public interface IOptionData
	{
		// Token: 0x06000F3E RID: 3902
		float GetDefaultValue();

		// Token: 0x06000F3F RID: 3903
		void Commit();

		// Token: 0x06000F40 RID: 3904
		float GetValue(bool forceRefresh);

		// Token: 0x06000F41 RID: 3905
		void SetValue(float value);

		// Token: 0x06000F42 RID: 3906
		object GetOptionType();

		// Token: 0x06000F43 RID: 3907
		bool IsNative();

		// Token: 0x06000F44 RID: 3908
		bool IsAction();

		// Token: 0x06000F45 RID: 3909
		ValueTuple<string, bool> GetIsDisabledAndReasonID();
	}
}
