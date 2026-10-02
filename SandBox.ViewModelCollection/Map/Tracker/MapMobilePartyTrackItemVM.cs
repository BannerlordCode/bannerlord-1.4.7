using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.Tracker;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Map.Tracker
{
	// Token: 0x02000049 RID: 73
	public class MapMobilePartyTrackItemVM : MapTrackerItemVM<MobileParty>
	{
		// Token: 0x0600048F RID: 1167 RVA: 0x00011EED File Offset: 0x000100ED
		public MapMobilePartyTrackItemVM(MobileParty party)
			: base(party)
		{
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00011EF6 File Offset: 0x000100F6
		protected override void OnShowTooltip()
		{
			InformationManager.ShowTooltip(typeof(MobileParty), new object[] { base.TrackedObject, true, false });
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00011F28 File Offset: 0x00010128
		protected override bool IsVisibleOnMap()
		{
			return base.TrackedObject.AttachedTo == null && !base.TrackedObject.IsVisible;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00011F47 File Offset: 0x00010147
		protected override bool GetCanToggleTrack()
		{
			return true;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00011F4A File Offset: 0x0001014A
		protected override string GetTrackerType()
		{
			return "MobileParty";
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00011F51 File Offset: 0x00010151
		protected override CampaignUIHelper.IssueQuestFlags GetRelatedQuests()
		{
			return CampaignUIHelper.IssueQuestFlags.None;
		}
	}
}
