using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x020002FF RID: 767
	public class PartyScreenLogic
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06002C90 RID: 11408 RVA: 0x000BAC8C File Offset: 0x000B8E8C
		// (remove) Token: 0x06002C91 RID: 11409 RVA: 0x000BACC4 File Offset: 0x000B8EC4
		public event PartyScreenLogic.PartyGoldDelegate PartyGoldChange;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06002C92 RID: 11410 RVA: 0x000BACFC File Offset: 0x000B8EFC
		// (remove) Token: 0x06002C93 RID: 11411 RVA: 0x000BAD34 File Offset: 0x000B8F34
		public event PartyScreenLogic.PartyMoraleDelegate PartyMoraleChange;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06002C94 RID: 11412 RVA: 0x000BAD6C File Offset: 0x000B8F6C
		// (remove) Token: 0x06002C95 RID: 11413 RVA: 0x000BADA4 File Offset: 0x000B8FA4
		public event PartyScreenLogic.PartyInfluenceDelegate PartyInfluenceChange;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06002C96 RID: 11414 RVA: 0x000BADDC File Offset: 0x000B8FDC
		// (remove) Token: 0x06002C97 RID: 11415 RVA: 0x000BAE14 File Offset: 0x000B9014
		public event PartyScreenLogic.PartyHorseDelegate PartyHorseChange;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06002C98 RID: 11416 RVA: 0x000BAE4C File Offset: 0x000B904C
		// (remove) Token: 0x06002C99 RID: 11417 RVA: 0x000BAE84 File Offset: 0x000B9084
		public event PartyScreenLogic.PresentationUpdate Update;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06002C9A RID: 11418 RVA: 0x000BAEBC File Offset: 0x000B90BC
		// (remove) Token: 0x06002C9B RID: 11419 RVA: 0x000BAEF4 File Offset: 0x000B90F4
		public event PartyScreenClosedDelegate PartyScreenClosedEvent;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06002C9C RID: 11420 RVA: 0x000BAF2C File Offset: 0x000B912C
		// (remove) Token: 0x06002C9D RID: 11421 RVA: 0x000BAF64 File Offset: 0x000B9164
		public event PartyScreenLogic.AfterResetDelegate AfterReset;

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06002C9E RID: 11422 RVA: 0x000BAF99 File Offset: 0x000B9199
		// (set) Token: 0x06002C9F RID: 11423 RVA: 0x000BAFA1 File Offset: 0x000B91A1
		public PartyScreenLogic.TroopSortType ActiveOtherPartySortType
		{
			get
			{
				return this._activeOtherPartySortType;
			}
			set
			{
				this._activeOtherPartySortType = value;
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06002CA0 RID: 11424 RVA: 0x000BAFAA File Offset: 0x000B91AA
		// (set) Token: 0x06002CA1 RID: 11425 RVA: 0x000BAFB2 File Offset: 0x000B91B2
		public PartyScreenLogic.TroopSortType ActiveMainPartySortType
		{
			get
			{
				return this._activeMainPartySortType;
			}
			set
			{
				this._activeMainPartySortType = value;
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06002CA2 RID: 11426 RVA: 0x000BAFBB File Offset: 0x000B91BB
		// (set) Token: 0x06002CA3 RID: 11427 RVA: 0x000BAFC3 File Offset: 0x000B91C3
		public bool IsOtherPartySortAscending
		{
			get
			{
				return this._isOtherPartySortAscending;
			}
			set
			{
				this._isOtherPartySortAscending = value;
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06002CA4 RID: 11428 RVA: 0x000BAFCC File Offset: 0x000B91CC
		// (set) Token: 0x06002CA5 RID: 11429 RVA: 0x000BAFD4 File Offset: 0x000B91D4
		public bool IsMainPartySortAscending
		{
			get
			{
				return this._isMainPartySortAscending;
			}
			set
			{
				this._isMainPartySortAscending = value;
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06002CA6 RID: 11430 RVA: 0x000BAFDD File Offset: 0x000B91DD
		// (set) Token: 0x06002CA7 RID: 11431 RVA: 0x000BAFE5 File Offset: 0x000B91E5
		public PartyScreenLogic.TransferState MemberTransferState { get; private set; }

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06002CA8 RID: 11432 RVA: 0x000BAFEE File Offset: 0x000B91EE
		// (set) Token: 0x06002CA9 RID: 11433 RVA: 0x000BAFF6 File Offset: 0x000B91F6
		public PartyScreenLogic.TransferState PrisonerTransferState { get; private set; }

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06002CAA RID: 11434 RVA: 0x000BAFFF File Offset: 0x000B91FF
		// (set) Token: 0x06002CAB RID: 11435 RVA: 0x000BB007 File Offset: 0x000B9207
		public PartyScreenLogic.TransferState AccompanyingTransferState { get; private set; }

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06002CAC RID: 11436 RVA: 0x000BB010 File Offset: 0x000B9210
		// (set) Token: 0x06002CAD RID: 11437 RVA: 0x000BB018 File Offset: 0x000B9218
		public TextObject LeftPartyName { get; private set; }

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06002CAE RID: 11438 RVA: 0x000BB021 File Offset: 0x000B9221
		// (set) Token: 0x06002CAF RID: 11439 RVA: 0x000BB029 File Offset: 0x000B9229
		public TextObject RightPartyName { get; private set; }

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06002CB0 RID: 11440 RVA: 0x000BB032 File Offset: 0x000B9232
		// (set) Token: 0x06002CB1 RID: 11441 RVA: 0x000BB03A File Offset: 0x000B923A
		public TextObject Header { get; private set; }

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06002CB2 RID: 11442 RVA: 0x000BB043 File Offset: 0x000B9243
		// (set) Token: 0x06002CB3 RID: 11443 RVA: 0x000BB04B File Offset: 0x000B924B
		public int LeftPartyMembersSizeLimit { get; private set; }

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06002CB4 RID: 11444 RVA: 0x000BB054 File Offset: 0x000B9254
		// (set) Token: 0x06002CB5 RID: 11445 RVA: 0x000BB05C File Offset: 0x000B925C
		public int LeftPartyPrisonersSizeLimit { get; private set; }

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06002CB6 RID: 11446 RVA: 0x000BB065 File Offset: 0x000B9265
		// (set) Token: 0x06002CB7 RID: 11447 RVA: 0x000BB06D File Offset: 0x000B926D
		public int RightPartyMembersSizeLimit { get; private set; }

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06002CB8 RID: 11448 RVA: 0x000BB076 File Offset: 0x000B9276
		// (set) Token: 0x06002CB9 RID: 11449 RVA: 0x000BB07E File Offset: 0x000B927E
		public int RightPartyPrisonersSizeLimit { get; private set; }

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06002CBA RID: 11450 RVA: 0x000BB087 File Offset: 0x000B9287
		// (set) Token: 0x06002CBB RID: 11451 RVA: 0x000BB08F File Offset: 0x000B928F
		public bool DoNotApplyGoldTransactions { get; private set; }

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06002CBC RID: 11452 RVA: 0x000BB098 File Offset: 0x000B9298
		// (set) Token: 0x06002CBD RID: 11453 RVA: 0x000BB0A0 File Offset: 0x000B92A0
		public bool ShowProgressBar { get; private set; }

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06002CBE RID: 11454 RVA: 0x000BB0A9 File Offset: 0x000B92A9
		// (set) Token: 0x06002CBF RID: 11455 RVA: 0x000BB0B1 File Offset: 0x000B92B1
		public string DoneReasonString { get; private set; }

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06002CC0 RID: 11456 RVA: 0x000BB0BA File Offset: 0x000B92BA
		// (set) Token: 0x06002CC1 RID: 11457 RVA: 0x000BB0C2 File Offset: 0x000B92C2
		public bool IsTroopUpgradesDisabled { get; private set; }

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06002CC2 RID: 11458 RVA: 0x000BB0CB File Offset: 0x000B92CB
		// (set) Token: 0x06002CC3 RID: 11459 RVA: 0x000BB0D3 File Offset: 0x000B92D3
		public CharacterObject RightPartyLeader { get; private set; }

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06002CC4 RID: 11460 RVA: 0x000BB0DC File Offset: 0x000B92DC
		// (set) Token: 0x06002CC5 RID: 11461 RVA: 0x000BB0E4 File Offset: 0x000B92E4
		public CharacterObject LeftPartyLeader { get; private set; }

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06002CC6 RID: 11462 RVA: 0x000BB0ED File Offset: 0x000B92ED
		// (set) Token: 0x06002CC7 RID: 11463 RVA: 0x000BB0F5 File Offset: 0x000B92F5
		public PartyBase LeftOwnerParty { get; private set; }

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06002CC8 RID: 11464 RVA: 0x000BB0FE File Offset: 0x000B92FE
		// (set) Token: 0x06002CC9 RID: 11465 RVA: 0x000BB106 File Offset: 0x000B9306
		public PartyBase RightOwnerParty { get; private set; }

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06002CCA RID: 11466 RVA: 0x000BB10F File Offset: 0x000B930F
		// (set) Token: 0x06002CCB RID: 11467 RVA: 0x000BB117 File Offset: 0x000B9317
		public PartyScreenData CurrentData { get; private set; }

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06002CCC RID: 11468 RVA: 0x000BB120 File Offset: 0x000B9320
		// (set) Token: 0x06002CCD RID: 11469 RVA: 0x000BB128 File Offset: 0x000B9328
		public bool TransferHealthiesGetWoundedsFirst { get; private set; }

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06002CCE RID: 11470 RVA: 0x000BB131 File Offset: 0x000B9331
		// (set) Token: 0x06002CCF RID: 11471 RVA: 0x000BB139 File Offset: 0x000B9339
		public int QuestModeWageDaysMultiplier { get; private set; }

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06002CD0 RID: 11472 RVA: 0x000BB142 File Offset: 0x000B9342
		// (set) Token: 0x06002CD1 RID: 11473 RVA: 0x000BB14A File Offset: 0x000B934A
		public Game Game
		{
			get
			{
				return this._game;
			}
			set
			{
				this._game = value;
			}
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000BB154 File Offset: 0x000B9354
		public PartyScreenLogic()
		{
			this._game = Game.Current;
			this.MemberRosters = new TroopRoster[2];
			this.PrisonerRosters = new TroopRoster[2];
			this.CurrentData = new PartyScreenData();
			this._initialData = new PartyScreenData();
			this._defaultComparers = new Dictionary<PartyScreenLogic.TroopSortType, PartyScreenLogic.TroopComparer>
			{
				{
					PartyScreenLogic.TroopSortType.Custom,
					new PartyScreenLogic.TroopDefaultComparer()
				},
				{
					PartyScreenLogic.TroopSortType.Type,
					new PartyScreenLogic.TroopTypeComparer()
				},
				{
					PartyScreenLogic.TroopSortType.Name,
					new PartyScreenLogic.TroopNameComparer()
				},
				{
					PartyScreenLogic.TroopSortType.Count,
					new PartyScreenLogic.TroopCountComparer()
				},
				{
					PartyScreenLogic.TroopSortType.Tier,
					new PartyScreenLogic.TroopTierComparer()
				}
			};
			this.IsTroopUpgradesDisabled = false;
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x000BB1F0 File Offset: 0x000B93F0
		public void Initialize(PartyScreenLogicInitializationData initializationData)
		{
			this.MemberRosters[1] = initializationData.RightMemberRoster;
			this.PrisonerRosters[1] = initializationData.RightPrisonerRoster;
			this.MemberRosters[0] = initializationData.LeftMemberRoster;
			this.PrisonerRosters[0] = initializationData.LeftPrisonerRoster;
			Hero rightLeaderHero = initializationData.RightLeaderHero;
			this.RightPartyLeader = ((rightLeaderHero != null) ? rightLeaderHero.CharacterObject : null);
			Hero leftLeaderHero = initializationData.LeftLeaderHero;
			this.LeftPartyLeader = ((leftLeaderHero != null) ? leftLeaderHero.CharacterObject : null);
			this.RightOwnerParty = initializationData.RightOwnerParty;
			this.LeftOwnerParty = initializationData.LeftOwnerParty;
			this.RightPartyName = initializationData.RightPartyName;
			this.RightPartyMembersSizeLimit = initializationData.RightPartyMembersSizeLimit;
			this.RightPartyPrisonersSizeLimit = initializationData.RightPartyPrisonersSizeLimit;
			this.LeftPartyName = initializationData.LeftPartyName;
			this.LeftPartyMembersSizeLimit = initializationData.LeftPartyMembersSizeLimit;
			this.LeftPartyPrisonersSizeLimit = initializationData.LeftPartyPrisonersSizeLimit;
			this.Header = initializationData.Header;
			this.QuestModeWageDaysMultiplier = initializationData.QuestModeWageDaysMultiplier;
			this.TransferHealthiesGetWoundedsFirst = initializationData.TransferHealthiesGetWoundedsFirst;
			this.SetPartyGoldChangeAmount(0);
			this.SetHorseChangeAmount(0);
			this.SetInfluenceChangeAmount(0, 0, 0);
			this.SetMoraleChangeAmount(0);
			this.CurrentData.BindRostersFrom(this.MemberRosters[1], this.PrisonerRosters[1], this.MemberRosters[0], this.PrisonerRosters[0], this.RightOwnerParty, this.LeftOwnerParty);
			this._initialData.InitializeCopyFrom(initializationData.RightOwnerParty, initializationData.LeftOwnerParty);
			this._initialData.CopyFromPartyAndRoster(this.MemberRosters[1], this.PrisonerRosters[1], this.MemberRosters[0], this.PrisonerRosters[0], this.RightOwnerParty);
			if (initializationData.PartyPresentationDoneButtonDelegate == null)
			{
				Debug.FailedAssert("Done handler is given null for party screen!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenLogic.cs", "Initialize", 242);
				initializationData.PartyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenLogic.DefaultDoneHandler);
			}
			this.PartyPresentationDoneButtonDelegate = initializationData.PartyPresentationDoneButtonDelegate;
			this.PartyPresentationDoneButtonConditionDelegate = initializationData.PartyPresentationDoneButtonConditionDelegate;
			this.PartyPresentationCancelButtonActivateDelegate = initializationData.PartyPresentationCancelButtonActivateDelegate;
			this.PartyPresentationCancelButtonDelegate = initializationData.PartyPresentationCancelButtonDelegate;
			this.IsTroopUpgradesDisabled = initializationData.IsTroopUpgradesDisabled || initializationData.RightOwnerParty == null;
			this.MemberTransferState = initializationData.MemberTransferState;
			this.PrisonerTransferState = initializationData.PrisonerTransferState;
			this.AccompanyingTransferState = initializationData.AccompanyingTransferState;
			this.IsTroopTransferableDelegate = initializationData.TroopTransferableDelegate;
			this.CanTalkToHeroDelegate = initializationData.CanTalkToTroopDelegate;
			this.PartyPresentationCancelButtonActivateDelegate = initializationData.PartyPresentationCancelButtonActivateDelegate;
			this.PartyPresentationCancelButtonDelegate = initializationData.PartyPresentationCancelButtonDelegate;
			this.PartyScreenClosedEvent = initializationData.PartyScreenClosedDelegate;
			this.DoNotApplyGoldTransactions = initializationData.DoNotApplyGoldTransactions;
			this.ShowProgressBar = initializationData.ShowProgressBar;
			if (this._partyScreenMode == PartyScreenHelper.PartyScreenMode.QuestTroopManage)
			{
				int num = -this.MemberRosters[0].Sum((TroopRosterElement t) => t.Character.TroopWage * t.Number * this.QuestModeWageDaysMultiplier);
				this._initialData.PartyGoldChangeAmount = num;
				this.SetPartyGoldChangeAmount(num);
			}
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000BB4B3 File Offset: 0x000B96B3
		private void SetPartyGoldChangeAmount(int newTotalAmount)
		{
			this.CurrentData.PartyGoldChangeAmount = newTotalAmount;
			PartyScreenLogic.PartyGoldDelegate partyGoldChange = this.PartyGoldChange;
			if (partyGoldChange == null)
			{
				return;
			}
			partyGoldChange();
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000BB4D1 File Offset: 0x000B96D1
		private void SetMoraleChangeAmount(int newAmount)
		{
			this.CurrentData.PartyMoraleChangeAmount = newAmount;
			PartyScreenLogic.PartyMoraleDelegate partyMoraleChange = this.PartyMoraleChange;
			if (partyMoraleChange == null)
			{
				return;
			}
			partyMoraleChange();
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000BB4EF File Offset: 0x000B96EF
		private void SetHorseChangeAmount(int newAmount)
		{
			this.CurrentData.PartyHorseChangeAmount = newAmount;
			PartyScreenLogic.PartyHorseDelegate partyHorseChange = this.PartyHorseChange;
			if (partyHorseChange == null)
			{
				return;
			}
			partyHorseChange();
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000BB50D File Offset: 0x000B970D
		private void SetInfluenceChangeAmount(int heroInfluence, int troopInfluence, int prisonerInfluence)
		{
			this.CurrentData.PartyInfluenceChangeAmount = new ValueTuple<int, int, int>(heroInfluence, troopInfluence, prisonerInfluence);
			PartyScreenLogic.PartyInfluenceDelegate partyInfluenceChange = this.PartyInfluenceChange;
			if (partyInfluenceChange == null)
			{
				return;
			}
			partyInfluenceChange();
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000BB534 File Offset: 0x000B9734
		private void ProcessCommand(PartyScreenLogic.PartyCommand command)
		{
			switch (command.Code)
			{
			case PartyScreenLogic.PartyCommandCode.TransferTroop:
				this.TransferTroop(command, true);
				return;
			case PartyScreenLogic.PartyCommandCode.UpgradeTroop:
				this.UpgradeTroop(command);
				return;
			case PartyScreenLogic.PartyCommandCode.TransferPartyLeaderTroop:
				this.TransferPartyLeaderTroop(command);
				return;
			case PartyScreenLogic.PartyCommandCode.TransferTroopToLeaderSlot:
				this.TransferTroopToLeaderSlot(command);
				return;
			case PartyScreenLogic.PartyCommandCode.ShiftTroop:
				this.ShiftTroop(command);
				return;
			case PartyScreenLogic.PartyCommandCode.RecruitTroop:
				this.RecruitPrisoner(command);
				return;
			case PartyScreenLogic.PartyCommandCode.ExecuteTroop:
				this.ExecuteTroop(command);
				return;
			case PartyScreenLogic.PartyCommandCode.TransferAllTroops:
				this.TransferAllTroops(command);
				return;
			case PartyScreenLogic.PartyCommandCode.SortTroops:
				this.SortTroops(command);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000BB5BB File Offset: 0x000B97BB
		public void AddCommand(PartyScreenLogic.PartyCommand command)
		{
			this.ProcessCommand(command);
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000BB5C4 File Offset: 0x000B97C4
		public bool ValidateCommand(PartyScreenLogic.PartyCommand command)
		{
			if (command.Code == PartyScreenLogic.PartyCommandCode.TransferTroop || command.Code == PartyScreenLogic.PartyCommandCode.TransferTroopToLeaderSlot)
			{
				CharacterObject character = command.Character;
				if (character == CharacterObject.PlayerCharacter)
				{
					return false;
				}
				int num;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					num = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character);
					bool flag = num != -1 && this.MemberRosters[(int)command.RosterSide].GetElementNumber(num) >= command.TotalNumber;
					bool flag2 = command.RosterSide != PartyScreenLogic.PartyRosterSide.Left || command.Index != 0;
					return flag && flag2;
				}
				num = this.PrisonerRosters[(int)command.RosterSide].FindIndexOfTroop(character);
				return num != -1 && this.PrisonerRosters[(int)command.RosterSide].GetElementNumber(num) >= command.TotalNumber;
			}
			else if (command.Code == PartyScreenLogic.PartyCommandCode.ShiftTroop)
			{
				CharacterObject character2 = command.Character;
				if (character2 == this.LeftPartyLeader || character2 == this.RightPartyLeader || ((command.RosterSide != PartyScreenLogic.PartyRosterSide.Left || (this.LeftPartyLeader != null && command.Index == 0)) && (command.RosterSide != PartyScreenLogic.PartyRosterSide.Right || (this.RightPartyLeader != null && command.Index == 0))))
				{
					return false;
				}
				int num2;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					num2 = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character2);
					return num2 != -1 && num2 != command.Index;
				}
				num2 = this.PrisonerRosters[(int)command.RosterSide].FindIndexOfTroop(character2);
				return num2 != -1 && num2 != command.Index;
			}
			else
			{
				if (command.Code == PartyScreenLogic.PartyCommandCode.TransferPartyLeaderTroop)
				{
					CharacterObject character3 = command.Character;
					BasicCharacterObject playerTroop = this._game.PlayerTroop;
					return false;
				}
				if (command.Code == PartyScreenLogic.PartyCommandCode.UpgradeTroop)
				{
					CharacterObject character4 = command.Character;
					int num3 = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character4);
					if (num3 == -1 || this.MemberRosters[(int)command.RosterSide].GetElementNumber(num3) < command.TotalNumber || character4.UpgradeTargets.Length == 0)
					{
						return false;
					}
					if (command.UpgradeTarget >= character4.UpgradeTargets.Length)
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=kaQ7DsW3}Character does not have upgrade target.", null), 0, null, null, "");
						return false;
					}
					CharacterObject characterObject = character4.UpgradeTargets[command.UpgradeTarget];
					int upgradeXpCost = character4.GetUpgradeXpCost(PartyBase.MainParty, command.UpgradeTarget);
					int upgradeGoldCost = character4.GetUpgradeGoldCost(PartyBase.MainParty, command.UpgradeTarget);
					if (this.MemberRosters[(int)command.RosterSide].GetElementXp(num3) < upgradeXpCost * command.TotalNumber)
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=m1bIfPf1}Character does not have enough experience for upgrade.", null), 0, null, null, "");
						return false;
					}
					CharacterObject characterObject2 = ((command.RosterSide == PartyScreenLogic.PartyRosterSide.Left) ? this.LeftPartyLeader : this.RightPartyLeader);
					int? num4 = ((characterObject2 != null) ? new int?(characterObject2.HeroObject.Gold) : null) + this.CurrentData.PartyGoldChangeAmount;
					int num5 = upgradeGoldCost * command.TotalNumber;
					if (!((num4.GetValueOrDefault() >= num5) & (num4 != null)))
					{
						MBTextManager.SetTextVariable("VALUE", upgradeGoldCost);
						MBInformationManager.AddQuickInformation(GameTexts.FindText("str_gold_needed_for_upgrade", null), 0, null, null, "");
						return false;
					}
					if (characterObject.UpgradeRequiresItemFromCategory == null)
					{
						return true;
					}
					foreach (ItemRosterElement itemRosterElement in this.RightOwnerParty.ItemRoster)
					{
						if (itemRosterElement.EquipmentElement.Item.ItemCategory == characterObject.UpgradeRequiresItemFromCategory)
						{
							return true;
						}
					}
					MBTextManager.SetTextVariable("REQUIRED_ITEM", characterObject.UpgradeRequiresItemFromCategory.GetName(), false);
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_item_needed_for_upgrade", null), 0, null, null, "");
					return false;
				}
				else
				{
					if (command.Code == PartyScreenLogic.PartyCommandCode.RecruitTroop)
					{
						return this.IsPrisonerRecruitable(command.Type, command.Character, command.RosterSide);
					}
					if (command.Code == PartyScreenLogic.PartyCommandCode.ExecuteTroop)
					{
						return this.IsExecutable(command.Type, command.Character, command.RosterSide);
					}
					if (command.Code == PartyScreenLogic.PartyCommandCode.TransferAllTroops)
					{
						return this.GetRoster(command.RosterSide, command.Type).Count != 0;
					}
					if (command.Code == PartyScreenLogic.PartyCommandCode.SortTroops)
					{
						return this.GetActiveSortTypeForSide(command.RosterSide) != command.SortType || this.GetIsAscendingSortForSide(command.RosterSide) != command.IsSortAscending;
					}
					throw new MBUnknownTypeException("Unknown command type in ValidateCommand.");
				}
			}
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000BBA78 File Offset: 0x000B9C78
		private void OnReset(bool fromCancel)
		{
			PartyScreenLogic.AfterResetDelegate afterReset = this.AfterReset;
			if (afterReset == null)
			{
				return;
			}
			afterReset(this, fromCancel);
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000BBA8C File Offset: 0x000B9C8C
		protected void TransferTroopToLeaderSlot(PartyScreenLogic.PartyCommand command)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					int num = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character);
					TroopRosterElement elementCopyAtIndex = this.MemberRosters[(int)command.RosterSide].GetElementCopyAtIndex(num);
					int num2 = command.TotalNumber * (elementCopyAtIndex.Xp / elementCopyAtIndex.Number);
					this.MemberRosters[(int)command.RosterSide].AddToCounts(character, -command.TotalNumber, false, -command.WoundedNumber, 0, true, num);
					this.MemberRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)].AddToCounts(character, command.TotalNumber, false, command.WoundedNumber, 0, true, 0);
					if (elementCopyAtIndex.Number != command.TotalNumber)
					{
						this.MemberRosters[(int)command.RosterSide].AddXpToTroop(character, -num2);
					}
					this.MemberRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)].AddXpToTroop(character, num2);
				}
				flag = true;
			}
			if (flag)
			{
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000BBBAC File Offset: 0x000B9DAC
		protected void TransferTroop(PartyScreenLogic.PartyCommand command, bool invokeUpdate)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject troop = command.Character;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					TroopRoster troopRoster = this.MemberRosters[(int)command.RosterSide];
					TroopRoster troopRoster2 = this.MemberRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)];
					int num = troopRoster.FindIndexOfTroop(troop);
					TroopRosterElement elementCopyAtIndex = troopRoster.GetElementCopyAtIndex(num);
					int num2 = ((troop.UpgradeTargets.Length != 0) ? troop.UpgradeTargets.Max<CharacterObject>((CharacterObject x) => Campaign.Current.Models.PartyTroopUpgradeModel.GetXpCostForUpgrade(PartyBase.MainParty, troop, x)) : 0);
					int num4;
					if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Right)
					{
						int num3 = (elementCopyAtIndex.Number - command.TotalNumber) * num2;
						num4 = ((elementCopyAtIndex.Xp >= num3 && num3 >= 0) ? (elementCopyAtIndex.Xp - num3) : 0);
					}
					else
					{
						int num5 = command.TotalNumber * num2;
						num4 = ((elementCopyAtIndex.Xp > num5 && num5 >= 0) ? num5 : elementCopyAtIndex.Xp);
						troopRoster.AddXpToTroop(troop, -num4);
					}
					troopRoster.AddToCounts(troop, -command.TotalNumber, false, -command.WoundedNumber, 0, false, -1);
					int num6 = command.Index;
					if (num6 == troopRoster2.Count && troopRoster2.Contains(troop))
					{
						num6 = troopRoster2.Count - 1;
					}
					troopRoster2.AddToCounts(troop, command.TotalNumber, false, command.WoundedNumber, 0, false, num6);
					troopRoster2.AddXpToTroop(troop, num4);
				}
				else
				{
					TroopRoster troopRoster3 = this.PrisonerRosters[(int)command.RosterSide];
					TroopRoster troopRoster4 = this.PrisonerRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)];
					int num7 = troopRoster3.FindIndexOfTroop(troop);
					TroopRosterElement elementCopyAtIndex2 = troopRoster3.GetElementCopyAtIndex(num7);
					int conformityNeededToRecruitPrisoner = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetConformityNeededToRecruitPrisoner(elementCopyAtIndex2.Character);
					int num9;
					if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Right)
					{
						this.UpdatePrisonerTransferHistory(troop, -command.TotalNumber);
						int num8 = (elementCopyAtIndex2.Number - command.TotalNumber) * conformityNeededToRecruitPrisoner;
						num9 = ((elementCopyAtIndex2.Xp >= num8 && num8 >= 0) ? (elementCopyAtIndex2.Xp - num8) : 0);
					}
					else
					{
						this.UpdatePrisonerTransferHistory(troop, command.TotalNumber);
						int num10 = command.TotalNumber * conformityNeededToRecruitPrisoner;
						num9 = ((elementCopyAtIndex2.Xp > num10 && num10 >= 0) ? num10 : elementCopyAtIndex2.Xp);
						troopRoster3.AddXpToTroop(troop, -num9);
					}
					troopRoster3.AddToCounts(troop, -command.TotalNumber, false, -command.WoundedNumber, 0, false, -1);
					int num11 = command.Index;
					if (num11 == troopRoster4.Count && troopRoster4.Contains(troop))
					{
						num11 = troopRoster4.Count - 1;
					}
					troopRoster4.AddToCounts(troop, command.TotalNumber, false, command.WoundedNumber, 0, false, num11);
					troopRoster4.AddXpToTroop(troop, num9);
					if (this.CurrentData.RightRecruitableData.ContainsKey(troop))
					{
						this.CurrentData.RightRecruitableData[troop] = MathF.Max(MathF.Min(this.CurrentData.RightRecruitableData[troop], this.PrisonerRosters[1].GetElementNumber(troop)), Campaign.Current.Models.PrisonerRecruitmentCalculationModel.CalculateRecruitableNumber(PartyBase.MainParty, troop));
					}
				}
				flag = true;
			}
			if (flag)
			{
				if (this.PrisonerTransferState == PartyScreenLogic.TransferState.TransferableWithTrade && command.Type == PartyScreenLogic.TroopType.Prisoner)
				{
					int num12 = ((command.RosterSide == PartyScreenLogic.PartyRosterSide.Right) ? 1 : (-1));
					this.SetPartyGoldChangeAmount(this.CurrentData.PartyGoldChangeAmount + Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(command.Character, Hero.MainHero) * command.TotalNumber * num12);
				}
				if (this._partyScreenMode == PartyScreenHelper.PartyScreenMode.QuestTroopManage)
				{
					int num13 = ((command.RosterSide == PartyScreenLogic.PartyRosterSide.Right) ? (-1) : 1);
					this.SetPartyGoldChangeAmount(this.CurrentData.PartyGoldChangeAmount + command.Character.TroopWage * command.TotalNumber * this.QuestModeWageDaysMultiplier * num13);
				}
				PartyState activePartyState = PartyScreenHelper.GetActivePartyState();
				if (activePartyState != null && activePartyState.IsDonating)
				{
					Settlement currentSettlement = Hero.MainHero.CurrentSettlement;
					float num14 = 0f;
					float num15 = 0f;
					float num16 = 0f;
					foreach (TroopTradeDifference troopTradeDifference in this.CurrentData.GetTroopTradeDifferencesFromTo(this._initialData, PartyScreenLogic.PartyRosterSide.Left))
					{
						int differenceCount = troopTradeDifference.DifferenceCount;
						if (differenceCount > 0)
						{
							if (!troopTradeDifference.IsPrisoner)
							{
								num15 += (float)differenceCount * Campaign.Current.Models.PrisonerDonationModel.CalculateInfluenceGainAfterTroopDonation(PartyBase.MainParty, troopTradeDifference.Troop, currentSettlement);
							}
							else if (troopTradeDifference.Troop.IsHero)
							{
								num14 += Campaign.Current.Models.PrisonerDonationModel.CalculateInfluenceGainAfterPrisonerDonation(PartyBase.MainParty, troopTradeDifference.Troop, currentSettlement);
							}
							else
							{
								num16 += (float)differenceCount * Campaign.Current.Models.PrisonerDonationModel.CalculateInfluenceGainAfterPrisonerDonation(PartyBase.MainParty, troopTradeDifference.Troop, currentSettlement);
							}
						}
					}
					this.SetInfluenceChangeAmount((int)num14, (int)num15, (int)num16);
				}
				if (invokeUpdate)
				{
					PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
					if (updateDelegate != null)
					{
						updateDelegate(command);
					}
					PartyScreenLogic.PresentationUpdate update = this.Update;
					if (update == null)
					{
						return;
					}
					update(command);
				}
			}
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000BC13C File Offset: 0x000BA33C
		protected void ShiftTroop(PartyScreenLogic.PartyCommand command)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					int num = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character);
					int num2 = ((num < command.Index) ? (command.Index - 1) : command.Index);
					this.MemberRosters[(int)command.RosterSide].ShiftTroopToIndex(num, num2);
				}
				else
				{
					int num3 = this.PrisonerRosters[(int)command.RosterSide].FindIndexOfTroop(character);
					this.PrisonerRosters[(int)command.RosterSide].GetElementCopyAtIndex(num3);
					int num4 = ((num3 < command.Index) ? (command.Index - 1) : command.Index);
					this.PrisonerRosters[(int)command.RosterSide].ShiftTroopToIndex(num3, num4);
				}
				flag = true;
			}
			if (flag)
			{
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000BC22F File Offset: 0x000BA42F
		protected void TransferPartyLeaderTroop(PartyScreenLogic.PartyCommand command)
		{
			if (this.ValidateCommand(command))
			{
				PartyBase partyBase = ((command.RosterSide == PartyScreenLogic.PartyRosterSide.Left) ? this.LeftOwnerParty : this.RightOwnerParty);
			}
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x000BC254 File Offset: 0x000BA454
		protected void UpgradeTroop(PartyScreenLogic.PartyCommand command)
		{
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				CharacterObject characterObject = character.UpgradeTargets[command.UpgradeTarget];
				TroopRoster roster = this.GetRoster(command.RosterSide, command.Type);
				int num = roster.FindIndexOfTroop(character);
				int num2 = character.GetUpgradeXpCost(PartyBase.MainParty, command.UpgradeTarget) * command.TotalNumber;
				roster.SetElementXp(num, roster.GetElementXp(num) - num2);
				List<ValueTuple<EquipmentElement, int>> list = null;
				this.SetPartyGoldChangeAmount(this.CurrentData.PartyGoldChangeAmount - character.GetUpgradeGoldCost(PartyBase.MainParty, command.UpgradeTarget) * command.TotalNumber);
				if (characterObject.UpgradeRequiresItemFromCategory != null)
				{
					list = this.RemoveItemFromItemRoster(characterObject.UpgradeRequiresItemFromCategory, command.TotalNumber);
				}
				int num3 = 0;
				foreach (TroopRosterElement troopRosterElement in roster.GetTroopRoster())
				{
					if (troopRosterElement.Character == character && command.TotalNumber > troopRosterElement.Number - troopRosterElement.WoundedNumber)
					{
						num3 = command.TotalNumber - (troopRosterElement.Number - troopRosterElement.WoundedNumber);
					}
				}
				roster.AddToCounts(character, -command.TotalNumber, false, -num3, 0, true, -1);
				roster.AddToCounts(characterObject, command.TotalNumber, false, num3, 0, true, command.Index);
				this.AddUpgradeToHistory(character, characterObject, command.TotalNumber);
				this.AddUsedHorsesToHistory(list);
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate == null)
				{
					return;
				}
				updateDelegate(command);
			}
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x000BC3E4 File Offset: 0x000BA5E4
		protected void RecruitPrisoner(PartyScreenLogic.PartyCommand command)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				TroopRoster troopRoster = this.PrisonerRosters[(int)command.RosterSide];
				int num = MathF.Min(this.CurrentData.RightRecruitableData[character], command.TotalNumber);
				if (num > 0)
				{
					Dictionary<CharacterObject, int> rightRecruitableData = this.CurrentData.RightRecruitableData;
					CharacterObject characterObject = character;
					rightRecruitableData[characterObject] -= num;
					int conformityNeededToRecruitPrisoner = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetConformityNeededToRecruitPrisoner(character);
					troopRoster.AddXpToTroop(character, -conformityNeededToRecruitPrisoner * num);
					troopRoster.AddToCounts(character, -num, false, 0, 0, true, -1);
					this.MemberRosters[(int)command.RosterSide].AddToCounts(command.Character, num, false, 0, 0, true, command.Index);
					this.AddRecruitToHistory(character, num);
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			if (flag)
			{
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x000BC4E8 File Offset: 0x000BA6E8
		protected void ExecuteTroop(PartyScreenLogic.PartyCommand command)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				this.PrisonerRosters[(int)command.RosterSide].AddToCounts(character, -1, false, 0, 0, true, -1);
				KillCharacterAction.ApplyByExecution(character.HeroObject, Hero.MainHero, true, false);
				flag = true;
			}
			if (flag)
			{
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update != null)
				{
					update(command);
				}
				if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Left)
				{
					this._initialData.LeftPrisonerRoster.AddToCounts(command.Character, -1, false, 0, 0, true, -1);
					return;
				}
				if (PartyScreenLogic.PartyRosterSide.Right == command.RosterSide)
				{
					this._initialData.RightPrisonerRoster.AddToCounts(command.Character, -1, false, 0, 0, true, -1);
				}
			}
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000BC5A8 File Offset: 0x000BA7A8
		protected void TransferAllTroops(PartyScreenLogic.PartyCommand command)
		{
			if (this.ValidateCommand(command))
			{
				PartyScreenLogic.PartyRosterSide partyRosterSide = PartyScreenLogic.PartyRosterSide.Right - command.RosterSide;
				TroopRoster roster = this.GetRoster(command.RosterSide, command.Type);
				List<TroopRosterElement> listFromRoster = this.GetListFromRoster(roster);
				int num = -1;
				if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Right)
				{
					if (command.Type == PartyScreenLogic.TroopType.Prisoner)
					{
						num = this.LeftPartyPrisonersSizeLimit - this.PrisonerRosters[0].TotalManCount;
					}
					else
					{
						num = this.LeftPartyMembersSizeLimit - this.MemberRosters[0].TotalManCount;
					}
				}
				else if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Left)
				{
					if (command.Type == PartyScreenLogic.TroopType.Prisoner)
					{
						num = this.RightPartyPrisonersSizeLimit - this.PrisonerRosters[1].TotalManCount;
					}
					else
					{
						num = this.RightPartyMembersSizeLimit - this.MemberRosters[1].TotalManCount;
					}
				}
				if (num <= 0)
				{
					num = listFromRoster.Sum<TroopRosterElement>((TroopRosterElement x) => x.Number);
				}
				IEnumerable<string> enumerable = ((command.Type == PartyScreenLogic.TroopType.Member) ? Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetPartyTroopLocks() : Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetPartyPrisonerLocks());
				int num2 = 0;
				while (num2 < listFromRoster.Count && num > 0)
				{
					TroopRosterElement troopRosterElement = listFromRoster[num2];
					if ((command.RosterSide != PartyScreenLogic.PartyRosterSide.Right || !enumerable.Contains(troopRosterElement.Character.StringId)) && this.IsTroopTransferable(command.Type, troopRosterElement.Character, (int)command.RosterSide))
					{
						PartyScreenLogic.PartyCommand partyCommand = new PartyScreenLogic.PartyCommand();
						int num3 = MBMath.ClampInt(troopRosterElement.Number, 0, num);
						partyCommand.FillForTransferTroop(command.RosterSide, command.Type, troopRosterElement.Character, num3, troopRosterElement.WoundedNumber, -1);
						this.TransferTroop(partyCommand, false);
						num -= num3;
					}
					num2++;
				}
				PartyScreenLogic.TroopSortType activeSortTypeForSide = this.GetActiveSortTypeForSide(partyRosterSide);
				if (activeSortTypeForSide != PartyScreenLogic.TroopSortType.Custom)
				{
					TroopRoster roster2 = this.GetRoster(partyRosterSide, PartyScreenLogic.TroopType.Member);
					TroopRoster roster3 = this.GetRoster(partyRosterSide, PartyScreenLogic.TroopType.Prisoner);
					this.SortRoster(roster2, activeSortTypeForSide);
					this.SortRoster(roster3, activeSortTypeForSide);
				}
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x000BC7C0 File Offset: 0x000BA9C0
		protected void SortTroops(PartyScreenLogic.PartyCommand command)
		{
			if (this.ValidateCommand(command))
			{
				this.SetActiveSortTypeForSide(command.RosterSide, command.SortType);
				this.SetIsAscendingForSide(command.RosterSide, command.IsSortAscending);
				this.UpdateComparersAscendingOrder(command.IsSortAscending);
				if (command.SortType != PartyScreenLogic.TroopSortType.Custom)
				{
					TroopRoster roster = this.GetRoster(command.RosterSide, PartyScreenLogic.TroopType.Member);
					TroopRoster roster2 = this.GetRoster(command.RosterSide, PartyScreenLogic.TroopType.Prisoner);
					this.SortRoster(roster, command.SortType);
					this.SortRoster(roster2, command.SortType);
				}
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x000BC86C File Offset: 0x000BAA6C
		public int GetIndexToInsertTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, TroopRosterElement troop)
		{
			PartyScreenLogic.TroopSortType activeSortTypeForSide = this.GetActiveSortTypeForSide(side);
			if (activeSortTypeForSide != PartyScreenLogic.TroopSortType.Custom)
			{
				return -1;
			}
			PartyScreenLogic.TroopComparer comparer = this.GetComparer(activeSortTypeForSide);
			TroopRoster roster = this.GetRoster(side, type);
			for (int i = 0; i < roster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = roster.GetElementCopyAtIndex(i);
				if (!elementCopyAtIndex.Character.IsHero)
				{
					if (elementCopyAtIndex.Character.StringId == troop.Character.StringId)
					{
						return -1;
					}
					if (comparer.Compare(elementCopyAtIndex, troop) < 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x000BC8EE File Offset: 0x000BAAEE
		public PartyScreenLogic.TroopSortType GetActiveSortTypeForSide(PartyScreenLogic.PartyRosterSide side)
		{
			if (side == PartyScreenLogic.PartyRosterSide.Left)
			{
				return this.ActiveOtherPartySortType;
			}
			if (side == PartyScreenLogic.PartyRosterSide.Right)
			{
				return this.ActiveMainPartySortType;
			}
			return PartyScreenLogic.TroopSortType.Invalid;
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x000BC906 File Offset: 0x000BAB06
		private void SetActiveSortTypeForSide(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopSortType sortType)
		{
			if (side == PartyScreenLogic.PartyRosterSide.Left)
			{
				this.ActiveOtherPartySortType = sortType;
				return;
			}
			if (side == PartyScreenLogic.PartyRosterSide.Right)
			{
				this.ActiveMainPartySortType = sortType;
			}
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x000BC91E File Offset: 0x000BAB1E
		public bool GetIsAscendingSortForSide(PartyScreenLogic.PartyRosterSide side)
		{
			if (side == PartyScreenLogic.PartyRosterSide.Left)
			{
				return this.IsOtherPartySortAscending;
			}
			return side == PartyScreenLogic.PartyRosterSide.Right && this.IsMainPartySortAscending;
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x000BC936 File Offset: 0x000BAB36
		private void SetIsAscendingForSide(PartyScreenLogic.PartyRosterSide side, bool isAscending)
		{
			if (side == PartyScreenLogic.PartyRosterSide.Left)
			{
				this.IsOtherPartySortAscending = isAscending;
				return;
			}
			if (side == PartyScreenLogic.PartyRosterSide.Right)
			{
				this.IsMainPartySortAscending = isAscending;
			}
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x000BC950 File Offset: 0x000BAB50
		private List<TroopRosterElement> GetListFromRoster(TroopRoster roster)
		{
			List<TroopRosterElement> list = new List<TroopRosterElement>();
			for (int i = 0; i < roster.Count; i++)
			{
				list.Add(roster.GetElementCopyAtIndex(i));
			}
			return list;
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x000BC984 File Offset: 0x000BAB84
		private void SyncRosterWithList(TroopRoster roster, List<TroopRosterElement> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				TroopRosterElement troopRosterElement = list[i];
				int num = roster.FindIndexOfTroop(troopRosterElement.Character);
				roster.SwapTroopsAtIndices(num, i);
			}
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x000BC9C0 File Offset: 0x000BABC0
		[Conditional("DEBUG")]
		private void EnsureRosterIsSyncedWithList(TroopRoster roster, List<TroopRosterElement> list)
		{
			if (roster.Count != list.Count)
			{
				Debug.FailedAssert("Roster count is not synced with the list count", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenLogic.cs", "EnsureRosterIsSyncedWithList", 1081);
				return;
			}
			for (int i = 0; i < roster.Count; i++)
			{
				if (roster.GetCharacterAtIndex(i).StringId != list[i].Character.StringId)
				{
					Debug.FailedAssert("Roster is not synced with the list at index: " + i, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenLogic.cs", "EnsureRosterIsSyncedWithList", 1091);
					return;
				}
			}
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x000BCA50 File Offset: 0x000BAC50
		private void SortRoster(TroopRoster originalRoster, PartyScreenLogic.TroopSortType sortType)
		{
			PartyScreenLogic.TroopComparer troopComparer = this._defaultComparers[sortType];
			if (!this.IsRosterOrdered(originalRoster, troopComparer))
			{
				List<TroopRosterElement> listFromRoster = this.GetListFromRoster(originalRoster);
				listFromRoster.Sort(this._defaultComparers[sortType]);
				this.SyncRosterWithList(originalRoster, listFromRoster);
			}
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x000BCA98 File Offset: 0x000BAC98
		private bool IsRosterOrdered(TroopRoster roster, PartyScreenLogic.TroopComparer comparer)
		{
			for (int i = 1; i < roster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = roster.GetElementCopyAtIndex(i - 1);
				TroopRosterElement elementCopyAtIndex2 = roster.GetElementCopyAtIndex(i);
				if (comparer.Compare(elementCopyAtIndex, elementCopyAtIndex2) >= 1)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x000BCAD8 File Offset: 0x000BACD8
		public bool IsDoneActive()
		{
			object obj = Hero.MainHero.Gold < -this.CurrentData.PartyGoldChangeAmount && this.CurrentData.PartyGoldChangeAmount < 0;
			PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = this.PartyPresentationDoneButtonConditionDelegate;
			Tuple<bool, TextObject> tuple = ((partyPresentationDoneButtonConditionDelegate != null) ? partyPresentationDoneButtonConditionDelegate(this.MemberRosters[0], this.PrisonerRosters[0], this.MemberRosters[1], this.PrisonerRosters[1], this.LeftPartyMembersSizeLimit, 0) : null);
			bool flag = this.PartyPresentationDoneButtonConditionDelegate == null || (tuple != null && tuple.Item1);
			this.DoneReasonString = null;
			object obj2 = obj;
			if (obj2 != null)
			{
				this.DoneReasonString = GameTexts.FindText("str_inventory_popup_player_not_enough_gold", null).ToString();
			}
			else
			{
				string text;
				if (tuple == null)
				{
					text = null;
				}
				else
				{
					TextObject item = tuple.Item2;
					text = ((item != null) ? item.ToString() : null);
				}
				this.DoneReasonString = text ?? string.Empty;
			}
			return obj2 == 0 && flag;
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x000BCBAE File Offset: 0x000BADAE
		public bool IsCancelActive()
		{
			return this.PartyPresentationCancelButtonActivateDelegate == null || this.PartyPresentationCancelButtonActivateDelegate();
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x000BCBC8 File Offset: 0x000BADC8
		public bool DoneLogic(bool isForced)
		{
			if (Hero.MainHero.Gold < -this.CurrentData.PartyGoldChangeAmount && this.CurrentData.PartyGoldChangeAmount < 0)
			{
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_inventory_popup_player_not_enough_gold", null), 0, null, null, "");
				return false;
			}
			FlattenedTroopRoster flattenedTroopRoster = new FlattenedTroopRoster(4);
			FlattenedTroopRoster flattenedTroopRoster2 = new FlattenedTroopRoster(4);
			foreach (Tuple<CharacterObject, int> tuple in this.CurrentData.TransferredPrisonersHistory)
			{
				int num = MathF.Abs(tuple.Item2);
				if (tuple.Item2 < 0)
				{
					flattenedTroopRoster.Add(tuple.Item1, num, 0);
				}
				else if (tuple.Item2 > 0)
				{
					flattenedTroopRoster2.Add(tuple.Item1, num, 0);
				}
			}
			if (Settlement.CurrentSettlement != null && !flattenedTroopRoster2.IsEmpty<FlattenedTroopRosterElement>())
			{
				CampaignEventDispatcher.Instance.OnPrisonersChangeInSettlement(Settlement.CurrentSettlement, flattenedTroopRoster2, null, true);
			}
			bool flag = this.PartyPresentationDoneButtonDelegate(this.MemberRosters[0], this.PrisonerRosters[0], this.MemberRosters[1], this.PrisonerRosters[1], flattenedTroopRoster2, flattenedTroopRoster, isForced, this.LeftOwnerParty, this.RightOwnerParty);
			if (flag)
			{
				if (!this.DoNotApplyGoldTransactions)
				{
					GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.CurrentData.PartyGoldChangeAmount, false);
				}
				if (this.CurrentData.PartyInfluenceChangeAmount.Item2 != 0)
				{
					GainKingdomInfluenceAction.ApplyForLeavingTroopToGarrison(Hero.MainHero, (float)this.CurrentData.PartyInfluenceChangeAmount.Item2);
				}
				this.FireCampaignRelatedEvents();
				this.SetPartyGoldChangeAmount(0);
				this.SetHorseChangeAmount(0);
				this.SetInfluenceChangeAmount(0, 0, 0);
				this.SetMoraleChangeAmount(0);
				this.CurrentData.UpgradedTroopsHistory = new List<Tuple<CharacterObject, CharacterObject, int>>();
				this.CurrentData.TransferredPrisonersHistory = new List<Tuple<CharacterObject, int>>();
				this.CurrentData.RecruitedPrisonersHistory = new List<Tuple<CharacterObject, int>>();
				this.CurrentData.UsedUpgradeHorsesHistory = new List<Tuple<EquipmentElement, int>>();
				this._initialData.CopyFromScreenData(this.CurrentData);
			}
			return flag;
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x000BCDC8 File Offset: 0x000BAFC8
		public void OnPartyScreenClosed(bool fromCancel)
		{
			if (fromCancel)
			{
				PartyPresentationCancelButtonDelegate partyPresentationCancelButtonDelegate = this.PartyPresentationCancelButtonDelegate;
				if (partyPresentationCancelButtonDelegate != null)
				{
					partyPresentationCancelButtonDelegate();
				}
			}
			PartyScreenClosedDelegate partyScreenClosedEvent = this.PartyScreenClosedEvent;
			if (partyScreenClosedEvent == null)
			{
				return;
			}
			partyScreenClosedEvent(this.LeftOwnerParty, this.MemberRosters[0], this.PrisonerRosters[0], this.RightOwnerParty, this.MemberRosters[1], this.PrisonerRosters[1], fromCancel);
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x000BCE28 File Offset: 0x000BB028
		private void UpdateComparersAscendingOrder(bool isAscending)
		{
			foreach (KeyValuePair<PartyScreenLogic.TroopSortType, PartyScreenLogic.TroopComparer> keyValuePair in this._defaultComparers)
			{
				keyValuePair.Value.SetIsAscending(isAscending);
			}
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x000BCE84 File Offset: 0x000BB084
		private void FireCampaignRelatedEvents()
		{
			foreach (Tuple<CharacterObject, CharacterObject, int> tuple in this.CurrentData.UpgradedTroopsHistory)
			{
				CampaignEventDispatcher.Instance.OnPlayerUpgradedTroops(tuple.Item1, tuple.Item2, tuple.Item3);
			}
			FlattenedTroopRoster flattenedTroopRoster = new FlattenedTroopRoster(4);
			foreach (Tuple<CharacterObject, int> tuple2 in this.CurrentData.RecruitedPrisonersHistory)
			{
				flattenedTroopRoster.Add(tuple2.Item1, tuple2.Item2, 0);
			}
			if (!flattenedTroopRoster.IsEmpty<FlattenedTroopRosterElement>())
			{
				CampaignEventDispatcher.Instance.OnMainPartyPrisonerRecruited(flattenedTroopRoster);
			}
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x000BCF64 File Offset: 0x000BB164
		public bool IsTroopTransferable(PartyScreenLogic.TroopType troopType, CharacterObject character, int side)
		{
			return this.IsTroopRosterTransferable(troopType) && !character.IsNotTransferableInPartyScreen && character != CharacterObject.PlayerCharacter && (this.IsTroopTransferableDelegate == null || this.IsTroopTransferableDelegate(character, troopType, (PartyScreenLogic.PartyRosterSide)side, this.LeftOwnerParty));
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x000BCFA0 File Offset: 0x000BB1A0
		public bool IsTroopRosterTransferable(PartyScreenLogic.TroopType troopType)
		{
			if (troopType == PartyScreenLogic.TroopType.Prisoner)
			{
				return this.PrisonerTransferState == PartyScreenLogic.TransferState.Transferable || this.PrisonerTransferState == PartyScreenLogic.TransferState.TransferableWithTrade;
			}
			return troopType == PartyScreenLogic.TroopType.Member && (this.MemberTransferState == PartyScreenLogic.TransferState.Transferable || this.MemberTransferState == PartyScreenLogic.TransferState.TransferableWithTrade);
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x000BCFD5 File Offset: 0x000BB1D5
		public bool IsPrisonerRecruitable(PartyScreenLogic.TroopType troopType, CharacterObject character, PartyScreenLogic.PartyRosterSide side)
		{
			return side == PartyScreenLogic.PartyRosterSide.Right && troopType == PartyScreenLogic.TroopType.Prisoner && !character.IsHero && this.CurrentData.RightRecruitableData.ContainsKey(character) && this.CurrentData.RightRecruitableData[character] > 0;
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x000BD014 File Offset: 0x000BB214
		public string GetRecruitableReasonString(CharacterObject character, bool isRecruitable, int troopCount, out bool showStackModifierText)
		{
			showStackModifierText = false;
			if (isRecruitable)
			{
				showStackModifierText = true;
				if (this.RightOwnerParty.PartySizeLimit <= this.MemberRosters[1].TotalManCount)
				{
					return GameTexts.FindText("str_recruit_party_size_limit", null).ToString();
				}
				return GameTexts.FindText("str_recruit_prisoner", null).ToString();
			}
			else
			{
				if (character.IsHero)
				{
					return GameTexts.FindText("str_cannot_recruit_hero", null).ToString();
				}
				return GameTexts.FindText("str_cannot_recruit_prisoner", null).ToString();
			}
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x000BD094 File Offset: 0x000BB294
		public bool IsExecutable(PartyScreenLogic.TroopType troopType, CharacterObject character, PartyScreenLogic.PartyRosterSide side)
		{
			return troopType == PartyScreenLogic.TroopType.Prisoner && side == PartyScreenLogic.PartyRosterSide.Right && character.IsHero && character.HeroObject.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && PlayerEncounter.Current == null && FaceGen.GetMaturityTypeWithAge(character.Age) > BodyMeshMaturityType.Tween;
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x000BD0EA File Offset: 0x000BB2EA
		public string GetExecutableReasonString(CharacterObject character, bool isExecutable)
		{
			if (isExecutable)
			{
				return GameTexts.FindText("str_execute_prisoner", null).ToString();
			}
			if (!character.IsHero)
			{
				return GameTexts.FindText("str_cannot_execute_nonhero", null).ToString();
			}
			return GameTexts.FindText("str_cannot_execute_hero", null).ToString();
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x000BD12C File Offset: 0x000BB32C
		public int GetCurrentQuestCurrentCount(bool includePrisoners, bool includeMembers)
		{
			int num = 0;
			if (includeMembers)
			{
				num += this.MemberRosters[0].Sum((TroopRosterElement item) => item.Number - item.WoundedNumber);
			}
			if (includePrisoners)
			{
				num += this.PrisonerRosters[0].Sum((TroopRosterElement item) => item.Number - item.WoundedNumber);
			}
			return num;
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x000BD1A0 File Offset: 0x000BB3A0
		public int GetCurrentQuestRequiredCount()
		{
			return this.LeftPartyMembersSizeLimit;
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x000BD1A8 File Offset: 0x000BB3A8
		private static bool DefaultDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			return true;
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x000BD1AC File Offset: 0x000BB3AC
		private void AddUpgradeToHistory(CharacterObject fromTroop, CharacterObject toTroop, int num)
		{
			Tuple<CharacterObject, CharacterObject, int> tuple = this.CurrentData.UpgradedTroopsHistory.Find((Tuple<CharacterObject, CharacterObject, int> t) => t.Item1 == fromTroop && t.Item2 == toTroop);
			if (tuple != null)
			{
				int item = tuple.Item3;
				this.CurrentData.UpgradedTroopsHistory.Remove(tuple);
				this.CurrentData.UpgradedTroopsHistory.Add(new Tuple<CharacterObject, CharacterObject, int>(fromTroop, toTroop, num + item));
				return;
			}
			this.CurrentData.UpgradedTroopsHistory.Add(new Tuple<CharacterObject, CharacterObject, int>(fromTroop, toTroop, num));
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x000BD250 File Offset: 0x000BB450
		private void AddUsedHorsesToHistory(List<ValueTuple<EquipmentElement, int>> usedHorses)
		{
			if (usedHorses != null)
			{
				using (List<ValueTuple<EquipmentElement, int>>.Enumerator enumerator = usedHorses.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ValueTuple<EquipmentElement, int> usedHorse = enumerator.Current;
						Tuple<EquipmentElement, int> tuple = this.CurrentData.UsedUpgradeHorsesHistory.Find((Tuple<EquipmentElement, int> t) => t.Equals(usedHorse.Item1));
						if (tuple != null)
						{
							int item = tuple.Item2;
							this.CurrentData.UsedUpgradeHorsesHistory.Remove(tuple);
							this.CurrentData.UsedUpgradeHorsesHistory.Add(new Tuple<EquipmentElement, int>(usedHorse.Item1, item + usedHorse.Item2));
						}
						else
						{
							this.CurrentData.UsedUpgradeHorsesHistory.Add(new Tuple<EquipmentElement, int>(usedHorse.Item1, usedHorse.Item2));
						}
					}
				}
				PartyScreenData currentData = this.CurrentData;
				this.SetHorseChangeAmount(currentData.PartyHorseChangeAmount += usedHorses.Sum<ValueTuple<EquipmentElement, int>>((ValueTuple<EquipmentElement, int> t) => t.Item2));
			}
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x000BD384 File Offset: 0x000BB584
		private void UpdatePrisonerTransferHistory(CharacterObject troop, int amount)
		{
			Tuple<CharacterObject, int> tuple = this.CurrentData.TransferredPrisonersHistory.Find((Tuple<CharacterObject, int> t) => t.Item1 == troop);
			if (tuple != null)
			{
				int item = tuple.Item2;
				this.CurrentData.TransferredPrisonersHistory.Remove(tuple);
				this.CurrentData.TransferredPrisonersHistory.Add(new Tuple<CharacterObject, int>(troop, amount + item));
				return;
			}
			this.CurrentData.TransferredPrisonersHistory.Add(new Tuple<CharacterObject, int>(troop, amount));
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x000BD414 File Offset: 0x000BB614
		private void AddRecruitToHistory(CharacterObject troop, int amount)
		{
			Tuple<CharacterObject, int> tuple = this.CurrentData.RecruitedPrisonersHistory.Find((Tuple<CharacterObject, int> t) => t.Item1 == troop);
			if (tuple != null)
			{
				int item = tuple.Item2;
				this.CurrentData.RecruitedPrisonersHistory.Remove(tuple);
				this.CurrentData.RecruitedPrisonersHistory.Add(new Tuple<CharacterObject, int>(troop, amount + item));
			}
			else
			{
				this.CurrentData.RecruitedPrisonersHistory.Add(new Tuple<CharacterObject, int>(troop, amount));
			}
			int prisonerRecruitmentMoraleEffect = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetPrisonerRecruitmentMoraleEffect(this.RightOwnerParty, troop, amount);
			this.SetMoraleChangeAmount(this.CurrentData.PartyMoraleChangeAmount + prisonerRecruitmentMoraleEffect);
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x000BD4D8 File Offset: 0x000BB6D8
		private string GetItemLockStringID(EquipmentElement equipmentElement)
		{
			return equipmentElement.Item.StringId + ((equipmentElement.ItemModifier != null) ? equipmentElement.ItemModifier.StringId : "");
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x000BD508 File Offset: 0x000BB708
		private List<ValueTuple<EquipmentElement, int>> RemoveItemFromItemRoster(ItemCategory itemCategory, int numOfItemsLeftToRemove = 1)
		{
			List<ValueTuple<EquipmentElement, int>> list = new List<ValueTuple<EquipmentElement, int>>();
			IEnumerable<string> lockedItems = Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetInventoryLocks();
			foreach (ItemRosterElement itemRosterElement in from x in this.RightOwnerParty.ItemRoster.Where<ItemRosterElement>(delegate(ItemRosterElement x)
				{
					ItemObject item = x.EquipmentElement.Item;
					return ((item != null) ? item.ItemCategory : null) == itemCategory;
				})
				orderby x.EquipmentElement.Item.Value
				orderby lockedItems.Contains(this.GetItemLockStringID(x.EquipmentElement))
				select x)
			{
				int num = MathF.Min(numOfItemsLeftToRemove, itemRosterElement.Amount);
				this.RightOwnerParty.ItemRoster.AddToCounts(itemRosterElement.EquipmentElement, -num);
				numOfItemsLeftToRemove -= num;
				list.Add(new ValueTuple<EquipmentElement, int>(itemRosterElement.EquipmentElement, num));
				if (numOfItemsLeftToRemove <= 0)
				{
					break;
				}
			}
			if (numOfItemsLeftToRemove > 0)
			{
				Debug.FailedAssert("Couldn't find enough upgrade req items in the inventory.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenLogic.cs", "RemoveItemFromItemRoster", 1509);
			}
			return list;
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x000BD630 File Offset: 0x000BB830
		public void Reset(bool fromCancel)
		{
			this.ResetLogic(fromCancel);
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x000BD639 File Offset: 0x000BB839
		private void ResetLogic(bool fromCancel)
		{
			if (this.CurrentData != this._initialData)
			{
				this.CurrentData.ResetUsing(this._initialData);
				PartyScreenLogic.AfterResetDelegate afterReset = this.AfterReset;
				if (afterReset == null)
				{
					return;
				}
				afterReset(this, fromCancel);
			}
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x000BD671 File Offset: 0x000BB871
		public void SavePartyScreenData()
		{
			this._savedData = new PartyScreenData();
			this._savedData.InitializeCopyFrom(this.CurrentData.RightParty, this.CurrentData.LeftParty);
			this._savedData.CopyFromScreenData(this.CurrentData);
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x000BD6B0 File Offset: 0x000BB8B0
		public void ResetToLastSavedPartyScreenData(bool fromCancel)
		{
			if (this.CurrentData != this._savedData)
			{
				this.CurrentData.ResetUsing(this._savedData);
				PartyScreenLogic.AfterResetDelegate afterReset = this.AfterReset;
				if (afterReset == null)
				{
					return;
				}
				afterReset(this, fromCancel);
			}
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x000BD6E8 File Offset: 0x000BB8E8
		public void RemoveZeroCounts()
		{
			for (int i = 0; i < this.MemberRosters.Length; i++)
			{
				this.MemberRosters[i].RemoveZeroCounts();
			}
			for (int j = 0; j < this.PrisonerRosters.Length; j++)
			{
				this.PrisonerRosters[j].RemoveZeroCounts();
			}
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x000BD735 File Offset: 0x000BB935
		public int GetTroopRecruitableAmount(CharacterObject troop)
		{
			if (!this.CurrentData.RightRecruitableData.ContainsKey(troop))
			{
				return 0;
			}
			return this.CurrentData.RightRecruitableData[troop];
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x000BD75D File Offset: 0x000BB95D
		public TroopRoster GetRoster(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType troopType)
		{
			if (troopType == PartyScreenLogic.TroopType.Member)
			{
				return this.MemberRosters[(int)side];
			}
			if (troopType == PartyScreenLogic.TroopType.Prisoner)
			{
				return this.PrisonerRosters[(int)side];
			}
			return null;
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000BD77A File Offset: 0x000BB97A
		internal void OnDoneEvent(List<TroopTradeDifference> freshlySellList)
		{
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000BD77C File Offset: 0x000BB97C
		public bool IsThereAnyChanges()
		{
			return this._initialData.IsThereAnyTroopTradeDifferenceBetween(this.CurrentData);
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x000BD790 File Offset: 0x000BB990
		public bool HaveRightSideGainedTroops()
		{
			foreach (TroopTradeDifference troopTradeDifference in this._initialData.GetTroopTradeDifferencesFromTo(this.CurrentData, PartyScreenLogic.PartyRosterSide.None))
			{
				if (!troopTradeDifference.IsPrisoner && troopTradeDifference.FromCount < troopTradeDifference.ToCount)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x000BD80C File Offset: 0x000BBA0C
		public PartyScreenLogic.TroopComparer GetComparer(PartyScreenLogic.TroopSortType sortType)
		{
			return this._defaultComparers[sortType];
		}

		// Token: 0x04000CDD RID: 3293
		public PartyPresentationDoneButtonDelegate PartyPresentationDoneButtonDelegate;

		// Token: 0x04000CDE RID: 3294
		public PartyPresentationDoneButtonConditionDelegate PartyPresentationDoneButtonConditionDelegate;

		// Token: 0x04000CDF RID: 3295
		public PartyPresentationCancelButtonActivateDelegate PartyPresentationCancelButtonActivateDelegate;

		// Token: 0x04000CE0 RID: 3296
		public PartyPresentationCancelButtonDelegate PartyPresentationCancelButtonDelegate;

		// Token: 0x04000CE1 RID: 3297
		public PartyScreenLogic.PresentationUpdate UpdateDelegate;

		// Token: 0x04000CE2 RID: 3298
		public IsTroopTransferableDelegate IsTroopTransferableDelegate;

		// Token: 0x04000CE3 RID: 3299
		public CanTalkToHeroDelegate CanTalkToHeroDelegate;

		// Token: 0x04000CEB RID: 3307
		private PartyScreenLogic.TroopSortType _activeOtherPartySortType;

		// Token: 0x04000CEC RID: 3308
		private PartyScreenLogic.TroopSortType _activeMainPartySortType;

		// Token: 0x04000CED RID: 3309
		private bool _isOtherPartySortAscending;

		// Token: 0x04000CEE RID: 3310
		private bool _isMainPartySortAscending;

		// Token: 0x04000D04 RID: 3332
		public TroopRoster[] MemberRosters;

		// Token: 0x04000D05 RID: 3333
		public TroopRoster[] PrisonerRosters;

		// Token: 0x04000D06 RID: 3334
		public bool IsConsumablesChanges;

		// Token: 0x04000D07 RID: 3335
		private PartyScreenHelper.PartyScreenMode _partyScreenMode;

		// Token: 0x04000D08 RID: 3336
		private readonly Dictionary<PartyScreenLogic.TroopSortType, PartyScreenLogic.TroopComparer> _defaultComparers;

		// Token: 0x04000D09 RID: 3337
		private readonly PartyScreenData _initialData;

		// Token: 0x04000D0A RID: 3338
		private PartyScreenData _savedData;

		// Token: 0x04000D0B RID: 3339
		private Game _game;

		// Token: 0x020006A5 RID: 1701
		public enum TroopSortType
		{
			// Token: 0x04001AED RID: 6893
			Invalid = -1,
			// Token: 0x04001AEE RID: 6894
			Custom,
			// Token: 0x04001AEF RID: 6895
			Type,
			// Token: 0x04001AF0 RID: 6896
			Name,
			// Token: 0x04001AF1 RID: 6897
			Count,
			// Token: 0x04001AF2 RID: 6898
			Tier
		}

		// Token: 0x020006A6 RID: 1702
		public enum PartyRosterSide : byte
		{
			// Token: 0x04001AF4 RID: 6900
			None = 99,
			// Token: 0x04001AF5 RID: 6901
			Right = 1,
			// Token: 0x04001AF6 RID: 6902
			Left = 0
		}

		// Token: 0x020006A7 RID: 1703
		[Flags]
		public enum TroopType
		{
			// Token: 0x04001AF8 RID: 6904
			Member = 1,
			// Token: 0x04001AF9 RID: 6905
			Prisoner = 2,
			// Token: 0x04001AFA RID: 6906
			None = 3
		}

		// Token: 0x020006A8 RID: 1704
		public enum PartyCommandCode
		{
			// Token: 0x04001AFC RID: 6908
			TransferTroop,
			// Token: 0x04001AFD RID: 6909
			UpgradeTroop,
			// Token: 0x04001AFE RID: 6910
			TransferPartyLeaderTroop,
			// Token: 0x04001AFF RID: 6911
			TransferTroopToLeaderSlot,
			// Token: 0x04001B00 RID: 6912
			ShiftTroop,
			// Token: 0x04001B01 RID: 6913
			RecruitTroop,
			// Token: 0x04001B02 RID: 6914
			ExecuteTroop,
			// Token: 0x04001B03 RID: 6915
			TransferAllTroops,
			// Token: 0x04001B04 RID: 6916
			SortTroops
		}

		// Token: 0x020006A9 RID: 1705
		public enum TransferState
		{
			// Token: 0x04001B06 RID: 6918
			NotTransferable,
			// Token: 0x04001B07 RID: 6919
			Transferable,
			// Token: 0x04001B08 RID: 6920
			TransferableWithTrade
		}

		// Token: 0x020006AA RID: 1706
		// (Invoke) Token: 0x06005342 RID: 21314
		public delegate void PresentationUpdate(PartyScreenLogic.PartyCommand command);

		// Token: 0x020006AB RID: 1707
		// (Invoke) Token: 0x06005346 RID: 21318
		public delegate void PartyGoldDelegate();

		// Token: 0x020006AC RID: 1708
		// (Invoke) Token: 0x0600534A RID: 21322
		public delegate void PartyMoraleDelegate();

		// Token: 0x020006AD RID: 1709
		// (Invoke) Token: 0x0600534E RID: 21326
		public delegate void PartyInfluenceDelegate();

		// Token: 0x020006AE RID: 1710
		// (Invoke) Token: 0x06005352 RID: 21330
		public delegate void PartyHorseDelegate();

		// Token: 0x020006AF RID: 1711
		// (Invoke) Token: 0x06005356 RID: 21334
		public delegate void AfterResetDelegate(PartyScreenLogic partyScreenLogic, bool fromCancel);

		// Token: 0x020006B0 RID: 1712
		public class PartyCommand : ISerializableObject
		{
			// Token: 0x17000F7E RID: 3966
			// (get) Token: 0x06005359 RID: 21337 RVA: 0x0018CF0B File Offset: 0x0018B10B
			// (set) Token: 0x0600535A RID: 21338 RVA: 0x0018CF13 File Offset: 0x0018B113
			public PartyScreenLogic.PartyCommandCode Code { get; private set; }

			// Token: 0x17000F7F RID: 3967
			// (get) Token: 0x0600535B RID: 21339 RVA: 0x0018CF1C File Offset: 0x0018B11C
			// (set) Token: 0x0600535C RID: 21340 RVA: 0x0018CF24 File Offset: 0x0018B124
			public PartyScreenLogic.PartyRosterSide RosterSide { get; private set; }

			// Token: 0x17000F80 RID: 3968
			// (get) Token: 0x0600535D RID: 21341 RVA: 0x0018CF2D File Offset: 0x0018B12D
			// (set) Token: 0x0600535E RID: 21342 RVA: 0x0018CF35 File Offset: 0x0018B135
			public CharacterObject Character { get; private set; }

			// Token: 0x17000F81 RID: 3969
			// (get) Token: 0x0600535F RID: 21343 RVA: 0x0018CF3E File Offset: 0x0018B13E
			// (set) Token: 0x06005360 RID: 21344 RVA: 0x0018CF46 File Offset: 0x0018B146
			public int TotalNumber { get; private set; }

			// Token: 0x17000F82 RID: 3970
			// (get) Token: 0x06005361 RID: 21345 RVA: 0x0018CF4F File Offset: 0x0018B14F
			// (set) Token: 0x06005362 RID: 21346 RVA: 0x0018CF57 File Offset: 0x0018B157
			public int WoundedNumber { get; private set; }

			// Token: 0x17000F83 RID: 3971
			// (get) Token: 0x06005363 RID: 21347 RVA: 0x0018CF60 File Offset: 0x0018B160
			// (set) Token: 0x06005364 RID: 21348 RVA: 0x0018CF68 File Offset: 0x0018B168
			public int Index { get; private set; }

			// Token: 0x17000F84 RID: 3972
			// (get) Token: 0x06005365 RID: 21349 RVA: 0x0018CF71 File Offset: 0x0018B171
			// (set) Token: 0x06005366 RID: 21350 RVA: 0x0018CF79 File Offset: 0x0018B179
			public int UpgradeTarget { get; private set; }

			// Token: 0x17000F85 RID: 3973
			// (get) Token: 0x06005367 RID: 21351 RVA: 0x0018CF82 File Offset: 0x0018B182
			// (set) Token: 0x06005368 RID: 21352 RVA: 0x0018CF8A File Offset: 0x0018B18A
			public PartyScreenLogic.TroopType Type { get; private set; }

			// Token: 0x17000F86 RID: 3974
			// (get) Token: 0x06005369 RID: 21353 RVA: 0x0018CF93 File Offset: 0x0018B193
			// (set) Token: 0x0600536A RID: 21354 RVA: 0x0018CF9B File Offset: 0x0018B19B
			public PartyScreenLogic.TroopSortType SortType { get; private set; }

			// Token: 0x17000F87 RID: 3975
			// (get) Token: 0x0600536B RID: 21355 RVA: 0x0018CFA4 File Offset: 0x0018B1A4
			// (set) Token: 0x0600536C RID: 21356 RVA: 0x0018CFAC File Offset: 0x0018B1AC
			public bool IsSortAscending { get; private set; }

			// Token: 0x0600536E RID: 21358 RVA: 0x0018CFBD File Offset: 0x0018B1BD
			public void FillForTransferTroop(PartyScreenLogic.PartyRosterSide fromSide, PartyScreenLogic.TroopType type, CharacterObject character, int totalNumber, int woundedNumber, int targetIndex)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.TransferTroop;
				this.RosterSide = fromSide;
				this.TotalNumber = totalNumber;
				this.WoundedNumber = woundedNumber;
				this.Character = character;
				this.Type = type;
				this.Index = targetIndex;
			}

			// Token: 0x0600536F RID: 21359 RVA: 0x0018CFF3 File Offset: 0x0018B1F3
			public void FillForShiftTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int targetIndex)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.ShiftTroop;
				this.RosterSide = side;
				this.Character = character;
				this.Type = type;
				this.Index = targetIndex;
			}

			// Token: 0x06005370 RID: 21360 RVA: 0x0018D019 File Offset: 0x0018B219
			public void FillForTransferTroopToLeaderSlot(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int totalNumber, int woundedNumber, int targetIndex)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.TransferTroopToLeaderSlot;
				this.RosterSide = side;
				this.TotalNumber = totalNumber;
				this.WoundedNumber = woundedNumber;
				this.Character = character;
				this.Type = type;
				this.Index = targetIndex;
			}

			// Token: 0x06005371 RID: 21361 RVA: 0x0018D04F File Offset: 0x0018B24F
			public void FillForTransferPartyLeaderTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int totalNumber)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.TransferPartyLeaderTroop;
				this.RosterSide = side;
				this.TotalNumber = totalNumber;
				this.Character = character;
				this.Type = type;
			}

			// Token: 0x06005372 RID: 21362 RVA: 0x0018D075 File Offset: 0x0018B275
			public void FillForUpgradeTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int number, int upgradeTargetType, int index)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.UpgradeTroop;
				this.RosterSide = side;
				this.TotalNumber = number;
				this.Character = character;
				this.UpgradeTarget = upgradeTargetType;
				this.Type = type;
				this.Index = index;
			}

			// Token: 0x06005373 RID: 21363 RVA: 0x0018D0AB File Offset: 0x0018B2AB
			public void FillForRecruitTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int number, int index)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.RecruitTroop;
				this.RosterSide = side;
				this.Character = character;
				this.Type = type;
				this.TotalNumber = number;
				this.Index = index;
			}

			// Token: 0x06005374 RID: 21364 RVA: 0x0018D0D9 File Offset: 0x0018B2D9
			public void FillForExecuteTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.ExecuteTroop;
				this.RosterSide = side;
				this.Character = character;
				this.Type = type;
			}

			// Token: 0x06005375 RID: 21365 RVA: 0x0018D0F7 File Offset: 0x0018B2F7
			public void FillForTransferAllTroops(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.TransferAllTroops;
				this.RosterSide = side;
				this.Type = type;
			}

			// Token: 0x06005376 RID: 21366 RVA: 0x0018D10E File Offset: 0x0018B30E
			public void FillForSortTroops(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopSortType sortType, bool isAscending)
			{
				this.RosterSide = side;
				this.Code = PartyScreenLogic.PartyCommandCode.SortTroops;
				this.SortType = sortType;
				this.IsSortAscending = isAscending;
			}

			// Token: 0x06005377 RID: 21367 RVA: 0x0018D12C File Offset: 0x0018B32C
			void ISerializableObject.SerializeTo(IWriter writer)
			{
				writer.WriteByte((byte)this.Code);
				writer.WriteByte((byte)this.RosterSide);
				writer.WriteUInt(this.Character.Id.InternalValue);
				writer.WriteInt(this.TotalNumber);
				writer.WriteInt(this.WoundedNumber);
				writer.WriteInt(this.UpgradeTarget);
				writer.WriteByte((byte)this.Type);
			}

			// Token: 0x06005378 RID: 21368 RVA: 0x0018D19C File Offset: 0x0018B39C
			void ISerializableObject.DeserializeFrom(IReader reader)
			{
				this.Code = (PartyScreenLogic.PartyCommandCode)reader.ReadByte();
				this.RosterSide = (PartyScreenLogic.PartyRosterSide)reader.ReadByte();
				MBGUID mbguid = new MBGUID(reader.ReadUInt());
				this.Character = (CharacterObject)MBObjectManager.Instance.GetObject(mbguid);
				this.TotalNumber = reader.ReadInt();
				this.WoundedNumber = reader.ReadInt();
				this.UpgradeTarget = reader.ReadInt();
				this.Type = (PartyScreenLogic.TroopType)reader.ReadByte();
			}
		}

		// Token: 0x020006B1 RID: 1713
		public abstract class TroopComparer : IComparer<TroopRosterElement>
		{
			// Token: 0x06005379 RID: 21369 RVA: 0x0018D214 File Offset: 0x0018B414
			public void SetIsAscending(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x0600537A RID: 21370 RVA: 0x0018D21D File Offset: 0x0018B41D
			private int GetHeroComparisonResult(TroopRosterElement x, TroopRosterElement y)
			{
				if (x.Character.HeroObject != null)
				{
					if (x.Character.HeroObject == Hero.MainHero)
					{
						return -2;
					}
					if (y.Character.HeroObject == null)
					{
						return -1;
					}
				}
				return 0;
			}

			// Token: 0x0600537B RID: 21371 RVA: 0x0018D254 File Offset: 0x0018B454
			public int Compare(TroopRosterElement x, TroopRosterElement y)
			{
				int num = (this._isAscending ? 1 : (-1));
				int num2 = this.GetHeroComparisonResult(x, y);
				if (num2 != 0)
				{
					return num2;
				}
				num2 = this.GetHeroComparisonResult(y, x);
				if (num2 != 0)
				{
					return num2 * -1;
				}
				return this.CompareTroops(x, y) * num;
			}

			// Token: 0x0600537C RID: 21372
			protected abstract int CompareTroops(TroopRosterElement x, TroopRosterElement y);

			// Token: 0x04001B13 RID: 6931
			private bool _isAscending;
		}

		// Token: 0x020006B2 RID: 1714
		private class TroopDefaultComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x0600537E RID: 21374 RVA: 0x0018D29E File Offset: 0x0018B49E
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				return 0;
			}
		}

		// Token: 0x020006B3 RID: 1715
		private class TroopTypeComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x06005380 RID: 21376 RVA: 0x0018D2AC File Offset: 0x0018B4AC
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				int defaultFormationClass = (int)x.Character.DefaultFormationClass;
				int defaultFormationClass2 = (int)y.Character.DefaultFormationClass;
				return defaultFormationClass.CompareTo(defaultFormationClass2);
			}
		}

		// Token: 0x020006B4 RID: 1716
		private class TroopNameComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x06005382 RID: 21378 RVA: 0x0018D2E1 File Offset: 0x0018B4E1
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				return x.Character.Name.ToString().CompareTo(y.Character.Name.ToString());
			}
		}

		// Token: 0x020006B5 RID: 1717
		private class TroopCountComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x06005384 RID: 21380 RVA: 0x0018D310 File Offset: 0x0018B510
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				return x.Number.CompareTo(y.Number);
			}
		}

		// Token: 0x020006B6 RID: 1718
		private class TroopTierComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x06005386 RID: 21382 RVA: 0x0018D33C File Offset: 0x0018B53C
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				return x.Character.Tier.CompareTo(y.Character.Tier);
			}
		}
	}
}
