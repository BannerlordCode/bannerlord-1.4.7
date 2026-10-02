using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x0200002D RID: 45
	public abstract class VisualOrderProvider
	{
		// Token: 0x0600037E RID: 894
		public abstract bool IsAvailable();

		// Token: 0x0600037F RID: 895
		public abstract MBReadOnlyList<VisualOrderSet> GetOrders();
	}
}
