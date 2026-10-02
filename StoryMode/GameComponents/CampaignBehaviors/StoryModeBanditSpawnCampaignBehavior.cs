using System;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;

namespace StoryMode.GameComponents.CampaignBehaviors
{
	// Token: 0x02000052 RID: 82
	public class StoryModeBanditSpawnCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060004FF RID: 1279 RVA: 0x0001C178 File Offset: 0x0001A378
		public override void RegisterEvents()
		{
			if (!TutorialPhase.Instance.IsCompleted)
			{
				StoryModeEvents.OnStoryModeTutorialEndedEvent.AddNonSerializedListener(this, new Action(this.OnTutorialEnded));
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001C19D File Offset: 0x0001A39D
		private void OnTutorialEnded()
		{
			if (TutorialPhase.Instance.IsSkipped)
			{
				this.SpawnInitialBanditsAndLooters();
			}
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001C1BC File Offset: 0x0001A3BC
		private void SpawnInitialBanditsAndLooters()
		{
			BanditSpawnCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<BanditSpawnCampaignBehavior>();
			if (campaignBehavior != null)
			{
				campaignBehavior.InitializeInitialHideouts();
				campaignBehavior.SpawnBanditsAroundHideoutAtNewGame();
				campaignBehavior.SpawnLootersAtNewGame();
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001C1E9 File Offset: 0x0001A3E9
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
