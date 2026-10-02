using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001A9 RID: 425
	[ScriptingInterfaceBase]
	internal interface IMBItem
	{
		// Token: 0x060017C6 RID: 6086
		[EngineMethod("get_item_usage_index", false, null, false)]
		int GetItemUsageIndex(string itemusagename);

		// Token: 0x060017C7 RID: 6087
		[EngineMethod("get_item_holster_index", false, null, false)]
		int GetItemHolsterIndex(string itemholstername);

		// Token: 0x060017C8 RID: 6088
		[EngineMethod("get_item_is_passive_usage", false, null, false)]
		bool GetItemIsPassiveUsage(string itemUsageName);

		// Token: 0x060017C9 RID: 6089
		[EngineMethod("get_holster_frame_by_index", false, null, false)]
		void GetHolsterFrameByIndex(int index, ref MatrixFrame outFrame);

		// Token: 0x060017CA RID: 6090
		[EngineMethod("get_item_usage_set_flags", false, null, false)]
		int GetItemUsageSetFlags(string ItemUsageName);

		// Token: 0x060017CB RID: 6091
		[EngineMethod("get_item_usage_reload_action_code", false, null, false)]
		int GetItemUsageReloadActionCode(string itemUsageName, int usageDirection, bool isMounted, int leftHandUsageSetIndex, bool isLeftStance, bool isLowLookDirection);

		// Token: 0x060017CC RID: 6092
		[EngineMethod("get_item_usage_strike_type", false, null, false)]
		int GetItemUsageStrikeType(string itemUsageName, int usageDirection, bool isMounted, int leftHandUsageSetIndex, bool isLeftStance, bool isLowLookDirection);

		// Token: 0x060017CD RID: 6093
		[EngineMethod("get_missile_range", false, null, false)]
		float GetMissileRange(float shootSpeed, float zDiff);
	}
}
