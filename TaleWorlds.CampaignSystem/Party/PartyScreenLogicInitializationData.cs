using System;
using Helpers;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x02000302 RID: 770
	public struct PartyScreenLogicInitializationData
	{
		// Token: 0x06002D35 RID: 11573 RVA: 0x000BF0E4 File Offset: 0x000BD2E4
		public static PartyScreenLogicInitializationData CreateBasicInitDataWithMainParty(TroopRoster leftMemberRoster, TroopRoster leftPrisonerRoster, PartyScreenLogic.TransferState memberTransferState, PartyScreenLogic.TransferState prisonerTransferState, PartyScreenLogic.TransferState accompanyingTransferState, IsTroopTransferableDelegate troopTransferableDelegate, PartyScreenHelper.PartyScreenMode partyScreenMode, PartyBase leftOwnerParty = null, TextObject leftPartyName = null, TextObject header = null, Hero leftLeaderHero = null, int leftPartyMembersSizeLimit = 0, int leftPartyPrisonersSizeLimit = 0, PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = null, PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = null, PartyPresentationCancelButtonDelegate partyPresentationCancelButtonDelegate = null, PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null, PartyScreenClosedDelegate partyScreenClosedDelegate = null, bool isDismissMode = false, bool transferHealthiesGetWoundedsFirst = false, bool isTroopUpgradesDisabled = false, bool showProgressBar = false, int questModeWageDaysMultiplier = 0)
		{
			return new PartyScreenLogicInitializationData
			{
				LeftOwnerParty = leftOwnerParty,
				RightOwnerParty = PartyBase.MainParty,
				LeftMemberRoster = leftMemberRoster,
				LeftPrisonerRoster = leftPrisonerRoster,
				RightMemberRoster = PartyBase.MainParty.MemberRoster,
				RightPrisonerRoster = PartyBase.MainParty.PrisonRoster,
				LeftLeaderHero = leftLeaderHero,
				RightLeaderHero = PartyBase.MainParty.LeaderHero,
				LeftPartyMembersSizeLimit = leftPartyMembersSizeLimit,
				LeftPartyPrisonersSizeLimit = leftPartyPrisonersSizeLimit,
				RightPartyMembersSizeLimit = PartyBase.MainParty.PartySizeLimit,
				RightPartyPrisonersSizeLimit = PartyBase.MainParty.PrisonerSizeLimit,
				LeftPartyName = leftPartyName,
				RightPartyName = PartyBase.MainParty.Name,
				TroopTransferableDelegate = troopTransferableDelegate,
				PartyScreenMode = partyScreenMode,
				PartyPresentationDoneButtonDelegate = partyPresentationDoneButtonDelegate,
				PartyPresentationDoneButtonConditionDelegate = partyPresentationDoneButtonConditionDelegate,
				PartyPresentationCancelButtonActivateDelegate = partyPresentationCancelButtonActivateDelegate,
				PartyPresentationCancelButtonDelegate = partyPresentationCancelButtonDelegate,
				IsDismissMode = isDismissMode,
				IsTroopUpgradesDisabled = isTroopUpgradesDisabled,
				Header = header,
				PartyScreenClosedDelegate = partyScreenClosedDelegate,
				TransferHealthiesGetWoundedsFirst = transferHealthiesGetWoundedsFirst,
				ShowProgressBar = showProgressBar,
				MemberTransferState = memberTransferState,
				PrisonerTransferState = prisonerTransferState,
				AccompanyingTransferState = accompanyingTransferState,
				QuestModeWageDaysMultiplier = questModeWageDaysMultiplier
			};
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000BF238 File Offset: 0x000BD438
		public static PartyScreenLogicInitializationData CreateBasicInitDataWithMainPartyAndOther(MobileParty party, PartyScreenLogic.TransferState memberTransferState, PartyScreenLogic.TransferState prisonerTransferState, PartyScreenLogic.TransferState accompanyingTransferState, IsTroopTransferableDelegate troopTransferableDelegate, PartyScreenHelper.PartyScreenMode partyScreenMode, TextObject header = null, PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = null, PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = null, PartyPresentationCancelButtonDelegate partyPresentationCancelButtonDelegate = null, PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null, PartyScreenClosedDelegate partyScreenClosedDelegate = null, bool isDismissMode = false, bool transferHealthiesGetWoundedsFirst = false, bool isTroopUpgradesDisabled = true, bool showProgressBar = false)
		{
			return new PartyScreenLogicInitializationData
			{
				LeftOwnerParty = party.Party,
				RightOwnerParty = PartyBase.MainParty,
				LeftMemberRoster = party.MemberRoster,
				LeftPrisonerRoster = party.PrisonRoster,
				RightMemberRoster = PartyBase.MainParty.MemberRoster,
				RightPrisonerRoster = PartyBase.MainParty.PrisonRoster,
				LeftLeaderHero = party.LeaderHero,
				RightLeaderHero = PartyBase.MainParty.LeaderHero,
				LeftPartyMembersSizeLimit = party.Party.PartySizeLimit,
				LeftPartyPrisonersSizeLimit = party.Party.PrisonerSizeLimit,
				RightPartyMembersSizeLimit = PartyBase.MainParty.PartySizeLimit,
				RightPartyPrisonersSizeLimit = PartyBase.MainParty.PrisonerSizeLimit,
				LeftPartyName = party.Name,
				RightPartyName = PartyBase.MainParty.Name,
				TroopTransferableDelegate = troopTransferableDelegate,
				PartyScreenMode = partyScreenMode,
				PartyPresentationDoneButtonDelegate = partyPresentationDoneButtonDelegate,
				PartyPresentationDoneButtonConditionDelegate = partyPresentationDoneButtonConditionDelegate,
				PartyPresentationCancelButtonActivateDelegate = partyPresentationCancelButtonActivateDelegate,
				PartyPresentationCancelButtonDelegate = partyPresentationCancelButtonDelegate,
				IsDismissMode = isDismissMode,
				IsTroopUpgradesDisabled = isTroopUpgradesDisabled,
				Header = header,
				PartyScreenClosedDelegate = partyScreenClosedDelegate,
				TransferHealthiesGetWoundedsFirst = transferHealthiesGetWoundedsFirst,
				ShowProgressBar = showProgressBar,
				MemberTransferState = memberTransferState,
				PrisonerTransferState = prisonerTransferState,
				AccompanyingTransferState = accompanyingTransferState
			};
		}

		// Token: 0x04000D23 RID: 3363
		public TroopRoster LeftMemberRoster;

		// Token: 0x04000D24 RID: 3364
		public TroopRoster LeftPrisonerRoster;

		// Token: 0x04000D25 RID: 3365
		public TroopRoster RightMemberRoster;

		// Token: 0x04000D26 RID: 3366
		public TroopRoster RightPrisonerRoster;

		// Token: 0x04000D27 RID: 3367
		public PartyBase LeftOwnerParty;

		// Token: 0x04000D28 RID: 3368
		public PartyBase RightOwnerParty;

		// Token: 0x04000D29 RID: 3369
		public TextObject LeftPartyName;

		// Token: 0x04000D2A RID: 3370
		public TextObject RightPartyName;

		// Token: 0x04000D2B RID: 3371
		public TextObject Header;

		// Token: 0x04000D2C RID: 3372
		public Hero LeftLeaderHero;

		// Token: 0x04000D2D RID: 3373
		public Hero RightLeaderHero;

		// Token: 0x04000D2E RID: 3374
		public int LeftPartyMembersSizeLimit;

		// Token: 0x04000D2F RID: 3375
		public int LeftPartyPrisonersSizeLimit;

		// Token: 0x04000D30 RID: 3376
		public int RightPartyMembersSizeLimit;

		// Token: 0x04000D31 RID: 3377
		public int RightPartyPrisonersSizeLimit;

		// Token: 0x04000D32 RID: 3378
		public PartyPresentationDoneButtonDelegate PartyPresentationDoneButtonDelegate;

		// Token: 0x04000D33 RID: 3379
		public PartyPresentationDoneButtonConditionDelegate PartyPresentationDoneButtonConditionDelegate;

		// Token: 0x04000D34 RID: 3380
		public PartyPresentationCancelButtonActivateDelegate PartyPresentationCancelButtonActivateDelegate;

		// Token: 0x04000D35 RID: 3381
		public IsTroopTransferableDelegate TroopTransferableDelegate;

		// Token: 0x04000D36 RID: 3382
		public CanTalkToHeroDelegate CanTalkToTroopDelegate;

		// Token: 0x04000D37 RID: 3383
		public PartyPresentationCancelButtonDelegate PartyPresentationCancelButtonDelegate;

		// Token: 0x04000D38 RID: 3384
		public PartyScreenClosedDelegate PartyScreenClosedDelegate;

		// Token: 0x04000D39 RID: 3385
		public bool DoNotApplyGoldTransactions;

		// Token: 0x04000D3A RID: 3386
		public bool IsDismissMode;

		// Token: 0x04000D3B RID: 3387
		public bool TransferHealthiesGetWoundedsFirst;

		// Token: 0x04000D3C RID: 3388
		public bool IsTroopUpgradesDisabled;

		// Token: 0x04000D3D RID: 3389
		public bool ShowProgressBar;

		// Token: 0x04000D3E RID: 3390
		public int QuestModeWageDaysMultiplier;

		// Token: 0x04000D3F RID: 3391
		public PartyScreenLogic.TransferState MemberTransferState;

		// Token: 0x04000D40 RID: 3392
		public PartyScreenLogic.TransferState PrisonerTransferState;

		// Token: 0x04000D41 RID: 3393
		public PartyScreenLogic.TransferState AccompanyingTransferState;

		// Token: 0x04000D42 RID: 3394
		public PartyScreenHelper.PartyScreenMode PartyScreenMode;
	}
}
