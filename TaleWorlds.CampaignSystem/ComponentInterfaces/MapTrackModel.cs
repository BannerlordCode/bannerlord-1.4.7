using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B9 RID: 441
	public abstract class MapTrackModel : MBGameModel<MapTrackModel>
	{
		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06001DB3 RID: 7603
		public abstract float MaxTrackLife { get; }

		// Token: 0x06001DB4 RID: 7604
		public abstract float GetSkipTrackChance(MobileParty mobileParty);

		// Token: 0x06001DB5 RID: 7605
		public abstract float GetMaxTrackSpottingDistanceForMainParty();

		// Token: 0x06001DB6 RID: 7606
		public abstract bool CanPartyLeaveTrack(MobileParty mobileParty);

		// Token: 0x06001DB7 RID: 7607
		public abstract float GetTrackDetectionDifficultyForMainParty(Track track, float trackSpottingDistance);

		// Token: 0x06001DB8 RID: 7608
		public abstract float GetSkillFromTrackDetected(Track track);

		// Token: 0x06001DB9 RID: 7609
		public abstract int GetTrackLife(MobileParty mobileParty);

		// Token: 0x06001DBA RID: 7610
		public abstract TextObject TrackTitle(Track track);

		// Token: 0x06001DBB RID: 7611
		public abstract IEnumerable<ValueTuple<TextObject, string>> GetTrackDescription(Track track);

		// Token: 0x06001DBC RID: 7612
		public abstract uint GetTrackColor(Track track);

		// Token: 0x06001DBD RID: 7613
		public abstract float GetTrackScale(Track track);
	}
}
