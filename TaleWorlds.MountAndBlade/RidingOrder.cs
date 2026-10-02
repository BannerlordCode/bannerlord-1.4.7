using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000158 RID: 344
	public struct RidingOrder
	{
		// Token: 0x0600122E RID: 4654 RVA: 0x0003943E File Offset: 0x0003763E
		private RidingOrder(RidingOrder.RidingOrderEnum orderEnum)
		{
			this.OrderEnum = orderEnum;
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x00039447 File Offset: 0x00037647
		public OrderType OrderType
		{
			get
			{
				if (this.OrderEnum == RidingOrder.RidingOrderEnum.Free)
				{
					return OrderType.RideFree;
				}
				if (this.OrderEnum != RidingOrder.RidingOrderEnum.Mount)
				{
					return OrderType.Dismount;
				}
				return OrderType.Mount;
			}
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00039464 File Offset: 0x00037664
		public override bool Equals(object obj)
		{
			if (obj is RidingOrder)
			{
				RidingOrder ridingOrder = (RidingOrder)obj;
				return ridingOrder == this;
			}
			return false;
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00039490 File Offset: 0x00037690
		public override int GetHashCode()
		{
			return (int)this.OrderEnum;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00039498 File Offset: 0x00037698
		public static bool operator !=(RidingOrder r1, RidingOrder r2)
		{
			return r1.OrderEnum != r2.OrderEnum;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x000394AB File Offset: 0x000376AB
		public static bool operator ==(RidingOrder r1, RidingOrder r2)
		{
			return r1.OrderEnum == r2.OrderEnum;
		}

		// Token: 0x04000471 RID: 1137
		public readonly RidingOrder.RidingOrderEnum OrderEnum;

		// Token: 0x04000472 RID: 1138
		public static readonly RidingOrder RidingOrderFree = new RidingOrder(RidingOrder.RidingOrderEnum.Free);

		// Token: 0x04000473 RID: 1139
		public static readonly RidingOrder RidingOrderMount = new RidingOrder(RidingOrder.RidingOrderEnum.Mount);

		// Token: 0x04000474 RID: 1140
		public static readonly RidingOrder RidingOrderDismount = new RidingOrder(RidingOrder.RidingOrderEnum.Dismount);

		// Token: 0x02000485 RID: 1157
		public enum RidingOrderEnum
		{
			// Token: 0x04001AA7 RID: 6823
			Free,
			// Token: 0x04001AA8 RID: 6824
			Mount,
			// Token: 0x04001AA9 RID: 6825
			Dismount
		}
	}
}
