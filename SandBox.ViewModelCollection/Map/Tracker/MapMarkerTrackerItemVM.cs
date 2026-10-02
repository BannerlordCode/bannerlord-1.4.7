using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.Tracker;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Map.Tracker
{
	// Token: 0x02000048 RID: 72
	public class MapMarkerTrackerItemVM : MapTrackerItemVM<MapMarker>
	{
		// Token: 0x06000488 RID: 1160 RVA: 0x00011E42 File Offset: 0x00010042
		public MapMarkerTrackerItemVM(MapMarker marker)
			: base(marker)
		{
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00011E4B File Offset: 0x0001004B
		protected override void OnShowTooltip()
		{
			InformationManager.ShowTooltip(typeof(MapMarker), new object[] { base.TrackedObject, true, false });
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00011E7D File Offset: 0x0001007D
		protected override bool IsVisibleOnMap()
		{
			return base.TrackedObject.IsVisibleOnMap;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00011E8A File Offset: 0x0001008A
		protected override bool GetCanToggleTrack()
		{
			return true;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00011E8D File Offset: 0x0001008D
		protected override string GetTrackerType()
		{
			return "Default";
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00011E94 File Offset: 0x00010094
		protected override CampaignUIHelper.IssueQuestFlags GetRelatedQuests()
		{
			CampaignUIHelper.IssueQuestFlags issueQuestFlags = CampaignUIHelper.IssueQuestFlags.None;
			QuestBase questBase = Campaign.Current.QuestManager.Quests.FirstOrDefault<QuestBase>((QuestBase q) => q.StringId == base.TrackedObject.QuestId);
			if (questBase != null)
			{
				issueQuestFlags = (questBase.IsSpecialQuest ? CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest : CampaignUIHelper.IssueQuestFlags.ActiveIssue);
			}
			return issueQuestFlags;
		}
	}
}
