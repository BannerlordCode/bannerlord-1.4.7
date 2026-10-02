using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000402 RID: 1026
	public interface IMapTracksCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06004067 RID: 16487
		MBReadOnlyList<Track> DetectedTracks { get; }

		// Token: 0x06004068 RID: 16488
		void AddTrack(MobileParty target, CampaignVec2 trackPosition, Vec2 trackDirection);

		// Token: 0x06004069 RID: 16489
		void AddMapArrow(TextObject pointerName, CampaignVec2 trackPosition, Vec2 trackDirection, float life);
	}
}
