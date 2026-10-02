using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200024A RID: 586
	public static class ItemCollectionElementMissionExtensions
	{
		// Token: 0x0600219A RID: 8602 RVA: 0x000757C4 File Offset: 0x000739C4
		public static StackArray.StackArray4Int GetItemHolsterIndices(this ItemObject item)
		{
			StackArray.StackArray4Int stackArray4Int = default(StackArray.StackArray4Int);
			for (int i = 0; i < item.ItemHolsters.Length; i++)
			{
				stackArray4Int[i] = ((item.ItemHolsters[i].Length > 0) ? MBItem.GetItemHolsterIndex(item.ItemHolsters[i]) : (-1));
			}
			for (int j = item.ItemHolsters.Length; j < 4; j++)
			{
				stackArray4Int[j] = -1;
			}
			return stackArray4Int;
		}
	}
}
