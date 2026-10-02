using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x0200002E RID: 46
	public abstract class VisualOrderSet
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0000D47F File Offset: 0x0000B67F
		public MBReadOnlyList<VisualOrder> Orders
		{
			get
			{
				return this._orders;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0000D487 File Offset: 0x0000B687
		public VisualOrder SoloOrder
		{
			get
			{
				if (!this.IsSoloOrder)
				{
					return null;
				}
				return this._orders[0];
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000383 RID: 899
		public abstract bool IsSoloOrder { get; }

		// Token: 0x06000384 RID: 900
		public abstract TextObject GetName(OrderController orderController);

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000385 RID: 901
		public abstract string StringId { get; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000386 RID: 902
		public abstract string IconId { get; }

		// Token: 0x06000387 RID: 903 RVA: 0x0000D49F File Offset: 0x0000B69F
		public VisualOrderSet()
		{
			this._orders = new MBList<VisualOrder>();
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000D4B4 File Offset: 0x0000B6B4
		public void AddOrder(VisualOrder order)
		{
			if (this.IsSoloOrder)
			{
				Debug.FailedAssert("Can't add additional orders to solo orders", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\Visual\\VisualOrderSet.cs", "AddOrder", 32);
				return;
			}
			if (this._orders.Contains(order))
			{
				Debug.FailedAssert("Order:" + order.StringId + " is already in collection: " + this.StringId, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\Visual\\VisualOrderSet.cs", "AddOrder", 38);
				return;
			}
			this._orders.Add(order);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000D528 File Offset: 0x0000B728
		public void RemoveOrder(VisualOrder order)
		{
			if (this.IsSoloOrder)
			{
				Debug.FailedAssert("Can't remove orders from solo orders", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\Visual\\VisualOrderSet.cs", "RemoveOrder", 49);
				return;
			}
			if (!this._orders.Contains(order))
			{
				Debug.FailedAssert("Order:" + order.StringId + " is not in collection: " + this.StringId, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\Visual\\VisualOrderSet.cs", "RemoveOrder", 55);
				return;
			}
			this._orders.Remove(order);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000D59C File Offset: 0x0000B79C
		public void ClearOrders()
		{
			if (this.IsSoloOrder)
			{
				Debug.FailedAssert("Can't remove orders from solo orders", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\Visual\\VisualOrderSet.cs", "ClearOrders", 66);
				return;
			}
			this._orders.Clear();
		}

		// Token: 0x0400018B RID: 395
		private MBList<VisualOrder> _orders;
	}
}
