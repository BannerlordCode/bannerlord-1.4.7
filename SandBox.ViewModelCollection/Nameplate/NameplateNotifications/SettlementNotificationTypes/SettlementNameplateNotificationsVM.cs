using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications.SettlementNotificationTypes
{
	// Token: 0x02000027 RID: 39
	public class SettlementNameplateNotificationsVM : ViewModel
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000E808 File Offset: 0x0000CA08
		// (set) Token: 0x0600035F RID: 863 RVA: 0x0000E810 File Offset: 0x0000CA10
		public bool IsEventsRegistered { get; private set; }

		// Token: 0x06000360 RID: 864 RVA: 0x0000E819 File Offset: 0x0000CA19
		public SettlementNameplateNotificationsVM(Settlement settlement)
		{
			this._settlement = settlement;
			this.Notifications = new MBBindingList<SettlementNotificationItemBaseVM>();
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000E833 File Offset: 0x0000CA33
		public void Tick()
		{
			this._tickSinceEnabled++;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000E844 File Offset: 0x0000CA44
		private void OnTroopRecruited(Hero recruiterHero, Settlement settlement, Hero troopSource, CharacterObject troop, int amount)
		{
			if (amount > 0 && settlement == this._settlement && this._settlement.IsInspected && recruiterHero != null && (recruiterHero.CurrentSettlement == this._settlement || (recruiterHero.PartyBelongedTo != null && recruiterHero.PartyBelongedTo.LastVisitedSettlement == this._settlement)))
			{
				TroopRecruitmentNotificationItemVM updatableNotificationByPredicate = this.GetUpdatableNotificationByPredicate<TroopRecruitmentNotificationItemVM>((TroopRecruitmentNotificationItemVM n) => n.RecruiterHero == recruiterHero);
				if (updatableNotificationByPredicate != null)
				{
					updatableNotificationByPredicate.AddNewAction(amount);
					return;
				}
				this.Notifications.Add(new TroopRecruitmentNotificationItemVM(new Action<SettlementNotificationItemBaseVM>(this.RemoveItem), recruiterHero, amount, this._tickSinceEnabled));
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000E910 File Offset: 0x0000CB10
		private void OnCaravanTransactionCompleted(MobileParty caravanParty, Town town, List<ValueTuple<EquipmentElement, int>> items)
		{
			if (this._settlement != town.Owner.Settlement)
			{
				return;
			}
			CaravanTransactionNotificationItemVM updatableNotificationByPredicate = this.GetUpdatableNotificationByPredicate<CaravanTransactionNotificationItemVM>((CaravanTransactionNotificationItemVM n) => n.CaravanParty == caravanParty);
			if (updatableNotificationByPredicate != null)
			{
				updatableNotificationByPredicate.AddNewItems(items);
				return;
			}
			this.Notifications.Add(new CaravanTransactionNotificationItemVM(new Action<SettlementNotificationItemBaseVM>(this.RemoveItem), caravanParty, items, this._tickSinceEnabled));
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000E988 File Offset: 0x0000CB88
		private void OnPrisonerSold(PartyBase sellerParty, PartyBase buyerParty, TroopRoster prisoners)
		{
			if (sellerParty.IsMobile && buyerParty != null && buyerParty.IsSettlement && buyerParty.Settlement == this._settlement && this._settlement.IsInspected && prisoners.Count > 0 && sellerParty.LeaderHero != null)
			{
				MobileParty sellerMobileParty = sellerParty.MobileParty;
				PrisonerSoldNotificationItemVM updatableNotificationByPredicate = this.GetUpdatableNotificationByPredicate<PrisonerSoldNotificationItemVM>((PrisonerSoldNotificationItemVM n) => n.Party == sellerMobileParty);
				if (updatableNotificationByPredicate != null)
				{
					updatableNotificationByPredicate.AddNewPrisoners(prisoners);
					return;
				}
				this.Notifications.Add(new PrisonerSoldNotificationItemVM(new Action<SettlementNotificationItemBaseVM>(this.RemoveItem), sellerMobileParty, prisoners, this._tickSinceEnabled));
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000EA38 File Offset: 0x0000CC38
		private void OnTroopGivenToSettlement(Hero giverHero, Settlement givenSettlement, TroopRoster givenTroops)
		{
			if (this._settlement.IsInspected && givenTroops.TotalManCount > 0 && giverHero != null && givenSettlement == this._settlement)
			{
				TroopGivenToSettlementNotificationItemVM updatableNotificationByPredicate = this.GetUpdatableNotificationByPredicate<TroopGivenToSettlementNotificationItemVM>((TroopGivenToSettlementNotificationItemVM n) => n.GiverHero == giverHero);
				if (updatableNotificationByPredicate != null)
				{
					updatableNotificationByPredicate.AddNewAction(givenTroops);
					return;
				}
				this.Notifications.Add(new TroopGivenToSettlementNotificationItemVM(new Action<SettlementNotificationItemBaseVM>(this.RemoveItem), giverHero, givenTroops, this._tickSinceEnabled));
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000EAC0 File Offset: 0x0000CCC0
		private void OnItemSold(PartyBase receiverParty, PartyBase payerParty, ItemRosterElement item, int number, Settlement currentSettlement)
		{
			if (this._settlement.IsInspected && number > 0 && currentSettlement == this._settlement)
			{
				int num = (receiverParty.IsSettlement ? (-1) : 1);
				ItemSoldNotificationItemVM updatableNotificationByPredicate = this.GetUpdatableNotificationByPredicate<ItemSoldNotificationItemVM>((ItemSoldNotificationItemVM n) => n.Item.EquipmentElement.Item == item.EquipmentElement.Item && (n.PayerParty == receiverParty || n.PayerParty == payerParty));
				if (updatableNotificationByPredicate != null)
				{
					updatableNotificationByPredicate.AddNewTransaction(number * num);
					return;
				}
				this.Notifications.Add(new ItemSoldNotificationItemVM(new Action<SettlementNotificationItemBaseVM>(this.RemoveItem), receiverParty, payerParty, item, number * num, this._tickSinceEnabled));
			}
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000EB6F File Offset: 0x0000CD6F
		private void OnIssueUpdated(IssueBase issue, IssueBase.IssueUpdateDetails updateType, Hero relatedHero)
		{
			if (updateType == IssueBase.IssueUpdateDetails.IssueFinishedByAILord && relatedHero != null && relatedHero.CurrentSettlement == this._settlement)
			{
				this.Notifications.Add(new IssueSolvedByLordNotificationItemVM(new Action<SettlementNotificationItemBaseVM>(this.RemoveItem), relatedHero, this._tickSinceEnabled));
			}
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000EBAC File Offset: 0x0000CDAC
		private void OnShipOwnerChanged(Ship ship, PartyBase oldOwner, ChangeShipOwnerAction.ShipOwnerChangeDetail detail)
		{
			if (this._settlement.IsInspected && detail == ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByTrade && (oldOwner == this._settlement.Party || ship.Owner == this._settlement.Party))
			{
				bool flag = ship.Owner == this._settlement.Party;
				int num = (flag ? (-1) : 1);
				PartyBase heroParty = (flag ? oldOwner : ship.Owner);
				ShipSoldNotificationItemVM updatableNotificationByPredicate = this.GetUpdatableNotificationByPredicate<ShipSoldNotificationItemVM>((ShipSoldNotificationItemVM n) => n.Ship.ShipHull.Name == ship.ShipHull.Name && n.SettlementParty == this._settlement.Party && n.HeroParty == heroParty);
				if (updatableNotificationByPredicate != null)
				{
					updatableNotificationByPredicate.AddNewTransaction(num);
					return;
				}
				this.Notifications.Add(new ShipSoldNotificationItemVM(new Action<SettlementNotificationItemBaseVM>(this.RemoveItem), ship, this._settlement.Party, heroParty, num, this._tickSinceEnabled));
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000ECBB File Offset: 0x0000CEBB
		private void RemoveItem(SettlementNotificationItemBaseVM item)
		{
			this.Notifications.Remove(item);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000ECCC File Offset: 0x0000CECC
		public void RegisterEvents()
		{
			if (!this.IsEventsRegistered)
			{
				CampaignEvents.OnTroopRecruitedEvent.AddNonSerializedListener(this, new Action<Hero, Settlement, Hero, CharacterObject, int>(this.OnTroopRecruited));
				CampaignEvents.OnPrisonerSoldEvent.AddNonSerializedListener(this, new Action<PartyBase, PartyBase, TroopRoster>(this.OnPrisonerSold));
				CampaignEvents.OnCaravanTransactionCompletedEvent.AddNonSerializedListener(this, new Action<MobileParty, Town, List<ValueTuple<EquipmentElement, int>>>(this.OnCaravanTransactionCompleted));
				CampaignEvents.OnTroopGivenToSettlementEvent.AddNonSerializedListener(this, new Action<Hero, Settlement, TroopRoster>(this.OnTroopGivenToSettlement));
				CampaignEvents.OnItemSoldEvent.AddNonSerializedListener(this, new Action<PartyBase, PartyBase, ItemRosterElement, int, Settlement>(this.OnItemSold));
				CampaignEvents.OnIssueUpdatedEvent.AddNonSerializedListener(this, new Action<IssueBase, IssueBase.IssueUpdateDetails, Hero>(this.OnIssueUpdated));
				CampaignEvents.OnShipOwnerChangedEvent.AddNonSerializedListener(this, new Action<Ship, PartyBase, ChangeShipOwnerAction.ShipOwnerChangeDetail>(this.OnShipOwnerChanged));
				this._tickSinceEnabled = 0;
				this.IsEventsRegistered = true;
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000ED94 File Offset: 0x0000CF94
		public void UnloadEvents()
		{
			if (this.IsEventsRegistered)
			{
				CampaignEvents.OnTroopRecruitedEvent.ClearListeners(this);
				CampaignEvents.OnItemSoldEvent.ClearListeners(this);
				CampaignEvents.OnPrisonerSoldEvent.ClearListeners(this);
				CampaignEvents.OnCaravanTransactionCompletedEvent.ClearListeners(this);
				CampaignEvents.OnTroopGivenToSettlementEvent.ClearListeners(this);
				CampaignEvents.OnIssueUpdatedEvent.ClearListeners(this);
				CampaignEvents.OnShipOwnerChangedEvent.ClearListeners(this);
				this._tickSinceEnabled = 0;
				this.IsEventsRegistered = false;
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000EE04 File Offset: 0x0000D004
		public bool IsValidItemForNotification(ItemRosterElement item)
		{
			switch (item.EquipmentElement.Item.Type)
			{
			case ItemObject.ItemTypeEnum.Horse:
			case ItemObject.ItemTypeEnum.OneHandedWeapon:
			case ItemObject.ItemTypeEnum.TwoHandedWeapon:
			case ItemObject.ItemTypeEnum.Polearm:
			case ItemObject.ItemTypeEnum.Arrows:
			case ItemObject.ItemTypeEnum.Bolts:
			case ItemObject.ItemTypeEnum.SlingStones:
			case ItemObject.ItemTypeEnum.Shield:
			case ItemObject.ItemTypeEnum.Bow:
			case ItemObject.ItemTypeEnum.Crossbow:
			case ItemObject.ItemTypeEnum.Sling:
			case ItemObject.ItemTypeEnum.Thrown:
			case ItemObject.ItemTypeEnum.Goods:
			case ItemObject.ItemTypeEnum.HeadArmor:
			case ItemObject.ItemTypeEnum.BodyArmor:
			case ItemObject.ItemTypeEnum.LegArmor:
			case ItemObject.ItemTypeEnum.HandArmor:
			case ItemObject.ItemTypeEnum.Pistol:
			case ItemObject.ItemTypeEnum.Musket:
			case ItemObject.ItemTypeEnum.Bullets:
			case ItemObject.ItemTypeEnum.Animal:
			case ItemObject.ItemTypeEnum.ChestArmor:
			case ItemObject.ItemTypeEnum.Cape:
			case ItemObject.ItemTypeEnum.HorseHarness:
				return true;
			}
			return false;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
		private T GetUpdatableNotificationByPredicate<T>(Func<T, bool> predicate) where T : SettlementNotificationItemBaseVM
		{
			for (int i = 0; i < this.Notifications.Count; i++)
			{
				SettlementNotificationItemBaseVM settlementNotificationItemBaseVM = this.Notifications[i];
				T t;
				if (this._tickSinceEnabled - settlementNotificationItemBaseVM.CreatedTick < 10 && (t = settlementNotificationItemBaseVM as T) != null && predicate(t))
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0000EF0B File Offset: 0x0000D10B
		// (set) Token: 0x0600036F RID: 879 RVA: 0x0000EF13 File Offset: 0x0000D113
		public MBBindingList<SettlementNotificationItemBaseVM> Notifications
		{
			get
			{
				return this._notifications;
			}
			set
			{
				if (value != this._notifications)
				{
					this._notifications = value;
					base.OnPropertyChangedWithValue<MBBindingList<SettlementNotificationItemBaseVM>>(value, "Notifications");
				}
			}
		}

		// Token: 0x040001B9 RID: 441
		private readonly Settlement _settlement;

		// Token: 0x040001BB RID: 443
		private int _tickSinceEnabled;

		// Token: 0x040001BC RID: 444
		private const int _maxTickDeltaToCongregate = 10;

		// Token: 0x040001BD RID: 445
		private MBBindingList<SettlementNotificationItemBaseVM> _notifications;
	}
}
