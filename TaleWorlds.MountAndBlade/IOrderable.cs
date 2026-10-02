using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000371 RID: 881
	public interface IOrderable
	{
		// Token: 0x06003252 RID: 12882
		OrderType GetOrder(BattleSideEnum side);
	}
}
