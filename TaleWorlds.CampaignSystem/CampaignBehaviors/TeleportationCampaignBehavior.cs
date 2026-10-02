using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000445 RID: 1093
	public class TeleportationCampaignBehavior : CampaignBehaviorBase, ITeleportationCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x06004658 RID: 18008 RVA: 0x0015F691 File Offset: 0x0015D891
		private TextObject _partyLeaderChangeNotificationText
		{
			get
			{
				return new TextObject("{=QSaufZ9i}One of your parties has lost its leader. It will disband after a day has passed. You can assign a new clan member to lead it, if you wish to keep the party.", null);
			}
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x0015F6A0 File Offset: 0x0015D8A0
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickParty));
			CampaignEvents.HeroComesOfAgeEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroComesOfAge));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.OnPartyDisbandStartedEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyDisbandStarted));
			CampaignEvents.OnGovernorChangedEvent.AddNonSerializedListener(this, new Action<Town, Hero, Hero>(this.OnGovernorChanged));
			CampaignEvents.OnHeroTeleportationRequestedEvent.AddNonSerializedListener(this, new Action<Hero, Settlement, MobileParty, TeleportHeroAction.TeleportationDetail>(this.OnHeroTeleportationRequested));
			CampaignEvents.OnPartyDisbandedEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnPartyDisbanded));
			CampaignEvents.OnClanLeaderChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnClanLeaderChanged));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.HeroPrisonerTaken));
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x0015F7C1 File Offset: 0x0015D9C1
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<TeleportationCampaignBehavior.TeleportationData>>("_teleportationList", ref this._teleportationList);
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x0015F7D8 File Offset: 0x0015D9D8
		public bool GetTargetOfTeleportingHero(Hero teleportingHero, out bool isGovernor, out bool isPartyLeader, out IMapPoint target)
		{
			isGovernor = false;
			isPartyLeader = false;
			target = null;
			for (int i = 0; i < this._teleportationList.Count; i++)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TeleportingHero == teleportingHero)
				{
					if (teleportationData.TargetSettlement != null)
					{
						isGovernor = teleportationData.IsGovernor;
						target = teleportationData.TargetSettlement;
						return true;
					}
					if (teleportationData.TargetParty != null)
					{
						isPartyLeader = teleportationData.IsPartyLeader;
						target = teleportationData.TargetParty;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x0015F854 File Offset: 0x0015DA54
		public CampaignTime GetHeroArrivalTimeToDestination(Hero teleportingHero)
		{
			TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList.FirstOrDefaultQ<TeleportationCampaignBehavior.TeleportationData>((TeleportationCampaignBehavior.TeleportationData x) => x.TeleportingHero == teleportingHero);
			if (teleportationData != null)
			{
				return teleportationData.TeleportationTime;
			}
			return CampaignTime.Never;
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x0015F898 File Offset: 0x0015DA98
		private void HourlyTick()
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TeleportationTime.IsPast && Campaign.Current.Models.DelayedTeleportationModel.CanPerformImmediateTeleport(teleportationData.TeleportingHero, teleportationData.TargetParty, teleportationData.TargetSettlement))
				{
					TeleportationCampaignBehavior.TeleportationData teleportationData2 = teleportationData;
					this.RemoveTeleportationData(teleportationData, false, true);
					this.ApplyImmediateTeleport(teleportationData2);
				}
			}
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x0015F914 File Offset: 0x0015DB14
		private void DailyTickParty(MobileParty mobileParty)
		{
			if (mobileParty.IsActive && mobileParty.Army == null && mobileParty.MapEvent == null && mobileParty.LeaderHero != null && mobileParty.LeaderHero.IsNoncombatant && mobileParty.LeaderHero.DeathMark == KillCharacterAction.KillCharacterActionDetail.None && mobileParty.ActualClan != null && mobileParty.ActualClan != Clan.PlayerClan && mobileParty.ActualClan.Leader != mobileParty.LeaderHero && !mobileParty.IsInRaftState && (!mobileParty.IsCurrentlyAtSea || mobileParty.CurrentSettlement != null))
			{
				MBList<Hero> mblist = mobileParty.ActualClan.Heroes.WhereQ<Hero>((Hero h) => h.IsActive && h.IsCommander && h.PartyBelongedTo == null).ToMBList<Hero>();
				if (!mblist.IsEmpty<Hero>())
				{
					Hero leaderHero = mobileParty.LeaderHero;
					mobileParty.RemovePartyLeader();
					MakeHeroFugitiveAction.Apply(leaderHero, false);
					TeleportHeroAction.ApplyDelayedTeleportToPartyAsPartyLeader(mblist.GetRandomElementInefficiently<Hero>(), mobileParty);
				}
			}
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x0015FA10 File Offset: 0x0015DC10
		private void OnHeroComesOfAge(Hero hero)
		{
			if (hero.Clan != Clan.PlayerClan && !hero.IsNoncombatant)
			{
				foreach (WarPartyComponent warPartyComponent in hero.Clan.WarPartyComponents)
				{
					MobileParty mobileParty = warPartyComponent.MobileParty;
					if (mobileParty != null && mobileParty.Army == null && mobileParty.MapEvent == null && mobileParty.LeaderHero != null && mobileParty.LeaderHero.IsNoncombatant && (!mobileParty.IsCurrentlyAtSea || mobileParty.CurrentSettlement != null))
					{
						Hero leaderHero = mobileParty.LeaderHero;
						mobileParty.RemovePartyLeader();
						MakeHeroFugitiveAction.Apply(leaderHero, false);
						TeleportHeroAction.ApplyDelayedTeleportToPartyAsPartyLeader(hero, warPartyComponent.Party.MobileParty);
						break;
					}
				}
			}
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x0015FAE0 File Offset: 0x0015DCE0
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TargetParty != null && teleportationData.TargetParty == mobileParty)
				{
					this.RemoveTeleportationData(teleportationData, true, false);
				}
			}
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x0015FB2C File Offset: 0x0015DD2C
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TargetSettlement != null && teleportationData.TargetSettlement == settlement && newOwner.Clan != teleportationData.TeleportingHero.Clan)
				{
					this.RemoveTeleportationData(teleportationData, true, true);
				}
			}
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x0015FB8C File Offset: 0x0015DD8C
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TeleportingHero == victim)
				{
					this.RemoveTeleportationData(teleportationData, true, true);
				}
			}
		}

		// Token: 0x06004663 RID: 18019 RVA: 0x0015FBD0 File Offset: 0x0015DDD0
		private void OnPartyDisbandStarted(MobileParty disbandParty)
		{
			if (disbandParty.ActualClan == Clan.PlayerClan && disbandParty.LeaderHero == null && (disbandParty.IsLordParty || disbandParty.IsCaravan) && !disbandParty.IsCurrentlyAtSea)
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new PartyLeaderChangeNotification(disbandParty, this._partyLeaderChangeNotificationText));
			}
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TargetParty != null && teleportationData.TargetParty == disbandParty)
				{
					this.RemoveTeleportationData(teleportationData, true, false);
				}
			}
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x0015FC64 File Offset: 0x0015DE64
		private void OnGovernorChanged(Town fortification, Hero oldGovernor, Hero newGovernor)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TeleportingHero != newGovernor && teleportationData.IsGovernor && teleportationData.TargetSettlement.Town == fortification)
				{
					teleportationData.IsGovernor = false;
				}
			}
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x0015FCBC File Offset: 0x0015DEBC
		private void OnHeroTeleportationRequested(Hero hero, Settlement targetSettlement, MobileParty targetParty, TeleportHeroAction.TeleportationDetail detail)
		{
			switch (detail)
			{
			case TeleportHeroAction.TeleportationDetail.ImmediateTeleportToSettlement:
			{
				for (int i = this._teleportationList.Count - 1; i >= 0; i--)
				{
					TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
					if (hero == teleportationData.TeleportingHero && teleportationData.TargetSettlement == targetSettlement)
					{
						this.RemoveTeleportationData(teleportationData, true, false);
					}
				}
				break;
			}
			case TeleportHeroAction.TeleportationDetail.ImmediateTeleportToParty:
			case TeleportHeroAction.TeleportationDetail.ImmediateTeleportToPartyAsPartyLeader:
				break;
			case TeleportHeroAction.TeleportationDetail.DelayedTeleportToSettlement:
			case TeleportHeroAction.TeleportationDetail.DelayedTeleportToSettlementAsGovernor:
				this._teleportationList.Add(new TeleportationCampaignBehavior.TeleportationData(hero, targetSettlement, detail == TeleportHeroAction.TeleportationDetail.DelayedTeleportToSettlementAsGovernor));
				return;
			case TeleportHeroAction.TeleportationDetail.DelayedTeleportToParty:
			case TeleportHeroAction.TeleportationDetail.DelayedTeleportToPartyAsPartyLeader:
				if (detail == TeleportHeroAction.TeleportationDetail.DelayedTeleportToPartyAsPartyLeader)
				{
					for (int j = this._teleportationList.Count - 1; j >= 0; j--)
					{
						TeleportationCampaignBehavior.TeleportationData teleportationData2 = this._teleportationList[j];
						if (teleportationData2.TargetParty == targetParty && teleportationData2.IsPartyLeader)
						{
							this.RemoveTeleportationData(teleportationData2, true, false);
						}
					}
				}
				this._teleportationList.Add(new TeleportationCampaignBehavior.TeleportationData(hero, targetParty, detail == TeleportHeroAction.TeleportationDetail.DelayedTeleportToPartyAsPartyLeader));
				return;
			default:
				return;
			}
		}

		// Token: 0x06004666 RID: 18022 RVA: 0x0015FDA4 File Offset: 0x0015DFA4
		private void OnPartyDisbanded(MobileParty disbandParty, Settlement relatedSettlement)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TargetParty != null && teleportationData.TargetParty == disbandParty)
				{
					this.RemoveTeleportationData(teleportationData, true, true);
				}
			}
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x0015FDF0 File Offset: 0x0015DFF0
		private void OnClanLeaderChanged(Hero oldLeader, Hero newLeader)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TeleportingHero == newLeader && !teleportationData.IsPartyLeader)
				{
					this.RemoveTeleportationData(teleportationData, true, true);
				}
			}
		}

		// Token: 0x06004668 RID: 18024 RVA: 0x0015FE3C File Offset: 0x0015E03C
		private void HeroPrisonerTaken(PartyBase capturer, Hero prisoner)
		{
			for (int i = this._teleportationList.Count - 1; i >= 0; i--)
			{
				TeleportationCampaignBehavior.TeleportationData teleportationData = this._teleportationList[i];
				if (teleportationData.TeleportingHero == prisoner)
				{
					this.RemoveTeleportationData(teleportationData, true, true);
				}
			}
		}

		// Token: 0x06004669 RID: 18025 RVA: 0x0015FE80 File Offset: 0x0015E080
		private void RemoveTeleportationData(TeleportationCampaignBehavior.TeleportationData data, bool isCanceled, bool disbandTargetParty = true)
		{
			this._teleportationList.Remove(data);
			if (isCanceled)
			{
				if (data.TeleportingHero.IsTraveling && data.TeleportingHero.DeathMark == KillCharacterAction.KillCharacterActionDetail.None)
				{
					MakeHeroFugitiveAction.Apply(data.TeleportingHero, false);
				}
				if (data.TargetParty != null)
				{
					if (data.TargetParty.ActualClan == Clan.PlayerClan)
					{
						CampaignEventDispatcher.Instance.OnPartyLeaderChangeOfferCanceled(data.TargetParty);
					}
					if (disbandTargetParty && data.TargetParty.IsActive && data.IsPartyLeader)
					{
						IDisbandPartyCampaignBehavior behavior = Campaign.Current.CampaignBehaviorManager.GetBehavior<IDisbandPartyCampaignBehavior>();
						if (behavior != null && !behavior.IsPartyWaitingForDisband(data.TargetParty))
						{
							DisbandPartyAction.StartDisband(data.TargetParty);
						}
					}
				}
			}
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x0015FF34 File Offset: 0x0015E134
		private void ApplyImmediateTeleport(TeleportationCampaignBehavior.TeleportationData data)
		{
			if (data.TargetSettlement == null)
			{
				if (data.TargetParty != null)
				{
					if (data.IsPartyLeader)
					{
						TeleportHeroAction.ApplyImmediateTeleportToPartyAsPartyLeader(data.TeleportingHero, data.TargetParty);
						return;
					}
					TeleportHeroAction.ApplyImmediateTeleportToParty(data.TeleportingHero, data.TargetParty);
				}
				return;
			}
			if (data.IsGovernor)
			{
				data.TargetSettlement.Town.Governor = data.TeleportingHero;
				TeleportHeroAction.ApplyImmediateTeleportToSettlement(data.TeleportingHero, data.TargetSettlement);
				return;
			}
			TeleportHeroAction.ApplyImmediateTeleportToSettlement(data.TeleportingHero, data.TargetSettlement);
		}

		// Token: 0x040013BA RID: 5050
		private List<TeleportationCampaignBehavior.TeleportationData> _teleportationList = new List<TeleportationCampaignBehavior.TeleportationData>();

		// Token: 0x02000852 RID: 2130
		public class TeleportationCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x0600680B RID: 26635 RVA: 0x001C9D64 File Offset: 0x001C7F64
			public TeleportationCampaignBehaviorTypeDefiner()
				: base(151000)
			{
			}

			// Token: 0x0600680C RID: 26636 RVA: 0x001C9D71 File Offset: 0x001C7F71
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(TeleportationCampaignBehavior.TeleportationData), 1, null);
			}

			// Token: 0x0600680D RID: 26637 RVA: 0x001C9D85 File Offset: 0x001C7F85
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(List<TeleportationCampaignBehavior.TeleportationData>));
			}
		}

		// Token: 0x02000853 RID: 2131
		internal class TeleportationData
		{
			// Token: 0x0600680E RID: 26638 RVA: 0x001C9D98 File Offset: 0x001C7F98
			public TeleportationData(Hero teleportingHero, Settlement targetSettlement, bool isGovernor)
			{
				this.TeleportingHero = teleportingHero;
				this.TeleportationTime = CampaignTime.HoursFromNow(Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(teleportingHero, targetSettlement.Party).ResultNumber);
				this.TargetSettlement = targetSettlement;
				this.IsGovernor = isGovernor;
				this.TargetParty = null;
				this.IsPartyLeader = false;
			}

			// Token: 0x0600680F RID: 26639 RVA: 0x001C9DFC File Offset: 0x001C7FFC
			public TeleportationData(Hero teleportingHero, MobileParty targetParty, bool isPartyLeader)
			{
				this.TeleportingHero = teleportingHero;
				this.TeleportationTime = CampaignTime.HoursFromNow(Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(teleportingHero, targetParty.Party).ResultNumber);
				this.TargetParty = targetParty;
				this.IsPartyLeader = isPartyLeader;
				this.TargetSettlement = null;
				this.IsGovernor = false;
			}

			// Token: 0x06006810 RID: 26640 RVA: 0x001C9E60 File Offset: 0x001C8060
			internal static void AutoGeneratedStaticCollectObjectsTeleportationData(object o, List<object> collectedObjects)
			{
				((TeleportationCampaignBehavior.TeleportationData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006811 RID: 26641 RVA: 0x001C9E6E File Offset: 0x001C806E
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.TeleportingHero);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.TeleportationTime, collectedObjects);
				collectedObjects.Add(this.TargetSettlement);
				collectedObjects.Add(this.TargetParty);
			}

			// Token: 0x06006812 RID: 26642 RVA: 0x001C9EA5 File Offset: 0x001C80A5
			internal static object AutoGeneratedGetMemberValueTeleportingHero(object o)
			{
				return ((TeleportationCampaignBehavior.TeleportationData)o).TeleportingHero;
			}

			// Token: 0x06006813 RID: 26643 RVA: 0x001C9EB2 File Offset: 0x001C80B2
			internal static object AutoGeneratedGetMemberValueTeleportationTime(object o)
			{
				return ((TeleportationCampaignBehavior.TeleportationData)o).TeleportationTime;
			}

			// Token: 0x06006814 RID: 26644 RVA: 0x001C9EC4 File Offset: 0x001C80C4
			internal static object AutoGeneratedGetMemberValueTargetSettlement(object o)
			{
				return ((TeleportationCampaignBehavior.TeleportationData)o).TargetSettlement;
			}

			// Token: 0x06006815 RID: 26645 RVA: 0x001C9ED1 File Offset: 0x001C80D1
			internal static object AutoGeneratedGetMemberValueIsGovernor(object o)
			{
				return ((TeleportationCampaignBehavior.TeleportationData)o).IsGovernor;
			}

			// Token: 0x06006816 RID: 26646 RVA: 0x001C9EE3 File Offset: 0x001C80E3
			internal static object AutoGeneratedGetMemberValueTargetParty(object o)
			{
				return ((TeleportationCampaignBehavior.TeleportationData)o).TargetParty;
			}

			// Token: 0x06006817 RID: 26647 RVA: 0x001C9EF0 File Offset: 0x001C80F0
			internal static object AutoGeneratedGetMemberValueIsPartyLeader(object o)
			{
				return ((TeleportationCampaignBehavior.TeleportationData)o).IsPartyLeader;
			}

			// Token: 0x040023D7 RID: 9175
			[SaveableField(1)]
			public Hero TeleportingHero;

			// Token: 0x040023D8 RID: 9176
			[SaveableField(2)]
			public CampaignTime TeleportationTime;

			// Token: 0x040023D9 RID: 9177
			[SaveableField(3)]
			public Settlement TargetSettlement;

			// Token: 0x040023DA RID: 9178
			[SaveableField(4)]
			public bool IsGovernor;

			// Token: 0x040023DB RID: 9179
			[SaveableField(5)]
			public MobileParty TargetParty;

			// Token: 0x040023DC RID: 9180
			[SaveableField(6)]
			public bool IsPartyLeader;
		}
	}
}
