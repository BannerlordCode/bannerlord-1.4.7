using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000153 RID: 339
	public struct FiringOrder
	{
		// Token: 0x060011D0 RID: 4560 RVA: 0x00036D98 File Offset: 0x00034F98
		private FiringOrder(FiringOrder.RangedWeaponUsageOrderEnum orderEnum)
		{
			this.OrderEnum = orderEnum;
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x060011D1 RID: 4561 RVA: 0x00036DA1 File Offset: 0x00034FA1
		public OrderType OrderType
		{
			get
			{
				if (this.OrderEnum != FiringOrder.RangedWeaponUsageOrderEnum.FireAtWill)
				{
					return OrderType.HoldFire;
				}
				return OrderType.FireAtWill;
			}
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x00036DB0 File Offset: 0x00034FB0
		public override bool Equals(object obj)
		{
			if (obj is FiringOrder)
			{
				FiringOrder firingOrder = (FiringOrder)obj;
				return firingOrder == this;
			}
			return false;
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x00036DDC File Offset: 0x00034FDC
		public override int GetHashCode()
		{
			return (int)this.OrderEnum;
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x00036DE4 File Offset: 0x00034FE4
		public static bool operator !=(FiringOrder f1, FiringOrder f2)
		{
			return f1.OrderEnum != f2.OrderEnum;
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x00036DF7 File Offset: 0x00034FF7
		public static bool operator ==(FiringOrder f1, FiringOrder f2)
		{
			return f1.OrderEnum == f2.OrderEnum;
		}

		// Token: 0x0400044F RID: 1103
		public readonly FiringOrder.RangedWeaponUsageOrderEnum OrderEnum;

		// Token: 0x04000450 RID: 1104
		public static readonly FiringOrder FiringOrderFireAtWill = new FiringOrder(FiringOrder.RangedWeaponUsageOrderEnum.FireAtWill);

		// Token: 0x04000451 RID: 1105
		public static readonly FiringOrder FiringOrderHoldYourFire = new FiringOrder(FiringOrder.RangedWeaponUsageOrderEnum.HoldYourFire);

		// Token: 0x02000475 RID: 1141
		public enum RangedWeaponUsageOrderEnum
		{
			// Token: 0x04001A73 RID: 6771
			FireAtWill,
			// Token: 0x04001A74 RID: 6772
			HoldYourFire
		}
	}
}
