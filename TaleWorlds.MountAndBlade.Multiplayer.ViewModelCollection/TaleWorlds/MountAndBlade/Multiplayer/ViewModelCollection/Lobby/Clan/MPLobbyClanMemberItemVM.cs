using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000070 RID: 112
	public class MPLobbyClanMemberItemVM : MPLobbyPlayerBaseVM
	{
		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x0002123C File Offset: 0x0001F43C
		// (set) Token: 0x06000ADD RID: 2781 RVA: 0x00021244 File Offset: 0x0001F444
		public PlayerId Id { get; private set; }

		// Token: 0x06000ADE RID: 2782 RVA: 0x0002124D File Offset: 0x0001F44D
		public MPLobbyClanMemberItemVM(PlayerId playerId)
			: base(playerId, "", null, null)
		{
			this.Id = playerId;
			this.RefreshValues();
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002126C File Offset: 0x0001F46C
		public MPLobbyClanMemberItemVM(ClanPlayer member, bool isOnline, string selectedBadgeID, AnotherPlayerState state, Action<MPLobbyClanMemberItemVM> executeActivate = null)
			: base(member.PlayerId, "", null, null)
		{
			this._member = member;
			this.Id = this._member.PlayerId;
			this.IsOnline = isOnline;
			this._executeActivate = executeActivate;
			base.SelectedBadgeID = selectedBadgeID;
			if (isOnline)
			{
				base.StateText = GameTexts.FindText("str_multiplayer_lobby_state", state.ToString()).ToString();
			}
			this.IsClanLeader = this._member.Role == ClanPlayerRole.Leader;
			this.Rank = (int)this._member.Role;
			this.RankHint = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00021318 File Offset: 0x0001F518
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.NotEligibleInfo = "";
			ClanPlayer member = this._member;
			if (member != null && member.Role == ClanPlayerRole.Leader)
			{
				this.RankHint.HintText = new TextObject("{=SrfYbg3x}Leader", null);
				return;
			}
			ClanPlayer member2 = this._member;
			if (member2 != null && member2.Role == ClanPlayerRole.Officer)
			{
				this.RankHint.HintText = new TextObject("{=ZYF2t1VI}Officer", null);
			}
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00021394 File Offset: 0x0001F594
		public void SetNotEligibleInfo(PlayerNotEligibleError notEligibleError)
		{
			string text = "";
			if (notEligibleError == PlayerNotEligibleError.AlreadyInClan)
			{
				text = new TextObject("{=zEMWM4h3}Already In a Clan", null).ToString();
			}
			else if (notEligibleError == PlayerNotEligibleError.NotAtLobby)
			{
				text = new TextObject("{=hPbppi6E}Not At The Lobby", null).ToString();
			}
			else if (notEligibleError == PlayerNotEligibleError.DoesNotSupportFeature)
			{
				text = new TextObject("{=MsokbMx2}Does not support Clan feature", null).ToString();
			}
			if (string.IsNullOrEmpty(this.NotEligibleInfo))
			{
				this.NotEligibleInfo = text;
				return;
			}
			GameTexts.SetVariable("LEFT", this.NotEligibleInfo);
			GameTexts.SetVariable("RIGHT", text);
			this.NotEligibleInfo = GameTexts.FindText("str_LEFT_comma_RIGHT", null).ToString();
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0002142F File Offset: 0x0001F62F
		private void ExecuteSelection()
		{
			Action<MPLobbyClanMemberItemVM> executeActivate = this._executeActivate;
			if (executeActivate == null)
			{
				return;
			}
			executeActivate(this);
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x00021442 File Offset: 0x0001F642
		// (set) Token: 0x06000AE4 RID: 2788 RVA: 0x0002144A File Offset: 0x0001F64A
		[DataSourceProperty]
		public bool IsOnline
		{
			get
			{
				return this._isOnline;
			}
			set
			{
				if (value != this._isOnline)
				{
					this._isOnline = value;
					base.OnPropertyChangedWithValue(value, "IsOnline");
				}
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x00021468 File Offset: 0x0001F668
		// (set) Token: 0x06000AE6 RID: 2790 RVA: 0x00021470 File Offset: 0x0001F670
		[DataSourceProperty]
		public bool IsClanLeader
		{
			get
			{
				return this._isClanLeader;
			}
			set
			{
				if (value != this._isClanLeader)
				{
					this._isClanLeader = value;
					base.OnPropertyChangedWithValue(value, "IsClanLeader");
				}
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x0002148E File Offset: 0x0001F68E
		// (set) Token: 0x06000AE8 RID: 2792 RVA: 0x00021496 File Offset: 0x0001F696
		[DataSourceProperty]
		public string NotEligibleInfo
		{
			get
			{
				return this._notEligibleInfo;
			}
			set
			{
				if (value != this._notEligibleInfo)
				{
					this._notEligibleInfo = value;
					base.OnPropertyChangedWithValue<string>(value, "NotEligibleInfo");
				}
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x000214B9 File Offset: 0x0001F6B9
		// (set) Token: 0x06000AEA RID: 2794 RVA: 0x000214C1 File Offset: 0x0001F6C1
		[DataSourceProperty]
		public string InviteAcceptInfo
		{
			get
			{
				return this._inviteAcceptInfo;
			}
			set
			{
				if (value != this._inviteAcceptInfo)
				{
					this._inviteAcceptInfo = value;
					base.OnPropertyChangedWithValue<string>(value, "InviteAcceptInfo");
				}
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x000214E4 File Offset: 0x0001F6E4
		// (set) Token: 0x06000AEC RID: 2796 RVA: 0x000214EC File Offset: 0x0001F6EC
		[DataSourceProperty]
		public int Rank
		{
			get
			{
				return this._rank;
			}
			set
			{
				if (value != this._rank)
				{
					this._rank = value;
					base.OnPropertyChangedWithValue(value, "Rank");
				}
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x0002150A File Offset: 0x0001F70A
		// (set) Token: 0x06000AEE RID: 2798 RVA: 0x00021512 File Offset: 0x0001F712
		[DataSourceProperty]
		public MBBindingList<StringPairItemWithActionVM> UserActionsList
		{
			get
			{
				return this._userActionsList;
			}
			set
			{
				if (value != this._userActionsList)
				{
					this._userActionsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemWithActionVM>>(value, "UserActionsList");
				}
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x00021530 File Offset: 0x0001F730
		// (set) Token: 0x06000AF0 RID: 2800 RVA: 0x00021538 File Offset: 0x0001F738
		[DataSourceProperty]
		public HintViewModel RankHint
		{
			get
			{
				return this._rankHint;
			}
			set
			{
				if (value != this._rankHint)
				{
					this._rankHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RankHint");
				}
			}
		}

		// Token: 0x040004F6 RID: 1270
		private ClanPlayer _member;

		// Token: 0x040004F8 RID: 1272
		private Action<MPLobbyClanMemberItemVM> _executeActivate;

		// Token: 0x040004F9 RID: 1273
		private bool _isOnline;

		// Token: 0x040004FA RID: 1274
		private bool _isClanLeader;

		// Token: 0x040004FB RID: 1275
		private string _notEligibleInfo;

		// Token: 0x040004FC RID: 1276
		private string _inviteAcceptInfo;

		// Token: 0x040004FD RID: 1277
		private int _rank;

		// Token: 0x040004FE RID: 1278
		private MBBindingList<StringPairItemWithActionVM> _userActionsList;

		// Token: 0x040004FF RID: 1279
		private HintViewModel _rankHint;
	}
}
