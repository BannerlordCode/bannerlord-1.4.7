using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x02000026 RID: 38
	public sealed class ReturnVisualOrder : VisualOrder
	{
		// Token: 0x0600035F RID: 863 RVA: 0x0000CFC5 File Offset: 0x0000B1C5
		public ReturnVisualOrder()
			: base("order_return")
		{
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000CFD2 File Offset: 0x0000B1D2
		public override TextObject GetName(OrderController orderController)
		{
			return new TextObject("{=EmVbbIUc}Return", null);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000CFDF File Offset: 0x0000B1DF
		public override bool IsTargeted()
		{
			return false;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000CFE2 File Offset: 0x0000B1E2
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000CFE4 File Offset: 0x0000B1E4
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(false);
		}
	}
}
