using System;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200044B RID: 1099
	public class TributesCampaignBehaviour : CampaignBehaviorBase
	{
		// Token: 0x060046A6 RID: 18086 RVA: 0x00161772 File Offset: 0x0015F972
		public override void RegisterEvents()
		{
			CampaignEvents.OnClanEarnedGoldFromTributeEvent.AddNonSerializedListener(this, new Action<Clan, IFaction>(TributesCampaignBehaviour.OnClanEarnedGoldFromTribute));
		}

		// Token: 0x060046A7 RID: 18087 RVA: 0x0016178C File Offset: 0x0015F98C
		private static void OnClanEarnedGoldFromTribute(Clan clan, IFaction payerFaction)
		{
			StanceLink stanceWith = clan.MapFaction.GetStanceWith(payerFaction);
			if ((clan == Clan.PlayerClan || payerFaction == Clan.PlayerClan.MapFaction) && stanceWith.GetRemainingTributePaymentCount() == 0)
			{
				bool flag = payerFaction == Clan.PlayerClan.MapFaction;
				TextObject textObject = (flag ? new TextObject("{=LJFXfmpn}The tribute your kingdom owed to {ENEMY_FACTION} is now complete.", null) : new TextObject("{=aod7KVc8}The tribute {ENEMY_FACTION} owed to your kingdom is now complete.", null));
				IFaction faction = (flag ? clan.MapFaction : payerFaction);
				textObject.SetTextVariable("ENEMY_FACTION", faction.Name);
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new TributeFinishedMapNotification(textObject, faction));
			}
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x00161825 File Offset: 0x0015FA25
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
