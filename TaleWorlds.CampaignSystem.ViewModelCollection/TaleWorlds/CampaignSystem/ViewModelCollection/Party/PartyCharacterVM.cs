using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Events;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x02000027 RID: 39
	public class PartyCharacterVM : ViewModel
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600028F RID: 655 RVA: 0x000143E1 File Offset: 0x000125E1
		// (set) Token: 0x06000290 RID: 656 RVA: 0x000143E9 File Offset: 0x000125E9
		public TroopRoster Troops { get; private set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000291 RID: 657 RVA: 0x000143F2 File Offset: 0x000125F2
		// (set) Token: 0x06000292 RID: 658 RVA: 0x000143FA File Offset: 0x000125FA
		public string StringId { get; private set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000293 RID: 659 RVA: 0x00014403 File Offset: 0x00012603
		// (set) Token: 0x06000294 RID: 660 RVA: 0x0001440C File Offset: 0x0001260C
		public TroopRosterElement Troop
		{
			get
			{
				return this._troop;
			}
			set
			{
				this._troop = value;
				this.Character = value.Character;
				this.TroopID = this.Character.StringId;
				this.CheckTransferAmountDefaultValue();
				this.TroopXPTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetTroopXPTooltip(value));
				this.TroopConformityTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetTroopConformityTooltip(value));
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000295 RID: 661 RVA: 0x00014488 File Offset: 0x00012688
		// (set) Token: 0x06000296 RID: 662 RVA: 0x00014490 File Offset: 0x00012690
		public CharacterObject Character
		{
			get
			{
				return this._character;
			}
			set
			{
				if (this._character != value)
				{
					this._character = value;
					CharacterCode characterCode = this.GetCharacterCode(value, this.Type, this.Side);
					this.Code = new CharacterImageIdentifierVM(characterCode);
					CharacterObject[] upgradeTargets = this._character.UpgradeTargets;
					if (upgradeTargets != null && upgradeTargets.Length != 0)
					{
						this.Upgrades = new MBBindingList<UpgradeTargetVM>();
						for (int i = 0; i < this._character.UpgradeTargets.Length; i++)
						{
							CharacterCode characterCode2 = this.GetCharacterCode(this._character.UpgradeTargets[i], this.Type, this.Side);
							this.Upgrades.Add(new UpgradeTargetVM(i, value, characterCode2, new Action<int, int>(this.Upgrade), new Action<UpgradeTargetVM>(this.FocusUpgrade)));
						}
					}
				}
				this.CheckTransferAmountDefaultValue();
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0001455C File Offset: 0x0001275C
		public PartyCharacterVM(PartyScreenLogic partyScreenLogic, PartyVM partyVm, TroopRoster troops, int index, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, bool isTroopTransferrable)
		{
			this.Upgrades = new MBBindingList<UpgradeTargetVM>();
			this._partyScreenLogic = partyScreenLogic;
			this._partyVm = partyVm;
			this.Troops = troops;
			this.Side = side;
			this.Type = type;
			this.Troop = troops.GetElementCopyAtIndex(index);
			this.Index = index;
			this.IsHero = this.Troop.Character.IsHero;
			this.IsMainHero = Hero.MainHero.CharacterObject == this.Troop.Character;
			this.IsPrisoner = this.Type == PartyScreenLogic.TroopType.Prisoner;
			this.TierIconData = CampaignUIHelper.GetCharacterTierData(this.Troop.Character, true);
			this.TypeIconData = CampaignUIHelper.GetCharacterTypeData(this.Troop.Character, false);
			this.StringId = CampaignUIHelper.GetTroopLockStringID(this.Troop);
			this._initIsTroopTransferable = isTroopTransferrable;
			this.IsTroopTransferrable = this._initIsTroopTransferable;
			this.TradeData = new PartyTradeVM(partyScreenLogic, this.Troop, this.Side, this.IsTroopTransferrable, this.IsPrisoner, new Action<int, bool>(this.OnTradeApplyTransaction));
			this.IsPrisonerOfPlayer = this.IsPrisoner && this.Side == PartyScreenLogic.PartyRosterSide.Right;
			this.IsHeroPrisonerOfPlayer = this.IsPrisonerOfPlayer && this.Character.IsHero;
			this.IsExecutable = this._partyScreenLogic.IsExecutable(this.Type, this.Character, this.Side);
			this.IsUpgradableTroop = this.Side == PartyScreenLogic.PartyRosterSide.Right && !this.IsHero && !this.IsPrisoner && this.Character.UpgradeTargets.Length != 0;
			this.InitializeUpgrades();
			this.ThrowOnPropertyChanged();
			this.CheckTransferAmountDefaultValue();
			this.UpdateRecruitable();
			this.RefreshValues();
			this.SetMoraleCost();
			this.UpdateTalkable();
			this.TransferHint = new BasicTooltipViewModel(() => this.GetTransferHint());
			this.RecruitPrisonerHint = new BasicTooltipViewModel(() => this.GetRecruitHint());
			this.ExecutePrisonerHint = new BasicTooltipViewModel(() => this._partyScreenLogic.GetExecutableReasonString(this.Troop.Character, this.IsExecutable));
			this.HeroHealthHint = (this.Troop.Character.IsHero ? new BasicTooltipViewModel(() => CampaignUIHelper.GetHeroHealthTooltip(this.Troop.Character.HeroObject)) : null);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x000147B0 File Offset: 0x000129B0
		public void UpdateTalkable()
		{
			bool flag = this.Side == PartyScreenLogic.PartyRosterSide.Right;
			bool flag2 = this.Troop.Character != CharacterObject.PlayerCharacter;
			bool isHero = this.Troop.Character.IsHero;
			this.IsTalkableCharacter = flag2 && flag && isHero;
			if (this.TalkHint == null)
			{
				this.TalkHint = new HintViewModel();
			}
			if (this.IsTalkableCharacter)
			{
				this._partyCharacterTalkPermission = null;
				Game.Current.EventManager.TriggerEvent<PartyScreenCharacterTalkPermissionEvent>(new PartyScreenCharacterTalkPermissionEvent(this.Character.HeroObject, new Action<bool, TextObject>(this.OnPartyCharacterTalkPermissionResult)));
				if (this._partyCharacterTalkPermission != null && !this._partyCharacterTalkPermission.Item1)
				{
					this.CanTalk = false;
					this.TalkHint.HintText = this._partyCharacterTalkPermission.Item2;
					if (this.TalkHint.HintText.IsEmpty())
					{
						this.TalkHint.HintText = new TextObject("{=epQYhd1A}Cannot talk to hero right now", null);
						return;
					}
				}
				else
				{
					CanTalkToHeroDelegate canTalkToHeroDelegate = this._partyVm.PartyScreenLogic.CanTalkToHeroDelegate;
					this.CanTalk = (canTalkToHeroDelegate == null || canTalkToHeroDelegate(this.Character.HeroObject, this.Type, this.Side, this._partyScreenLogic.LeftOwnerParty, out this.TalkHint.HintText)) && CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out this.TalkHint.HintText);
					if (this.CanTalk)
					{
						this.TalkHint.HintText = GameTexts.FindText("str_talk_button", null);
						return;
					}
					if (this.TalkHint.HintText.IsEmpty())
					{
						this.TalkHint.HintText = new TextObject("{=epQYhd1A}Cannot talk to hero right now", null);
						return;
					}
				}
			}
			else
			{
				this.TalkHint.HintText = TextObject.GetEmpty();
				this.CanTalk = false;
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0001496C File Offset: 0x00012B6C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.Troop.Character.Name.ToString();
			this.LockHint = new HintViewModel(GameTexts.FindText("str_lock_in_party", null).SetTextVariable("TRANSFERABLE", this.IsPrisoner ? GameTexts.FindText("str_prisoners", null).ToString() : GameTexts.FindText("str_troops", null).ToString()), null);
			MBBindingList<UpgradeTargetVM> upgrades = this.Upgrades;
			if (upgrades != null)
			{
				upgrades.ApplyActionOnAllItems(delegate(UpgradeTargetVM x)
				{
					x.RefreshValues();
				});
			}
			PartyTradeVM tradeData = this.TradeData;
			if (tradeData == null)
			{
				return;
			}
			tradeData.RefreshValues();
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00014A25 File Offset: 0x00012C25
		private void OnPartyCharacterTalkPermissionResult(bool isAvailable, TextObject reasonStr)
		{
			this._partyCharacterTalkPermission = new Tuple<bool, TextObject>(isAvailable, reasonStr);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00014A34 File Offset: 0x00012C34
		private string GetTransferHint()
		{
			string text = GameTexts.FindText("str_transfer", null).ToString();
			string stackModifierString = CampaignUIHelper.GetStackModifierString(GameTexts.FindText("str_entire_stack_shortcut_transfer", null), GameTexts.FindText("str_five_stack_shortcut_transfer", null), this.Troop.Number >= 5);
			if (string.IsNullOrEmpty(stackModifierString))
			{
				return text;
			}
			return GameTexts.FindText("str_string_newline_string", null).SetTextVariable("STR1", text).SetTextVariable("STR2", stackModifierString)
				.ToString();
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00014AB4 File Offset: 0x00012CB4
		private string GetRecruitHint()
		{
			bool flag;
			string recruitableReasonString = this._partyScreenLogic.GetRecruitableReasonString(this.Troop.Character, this.IsTroopRecruitable, this.Troop.Number, out flag);
			string stackModifierString = CampaignUIHelper.GetStackModifierString(GameTexts.FindText("str_entire_stack_shortcut_recruit_units", null), GameTexts.FindText("str_five_stack_shortcut_recruit_units", null), this.Troop.Number >= 5);
			if (string.IsNullOrEmpty(stackModifierString) || !flag)
			{
				return recruitableReasonString;
			}
			return GameTexts.FindText("str_string_newline_string", null).SetTextVariable("STR1", recruitableReasonString).SetTextVariable("STR2", stackModifierString)
				.ToString();
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00014B54 File Offset: 0x00012D54
		private void CheckTransferAmountDefaultValue()
		{
			if (this.TransferAmount == 0 && this.Troop.Character != null && this.Troop.Number > 0)
			{
				this.TransferAmount = 1;
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00014B8E File Offset: 0x00012D8E
		public void ExecuteSetSelected()
		{
			if (this.Character != null)
			{
				PartyCharacterVM.SetSelected(this);
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00014BA3 File Offset: 0x00012DA3
		public void ExecuteTalk()
		{
			PartyVM partyVm = this._partyVm;
			if (partyVm == null)
			{
				return;
			}
			partyVm.ExecuteTalk();
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00014BB5 File Offset: 0x00012DB5
		public void UpdateTradeData()
		{
			PartyTradeVM tradeData = this.TradeData;
			if (tradeData == null)
			{
				return;
			}
			tradeData.UpdateTroopData(this.Troop, this.Side, true);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00014BD4 File Offset: 0x00012DD4
		public void UpdateRecruitable()
		{
			this.MaxConformity = this.Troop.Character.ConformityNeededToRecruitPrisoner;
			int elementXp = PartyBase.MainParty.PrisonRoster.GetElementXp(this.Troop.Character);
			this.CurrentConformity = ((elementXp >= this.Troop.Number * this.MaxConformity) ? this.MaxConformity : (elementXp % this.MaxConformity));
			this.IsRecruitablePrisoner = !this._character.IsHero && this.Type == PartyScreenLogic.TroopType.Prisoner;
			this.IsTroopRecruitable = this._partyScreenLogic.IsPrisonerRecruitable(this.Type, this.Character, this.Side) && !this._partyScreenLogic.IsTroopUpgradesDisabled;
			this.NumOfRecruitablePrisoners = this._partyScreenLogic.GetTroopRecruitableAmount(this.Character);
			GameTexts.SetVariable("LEFT", this.NumOfRecruitablePrisoners);
			GameTexts.SetVariable("RIGHT", this.Troop.Number);
			this.StrNumOfRecruitableTroop = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00014CEC File Offset: 0x00012EEC
		private void OnTradeApplyTransaction(int amount, bool isIncreasing)
		{
			this.TransferAmount = amount;
			PartyScreenLogic.PartyRosterSide partyRosterSide = (isIncreasing ? PartyScreenLogic.PartyRosterSide.Left : PartyScreenLogic.PartyRosterSide.Right);
			this.ApplyTransfer(this.TransferAmount, partyRosterSide);
			this.IsExecutable = this._partyScreenLogic.IsExecutable(this.Type, this.Character, this.Side) && this.Troop.Number > 0;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00014D50 File Offset: 0x00012F50
		public void InitializeUpgrades()
		{
			if (this.IsUpgradableTroop)
			{
				for (int i = 0; i < this.Character.UpgradeTargets.Length; i++)
				{
					CharacterObject characterObject = this.Character.UpgradeTargets[i];
					int level = characterObject.Level;
					int upgradeGoldCost = this.Character.GetUpgradeGoldCost(PartyBase.MainParty, i);
					if (!this.Character.Culture.IsBandit)
					{
						int level2 = this.Character.Level;
					}
					else
					{
						int level3 = this.Character.Level;
					}
					PerkObject perkObject;
					bool flag = Campaign.Current.Models.PartyTroopUpgradeModel.DoesPartyHaveRequiredPerksForUpgrade(PartyBase.MainParty, this.Character, characterObject, out perkObject);
					int num = (flag ? this.Troop.Number : 0);
					bool flag2 = true;
					int numOfCategoryItemPartyHas = this.GetNumOfCategoryItemPartyHas(this._partyScreenLogic.RightOwnerParty.ItemRoster, characterObject.UpgradeRequiresItemFromCategory);
					if (characterObject.UpgradeRequiresItemFromCategory != null)
					{
						flag2 = numOfCategoryItemPartyHas > 0;
					}
					bool flag3 = Hero.MainHero.Gold + this._partyScreenLogic.CurrentData.PartyGoldChangeAmount >= upgradeGoldCost;
					bool flag4 = level >= this.Character.Level && this.Troop.Xp >= this.Character.GetUpgradeXpCost(PartyBase.MainParty, i);
					bool flag5 = !flag2 || !flag3;
					int num2 = this.Troop.Number;
					if (upgradeGoldCost > 0)
					{
						num2 = (int)MathF.Clamp((float)MathF.Floor((float)(Hero.MainHero.Gold + this._partyScreenLogic.CurrentData.PartyGoldChangeAmount) / (float)upgradeGoldCost), 0f, (float)this.Troop.Number);
					}
					int num3 = ((characterObject.UpgradeRequiresItemFromCategory != null) ? numOfCategoryItemPartyHas : this.Troop.Number);
					int num4 = (flag4 ? ((int)MathF.Clamp((float)MathF.Floor((float)this.Troop.Xp / (float)this.Character.GetUpgradeXpCost(PartyBase.MainParty, i)), 0f, (float)this.Troop.Number)) : 0);
					int num5 = MathF.Min(MathF.Min(num2, num3), MathF.Min(num4, num));
					if (this.Character.Culture.IsBandit)
					{
						flag5 = flag5 || !Campaign.Current.Models.PartyTroopUpgradeModel.CanPartyUpgradeTroopToTarget(PartyBase.MainParty, this.Character, characterObject);
						num5 = ((!flag4) ? 0 : num5);
					}
					flag4 = flag4 && !this._partyVm.PartyScreenLogic.IsTroopUpgradesDisabled;
					string upgradeHint = CampaignUIHelper.GetUpgradeHint(i, numOfCategoryItemPartyHas, num5, upgradeGoldCost, flag, perkObject, this.Character, this.Troop, this._partyScreenLogic.CurrentData.PartyGoldChangeAmount, this._partyVm.PartyScreenLogic.IsTroopUpgradesDisabled);
					this.Upgrades[i].Refresh(num5, flag4, flag5, flag2, flag, upgradeHint, !this.Character.IsHero && this.Character.IsMariner);
					if (i == 0)
					{
						this.UpgradeCostText = upgradeGoldCost.ToString();
						this.HasEnoughGold = flag3;
						this.NumOfReadyToUpgradeTroops = num4;
						this.MaxXP = this.Character.GetUpgradeXpCost(PartyBase.MainParty, i);
						this.CurrentXP = ((this.Troop.Xp >= this.Troop.Number * this.MaxXP) ? this.MaxXP : (this.Troop.Xp % this.MaxXP));
					}
				}
				this.AnyUpgradeHasRequirement = this.Upgrades.Any<UpgradeTargetVM>((UpgradeTargetVM x) => x.Requirements.HasItemRequirement || x.Requirements.HasPerkRequirement);
			}
			int num6 = 0;
			foreach (UpgradeTargetVM upgradeTargetVM in this.Upgrades)
			{
				if (upgradeTargetVM.AvailableUpgrades > num6)
				{
					num6 = upgradeTargetVM.AvailableUpgrades;
				}
			}
			this.NumOfUpgradeableTroops = num6;
			this.IsTroopUpgradable = this.NumOfUpgradeableTroops > 0 && !this._partyVm.PartyScreenLogic.IsTroopUpgradesDisabled;
			GameTexts.SetVariable("LEFT", this.NumOfReadyToUpgradeTroops);
			GameTexts.SetVariable("RIGHT", this.Troop.Number);
			this.StrNumOfUpgradableTroop = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
			base.OnPropertyChanged("AmountOfUpgrades");
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x000151E4 File Offset: 0x000133E4
		public void OnTransferred()
		{
			if (this.Side != PartyScreenLogic.PartyRosterSide.Left || this.IsPrisoner)
			{
				this.InitializeUpgrades();
				return;
			}
			PartyCharacterVM partyCharacterVM = this._partyVm.MainPartyTroops.FirstOrDefault<PartyCharacterVM>((PartyCharacterVM x) => x.Character == this.Character);
			if (partyCharacterVM == null)
			{
				return;
			}
			partyCharacterVM.InitializeUpgrades();
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00015224 File Offset: 0x00013424
		public void ThrowOnPropertyChanged()
		{
			base.OnPropertyChanged("Name");
			base.OnPropertyChanged("Number");
			base.OnPropertyChanged("WoundedCount");
			base.OnPropertyChanged("IsTroopTransferrable");
			base.OnPropertyChanged("MaxCount");
			base.OnPropertyChanged("AmountOfUpgrades");
			base.OnPropertyChanged("Level");
			base.OnPropertyChanged("PartyIndex");
			base.OnPropertyChanged("Index");
			base.OnPropertyChanged("TroopNum");
			base.OnPropertyChanged("TransferString");
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000152AC File Offset: 0x000134AC
		public override bool Equals(object obj)
		{
			PartyCharacterVM partyCharacterVM;
			return obj != null && (partyCharacterVM = obj as PartyCharacterVM) != null && ((partyCharacterVM.Character == null && this.Code == null) || partyCharacterVM.Character == this.Character);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x000152EA File Offset: 0x000134EA
		private void ApplyTransfer(int transferAmount, PartyScreenLogic.PartyRosterSide side)
		{
			PartyCharacterVM.OnTransfer(this, -1, transferAmount, side);
			this.ThrowOnPropertyChanged();
			this.UpdateTalkable();
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00015306 File Offset: 0x00013506
		private void ExecuteTransfer()
		{
			this.ApplyTransfer(this.TransferAmount, this.Side);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0001531C File Offset: 0x0001351C
		private void ExecuteTransferAll()
		{
			this.ApplyTransfer(this.Troop.Number, this.Side);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00015343 File Offset: 0x00013543
		public void ExecuteSetFocused()
		{
			Action<PartyCharacterVM> onFocus = PartyCharacterVM.OnFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(this);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00015355 File Offset: 0x00013555
		public void ExecuteSetUnfocused()
		{
			Action<PartyCharacterVM> onFocus = PartyCharacterVM.OnFocus;
			if (onFocus == null)
			{
				return;
			}
			onFocus(null);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00015368 File Offset: 0x00013568
		public void ExecuteTransferSingle()
		{
			int num = 1;
			if (this._partyVm.IsEntireStackModifierActive)
			{
				num = this.Troop.Number;
			}
			else if (this._partyVm.IsFiveStackModifierActive)
			{
				num = MathF.Min(5, this.Troop.Number);
			}
			this.ApplyTransfer(num, this.Side);
			this._partyVm.ExecuteRemoveZeroCounts();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000153CF File Offset: 0x000135CF
		public void ExecuteResetTrade()
		{
			this.TradeData.ExecuteReset();
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000153DC File Offset: 0x000135DC
		public void Upgrade(int upgradeIndex, int maxUpgradeCount)
		{
			PartyVM partyVm = this._partyVm;
			if (partyVm == null)
			{
				return;
			}
			partyVm.ExecuteUpgrade(this, upgradeIndex, maxUpgradeCount);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000153F1 File Offset: 0x000135F1
		public void FocusUpgrade(UpgradeTargetVM upgrade)
		{
			this._partyVm.CurrentFocusedUpgrade = upgrade;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x000153FF File Offset: 0x000135FF
		public void RecruitAll()
		{
			if (this.IsTroopRecruitable)
			{
				this._partyVm.ExecuteRecruit(this, true);
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00015416 File Offset: 0x00013616
		public void ExecuteRecruitTroop()
		{
			if (this.IsTroopRecruitable)
			{
				this._partyVm.ExecuteRecruit(this, false);
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00015430 File Offset: 0x00013630
		public void ExecuteExecuteTroop()
		{
			if (this.IsExecutable)
			{
				if (FaceGen.GetMaturityTypeWithAge(this.Character.HeroObject.BodyProperties.Age) <= BodyMeshMaturityType.Tween)
				{
					return;
				}
				MBInformationManager.ShowSceneNotification(HeroExecutionSceneNotificationData.CreateForPlayerExecutingHero(this.Character.HeroObject, delegate
				{
					this._partyVm.ExecuteExecution();
				}, SceneNotificationData.RelevantContextType.Any, true));
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0001548C File Offset: 0x0001368C
		public void ExecuteOpenTroopEncyclopedia()
		{
			if (!this.Troop.Character.IsHero)
			{
				if (Campaign.Current.EncyclopediaManager.GetPageOf(typeof(CharacterObject)).IsValidEncyclopediaItem(this.Troop.Character))
				{
					Campaign.Current.EncyclopediaManager.GoToLink(this.Troop.Character.EncyclopediaLink);
					return;
				}
			}
			else if (Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero)).IsValidEncyclopediaItem(this.Troop.Character.HeroObject))
			{
				Campaign.Current.EncyclopediaManager.GoToLink(this.Troop.Character.HeroObject.EncyclopediaLink);
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0001554C File Offset: 0x0001374C
		private CharacterCode GetCharacterCode(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side)
		{
			IFaction faction = null;
			if (type != PartyScreenLogic.TroopType.Prisoner)
			{
				if (side == PartyScreenLogic.PartyRosterSide.Left && this._partyScreenLogic.LeftOwnerParty != null)
				{
					faction = this._partyScreenLogic.LeftOwnerParty.MapFaction;
				}
				else if (this.Side == PartyScreenLogic.PartyRosterSide.Right && this._partyScreenLogic.RightOwnerParty != null)
				{
					faction = this._partyScreenLogic.RightOwnerParty.MapFaction;
				}
			}
			uint num = Color.White.ToUnsignedInteger();
			uint num2 = Color.White.ToUnsignedInteger();
			if (faction != null)
			{
				num = faction.Color;
				num2 = faction.Color2;
			}
			else if (character.Culture != null)
			{
				num = character.Culture.Color;
				num2 = character.Culture.Color2;
			}
			Equipment equipment = character.Equipment;
			string text = ((equipment != null) ? equipment.CalculateEquipmentCode() : null);
			BodyProperties bodyProperties = character.GetBodyProperties(character.Equipment, -1);
			return CharacterCode.CreateFrom(text, bodyProperties, character.IsFemale, character.IsHero, num, num2, character.DefaultFormationClass, character.Race);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0001563C File Offset: 0x0001383C
		private void SetMoraleCost()
		{
			if (this.IsTroopRecruitable)
			{
				this.RecruitMoraleCostText = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetPrisonerRecruitmentMoraleEffect(this._partyScreenLogic.RightOwnerParty, this.Character, 1).ToString();
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00015688 File Offset: 0x00013888
		public void SetIsUpgradeButtonHighlighted(bool isHighlighted)
		{
			MBBindingList<UpgradeTargetVM> upgrades = this.Upgrades;
			if (upgrades == null)
			{
				return;
			}
			upgrades.ApplyActionOnAllItems(delegate(UpgradeTargetVM x)
			{
				x.IsHighlighted = isHighlighted;
			});
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x000156C0 File Offset: 0x000138C0
		public int GetNumOfCategoryItemPartyHas(ItemRoster items, ItemCategory itemCategory)
		{
			int num = 0;
			foreach (ItemRosterElement itemRosterElement in items)
			{
				if (itemRosterElement.EquipmentElement.Item.ItemCategory == itemCategory)
				{
					num += itemRosterElement.Amount;
				}
			}
			return num;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00015728 File Offset: 0x00013928
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x00015730 File Offset: 0x00013930
		// (set) Token: 0x060002BA RID: 698 RVA: 0x00015738 File Offset: 0x00013938
		[DataSourceProperty]
		public bool IsFormationEnabled
		{
			get
			{
				return this._isFormationEnabled;
			}
			set
			{
				if (this._isFormationEnabled != value)
				{
					this._isFormationEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsFormationEnabled");
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00015758 File Offset: 0x00013958
		[DataSourceProperty]
		public string TransferString
		{
			get
			{
				return this.TransferAmount.ToString() + "/" + this.Number.ToString();
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0001578B File Offset: 0x0001398B
		// (set) Token: 0x060002BD RID: 701 RVA: 0x00015793 File Offset: 0x00013993
		[DataSourceProperty]
		public bool IsTroopUpgradable
		{
			get
			{
				return this._isTroopUpgradable;
			}
			set
			{
				if (value != this._isTroopUpgradable)
				{
					this._isTroopUpgradable = value;
					base.OnPropertyChangedWithValue(value, "IsTroopUpgradable");
				}
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002BE RID: 702 RVA: 0x000157B1 File Offset: 0x000139B1
		// (set) Token: 0x060002BF RID: 703 RVA: 0x000157B9 File Offset: 0x000139B9
		[DataSourceProperty]
		public bool IsTroopRecruitable
		{
			get
			{
				return this._isTroopRecruitable;
			}
			set
			{
				if (value != this._isTroopRecruitable)
				{
					this._isTroopRecruitable = value;
					base.OnPropertyChangedWithValue(value, "IsTroopRecruitable");
				}
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x000157D7 File Offset: 0x000139D7
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x000157DF File Offset: 0x000139DF
		[DataSourceProperty]
		public bool IsRecruitablePrisoner
		{
			get
			{
				return this._isRecruitablePrisoner;
			}
			set
			{
				if (value != this._isRecruitablePrisoner)
				{
					this._isRecruitablePrisoner = value;
					base.OnPropertyChangedWithValue(value, "IsRecruitablePrisoner");
				}
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x000157FD File Offset: 0x000139FD
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x00015805 File Offset: 0x00013A05
		[DataSourceProperty]
		public bool IsUpgradableTroop
		{
			get
			{
				return this._isUpgradableTroop;
			}
			set
			{
				if (value != this._isUpgradableTroop)
				{
					this._isUpgradableTroop = value;
					base.OnPropertyChangedWithValue(value, "IsUpgradableTroop");
				}
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x00015823 File Offset: 0x00013A23
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x0001582B File Offset: 0x00013A2B
		[DataSourceProperty]
		public bool IsExecutable
		{
			get
			{
				return this._isExecutable;
			}
			set
			{
				if (value != this._isExecutable)
				{
					this._isExecutable = value;
					base.OnPropertyChangedWithValue(value, "IsExecutable");
				}
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x00015849 File Offset: 0x00013A49
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x00015851 File Offset: 0x00013A51
		[DataSourceProperty]
		public int NumOfReadyToUpgradeTroops
		{
			get
			{
				return this._numOfReadyToUpgradeTroops;
			}
			set
			{
				if (value != this._numOfReadyToUpgradeTroops)
				{
					this._numOfReadyToUpgradeTroops = value;
					base.OnPropertyChangedWithValue(value, "NumOfReadyToUpgradeTroops");
				}
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0001586F File Offset: 0x00013A6F
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x00015877 File Offset: 0x00013A77
		[DataSourceProperty]
		public int NumOfUpgradeableTroops
		{
			get
			{
				return this._numOfUpgradeableTroops;
			}
			set
			{
				if (value != this._numOfUpgradeableTroops)
				{
					this._numOfUpgradeableTroops = value;
					base.OnPropertyChangedWithValue(value, "NumOfUpgradeableTroops");
				}
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00015895 File Offset: 0x00013A95
		// (set) Token: 0x060002CB RID: 715 RVA: 0x0001589D File Offset: 0x00013A9D
		[DataSourceProperty]
		public int NumOfRecruitablePrisoners
		{
			get
			{
				return this._numOfRecruitablePrisoners;
			}
			set
			{
				if (value != this._numOfRecruitablePrisoners)
				{
					this._numOfRecruitablePrisoners = value;
					base.OnPropertyChangedWithValue(value, "NumOfRecruitablePrisoners");
				}
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002CC RID: 716 RVA: 0x000158BB File Offset: 0x00013ABB
		// (set) Token: 0x060002CD RID: 717 RVA: 0x000158C3 File Offset: 0x00013AC3
		[DataSourceProperty]
		public int MaxXP
		{
			get
			{
				return this._maxXP;
			}
			set
			{
				if (value != this._maxXP)
				{
					this._maxXP = value;
					base.OnPropertyChangedWithValue(value, "MaxXP");
				}
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002CE RID: 718 RVA: 0x000158E1 File Offset: 0x00013AE1
		// (set) Token: 0x060002CF RID: 719 RVA: 0x000158E9 File Offset: 0x00013AE9
		[DataSourceProperty]
		public int CurrentXP
		{
			get
			{
				return this._currentXP;
			}
			set
			{
				if (value != this._currentXP)
				{
					this._currentXP = value;
					base.OnPropertyChangedWithValue(value, "CurrentXP");
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00015907 File Offset: 0x00013B07
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x0001590F File Offset: 0x00013B0F
		[DataSourceProperty]
		public int CurrentConformity
		{
			get
			{
				return this._currentConformity;
			}
			set
			{
				if (value != this._currentConformity)
				{
					this._currentConformity = value;
					base.OnPropertyChangedWithValue(value, "CurrentConformity");
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0001592D File Offset: 0x00013B2D
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x00015935 File Offset: 0x00013B35
		[DataSourceProperty]
		public int MaxConformity
		{
			get
			{
				return this._maxConformity;
			}
			set
			{
				if (value != this._maxConformity)
				{
					this._maxConformity = value;
					base.OnPropertyChangedWithValue(value, "MaxConformity");
				}
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00015953 File Offset: 0x00013B53
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x0001595B File Offset: 0x00013B5B
		[DataSourceProperty]
		public BasicTooltipViewModel TroopXPTooltip
		{
			get
			{
				return this._troopXPTooltip;
			}
			set
			{
				if (value != this._troopXPTooltip)
				{
					this._troopXPTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "TroopXPTooltip");
				}
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x00015979 File Offset: 0x00013B79
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x00015981 File Offset: 0x00013B81
		[DataSourceProperty]
		public BasicTooltipViewModel TroopConformityTooltip
		{
			get
			{
				return this._troopConformityTooltip;
			}
			set
			{
				if (value != this._troopConformityTooltip)
				{
					this._troopConformityTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "TroopConformityTooltip");
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0001599F File Offset: 0x00013B9F
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x000159A7 File Offset: 0x00013BA7
		[DataSourceProperty]
		public BasicTooltipViewModel TransferHint
		{
			get
			{
				return this._transferHint;
			}
			set
			{
				if (value != this._transferHint)
				{
					this._transferHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "TransferHint");
				}
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002DA RID: 730 RVA: 0x000159C5 File Offset: 0x00013BC5
		// (set) Token: 0x060002DB RID: 731 RVA: 0x000159CD File Offset: 0x00013BCD
		[DataSourceProperty]
		public bool IsRecruitButtonsHiglighted
		{
			get
			{
				return this._isRecruitButtonsHiglighted;
			}
			set
			{
				if (value != this._isRecruitButtonsHiglighted)
				{
					this._isRecruitButtonsHiglighted = value;
					base.OnPropertyChangedWithValue(value, "IsRecruitButtonsHiglighted");
				}
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002DC RID: 732 RVA: 0x000159EB File Offset: 0x00013BEB
		// (set) Token: 0x060002DD RID: 733 RVA: 0x000159F3 File Offset: 0x00013BF3
		[DataSourceProperty]
		public bool IsTransferButtonHiglighted
		{
			get
			{
				return this._isTransferButtonHiglighted;
			}
			set
			{
				if (value != this._isTransferButtonHiglighted)
				{
					this._isTransferButtonHiglighted = value;
					base.OnPropertyChangedWithValue(value, "IsTransferButtonHiglighted");
				}
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00015A11 File Offset: 0x00013C11
		// (set) Token: 0x060002DF RID: 735 RVA: 0x00015A19 File Offset: 0x00013C19
		[DataSourceProperty]
		public string StrNumOfUpgradableTroop
		{
			get
			{
				return this._strNumOfUpgradableTroop;
			}
			set
			{
				if (value != this._strNumOfUpgradableTroop)
				{
					this._strNumOfUpgradableTroop = value;
					base.OnPropertyChangedWithValue<string>(value, "StrNumOfUpgradableTroop");
				}
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00015A3C File Offset: 0x00013C3C
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x00015A44 File Offset: 0x00013C44
		[DataSourceProperty]
		public string StrNumOfRecruitableTroop
		{
			get
			{
				return this._strNumOfRecruitableTroop;
			}
			set
			{
				if (value != this._strNumOfRecruitableTroop)
				{
					this._strNumOfRecruitableTroop = value;
					base.OnPropertyChangedWithValue<string>(value, "StrNumOfRecruitableTroop");
				}
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00015A67 File Offset: 0x00013C67
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x00015A6F File Offset: 0x00013C6F
		[DataSourceProperty]
		public string TroopID
		{
			get
			{
				return this._troopID;
			}
			set
			{
				if (value != this._troopID)
				{
					this._troopID = value;
					base.OnPropertyChangedWithValue<string>(value, "TroopID");
				}
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00015A92 File Offset: 0x00013C92
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x00015A9A File Offset: 0x00013C9A
		[DataSourceProperty]
		public string UpgradeCostText
		{
			get
			{
				return this._upgradeCostText;
			}
			set
			{
				if (value != this._upgradeCostText)
				{
					this._upgradeCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "UpgradeCostText");
				}
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x00015ABD File Offset: 0x00013CBD
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x00015AC5 File Offset: 0x00013CC5
		[DataSourceProperty]
		public string RecruitMoraleCostText
		{
			get
			{
				return this._recruitMoraleCostText;
			}
			set
			{
				if (value != this._recruitMoraleCostText)
				{
					this._recruitMoraleCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "RecruitMoraleCostText");
				}
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x00015AE8 File Offset: 0x00013CE8
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x00015AF0 File Offset: 0x00013CF0
		[DataSourceProperty]
		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				if (this._index != value)
				{
					this._index = value;
					base.OnPropertyChangedWithValue(value, "Index");
				}
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00015B0E File Offset: 0x00013D0E
		// (set) Token: 0x060002EB RID: 747 RVA: 0x00015B16 File Offset: 0x00013D16
		[DataSourceProperty]
		public int TransferAmount
		{
			get
			{
				return this._transferAmount;
			}
			set
			{
				if (value <= 0)
				{
					value = 1;
				}
				if (this._transferAmount != value)
				{
					this._transferAmount = value;
					base.OnPropertyChangedWithValue(value, "TransferAmount");
					base.OnPropertyChanged("TransferString");
				}
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00015B46 File Offset: 0x00013D46
		// (set) Token: 0x060002ED RID: 749 RVA: 0x00015B4E File Offset: 0x00013D4E
		[DataSourceProperty]
		public bool IsTroopTransferrable
		{
			get
			{
				return this._isTroopTransferrable;
			}
			set
			{
				if (this.Character != CharacterObject.PlayerCharacter)
				{
					this._isTroopTransferrable = value;
					base.OnPropertyChangedWithValue(value, "IsTroopTransferrable");
				}
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00015B70 File Offset: 0x00013D70
		// (set) Token: 0x060002EF RID: 751 RVA: 0x00015B78 File Offset: 0x00013D78
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00015B9C File Offset: 0x00013D9C
		[DataSourceProperty]
		public string TroopNum
		{
			get
			{
				if (this.Character != null && this.Character.IsHero)
				{
					return "1";
				}
				if (this.Troop.Character == null)
				{
					return "-1";
				}
				int num = this.Troop.Number - this.Troop.WoundedNumber;
				string text = GameTexts.FindText("str_party_nameplate_wounded_abbr", null).ToString();
				if (num != this.Troop.Number && this.Type != PartyScreenLogic.TroopType.Prisoner)
				{
					return string.Concat(new object[]
					{
						num,
						"+",
						this.Troop.WoundedNumber,
						text
					});
				}
				return this.Troop.Number.ToString();
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x00015C70 File Offset: 0x00013E70
		[DataSourceProperty]
		public bool IsHeroWounded
		{
			get
			{
				CharacterObject character = this.Character;
				return character != null && character.IsHero && this.Character.HeroObject.IsWounded;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x00015C98 File Offset: 0x00013E98
		[DataSourceProperty]
		public int HeroHealth
		{
			get
			{
				CharacterObject character = this.Character;
				if (character != null && character.IsHero)
				{
					return MathF.Ceiling((float)this.Character.HeroObject.HitPoints * 100f / (float)this.Character.MaxHitPoints());
				}
				return 0;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x00015CE4 File Offset: 0x00013EE4
		[DataSourceProperty]
		public int Number
		{
			get
			{
				this.IsTroopTransferrable = this._initIsTroopTransferable && this.Troop.Number > 0;
				return this.Troop.Number;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00015D24 File Offset: 0x00013F24
		[DataSourceProperty]
		public int WoundedCount
		{
			get
			{
				if (this.Troop.Character == null)
				{
					return 0;
				}
				return this.Troop.WoundedNumber;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00015D4E File Offset: 0x00013F4E
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x00015D56 File Offset: 0x00013F56
		[DataSourceProperty]
		public BasicTooltipViewModel RecruitPrisonerHint
		{
			get
			{
				return this._recruitPrisonerHint;
			}
			set
			{
				if (value != this._recruitPrisonerHint)
				{
					this._recruitPrisonerHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "RecruitPrisonerHint");
				}
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x00015D74 File Offset: 0x00013F74
		// (set) Token: 0x060002F8 RID: 760 RVA: 0x00015D7C File Offset: 0x00013F7C
		[DataSourceProperty]
		public CharacterImageIdentifierVM Code
		{
			get
			{
				return this._code;
			}
			set
			{
				if (value != this._code)
				{
					this._code = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Code");
				}
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00015D9A File Offset: 0x00013F9A
		// (set) Token: 0x060002FA RID: 762 RVA: 0x00015DA2 File Offset: 0x00013FA2
		[DataSourceProperty]
		public BasicTooltipViewModel ExecutePrisonerHint
		{
			get
			{
				return this._executePrisonerHint;
			}
			set
			{
				if (value != this._executePrisonerHint)
				{
					this._executePrisonerHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ExecutePrisonerHint");
				}
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00015DC0 File Offset: 0x00013FC0
		// (set) Token: 0x060002FC RID: 764 RVA: 0x00015DC8 File Offset: 0x00013FC8
		[DataSourceProperty]
		public MBBindingList<UpgradeTargetVM> Upgrades
		{
			get
			{
				return this._upgrades;
			}
			set
			{
				if (value != this._upgrades)
				{
					this._upgrades = value;
					base.OnPropertyChangedWithValue<MBBindingList<UpgradeTargetVM>>(value, "Upgrades");
				}
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00015DE6 File Offset: 0x00013FE6
		// (set) Token: 0x060002FE RID: 766 RVA: 0x00015DEE File Offset: 0x00013FEE
		[DataSourceProperty]
		public BasicTooltipViewModel HeroHealthHint
		{
			get
			{
				return this._heroHealthHint;
			}
			set
			{
				if (value != this._heroHealthHint)
				{
					this._heroHealthHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "HeroHealthHint");
				}
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00015E0C File Offset: 0x0001400C
		// (set) Token: 0x06000300 RID: 768 RVA: 0x00015E14 File Offset: 0x00014014
		[DataSourceProperty]
		public bool IsHero
		{
			get
			{
				return this._isHero;
			}
			set
			{
				if (value != this._isHero)
				{
					this._isHero = value;
					base.OnPropertyChangedWithValue(value, "IsHero");
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00015E32 File Offset: 0x00014032
		// (set) Token: 0x06000302 RID: 770 RVA: 0x00015E3A File Offset: 0x0001403A
		[DataSourceProperty]
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (value != this._isMainHero)
				{
					this._isMainHero = value;
					base.OnPropertyChangedWithValue(value, "IsMainHero");
				}
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00015E58 File Offset: 0x00014058
		// (set) Token: 0x06000304 RID: 772 RVA: 0x00015E60 File Offset: 0x00014060
		[DataSourceProperty]
		public bool IsPrisoner
		{
			get
			{
				return this._isPrisoner;
			}
			set
			{
				if (value != this._isPrisoner)
				{
					this._isPrisoner = value;
					base.OnPropertyChangedWithValue(value, "IsPrisoner");
				}
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00015E7E File Offset: 0x0001407E
		// (set) Token: 0x06000306 RID: 774 RVA: 0x00015E86 File Offset: 0x00014086
		[DataSourceProperty]
		public bool IsPrisonerOfPlayer
		{
			get
			{
				return this._isPrisonerOfPlayer;
			}
			set
			{
				if (value != this._isPrisonerOfPlayer)
				{
					this._isPrisonerOfPlayer = value;
					base.OnPropertyChangedWithValue(value, "IsPrisonerOfPlayer");
				}
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00015EA4 File Offset: 0x000140A4
		// (set) Token: 0x06000308 RID: 776 RVA: 0x00015EAC File Offset: 0x000140AC
		[DataSourceProperty]
		public bool IsHeroPrisonerOfPlayer
		{
			get
			{
				return this._isHeroPrisonerOfPlayer;
			}
			set
			{
				if (value != this._isHeroPrisonerOfPlayer)
				{
					this._isHeroPrisonerOfPlayer = value;
					base.OnPropertyChangedWithValue(value, "IsHeroPrisonerOfPlayer");
				}
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00015ECA File Offset: 0x000140CA
		// (set) Token: 0x0600030A RID: 778 RVA: 0x00015ED2 File Offset: 0x000140D2
		[DataSourceProperty]
		public bool AnyUpgradeHasRequirement
		{
			get
			{
				return this._anyUpgradeHasRequirement;
			}
			set
			{
				if (value != this._anyUpgradeHasRequirement)
				{
					this._anyUpgradeHasRequirement = value;
					base.OnPropertyChangedWithValue(value, "AnyUpgradeHasRequirement");
				}
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00015EF0 File Offset: 0x000140F0
		// (set) Token: 0x0600030C RID: 780 RVA: 0x00015EF8 File Offset: 0x000140F8
		[DataSourceProperty]
		public StringItemWithHintVM TierIconData
		{
			get
			{
				return this._tierIconData;
			}
			set
			{
				if (value != this._tierIconData)
				{
					this._tierIconData = value;
					base.OnPropertyChangedWithValue<StringItemWithHintVM>(value, "TierIconData");
				}
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00015F16 File Offset: 0x00014116
		// (set) Token: 0x0600030E RID: 782 RVA: 0x00015F1E File Offset: 0x0001411E
		[DataSourceProperty]
		public StringItemWithHintVM TypeIconData
		{
			get
			{
				return this._typeIconData;
			}
			set
			{
				if (value != this._typeIconData)
				{
					this._typeIconData = value;
					base.OnPropertyChangedWithValue<StringItemWithHintVM>(value, "TypeIconData");
				}
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00015F3C File Offset: 0x0001413C
		// (set) Token: 0x06000310 RID: 784 RVA: 0x00015F44 File Offset: 0x00014144
		[DataSourceProperty]
		public bool HasEnoughGold
		{
			get
			{
				return this._hasEnoughGold;
			}
			set
			{
				if (value != this._hasEnoughGold)
				{
					this._hasEnoughGold = value;
					base.OnPropertyChangedWithValue(value, "HasEnoughGold");
				}
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00015F62 File Offset: 0x00014162
		// (set) Token: 0x06000312 RID: 786 RVA: 0x00015F6A File Offset: 0x0001416A
		[DataSourceProperty]
		public bool IsTalkableCharacter
		{
			get
			{
				return this._isTalkableCharacter;
			}
			set
			{
				if (value != this._isTalkableCharacter)
				{
					this._isTalkableCharacter = value;
					base.OnPropertyChangedWithValue(value, "IsTalkableCharacter");
				}
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00015F88 File Offset: 0x00014188
		// (set) Token: 0x06000314 RID: 788 RVA: 0x00015F90 File Offset: 0x00014190
		[DataSourceProperty]
		public bool CanTalk
		{
			get
			{
				return this._canTalk;
			}
			set
			{
				if (value != this._canTalk)
				{
					this._canTalk = value;
					base.OnPropertyChangedWithValue(value, "CanTalk");
				}
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00015FAE File Offset: 0x000141AE
		// (set) Token: 0x06000316 RID: 790 RVA: 0x00015FB6 File Offset: 0x000141B6
		[DataSourceProperty]
		public HintViewModel TalkHint
		{
			get
			{
				return this._talkHint;
			}
			set
			{
				if (value != this._talkHint)
				{
					this._talkHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "TalkHint");
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000317 RID: 791 RVA: 0x00015FD4 File Offset: 0x000141D4
		// (set) Token: 0x06000318 RID: 792 RVA: 0x00015FDC File Offset: 0x000141DC
		[DataSourceProperty]
		public PartyTradeVM TradeData
		{
			get
			{
				return this._tradeData;
			}
			set
			{
				if (value != this._tradeData)
				{
					this._tradeData = value;
					base.OnPropertyChangedWithValue<PartyTradeVM>(value, "TradeData");
				}
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00015FFA File Offset: 0x000141FA
		// (set) Token: 0x0600031A RID: 794 RVA: 0x00016002 File Offset: 0x00014202
		[DataSourceProperty]
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
			set
			{
				if (value != this._isLocked)
				{
					this._isLocked = value;
					base.OnPropertyChangedWithValue(value, "IsLocked");
					Action<PartyCharacterVM, bool> processCharacterLock = PartyCharacterVM.ProcessCharacterLock;
					if (processCharacterLock == null)
					{
						return;
					}
					processCharacterLock(this, value);
				}
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00016031 File Offset: 0x00014231
		// (set) Token: 0x0600031C RID: 796 RVA: 0x00016039 File Offset: 0x00014239
		[DataSourceProperty]
		public HintViewModel LockHint
		{
			get
			{
				return this._lockHint;
			}
			set
			{
				if (value != this._lockHint)
				{
					this._lockHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LockHint");
				}
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00016057 File Offset: 0x00014257
		// (set) Token: 0x0600031E RID: 798 RVA: 0x0001605F File Offset: 0x0001425F
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x0400012C RID: 300
		public static bool IsShiftingDisabled;

		// Token: 0x0400012D RID: 301
		public static Action<PartyCharacterVM, bool> ProcessCharacterLock;

		// Token: 0x0400012E RID: 302
		public static Action<PartyCharacterVM> SetSelected;

		// Token: 0x0400012F RID: 303
		public static Action<PartyCharacterVM, int, int, PartyScreenLogic.PartyRosterSide> OnTransfer;

		// Token: 0x04000130 RID: 304
		public static Action<PartyCharacterVM> OnShift;

		// Token: 0x04000131 RID: 305
		public static Action<PartyCharacterVM> OnFocus;

		// Token: 0x04000132 RID: 306
		public readonly PartyScreenLogic.PartyRosterSide Side;

		// Token: 0x04000133 RID: 307
		public readonly PartyScreenLogic.TroopType Type;

		// Token: 0x04000134 RID: 308
		protected readonly PartyVM _partyVm;

		// Token: 0x04000135 RID: 309
		protected readonly PartyScreenLogic _partyScreenLogic;

		// Token: 0x04000136 RID: 310
		protected readonly bool _initIsTroopTransferable;

		// Token: 0x04000137 RID: 311
		private Tuple<bool, TextObject> _partyCharacterTalkPermission;

		// Token: 0x0400013A RID: 314
		private TroopRosterElement _troop;

		// Token: 0x0400013B RID: 315
		private CharacterObject _character;

		// Token: 0x0400013C RID: 316
		private string _name;

		// Token: 0x0400013D RID: 317
		private string _strNumOfUpgradableTroop;

		// Token: 0x0400013E RID: 318
		private string _strNumOfRecruitableTroop;

		// Token: 0x0400013F RID: 319
		private string _troopID;

		// Token: 0x04000140 RID: 320
		private string _upgradeCostText;

		// Token: 0x04000141 RID: 321
		private string _recruitMoraleCostText;

		// Token: 0x04000142 RID: 322
		private MBBindingList<UpgradeTargetVM> _upgrades;

		// Token: 0x04000143 RID: 323
		private CharacterImageIdentifierVM _code;

		// Token: 0x04000144 RID: 324
		private BasicTooltipViewModel _transferHint;

		// Token: 0x04000145 RID: 325
		private BasicTooltipViewModel _recruitPrisonerHint;

		// Token: 0x04000146 RID: 326
		private BasicTooltipViewModel _executePrisonerHint;

		// Token: 0x04000147 RID: 327
		private BasicTooltipViewModel _heroHealthHint;

		// Token: 0x04000148 RID: 328
		private HintViewModel _talkHint;

		// Token: 0x04000149 RID: 329
		private int _transferAmount = 1;

		// Token: 0x0400014A RID: 330
		private int _index = -2;

		// Token: 0x0400014B RID: 331
		private int _numOfReadyToUpgradeTroops;

		// Token: 0x0400014C RID: 332
		private int _numOfUpgradeableTroops;

		// Token: 0x0400014D RID: 333
		private int _numOfRecruitablePrisoners;

		// Token: 0x0400014E RID: 334
		private int _maxXP;

		// Token: 0x0400014F RID: 335
		private int _currentXP;

		// Token: 0x04000150 RID: 336
		private int _maxConformity;

		// Token: 0x04000151 RID: 337
		private int _currentConformity;

		// Token: 0x04000152 RID: 338
		private BasicTooltipViewModel _troopXPTooltip;

		// Token: 0x04000153 RID: 339
		private BasicTooltipViewModel _troopConformityTooltip;

		// Token: 0x04000154 RID: 340
		private bool _isHero;

		// Token: 0x04000155 RID: 341
		private bool _isMainHero;

		// Token: 0x04000156 RID: 342
		private bool _isPrisoner;

		// Token: 0x04000157 RID: 343
		private bool _isPrisonerOfPlayer;

		// Token: 0x04000158 RID: 344
		private bool _isRecruitablePrisoner;

		// Token: 0x04000159 RID: 345
		private bool _isUpgradableTroop;

		// Token: 0x0400015A RID: 346
		private bool _isTroopTransferrable;

		// Token: 0x0400015B RID: 347
		private bool _isHeroPrisonerOfPlayer;

		// Token: 0x0400015C RID: 348
		private bool _isTroopUpgradable;

		// Token: 0x0400015D RID: 349
		private StringItemWithHintVM _tierIconData;

		// Token: 0x0400015E RID: 350
		private bool _hasEnoughGold;

		// Token: 0x0400015F RID: 351
		private bool _anyUpgradeHasRequirement;

		// Token: 0x04000160 RID: 352
		private StringItemWithHintVM _typeIconData;

		// Token: 0x04000161 RID: 353
		private bool _isRecruitButtonsHiglighted;

		// Token: 0x04000162 RID: 354
		private bool _isTransferButtonHiglighted;

		// Token: 0x04000163 RID: 355
		private bool _isFormationEnabled;

		// Token: 0x04000164 RID: 356
		private PartyTradeVM _tradeData;

		// Token: 0x04000165 RID: 357
		private bool _isTroopRecruitable;

		// Token: 0x04000166 RID: 358
		private bool _isExecutable;

		// Token: 0x04000167 RID: 359
		private bool _isLocked;

		// Token: 0x04000168 RID: 360
		private HintViewModel _lockHint;

		// Token: 0x04000169 RID: 361
		private bool _isTalkableCharacter;

		// Token: 0x0400016A RID: 362
		private bool _canTalk;

		// Token: 0x0400016B RID: 363
		private bool _isSelected;
	}
}
