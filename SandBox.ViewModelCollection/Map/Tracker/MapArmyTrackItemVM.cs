using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.Tracker;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Map.Tracker
{
	// Token: 0x02000047 RID: 71
	public class MapArmyTrackItemVM : MapTrackerItemVM<Army>
	{
		// Token: 0x06000482 RID: 1154 RVA: 0x00011DDF File Offset: 0x0000FFDF
		public MapArmyTrackItemVM(Army trackableObject)
			: base(trackableObject)
		{
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00011DE8 File Offset: 0x0000FFE8
		protected override void OnShowTooltip()
		{
			InformationManager.ShowTooltip(typeof(Army), new object[] { base.TrackedObject, true, false });
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00011E1A File Offset: 0x0001001A
		protected override bool IsVisibleOnMap()
		{
			MobileParty leaderParty = base.TrackedObject.LeaderParty;
			return leaderParty != null && !leaderParty.IsVisible;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00011E35 File Offset: 0x00010035
		protected override bool GetCanToggleTrack()
		{
			return true;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00011E38 File Offset: 0x00010038
		protected override string GetTrackerType()
		{
			return "Army";
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00011E3F File Offset: 0x0001003F
		protected override CampaignUIHelper.IssueQuestFlags GetRelatedQuests()
		{
			return CampaignUIHelper.IssueQuestFlags.None;
		}
	}
}
