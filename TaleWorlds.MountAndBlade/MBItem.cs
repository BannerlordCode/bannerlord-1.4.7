using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001CC RID: 460
	public static class MBItem
	{
		// Token: 0x06001B97 RID: 7063 RVA: 0x0006024A File Offset: 0x0005E44A
		public static int GetItemUsageIndex(string itemUsageName)
		{
			return MBAPI.IMBItem.GetItemUsageIndex(itemUsageName);
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x00060257 File Offset: 0x0005E457
		public static int GetItemHolsterIndex(string itemHolsterName)
		{
			return MBAPI.IMBItem.GetItemHolsterIndex(itemHolsterName);
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x00060264 File Offset: 0x0005E464
		public static bool GetItemIsPassiveUsage(string itemUsageName)
		{
			return MBAPI.IMBItem.GetItemIsPassiveUsage(itemUsageName);
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x00060274 File Offset: 0x0005E474
		public static MatrixFrame GetHolsterFrameByIndex(int index)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			MBAPI.IMBItem.GetHolsterFrameByIndex(index, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x00060297 File Offset: 0x0005E497
		public static ItemObject.ItemUsageSetFlags GetItemUsageSetFlags(string ItemUsageName)
		{
			return (ItemObject.ItemUsageSetFlags)MBAPI.IMBItem.GetItemUsageSetFlags(ItemUsageName);
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x000602A4 File Offset: 0x0005E4A4
		public static ActionIndexCache GetItemUsageReloadActionCode(string itemUsageName, int usageDirection, bool isMounted, int leftHandUsageSetIndex, bool isLeftStance, bool isLowLookDirection)
		{
			return new ActionIndexCache(MBAPI.IMBItem.GetItemUsageReloadActionCode(itemUsageName, usageDirection, isMounted, leftHandUsageSetIndex, isLeftStance, isLowLookDirection));
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x000602BD File Offset: 0x0005E4BD
		public static int GetItemUsageStrikeType(string itemUsageName, int usageDirection, bool isMounted, int leftHandUsageSetIndex, bool isLeftStance, bool isLowLookDirection)
		{
			return MBAPI.IMBItem.GetItemUsageStrikeType(itemUsageName, usageDirection, isMounted, leftHandUsageSetIndex, isLeftStance, isLowLookDirection);
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x000602D1 File Offset: 0x0005E4D1
		public static float GetMissileRange(float shotSpeed, float zDiff)
		{
			return MBAPI.IMBItem.GetMissileRange(shotSpeed, zDiff);
		}
	}
}
