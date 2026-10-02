using System;
using TaleWorlds.CampaignSystem.Actions;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003D8 RID: 984
	public class CampaignFactionManagerBehaviour : CampaignBehaviorBase
	{
		// Token: 0x06003B0B RID: 15115 RVA: 0x000F5608 File Offset: 0x000F3808
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomCreated));
			CampaignEvents.OnClanCreatedEvent.AddNonSerializedListener(this, new Action<Clan, bool>(this.OnClanCreated));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdomEvent));
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x000F5688 File Offset: 0x000F3888
		private void OnClanChangedKingdomEvent(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool arg5)
		{
			CampaignFactionManagerBehaviour.RefreshFactionsAtWarWith();
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x000F568F File Offset: 0x000F388F
		private void OnNewGameCreated(CampaignGameStarter obj)
		{
			CampaignFactionManagerBehaviour.RefreshFactionsAtWarWith();
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x000F5696 File Offset: 0x000F3896
		private void OnGameLoaded(CampaignGameStarter obj)
		{
			CampaignFactionManagerBehaviour.RefreshFactionsAtWarWith();
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x000F569D File Offset: 0x000F389D
		private void OnClanCreated(Clan obj, bool isCompanion)
		{
			CampaignFactionManagerBehaviour.RefreshFactionsAtWarWith();
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x000F56A4 File Offset: 0x000F38A4
		private void OnKingdomCreated(Kingdom obj)
		{
			CampaignFactionManagerBehaviour.RefreshFactionsAtWarWith();
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x000F56AC File Offset: 0x000F38AC
		private static void RefreshFactionsAtWarWith()
		{
			foreach (Kingdom kingdom in Kingdom.All)
			{
				kingdom.UpdateFactionsAtWarWith();
			}
			foreach (Clan clan in Clan.All)
			{
				clan.UpdateFactionsAtWarWith();
			}
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x000F573C File Offset: 0x000F393C
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
