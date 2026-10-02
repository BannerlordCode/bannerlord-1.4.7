using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000D8 RID: 216
	public class InventoryLogic
	{
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001489 RID: 5257 RVA: 0x0005F12C File Offset: 0x0005D32C
		// (set) Token: 0x0600148A RID: 5258 RVA: 0x0005F134 File Offset: 0x0005D334
		public bool DisableNetwork { get; set; }

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x0600148B RID: 5259 RVA: 0x0005F13D File Offset: 0x0005D33D
		// (set) Token: 0x0600148C RID: 5260 RVA: 0x0005F145 File Offset: 0x0005D345
		public Action<int> TotalAmountChange { get; set; }

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x0600148D RID: 5261 RVA: 0x0005F14E File Offset: 0x0005D34E
		// (set) Token: 0x0600148E RID: 5262 RVA: 0x0005F156 File Offset: 0x0005D356
		public Action DonationXpChange { get; set; }

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600148F RID: 5263 RVA: 0x0005F160 File Offset: 0x0005D360
		// (remove) Token: 0x06001490 RID: 5264 RVA: 0x0005F198 File Offset: 0x0005D398
		public event InventoryLogic.AfterResetDelegate AfterReset;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06001491 RID: 5265 RVA: 0x0005F1D0 File Offset: 0x0005D3D0
		// (remove) Token: 0x06001492 RID: 5266 RVA: 0x0005F208 File Offset: 0x0005D408
		public event InventoryLogic.ProcessResultListDelegate AfterTransfer;

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x0005F23D File Offset: 0x0005D43D
		// (set) Token: 0x06001494 RID: 5268 RVA: 0x0005F245 File Offset: 0x0005D445
		public TroopRoster RightMemberRoster { get; private set; }

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x0005F24E File Offset: 0x0005D44E
		// (set) Token: 0x06001496 RID: 5270 RVA: 0x0005F256 File Offset: 0x0005D456
		public TroopRoster LeftMemberRoster { get; private set; }

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x0005F25F File Offset: 0x0005D45F
		// (set) Token: 0x06001498 RID: 5272 RVA: 0x0005F267 File Offset: 0x0005D467
		public CharacterObject InitialEquipmentCharacter { get; private set; }

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x0005F270 File Offset: 0x0005D470
		// (set) Token: 0x0600149A RID: 5274 RVA: 0x0005F278 File Offset: 0x0005D478
		public bool IsTrading { get; private set; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x0600149B RID: 5275 RVA: 0x0005F281 File Offset: 0x0005D481
		// (set) Token: 0x0600149C RID: 5276 RVA: 0x0005F289 File Offset: 0x0005D489
		public bool IsSpecialActionsPermitted { get; private set; }

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x0005F292 File Offset: 0x0005D492
		// (set) Token: 0x0600149E RID: 5278 RVA: 0x0005F29A File Offset: 0x0005D49A
		public CharacterObject OwnerCharacter { get; private set; }

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x0600149F RID: 5279 RVA: 0x0005F2A3 File Offset: 0x0005D4A3
		// (set) Token: 0x060014A0 RID: 5280 RVA: 0x0005F2AB File Offset: 0x0005D4AB
		public MobileParty OwnerParty { get; private set; }

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x0005F2B4 File Offset: 0x0005D4B4
		// (set) Token: 0x060014A2 RID: 5282 RVA: 0x0005F2BC File Offset: 0x0005D4BC
		public PartyBase OtherParty { get; private set; }

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x060014A3 RID: 5283 RVA: 0x0005F2C5 File Offset: 0x0005D4C5
		// (set) Token: 0x060014A4 RID: 5284 RVA: 0x0005F2CD File Offset: 0x0005D4CD
		public IMarketData MarketData { get; private set; }

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x0005F2D6 File Offset: 0x0005D4D6
		// (set) Token: 0x060014A6 RID: 5286 RVA: 0x0005F2DE File Offset: 0x0005D4DE
		public InventoryLogic.CapacityData OtherSideCapacityData { get; private set; }

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x0005F2E8 File Offset: 0x0005D4E8
		public int OtherSideCurrentWeight
		{
			get
			{
				float num = 0f;
				PartyBase otherParty = this.OtherParty;
				MobileParty mobileParty = ((otherParty != null) ? otherParty.MobileParty : null);
				if (mobileParty != null)
				{
					ItemRoster itemRoster = this._rosters[0];
					InventoryCapacityModel inventoryCapacityModel = Campaign.Current.Models.InventoryCapacityModel;
					for (int i = 0; i < itemRoster.Count; i++)
					{
						TextObject textObject;
						num += inventoryCapacityModel.GetItemEffectiveWeight(itemRoster[i].EquipmentElement, mobileParty, mobileParty.IsCurrentlyAtSea, out textObject) * (float)itemRoster[i].Amount;
					}
				}
				else if (this._inventoryMode == InventoryScreenHelper.InventoryMode.Warehouse && this._workshopWarehouseBehavior != null)
				{
					num = this._workshopWarehouseBehavior.GetWarehouseItemRosterWeight(MobileParty.MainParty.CurrentSettlement);
				}
				return MathF.Ceiling(num);
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x060014A8 RID: 5288 RVA: 0x0005F3A4 File Offset: 0x0005D5A4
		// (set) Token: 0x060014A9 RID: 5289 RVA: 0x0005F3AC File Offset: 0x0005D5AC
		public TextObject LeftRosterName { get; private set; }

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x060014AA RID: 5290 RVA: 0x0005F3B5 File Offset: 0x0005D5B5
		// (set) Token: 0x060014AB RID: 5291 RVA: 0x0005F3BD File Offset: 0x0005D5BD
		public bool CanGainXpFromDiscarding { get; private set; }

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x060014AC RID: 5292 RVA: 0x0005F3C6 File Offset: 0x0005D5C6
		// (set) Token: 0x060014AD RID: 5293 RVA: 0x0005F3CE File Offset: 0x0005D5CE
		public bool IsOtherPartyFromPlayerClan { get; private set; }

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x060014AE RID: 5294 RVA: 0x0005F3D7 File Offset: 0x0005D5D7
		// (set) Token: 0x060014AF RID: 5295 RVA: 0x0005F3DF File Offset: 0x0005D5DF
		public InventoryListener InventoryListener { get; private set; }

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x060014B0 RID: 5296 RVA: 0x0005F3E8 File Offset: 0x0005D5E8
		public int TotalAmount
		{
			get
			{
				return this.TransactionDebt;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0005F3F0 File Offset: 0x0005D5F0
		public PartyBase OppositePartyFromListener
		{
			get
			{
				return this.InventoryListener.GetOppositeParty();
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x0005F3FD File Offset: 0x0005D5FD
		public SettlementComponent CurrentSettlementComponent
		{
			get
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				if (currentSettlement == null)
				{
					return null;
				}
				return currentSettlement.SettlementComponent;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x060014B3 RID: 5299 RVA: 0x0005F410 File Offset: 0x0005D610
		public MobileParty CurrentMobileParty
		{
			get
			{
				if (PlayerEncounter.Current != null)
				{
					return PlayerEncounter.EncounteredParty.MobileParty;
				}
				MapEvent mapEvent = PartyBase.MainParty.MapEvent;
				bool flag;
				if (mapEvent == null)
				{
					flag = null != null;
				}
				else
				{
					PartyBase leaderParty = mapEvent.GetLeaderParty(PartyBase.MainParty.OpponentSide);
					flag = ((leaderParty != null) ? leaderParty.MobileParty : null) != null;
				}
				if (flag)
				{
					return PartyBase.MainParty.MapEvent.GetLeaderParty(PartyBase.MainParty.OpponentSide).MobileParty;
				}
				return null;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x060014B4 RID: 5300 RVA: 0x0005F47D File Offset: 0x0005D67D
		// (set) Token: 0x060014B5 RID: 5301 RVA: 0x0005F485 File Offset: 0x0005D685
		public int TransactionDebt
		{
			get
			{
				return this._transactionDebt;
			}
			private set
			{
				if (value != this._transactionDebt)
				{
					this._transactionDebt = value;
					this.TotalAmountChange(this._transactionDebt);
				}
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x060014B6 RID: 5302 RVA: 0x0005F4A8 File Offset: 0x0005D6A8
		// (set) Token: 0x060014B7 RID: 5303 RVA: 0x0005F4B0 File Offset: 0x0005D6B0
		public float XpGainFromDonations
		{
			get
			{
				return this._xpGainFromDonations;
			}
			private set
			{
				if (value != this._xpGainFromDonations)
				{
					this._xpGainFromDonations = value;
					if (this._xpGainFromDonations < 0f)
					{
						this._xpGainFromDonations = 0f;
					}
					Action donationXpChange = this.DonationXpChange;
					if (donationXpChange == null)
					{
						return;
					}
					donationXpChange();
				}
			}
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x0005F4EC File Offset: 0x0005D6EC
		public InventoryLogic(MobileParty ownerParty, CharacterObject ownerCharacter, PartyBase merchantParty)
		{
			this._rosters = new ItemRoster[2];
			this._rostersBackup = new ItemRoster[2];
			this.OwnerParty = ownerParty;
			this.OwnerCharacter = ownerCharacter;
			this.OtherParty = merchantParty;
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0005F549 File Offset: 0x0005D749
		public InventoryLogic(PartyBase merchantParty)
			: this(MobileParty.MainParty, CharacterObject.PlayerCharacter, merchantParty)
		{
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x0005F55C File Offset: 0x0005D75C
		public void Initialize(ItemRoster leftItemRoster, MobileParty party, bool isTrading, bool isSpecialActionsPermitted, CharacterObject initialCharacterOfRightRoster, InventoryScreenHelper.InventoryCategoryType merchantItemType, IMarketData marketData, bool useBasePrices, InventoryScreenHelper.InventoryMode inventoryMode, TextObject leftRosterName = null, TroopRoster leftMemberRoster = null, InventoryLogic.CapacityData otherSideCapacityData = null)
		{
			this.Initialize(leftItemRoster, party.ItemRoster, party.MemberRoster, isTrading, isSpecialActionsPermitted, initialCharacterOfRightRoster, merchantItemType, marketData, useBasePrices, inventoryMode, leftRosterName, leftMemberRoster, otherSideCapacityData);
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0005F590 File Offset: 0x0005D790
		public void Initialize(ItemRoster leftItemRoster, ItemRoster rightItemRoster, TroopRoster rightMemberRoster, bool isTrading, bool isSpecialActionsPermitted, CharacterObject initialCharacterOfRightRoster, InventoryScreenHelper.InventoryCategoryType merchantItemType, IMarketData marketData, bool useBasePrices, InventoryScreenHelper.InventoryMode inventoryMode, TextObject leftRosterName = null, TroopRoster leftMemberRoster = null, InventoryLogic.CapacityData otherSideCapacityData = null)
		{
			this.OtherSideCapacityData = otherSideCapacityData;
			this.MarketData = marketData;
			this.TransactionDebt = 0;
			this.MerchantItemType = merchantItemType;
			this.InventoryListener = new FakeInventoryListener();
			this._useBasePrices = useBasePrices;
			this.LeftRosterName = leftRosterName;
			this.IsTrading = isTrading;
			this.IsSpecialActionsPermitted = isSpecialActionsPermitted;
			this._inventoryMode = inventoryMode;
			PartyBase otherParty = this.OtherParty;
			Clan clan;
			if (otherParty == null)
			{
				clan = null;
			}
			else
			{
				MobileParty mobileParty = otherParty.MobileParty;
				clan = ((mobileParty != null) ? mobileParty.ActualClan : null);
			}
			this.IsOtherPartyFromPlayerClan = clan == Hero.MainHero.Clan;
			this.InitializeRosters(leftItemRoster, rightItemRoster, rightMemberRoster, initialCharacterOfRightRoster, leftMemberRoster);
			this._transactionHistory.Clear();
			this.InitializeCategoryAverages();
			this.CanGainXpFromDiscarding = this._inventoryMode == InventoryScreenHelper.InventoryMode.Loot || (this._inventoryMode == InventoryScreenHelper.InventoryMode.Default && this.OtherParty == null);
			this.InitializeXpGainFromDonations();
			if (this._inventoryMode == InventoryScreenHelper.InventoryMode.Warehouse)
			{
				this._workshopWarehouseBehavior = Campaign.Current.GetCampaignBehavior<IWorkshopWarehouseCampaignBehavior>();
			}
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0005F682 File Offset: 0x0005D882
		private void InitializeRosters(ItemRoster leftItemRoster, ItemRoster rightItemRoster, TroopRoster rightMemberRoster, CharacterObject initialCharacterOfRightRoster, TroopRoster leftMemberRoster = null)
		{
			this._rosters[0] = leftItemRoster;
			this._rosters[1] = rightItemRoster;
			this.RightMemberRoster = rightMemberRoster;
			this.LeftMemberRoster = leftMemberRoster;
			this.InitialEquipmentCharacter = initialCharacterOfRightRoster;
			this.SetCurrentStateAsInitial();
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0005F6B3 File Offset: 0x0005D8B3
		public int GetItemTotalPrice(ItemRosterElement itemRosterElement, int absStockChange, out int lastPrice, bool isBuying)
		{
			lastPrice = this.GetItemPrice(itemRosterElement.EquipmentElement, isBuying);
			return lastPrice;
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0005F6C8 File Offset: 0x0005D8C8
		public void SetPlayerAcceptTraderOffer()
		{
			this._playerAcceptsTraderOffer = true;
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0005F6D4 File Offset: 0x0005D8D4
		public bool DoneLogic()
		{
			if (this.IsPreviewingItem)
			{
				return false;
			}
			SettlementComponent currentSettlementComponent = this.CurrentSettlementComponent;
			MobileParty currentMobileParty = this.CurrentMobileParty;
			PartyBase partyBase = null;
			if (currentMobileParty != null)
			{
				partyBase = currentMobileParty.Party;
			}
			else if (currentSettlementComponent != null)
			{
				partyBase = currentSettlementComponent.Owner;
			}
			if (!this._playerAcceptsTraderOffer)
			{
				InventoryListener inventoryListener = this.InventoryListener;
				int? num = ((inventoryListener != null) ? new int?(inventoryListener.GetGold()) : null) + this.TotalAmount;
				int num2 = 0;
				bool flag = (num.GetValueOrDefault() < num2) & (num != null);
			}
			if (this.InventoryListener != null && this.IsTrading && this.OwnerCharacter.HeroObject.Gold - this.TotalAmount < 0)
			{
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_warning_you_dont_have_enough_money", null), 0, null, null, "");
				return false;
			}
			if (this._playerAcceptsTraderOffer)
			{
				this._playerAcceptsTraderOffer = false;
				if (this.InventoryListener != null)
				{
					int gold = this.InventoryListener.GetGold();
					this.TransactionDebt = -gold;
				}
			}
			if (this.OwnerCharacter != null && this.OwnerCharacter.HeroObject != null && this.IsTrading)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, this.OwnerCharacter.HeroObject, MathF.Min(-this.TotalAmount, this.InventoryListener.GetGold()), false);
				if (currentSettlementComponent != null && currentSettlementComponent.IsTown && this.OwnerCharacter.GetPerkValue(DefaultPerks.Trade.TrickleDown))
				{
					int num3 = 0;
					List<ValueTuple<ItemRosterElement, int>> boughtItems = this._transactionHistory.GetBoughtItems();
					int num4 = 0;
					while (boughtItems != null && num4 < boughtItems.Count)
					{
						ItemObject item = boughtItems[num4].Item1.EquipmentElement.Item;
						if (item != null && item.IsTradeGood)
						{
							num3 += boughtItems[num4].Item2;
						}
						num4++;
					}
					if (num3 >= 10000)
					{
						for (int i = 0; i < currentSettlementComponent.Settlement.Notables.Count; i++)
						{
							if (currentSettlementComponent.Settlement.Notables[i].IsMerchant)
							{
								ChangeRelationAction.ApplyRelationChangeBetweenHeroes(currentSettlementComponent.Settlement.Notables[i], this.OwnerCharacter.HeroObject, MathF.Floor(DefaultPerks.Trade.TrickleDown.PrimaryBonus), true);
							}
						}
					}
				}
			}
			if (this.CanGainXpFromDiscarding)
			{
				CampaignEventDispatcher.Instance.OnItemsDiscardedByPlayer(this._rosters[0]);
			}
			CampaignEventDispatcher.Instance.OnPlayerInventoryExchange(this._transactionHistory.GetBoughtItems(), this._transactionHistory.GetSoldItems(), this.IsTrading);
			if (currentSettlementComponent != null && this.InventoryListener != null && this.IsTrading)
			{
				this.InventoryListener.SetGold(this.InventoryListener.GetGold() + this.TotalAmount);
			}
			else if (((currentMobileParty != null) ? currentMobileParty.Party.LeaderHero : null) != null && this.IsTrading)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, this.CurrentMobileParty.Party.LeaderHero, this.TotalAmount, false);
				if (this.CurrentMobileParty.Party.LeaderHero.CompanionOf != null)
				{
					this.CurrentMobileParty.AddTaxGold((int)((float)this.TotalAmount * 0.1f));
				}
			}
			else if (partyBase != null && partyBase.LeaderHero == null && this.IsTrading)
			{
				GiveGoldAction.ApplyForCharacterToParty(null, partyBase, this.TotalAmount, false);
			}
			this._partyInitialEquipment = new InventoryLogic.PartyEquipment(this.OwnerParty);
			if (this.IsOtherPartyFromPlayerClan && this.LeftMemberRoster != null)
			{
				this._otherPartyInitialEquipment = new InventoryLogic.PartyEquipment(this.OtherParty.MobileParty);
			}
			return true;
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0005FA7B File Offset: 0x0005DC7B
		public List<ValueTuple<ItemRosterElement, int>> GetBoughtItems()
		{
			return this._transactionHistory.GetBoughtItems();
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0005FA88 File Offset: 0x0005DC88
		public List<ValueTuple<ItemRosterElement, int>> GetSoldItems()
		{
			return this._transactionHistory.GetSoldItems();
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0005FA95 File Offset: 0x0005DC95
		public bool CanInventoryCapacityIncrease(InventoryLogic.InventorySide side)
		{
			return this._inventoryMode != InventoryScreenHelper.InventoryMode.Warehouse || side > InventoryLogic.InventorySide.OtherInventory;
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0005FAA6 File Offset: 0x0005DCA6
		public bool GetCanItemIncreaseInventoryCapacity(ItemObject item)
		{
			return item.HasHorseComponent;
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0005FAB0 File Offset: 0x0005DCB0
		private void InitializeCategoryAverages()
		{
			if (Campaign.Current != null && Settlement.CurrentSettlement != null)
			{
				Town town = (Settlement.CurrentSettlement.IsVillage ? Settlement.CurrentSettlement.Village.Bound.Town : Settlement.CurrentSettlement.Town);
				foreach (ItemCategory itemCategory in ItemCategories.All)
				{
					float num = 0f;
					for (int i = 0; i < Town.AllTowns.Count; i++)
					{
						if (Town.AllTowns[i] != town)
						{
							num += Town.AllTowns[i].MarketData.GetPriceFactor(itemCategory);
						}
					}
					float num2 = num / (float)(Town.AllTowns.Count - 1);
					this._itemCategoryAverages.Add(itemCategory, num2);
					Debug.Print(string.Format("Average value of {0} : {1}", itemCategory.GetName(), num2), 0, Debug.DebugColor.White, 17592186044416UL);
				}
			}
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0005FBD4 File Offset: 0x0005DDD4
		private void InitializeXpGainFromDonations()
		{
			this.XpGainFromDonations = 0f;
			bool flag = PerkHelper.PlayerHasAnyItemDonationPerk();
			bool flag2 = this._inventoryMode == InventoryScreenHelper.InventoryMode.Loot;
			if (flag && flag2)
			{
				this.XpGainFromDonations = (float)Campaign.Current.Models.ItemDiscardModel.GetXpBonusForDiscardingItems(this._rosters[0]);
			}
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0005FC24 File Offset: 0x0005DE24
		private void HandleDonationOnTransferItem(ItemRosterElement rosterElement, int amount, bool isBuying, bool isSelling)
		{
			ItemObject item = rosterElement.EquipmentElement.Item;
			ItemDiscardModel itemDiscardModel = Campaign.Current.Models.ItemDiscardModel;
			if (this.CanGainXpFromDiscarding && (isSelling || isBuying) && item != null)
			{
				this.XpGainFromDonations += (float)(itemDiscardModel.GetXpBonusForDiscardingItem(item, amount) * (isSelling ? 1 : (-1)));
			}
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0005FC81 File Offset: 0x0005DE81
		public float GetAveragePriceFactorItemCategory(ItemCategory category)
		{
			if (this._itemCategoryAverages.ContainsKey(category))
			{
				return this._itemCategoryAverages[category];
			}
			return -99f;
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0005FCA4 File Offset: 0x0005DEA4
		public bool IsThereAnyChanges()
		{
			if (this.IsThereAnyChangeBetweenRosters(this._rosters[1], this._rostersBackup[1]) || !this._partyInitialEquipment.IsEqual(new InventoryLogic.PartyEquipment(this.OwnerParty)))
			{
				return true;
			}
			InventoryLogic.PartyEquipment otherPartyInitialEquipment = this._otherPartyInitialEquipment;
			if (otherPartyInitialEquipment == null)
			{
				return false;
			}
			PartyBase otherParty = this.OtherParty;
			return !otherPartyInitialEquipment.IsEqual(new InventoryLogic.PartyEquipment((otherParty != null) ? otherParty.MobileParty : null));
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0005FD10 File Offset: 0x0005DF10
		private bool IsThereAnyChangeBetweenRosters(ItemRoster roster1, ItemRoster roster2)
		{
			if (roster1.Count != roster2.Count)
			{
				return true;
			}
			using (IEnumerator<ItemRosterElement> enumerator = roster1.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemRosterElement item = enumerator.Current;
					if (!roster2.Any<ItemRosterElement>((ItemRosterElement e) => e.IsEqualTo(item)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0005FD88 File Offset: 0x0005DF88
		public void Reset(bool fromCancel)
		{
			this.ResetLogic(fromCancel);
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0005FD94 File Offset: 0x0005DF94
		private void ResetLogic(bool fromCancel)
		{
			Debug.Print("InventoryLogic::Reset", 0, Debug.DebugColor.White, 17592186044416UL);
			for (int i = 0; i < 2; i++)
			{
				this._rosters[i].Clear();
				this._rosters[i].Add(this._rostersBackup[i]);
			}
			this.TransactionDebt = 0;
			this._transactionHistory.Clear();
			this.InitializeXpGainFromDonations();
			this._partyInitialEquipment.ResetEquipment();
			InventoryLogic.PartyEquipment otherPartyInitialEquipment = this._otherPartyInitialEquipment;
			if (otherPartyInitialEquipment != null)
			{
				otherPartyInitialEquipment.ResetEquipment();
			}
			InventoryLogic.AfterResetDelegate afterReset = this.AfterReset;
			if (afterReset != null)
			{
				afterReset(this, fromCancel);
			}
			List<TransferCommandResult> list = new List<TransferCommandResult>();
			if (!fromCancel)
			{
				this.OnAfterTransfer(list);
			}
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0005FE3C File Offset: 0x0005E03C
		public bool CanPlayerCompleteTransaction()
		{
			InventoryLogic.CapacityData otherSideCapacityData = this.OtherSideCapacityData;
			int num = ((otherSideCapacityData != null) ? otherSideCapacityData.GetCapacity() : (-1));
			return (num == -1 || this.OtherSideCurrentWeight <= num || this.OtherSideCapacityData.CanForceTransaction()) && (!this.IsPreviewingItem || !this.IsTrading || this.TotalAmount <= 0 || (this.TotalAmount >= 0 && this.OwnerCharacter.HeroObject.Gold - this.TotalAmount >= 0));
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0005FEBC File Offset: 0x0005E0BC
		public bool CanSlaughterItem(ItemRosterElement element, InventoryLogic.InventorySide sideOfItem)
		{
			return (!this.IsTrading || this._transactionHistory.IsEmpty) && (this.IsSpecialActionsPermitted && this.IsSlaughterable(element.EquipmentElement.Item) && sideOfItem == InventoryLogic.InventorySide.PlayerInventory && element.Amount > 0) && !this._transactionHistory.GetBoughtItems().Any<ValueTuple<ItemRosterElement, int>>((ValueTuple<ItemRosterElement, int> i) => i.Item1.EquipmentElement.Item == element.EquipmentElement.Item);
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0005FF43 File Offset: 0x0005E143
		public bool IsSlaughterable(ItemObject item)
		{
			return item.Type == ItemObject.ItemTypeEnum.Animal || item.Type == ItemObject.ItemTypeEnum.Horse;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0005FF5C File Offset: 0x0005E15C
		public bool CanDonateItem(ItemRosterElement element, InventoryLogic.InventorySide sideOfItem)
		{
			return Game.Current.IsDevelopmentMode && this.IsSpecialActionsPermitted && element.Amount > 0 && this.IsDonatable(element.EquipmentElement.Item) && sideOfItem == InventoryLogic.InventorySide.PlayerInventory;
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0005FFA4 File Offset: 0x0005E1A4
		public bool IsDonatable(ItemObject item)
		{
			return item.Type == ItemObject.ItemTypeEnum.Arrows || item.Type == ItemObject.ItemTypeEnum.BodyArmor || item.Type == ItemObject.ItemTypeEnum.Bolts || item.Type == ItemObject.ItemTypeEnum.SlingStones || item.Type == ItemObject.ItemTypeEnum.Bow || item.Type == ItemObject.ItemTypeEnum.Bullets || item.Type == ItemObject.ItemTypeEnum.Cape || item.Type == ItemObject.ItemTypeEnum.ChestArmor || item.Type == ItemObject.ItemTypeEnum.Crossbow || item.Type == ItemObject.ItemTypeEnum.Sling || item.Type == ItemObject.ItemTypeEnum.HandArmor || item.Type == ItemObject.ItemTypeEnum.HeadArmor || item.Type == ItemObject.ItemTypeEnum.HorseHarness || item.Type == ItemObject.ItemTypeEnum.LegArmor || item.Type == ItemObject.ItemTypeEnum.Musket || item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon || item.Type == ItemObject.ItemTypeEnum.Pistol || item.Type == ItemObject.ItemTypeEnum.Polearm || item.Type == ItemObject.ItemTypeEnum.Shield || item.Type == ItemObject.ItemTypeEnum.Thrown || item.Type == ItemObject.ItemTypeEnum.TwoHandedWeapon;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x00060093 File Offset: 0x0005E293
		public void SetInventoryListener(InventoryListener inventoryListener)
		{
			this.InventoryListener = inventoryListener;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0006009C File Offset: 0x0005E29C
		public int GetItemPrice(EquipmentElement equipmentElement, bool isBuying = false)
		{
			bool flag = !isBuying;
			bool flag2 = false;
			int num = 0;
			int num2;
			bool flag3;
			if (this._transactionHistory.GetLastTransfer(equipmentElement, out num2, out flag3) && flag3 != flag)
			{
				flag2 = true;
				num = num2;
			}
			if (this._useBasePrices)
			{
				return equipmentElement.GetBaseValue();
			}
			if (flag2)
			{
				return num;
			}
			return this.MarketData.GetPrice(equipmentElement, this.OwnerParty, flag, this.OtherParty);
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x000600FC File Offset: 0x0005E2FC
		public int GetCostOfItemRosterElement(ItemRosterElement itemRosterElement, InventoryLogic.InventorySide side)
		{
			bool flag = side == InventoryLogic.InventorySide.OtherInventory && this.IsTrading;
			return this.GetItemPrice(itemRosterElement.EquipmentElement, flag);
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x00060124 File Offset: 0x0005E324
		private void OnAfterTransfer(List<TransferCommandResult> resultList)
		{
			InventoryLogic.ProcessResultListDelegate afterTransfer = this.AfterTransfer;
			if (afterTransfer != null)
			{
				afterTransfer(this, resultList);
			}
			foreach (TransferCommandResult transferCommandResult in resultList)
			{
				if (transferCommandResult.EffectedNumber > 0)
				{
					Game.Current.EventManager.TriggerEvent<InventoryTransferItemEvent>(new InventoryTransferItemEvent(transferCommandResult.EffectedItemRosterElement.EquipmentElement.Item, transferCommandResult.ResultSide == InventoryLogic.InventorySide.PlayerInventory));
				}
			}
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x000601BC File Offset: 0x0005E3BC
		public void AddTransferCommand(TransferCommand command)
		{
			this.ProcessTransferCommand(command);
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x000601C8 File Offset: 0x0005E3C8
		public void AddTransferCommands(IEnumerable<TransferCommand> commands)
		{
			foreach (TransferCommand transferCommand in commands)
			{
				this.ProcessTransferCommand(transferCommand);
			}
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00060210 File Offset: 0x0005E410
		public bool CheckItemRosterHasElement(InventoryLogic.InventorySide side, ItemRosterElement rosterElement, int number)
		{
			int num = this._rosters[(int)side].FindIndexOfElement(rosterElement.EquipmentElement);
			return num != -1 && this._rosters[(int)side].GetElementCopyAtIndex(num).Amount >= number;
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x00060254 File Offset: 0x0005E454
		private void ProcessTransferCommand(TransferCommand command)
		{
			List<TransferCommandResult> list = this.TransferItem(ref command);
			this.OnAfterTransfer(list);
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x00060274 File Offset: 0x0005E474
		private List<TransferCommandResult> TransferItem(ref TransferCommand transferCommand)
		{
			List<TransferCommandResult> list = new List<TransferCommandResult>();
			string text = "TransferItem Name: {0} | From: {1} To: {2} | Amount: {3}";
			object[] array = new object[4];
			int num = 0;
			ItemObject item = transferCommand.ElementToTransfer.EquipmentElement.Item;
			array[num] = ((item != null) ? item.Name.ToString() : null) ?? "null";
			array[1] = transferCommand.FromSide;
			array[2] = transferCommand.ToSide;
			array[3] = transferCommand.Amount;
			Debug.Print(string.Format(text, array), 0, Debug.DebugColor.White, 17592186044416UL);
			if (transferCommand.ElementToTransfer.EquipmentElement.Item != null && InventoryLogic.TransferIsMovementValid(ref transferCommand) && this.DoesTransferItemExist(ref transferCommand))
			{
				int num2 = 0;
				bool flag = false;
				if (!InventoryLogic.IsEquipmentSide(transferCommand.FromSide) && transferCommand.FromSide != InventoryLogic.InventorySide.None)
				{
					int num3 = this._rosters[(int)transferCommand.FromSide].FindIndexOfElement(transferCommand.ElementToTransfer.EquipmentElement);
					ItemRosterElement elementCopyAtIndex = this._rosters[(int)transferCommand.FromSide].GetElementCopyAtIndex(num3);
					flag = transferCommand.Amount == elementCopyAtIndex.Amount;
				}
				bool flag2 = this.IsSell(transferCommand.FromSide, transferCommand.ToSide);
				bool flag3 = this.IsBuy(transferCommand.FromSide, transferCommand.ToSide);
				for (int i = 0; i < transferCommand.Amount; i++)
				{
					if (InventoryLogic.IsEquipmentSide(transferCommand.ToSide) && transferCommand.ToSideEquipment[(int)transferCommand.ToEquipmentIndex].Item != null)
					{
						TransferCommand transferCommand2 = TransferCommand.Transfer(1, transferCommand.ToSide, InventoryLogic.InventorySide.PlayerInventory, new ItemRosterElement(transferCommand.ToSideEquipment[(int)transferCommand.ToEquipmentIndex], 1), transferCommand.ToEquipmentIndex, EquipmentIndex.None, transferCommand.Character);
						list.AddRange(this.TransferItem(ref transferCommand2));
					}
					EquipmentElement equipmentElement = transferCommand.ElementToTransfer.EquipmentElement;
					int itemPrice = this.GetItemPrice(equipmentElement, flag3);
					if (flag3 || flag2)
					{
						this._transactionHistory.RecordTransaction(equipmentElement, flag2, itemPrice);
					}
					if (this.IsTrading)
					{
						if (flag3)
						{
							num2 += itemPrice;
						}
						else if (flag2)
						{
							num2 -= itemPrice;
						}
					}
					if (InventoryLogic.IsEquipmentSide(transferCommand.FromSide))
					{
						ItemRosterElement itemRosterElement = new ItemRosterElement(transferCommand.FromSideEquipment[(int)transferCommand.FromEquipmentIndex], transferCommand.Amount);
						itemRosterElement.Amount--;
						transferCommand.FromSideEquipment[(int)transferCommand.FromEquipmentIndex] = itemRosterElement.EquipmentElement;
					}
					else if (transferCommand.FromSide == InventoryLogic.InventorySide.PlayerInventory || transferCommand.FromSide == InventoryLogic.InventorySide.OtherInventory)
					{
						this._rosters[(int)transferCommand.FromSide].AddToCounts(transferCommand.ElementToTransfer.EquipmentElement, -1);
					}
					if (InventoryLogic.IsEquipmentSide(transferCommand.ToSide))
					{
						ItemRosterElement elementToTransfer = transferCommand.ElementToTransfer;
						elementToTransfer.Amount = 1;
						transferCommand.ToSideEquipment[(int)transferCommand.ToEquipmentIndex] = elementToTransfer.EquipmentElement;
					}
					else if (transferCommand.ToSide == InventoryLogic.InventorySide.PlayerInventory || transferCommand.ToSide == InventoryLogic.InventorySide.OtherInventory)
					{
						this._rosters[(int)transferCommand.ToSide].AddToCounts(transferCommand.ElementToTransfer.EquipmentElement, 1);
					}
				}
				if (InventoryLogic.IsEquipmentSide(transferCommand.FromSide))
				{
					ItemRosterElement itemRosterElement2 = new ItemRosterElement(transferCommand.FromSideEquipment[(int)transferCommand.FromEquipmentIndex], transferCommand.Amount);
					int amount = itemRosterElement2.Amount;
					itemRosterElement2.Amount = amount - 1;
					list.Add(new TransferCommandResult(transferCommand.FromSide, itemRosterElement2, -transferCommand.Amount, itemRosterElement2.Amount, transferCommand.FromEquipmentIndex, transferCommand.Character));
				}
				else if (transferCommand.FromSide == InventoryLogic.InventorySide.PlayerInventory || transferCommand.FromSide == InventoryLogic.InventorySide.OtherInventory)
				{
					if (flag)
					{
						list.Add(new TransferCommandResult(transferCommand.FromSide, new ItemRosterElement(transferCommand.ElementToTransfer.EquipmentElement, 0), -transferCommand.Amount, 0, transferCommand.FromEquipmentIndex, transferCommand.Character));
					}
					else
					{
						int num4 = this._rosters[(int)transferCommand.FromSide].FindIndexOfElement(transferCommand.ElementToTransfer.EquipmentElement);
						ItemRosterElement elementCopyAtIndex2 = this._rosters[(int)transferCommand.FromSide].GetElementCopyAtIndex(num4);
						list.Add(new TransferCommandResult(transferCommand.FromSide, elementCopyAtIndex2, -transferCommand.Amount, elementCopyAtIndex2.Amount, transferCommand.FromEquipmentIndex, transferCommand.Character));
					}
				}
				if (InventoryLogic.IsEquipmentSide(transferCommand.ToSide))
				{
					ItemRosterElement elementToTransfer2 = transferCommand.ElementToTransfer;
					elementToTransfer2.Amount = 1;
					list.Add(new TransferCommandResult(transferCommand.ToSide, elementToTransfer2, 1, 1, transferCommand.ToEquipmentIndex, transferCommand.Character));
				}
				else if (transferCommand.ToSide == InventoryLogic.InventorySide.PlayerInventory || transferCommand.ToSide == InventoryLogic.InventorySide.OtherInventory)
				{
					int num5 = this._rosters[(int)transferCommand.ToSide].FindIndexOfElement(transferCommand.ElementToTransfer.EquipmentElement);
					ItemRosterElement elementCopyAtIndex3 = this._rosters[(int)transferCommand.ToSide].GetElementCopyAtIndex(num5);
					list.Add(new TransferCommandResult(transferCommand.ToSide, elementCopyAtIndex3, transferCommand.Amount, elementCopyAtIndex3.Amount, transferCommand.ToEquipmentIndex, transferCommand.Character));
				}
				this.HandleDonationOnTransferItem(transferCommand.ElementToTransfer, transferCommand.Amount, flag3, flag2);
				this.TransactionDebt += num2;
			}
			return list;
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0006078A File Offset: 0x0005E98A
		public static bool IsEquipmentSide(InventoryLogic.InventorySide side)
		{
			return side == InventoryLogic.InventorySide.CivilianEquipment || side == InventoryLogic.InventorySide.BattleEquipment || side == InventoryLogic.InventorySide.StealthEquipment;
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0006079A File Offset: 0x0005E99A
		private bool IsSell(InventoryLogic.InventorySide fromSide, InventoryLogic.InventorySide toSide)
		{
			return toSide == InventoryLogic.InventorySide.OtherInventory && (InventoryLogic.IsEquipmentSide(fromSide) || fromSide == InventoryLogic.InventorySide.PlayerInventory);
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x000607AF File Offset: 0x0005E9AF
		private bool IsBuy(InventoryLogic.InventorySide fromSide, InventoryLogic.InventorySide toSide)
		{
			return fromSide == InventoryLogic.InventorySide.OtherInventory && (InventoryLogic.IsEquipmentSide(toSide) || toSide == InventoryLogic.InventorySide.PlayerInventory);
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x000607C4 File Offset: 0x0005E9C4
		public void SlaughterItem(ItemRosterElement itemRosterElement)
		{
			List<TransferCommandResult> list = new List<TransferCommandResult>();
			EquipmentElement equipmentElement = itemRosterElement.EquipmentElement;
			int meatCount = equipmentElement.Item.HorseComponent.MeatCount;
			int hideCount = equipmentElement.Item.HorseComponent.HideCount;
			int num = this._rosters[1].AddToCounts(DefaultItems.Meat, meatCount);
			ItemRosterElement elementCopyAtIndex = this._rosters[1].GetElementCopyAtIndex(num);
			bool flag = itemRosterElement.Amount == 1;
			int num2 = this._rosters[1].AddToCounts(itemRosterElement.EquipmentElement, -1);
			if (flag)
			{
				list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, new ItemRosterElement(equipmentElement, 0), -1, 0, EquipmentIndex.None, null));
			}
			else
			{
				ItemRosterElement elementCopyAtIndex2 = this._rosters[1].GetElementCopyAtIndex(num2);
				list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, elementCopyAtIndex2, -1, elementCopyAtIndex2.Amount, EquipmentIndex.None, null));
			}
			list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, elementCopyAtIndex, meatCount, elementCopyAtIndex.Amount, EquipmentIndex.None, null));
			if (hideCount > 0)
			{
				int num3 = this._rosters[1].AddToCounts(DefaultItems.Hides, hideCount);
				ItemRosterElement elementCopyAtIndex3 = this._rosters[1].GetElementCopyAtIndex(num3);
				list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, elementCopyAtIndex3, hideCount, elementCopyAtIndex3.Amount, EquipmentIndex.None, null));
			}
			this.SetCurrentStateAsInitial();
			this.OnAfterTransfer(list);
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x000608F8 File Offset: 0x0005EAF8
		public void DonateItem(ItemRosterElement itemRosterElement)
		{
			List<TransferCommandResult> list = new List<TransferCommandResult>();
			int tier = (int)itemRosterElement.EquipmentElement.Item.Tier;
			int num = 100 * (tier + 1);
			InventoryLogic.InventorySide inventorySide = InventoryLogic.InventorySide.PlayerInventory;
			int num2 = this._rosters[(int)inventorySide].AddToCounts(itemRosterElement.EquipmentElement, -1);
			ItemRosterElement elementCopyAtIndex = this._rosters[(int)inventorySide].GetElementCopyAtIndex(num2);
			list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, elementCopyAtIndex, -1, elementCopyAtIndex.Amount, EquipmentIndex.None, null));
			if (num > 0)
			{
				TroopRosterElement randomElementWithPredicate = PartyBase.MainParty.MemberRoster.GetTroopRoster().GetRandomElementWithPredicate<TroopRosterElement>((TroopRosterElement m) => !m.Character.IsHero && m.Character.UpgradeTargets.Length != 0);
				if (randomElementWithPredicate.Character != null)
				{
					PartyBase.MainParty.MemberRoster.AddXpToTroop(randomElementWithPredicate.Character, num);
					TextObject textObject = new TextObject("{=Kwja0a4s}Added {XPAMOUNT} amount of xp to {TROOPNAME}", null);
					textObject.SetTextVariable("XPAMOUNT", num);
					textObject.SetTextVariable("TROOPNAME", randomElementWithPredicate.Character.Name.ToString());
					Debug.Print(textObject.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
					MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
				}
			}
			this.SetCurrentStateAsInitial();
			this.OnAfterTransfer(list);
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x00060A2C File Offset: 0x0005EC2C
		private static bool TransferIsMovementValid(ref TransferCommand transferCommand)
		{
			if (transferCommand.ElementToTransfer.EquipmentElement.IsQuestItem)
			{
				BannerComponent bannerComponent = transferCommand.ElementToTransfer.EquipmentElement.Item.BannerComponent;
				if (((bannerComponent != null) ? bannerComponent.BannerEffect : null) == null || ((transferCommand.FromSide != InventoryLogic.InventorySide.PlayerInventory || !InventoryLogic.IsEquipmentSide(transferCommand.ToSide)) && (!InventoryLogic.IsEquipmentSide(transferCommand.FromSide) || transferCommand.ToSide != InventoryLogic.InventorySide.PlayerInventory)))
				{
					return false;
				}
			}
			bool flag = false;
			if (InventoryLogic.IsEquipmentSide(transferCommand.ToSide))
			{
				InventoryScreenHelper.InventoryItemType inventoryItemTypeOfItem = InventoryScreenHelper.GetInventoryItemTypeOfItem(transferCommand.ElementToTransfer.EquipmentElement.Item);
				switch (transferCommand.ToEquipmentIndex)
				{
				case EquipmentIndex.WeaponItemBeginSlot:
				case EquipmentIndex.Weapon1:
				case EquipmentIndex.Weapon2:
				case EquipmentIndex.Weapon3:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.Weapon || inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.Shield;
					break;
				case EquipmentIndex.ExtraWeaponSlot:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.Banner;
					break;
				case EquipmentIndex.NumAllWeaponSlots:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.HeadArmor;
					break;
				case EquipmentIndex.Body:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.BodyArmor;
					break;
				case EquipmentIndex.Leg:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.LegArmor;
					break;
				case EquipmentIndex.Gloves:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.HandArmor;
					break;
				case EquipmentIndex.Cape:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.Cape;
					break;
				case EquipmentIndex.ArmorItemEndSlot:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.Horse;
					break;
				case EquipmentIndex.HorseHarness:
					flag = inventoryItemTypeOfItem == InventoryScreenHelper.InventoryItemType.HorseHarness;
					break;
				}
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00060B70 File Offset: 0x0005ED70
		private bool DoesTransferItemExist(ref TransferCommand transferCommand)
		{
			if (transferCommand.FromSide == InventoryLogic.InventorySide.OtherInventory || transferCommand.FromSide == InventoryLogic.InventorySide.PlayerInventory)
			{
				return this.CheckItemRosterHasElement(transferCommand.FromSide, transferCommand.ElementToTransfer, transferCommand.Amount);
			}
			return transferCommand.FromSide != InventoryLogic.InventorySide.None && transferCommand.FromSideEquipment[(int)transferCommand.FromEquipmentIndex].Item != null && transferCommand.ElementToTransfer.EquipmentElement.IsEqualTo(transferCommand.FromSideEquipment[(int)transferCommand.FromEquipmentIndex]);
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x00060BF6 File Offset: 0x0005EDF6
		public void TransferOne(ItemRosterElement itemRosterElement)
		{
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x00060BF8 File Offset: 0x0005EDF8
		public int GetElementCountOnSide(InventoryLogic.InventorySide side)
		{
			return this._rosters[(int)side].Count;
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x00060C07 File Offset: 0x0005EE07
		public IReadOnlyList<ItemRosterElement> GetElementsInInitialRoster(InventoryLogic.InventorySide side)
		{
			return this._rostersBackup[(int)side];
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x00060C11 File Offset: 0x0005EE11
		public IReadOnlyList<ItemRosterElement> GetElementsInRoster(InventoryLogic.InventorySide side)
		{
			return this._rosters[(int)side];
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x00060C1C File Offset: 0x0005EE1C
		private void SetCurrentStateAsInitial()
		{
			for (int i = 0; i < this._rostersBackup.Length; i++)
			{
				this._rostersBackup[i] = new ItemRoster(this._rosters[i]);
			}
			this._partyInitialEquipment = new InventoryLogic.PartyEquipment(this.OwnerParty);
			if (this.IsOtherPartyFromPlayerClan && this.LeftMemberRoster != null)
			{
				this._otherPartyInitialEquipment = new InventoryLogic.PartyEquipment(this.OtherParty.MobileParty);
			}
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x00060C88 File Offset: 0x0005EE88
		public ItemRosterElement? FindItemFromSide(InventoryLogic.InventorySide side, EquipmentElement item)
		{
			int num = this._rosters[(int)side].FindIndexOfElement(item);
			if (num >= 0)
			{
				return new ItemRosterElement?(this._rosters[(int)side].ElementAt<ItemRosterElement>(num));
			}
			return null;
		}

		// Token: 0x040006D3 RID: 1747
		private ItemRoster[] _rosters;

		// Token: 0x040006D4 RID: 1748
		private ItemRoster[] _rostersBackup;

		// Token: 0x040006D5 RID: 1749
		private IWorkshopWarehouseCampaignBehavior _workshopWarehouseBehavior;

		// Token: 0x040006DB RID: 1755
		public bool IsPreviewingItem;

		// Token: 0x040006DC RID: 1756
		private InventoryLogic.PartyEquipment _partyInitialEquipment;

		// Token: 0x040006DD RID: 1757
		private InventoryLogic.PartyEquipment _otherPartyInitialEquipment;

		// Token: 0x040006E3 RID: 1763
		private float _xpGainFromDonations;

		// Token: 0x040006E4 RID: 1764
		private int _transactionDebt;

		// Token: 0x040006E5 RID: 1765
		private bool _playerAcceptsTraderOffer;

		// Token: 0x040006E6 RID: 1766
		private InventoryLogic.TransactionHistory _transactionHistory = new InventoryLogic.TransactionHistory();

		// Token: 0x040006E7 RID: 1767
		private Dictionary<ItemCategory, float> _itemCategoryAverages = new Dictionary<ItemCategory, float>();

		// Token: 0x040006E8 RID: 1768
		private bool _useBasePrices;

		// Token: 0x040006E9 RID: 1769
		public InventoryScreenHelper.InventoryCategoryType MerchantItemType = InventoryScreenHelper.InventoryCategoryType.None;

		// Token: 0x040006EA RID: 1770
		private InventoryScreenHelper.InventoryMode _inventoryMode;

		// Token: 0x02000555 RID: 1365
		public enum TransferType
		{
			// Token: 0x040016E1 RID: 5857
			Neutral,
			// Token: 0x040016E2 RID: 5858
			Sell,
			// Token: 0x040016E3 RID: 5859
			Buy
		}

		// Token: 0x02000556 RID: 1366
		public enum InventorySide
		{
			// Token: 0x040016E5 RID: 5861
			OtherInventory,
			// Token: 0x040016E6 RID: 5862
			PlayerInventory,
			// Token: 0x040016E7 RID: 5863
			CivilianEquipment,
			// Token: 0x040016E8 RID: 5864
			BattleEquipment,
			// Token: 0x040016E9 RID: 5865
			StealthEquipment,
			// Token: 0x040016EA RID: 5866
			None = -1
		}

		// Token: 0x02000557 RID: 1367
		// (Invoke) Token: 0x06004D8B RID: 19851
		public delegate void AfterResetDelegate(InventoryLogic inventoryLogic, bool fromCancel);

		// Token: 0x02000558 RID: 1368
		// (Invoke) Token: 0x06004D8F RID: 19855
		public delegate void TotalAmountChangeDelegate(int newTotalAmount);

		// Token: 0x02000559 RID: 1369
		// (Invoke) Token: 0x06004D93 RID: 19859
		public delegate void ProcessResultListDelegate(InventoryLogic inventoryLogic, List<TransferCommandResult> results);

		// Token: 0x0200055A RID: 1370
		private class PartyEquipment
		{
			// Token: 0x17000F17 RID: 3863
			// (get) Token: 0x06004D96 RID: 19862 RVA: 0x00180984 File Offset: 0x0017EB84
			// (set) Token: 0x06004D97 RID: 19863 RVA: 0x0018098C File Offset: 0x0017EB8C
			public Dictionary<CharacterObject, Equipment[]> CharacterEquipments { get; private set; }

			// Token: 0x06004D98 RID: 19864 RVA: 0x00180995 File Offset: 0x0017EB95
			public PartyEquipment(MobileParty party)
			{
				this.CharacterEquipments = new Dictionary<CharacterObject, Equipment[]>();
				this.InitializeCopyFrom(party);
			}

			// Token: 0x06004D99 RID: 19865 RVA: 0x001809B0 File Offset: 0x0017EBB0
			public void InitializeCopyFrom(MobileParty party)
			{
				this.CharacterEquipments = new Dictionary<CharacterObject, Equipment[]>();
				for (int i = 0; i < party.MemberRoster.Count; i++)
				{
					CharacterObject character = party.MemberRoster.GetElementCopyAtIndex(i).Character;
					if (character.IsHero)
					{
						this.CharacterEquipments.Add(character, new Equipment[]
						{
							new Equipment(character.FirstBattleEquipment),
							new Equipment(character.FirstCivilianEquipment),
							new Equipment(character.FirstStealthEquipment)
						});
					}
				}
			}

			// Token: 0x06004D9A RID: 19866 RVA: 0x00180A34 File Offset: 0x0017EC34
			internal void ResetEquipment()
			{
				foreach (KeyValuePair<CharacterObject, Equipment[]> keyValuePair in this.CharacterEquipments)
				{
					foreach (Equipment equipment in keyValuePair.Value)
					{
						if (equipment.IsBattle)
						{
							keyValuePair.Key.FirstBattleEquipment.FillFrom(equipment, true);
						}
						else if (equipment.IsCivilian)
						{
							keyValuePair.Key.FirstCivilianEquipment.FillFrom(equipment, true);
						}
						else if (equipment.IsStealth)
						{
							keyValuePair.Key.FirstStealthEquipment.FillFrom(equipment, true);
						}
						else
						{
							Debug.FailedAssert("Equipment type cannot be found!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Inventory\\InventoryLogic.cs", "ResetEquipment", 1182);
						}
					}
				}
			}

			// Token: 0x06004D9B RID: 19867 RVA: 0x00180B1C File Offset: 0x0017ED1C
			public void SetReference(InventoryLogic.PartyEquipment partyEquipment)
			{
				this.CharacterEquipments.Clear();
				this.CharacterEquipments = partyEquipment.CharacterEquipments;
			}

			// Token: 0x06004D9C RID: 19868 RVA: 0x00180B38 File Offset: 0x0017ED38
			public bool IsEqual(InventoryLogic.PartyEquipment partyEquipment)
			{
				if (partyEquipment.CharacterEquipments.Keys.Count != this.CharacterEquipments.Keys.Count)
				{
					return false;
				}
				foreach (CharacterObject characterObject in partyEquipment.CharacterEquipments.Keys)
				{
					if (!this.CharacterEquipments.Keys.Contains(characterObject))
					{
						return false;
					}
					Equipment[] array;
					if (!this.CharacterEquipments.TryGetValue(characterObject, out array))
					{
						return false;
					}
					Equipment[] array2;
					if (!partyEquipment.CharacterEquipments.TryGetValue(characterObject, out array2) || array2.Length != array.Length)
					{
						return false;
					}
					for (int i = 0; i < array.Length; i++)
					{
						if (!array[i].IsEquipmentEqualTo(array2[i]))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x0200055B RID: 1371
		private class ItemLog : IReadOnlyCollection<int>, IEnumerable<int>, IEnumerable
		{
			// Token: 0x17000F18 RID: 3864
			// (get) Token: 0x06004D9D RID: 19869 RVA: 0x00180C24 File Offset: 0x0017EE24
			public bool IsSelling
			{
				get
				{
					return this._isSelling;
				}
			}

			// Token: 0x17000F19 RID: 3865
			// (get) Token: 0x06004D9E RID: 19870 RVA: 0x00180C2C File Offset: 0x0017EE2C
			public int Count
			{
				get
				{
					return ((IReadOnlyCollection<int>)this._transactions).Count;
				}
			}

			// Token: 0x06004D9F RID: 19871 RVA: 0x00180C39 File Offset: 0x0017EE39
			private void AddTransaction(int price, bool isSelling)
			{
				if (this._transactions.IsEmpty<int>())
				{
					this._isSelling = isSelling;
				}
				this._transactions.Add(price);
			}

			// Token: 0x06004DA0 RID: 19872 RVA: 0x00180C5C File Offset: 0x0017EE5C
			private void RemoveLastTransaction()
			{
				if (!this._transactions.IsEmpty<int>())
				{
					this._transactions.RemoveAt(this._transactions.Count - 1);
					return;
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Inventory\\InventoryLogic.cs", "RemoveLastTransaction", 1262);
			}

			// Token: 0x06004DA1 RID: 19873 RVA: 0x00180CA8 File Offset: 0x0017EEA8
			public void RecordTransaction(int price, bool isSelling)
			{
				if (!this._transactions.IsEmpty<int>() && isSelling != this._isSelling)
				{
					this.RemoveLastTransaction();
					return;
				}
				this.AddTransaction(price, isSelling);
			}

			// Token: 0x06004DA2 RID: 19874 RVA: 0x00180CCF File Offset: 0x0017EECF
			public bool GetLastTransaction(out int price, out bool isSelling)
			{
				if (this._transactions.IsEmpty<int>())
				{
					price = 0;
					isSelling = false;
					return false;
				}
				price = this._transactions[this._transactions.Count - 1];
				isSelling = this._isSelling;
				return true;
			}

			// Token: 0x06004DA3 RID: 19875 RVA: 0x00180D09 File Offset: 0x0017EF09
			public IEnumerator<int> GetEnumerator()
			{
				return ((IEnumerable<int>)this._transactions).GetEnumerator();
			}

			// Token: 0x06004DA4 RID: 19876 RVA: 0x00180D16 File Offset: 0x0017EF16
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<int>)this._transactions).GetEnumerator();
			}

			// Token: 0x040016EC RID: 5868
			private List<int> _transactions = new List<int>();

			// Token: 0x040016ED RID: 5869
			private bool _isSelling;
		}

		// Token: 0x0200055C RID: 1372
		public class CapacityData
		{
			// Token: 0x06004DA6 RID: 19878 RVA: 0x00180D36 File Offset: 0x0017EF36
			public CapacityData(Func<int> getCapacity, Func<TextObject> getCapacityExceededWarningText, Func<TextObject> getCapacityExceededHintText, bool forceTransaction = false)
			{
				this._getCapacity = getCapacity;
				this._getCapacityExceededWarningText = getCapacityExceededWarningText;
				this._getCapacityExceededHintText = getCapacityExceededHintText;
				this._forceTransaction = forceTransaction;
			}

			// Token: 0x06004DA7 RID: 19879 RVA: 0x00180D5B File Offset: 0x0017EF5B
			public int GetCapacity()
			{
				Func<int> getCapacity = this._getCapacity;
				if (getCapacity == null)
				{
					return -1;
				}
				return getCapacity();
			}

			// Token: 0x06004DA8 RID: 19880 RVA: 0x00180D6E File Offset: 0x0017EF6E
			public bool CanForceTransaction()
			{
				return this._forceTransaction;
			}

			// Token: 0x06004DA9 RID: 19881 RVA: 0x00180D76 File Offset: 0x0017EF76
			public TextObject GetCapacityExceededWarningText()
			{
				Func<TextObject> getCapacityExceededWarningText = this._getCapacityExceededWarningText;
				if (getCapacityExceededWarningText == null)
				{
					return null;
				}
				return getCapacityExceededWarningText();
			}

			// Token: 0x06004DAA RID: 19882 RVA: 0x00180D89 File Offset: 0x0017EF89
			public TextObject GetCapacityExceededHintText()
			{
				Func<TextObject> getCapacityExceededHintText = this._getCapacityExceededHintText;
				if (getCapacityExceededHintText == null)
				{
					return null;
				}
				return getCapacityExceededHintText();
			}

			// Token: 0x040016EE RID: 5870
			private readonly Func<int> _getCapacity;

			// Token: 0x040016EF RID: 5871
			private readonly Func<TextObject> _getCapacityExceededWarningText;

			// Token: 0x040016F0 RID: 5872
			private readonly Func<TextObject> _getCapacityExceededHintText;

			// Token: 0x040016F1 RID: 5873
			private readonly bool _forceTransaction;
		}

		// Token: 0x0200055D RID: 1373
		private class TransactionHistory
		{
			// Token: 0x06004DAB RID: 19883 RVA: 0x00180D9C File Offset: 0x0017EF9C
			internal void RecordTransaction(EquipmentElement elementToTransfer, bool isSelling, int price)
			{
				InventoryLogic.ItemLog itemLog;
				if (!this._transactionLogs.TryGetValue(elementToTransfer, out itemLog))
				{
					itemLog = new InventoryLogic.ItemLog();
					this._transactionLogs[elementToTransfer] = itemLog;
				}
				itemLog.RecordTransaction(price, isSelling);
			}

			// Token: 0x17000F1A RID: 3866
			// (get) Token: 0x06004DAC RID: 19884 RVA: 0x00180DD4 File Offset: 0x0017EFD4
			public bool IsEmpty
			{
				get
				{
					return this._transactionLogs.IsEmpty<KeyValuePair<EquipmentElement, InventoryLogic.ItemLog>>();
				}
			}

			// Token: 0x06004DAD RID: 19885 RVA: 0x00180DE1 File Offset: 0x0017EFE1
			public void Clear()
			{
				this._transactionLogs.Clear();
			}

			// Token: 0x06004DAE RID: 19886 RVA: 0x00180DF0 File Offset: 0x0017EFF0
			public bool GetLastTransfer(EquipmentElement equipmentElement, out int lastPrice, out bool lastIsSelling)
			{
				InventoryLogic.ItemLog itemLog;
				bool flag = this._transactionLogs.TryGetValue(equipmentElement, out itemLog);
				lastPrice = 0;
				lastIsSelling = false;
				return flag && itemLog.GetLastTransaction(out lastPrice, out lastIsSelling);
			}

			// Token: 0x06004DAF RID: 19887 RVA: 0x00180E20 File Offset: 0x0017F020
			internal List<ValueTuple<ItemRosterElement, int>> GetTransferredItems(bool isSelling)
			{
				List<ValueTuple<ItemRosterElement, int>> list = new List<ValueTuple<ItemRosterElement, int>>();
				foreach (KeyValuePair<EquipmentElement, InventoryLogic.ItemLog> keyValuePair in this._transactionLogs)
				{
					if (keyValuePair.Value.Count > 0 && !keyValuePair.Value.IsSelling == isSelling)
					{
						int num = keyValuePair.Value.Sum();
						list.Add(new ValueTuple<ItemRosterElement, int>(new ItemRosterElement(keyValuePair.Key.Item, keyValuePair.Value.Count, keyValuePair.Key.ItemModifier), num));
					}
				}
				return list;
			}

			// Token: 0x06004DB0 RID: 19888 RVA: 0x00180EE0 File Offset: 0x0017F0E0
			internal List<ValueTuple<ItemRosterElement, int>> GetBoughtItems()
			{
				return this.GetTransferredItems(true);
			}

			// Token: 0x06004DB1 RID: 19889 RVA: 0x00180EE9 File Offset: 0x0017F0E9
			internal List<ValueTuple<ItemRosterElement, int>> GetSoldItems()
			{
				return this.GetTransferredItems(false);
			}

			// Token: 0x040016F2 RID: 5874
			private Dictionary<EquipmentElement, InventoryLogic.ItemLog> _transactionLogs = new Dictionary<EquipmentElement, InventoryLogic.ItemLog>();
		}
	}
}
