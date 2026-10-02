using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001FA RID: 506
	public abstract class CampaignTimeModel : MBGameModel<CampaignTimeModel>
	{
		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06001F7E RID: 8062
		public abstract CampaignTime CampaignStartTime { get; }

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06001F7F RID: 8063
		public abstract int SunRise { get; }

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06001F80 RID: 8064
		public abstract int SunSet { get; }

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06001F81 RID: 8065
		public abstract long TimeTicksPerMillisecond { get; }

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06001F82 RID: 8066
		public abstract int MillisecondInSecond { get; }

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06001F83 RID: 8067
		public abstract int SecondsInMinute { get; }

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06001F84 RID: 8068
		public abstract int MinutesInHour { get; }

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06001F85 RID: 8069
		public abstract int HoursInDay { get; }

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06001F86 RID: 8070
		public abstract int DaysInWeek { get; }

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06001F87 RID: 8071
		public abstract int WeeksInSeason { get; }

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06001F88 RID: 8072
		public abstract int SeasonsInYear { get; }
	}
}
