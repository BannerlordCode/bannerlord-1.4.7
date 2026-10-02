using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map.DistanceCache
{
	// Token: 0x02000227 RID: 551
	public readonly struct NavigationCacheElement<T> : IEquatable<NavigationCacheElement<T>> where T : ISettlementDataHolder
	{
		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x0600211E RID: 8478 RVA: 0x0009324C File Offset: 0x0009144C
		public CampaignVec2 PortPosition
		{
			get
			{
				T settlement = this.Settlement;
				return settlement.PortPosition;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x0600211F RID: 8479 RVA: 0x00093270 File Offset: 0x00091470
		public CampaignVec2 GatePosition
		{
			get
			{
				T settlement = this.Settlement;
				return settlement.GatePosition;
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06002120 RID: 8480 RVA: 0x00093294 File Offset: 0x00091494
		public string StringId
		{
			get
			{
				T settlement = this.Settlement;
				return settlement.StringId;
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000932B5 File Offset: 0x000914B5
		public NavigationCacheElement(T settlement, bool isPortUsed)
		{
			this.Settlement = settlement;
			this.IsPortUsed = isPortUsed;
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000932C8 File Offset: 0x000914C8
		public static void Sort(ref NavigationCacheElement<T> settlement1, ref NavigationCacheElement<T> settlement2, out bool isPairChanged)
		{
			isPairChanged = false;
			int num = string.Compare(settlement1.StringId, settlement2.StringId, StringComparison.Ordinal);
			if (num < 0 || (num == 0 && settlement1.IsPortUsed))
			{
				return;
			}
			NavigationCacheElement<T> navigationCacheElement = settlement2;
			NavigationCacheElement<T> navigationCacheElement2 = settlement1;
			settlement1 = navigationCacheElement;
			settlement2 = navigationCacheElement2;
			isPairChanged = true;
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x0009331A File Offset: 0x0009151A
		public override int GetHashCode()
		{
			return this.StringId.GetDeterministicHashCode() * 2 + (this.IsPortUsed ? 1 : 0);
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x00093338 File Offset: 0x00091538
		public override bool Equals(object obj)
		{
			if (obj is NavigationCacheElement<T>)
			{
				NavigationCacheElement<T> navigationCacheElement = (NavigationCacheElement<T>)obj;
				return this.StringId == navigationCacheElement.StringId && this.IsPortUsed == navigationCacheElement.IsPortUsed;
			}
			return false;
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x0009337E File Offset: 0x0009157E
		public bool Equals(NavigationCacheElement<T> other)
		{
			return EqualityComparer<T>.Default.Equals(this.Settlement, other.Settlement) && this.IsPortUsed == other.IsPortUsed;
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x000933A8 File Offset: 0x000915A8
		public static bool operator ==(NavigationCacheElement<T> left, NavigationCacheElement<T> right)
		{
			return left.Equals(right);
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x000933B2 File Offset: 0x000915B2
		public static bool operator !=(NavigationCacheElement<T> left, NavigationCacheElement<T> right)
		{
			return !left.Equals(right);
		}

		// Token: 0x040009BF RID: 2495
		public readonly T Settlement;

		// Token: 0x040009C0 RID: 2496
		public readonly bool IsPortUsed;
	}
}
