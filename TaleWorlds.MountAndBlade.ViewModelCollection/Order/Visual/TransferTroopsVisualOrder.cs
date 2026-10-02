using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x02000027 RID: 39
	public class TransferTroopsVisualOrder : VisualOrder
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000364 RID: 868 RVA: 0x0000CFEC File Offset: 0x0000B1EC
		// (remove) Token: 0x06000365 RID: 869 RVA: 0x0000D020 File Offset: 0x0000B220
		public static event Action OnTransferStarted;

		// Token: 0x06000366 RID: 870 RVA: 0x0000D053 File Offset: 0x0000B253
		public TransferTroopsVisualOrder()
			: base("order_toggle_transfer")
		{
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000D060 File Offset: 0x0000B260
		public override void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
			Action onTransferStarted = TransferTroopsVisualOrder.OnTransferStarted;
			if (onTransferStarted == null)
			{
				return;
			}
			onTransferStarted();
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000D071 File Offset: 0x0000B271
		public override TextObject GetName(OrderController orderController)
		{
			return new TextObject("{=AmbKQ7LT}Transfer", null);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000D07E File Offset: 0x0000B27E
		public override bool IsTargeted()
		{
			return false;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000D081 File Offset: 0x0000B281
		protected override bool? OnGetFormationHasOrder(Formation formation)
		{
			return new bool?(false);
		}
	}
}
