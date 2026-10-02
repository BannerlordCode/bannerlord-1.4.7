using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FF RID: 255
	public class DefaultCampaignTimeModel : CampaignTimeModel
	{
		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x060016BC RID: 5820 RVA: 0x000690E0 File Offset: 0x000672E0
		public override CampaignTime CampaignStartTime
		{
			get
			{
				return CampaignTime.Years(1084f) + CampaignTime.Weeks((float)CampaignTime.WeeksInSeason) + CampaignTime.Hours(9f);
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x060016BD RID: 5821 RVA: 0x0006910B File Offset: 0x0006730B
		public override int SunRise
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x060016BE RID: 5822 RVA: 0x0006910E File Offset: 0x0006730E
		public override int SunSet
		{
			get
			{
				return 22;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x060016BF RID: 5823 RVA: 0x00069112 File Offset: 0x00067312
		public override long TimeTicksPerMillisecond
		{
			get
			{
				return 10L;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x060016C0 RID: 5824 RVA: 0x00069117 File Offset: 0x00067317
		public override int MillisecondInSecond
		{
			get
			{
				return 1000;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x060016C1 RID: 5825 RVA: 0x0006911E File Offset: 0x0006731E
		public override int SecondsInMinute
		{
			get
			{
				return 60;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x060016C2 RID: 5826 RVA: 0x00069122 File Offset: 0x00067322
		public override int MinutesInHour
		{
			get
			{
				return 60;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x060016C3 RID: 5827 RVA: 0x00069126 File Offset: 0x00067326
		public override int HoursInDay
		{
			get
			{
				return 24;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x060016C4 RID: 5828 RVA: 0x0006912A File Offset: 0x0006732A
		public override int DaysInWeek
		{
			get
			{
				if (Campaign.Current.Options.AccelerationMode != GameAccelerationMode.Fast)
				{
					return 7;
				}
				return 3;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x060016C5 RID: 5829 RVA: 0x00069141 File Offset: 0x00067341
		public override int WeeksInSeason
		{
			get
			{
				if (Campaign.Current.Options.AccelerationMode != GameAccelerationMode.Fast)
				{
					return 3;
				}
				return 2;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x060016C6 RID: 5830 RVA: 0x00069158 File Offset: 0x00067358
		public override int SeasonsInYear
		{
			get
			{
				return 4;
			}
		}
	}
}
