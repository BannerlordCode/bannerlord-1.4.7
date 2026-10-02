using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard
{
	// Token: 0x0200001F RID: 31
	public class MissionScoreboardSideVM : ViewModel
	{
		// Token: 0x060001EB RID: 491 RVA: 0x00007C58 File Offset: 0x00005E58
		public MissionScoreboardSideVM(MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide, Action<MissionScoreboardPlayerVM> executeActivate, bool isSingleSide, bool isSecondSide, MultiplayerBattleColors.MultiplayerCultureColorInfo cultureColorInfo)
		{
			this._executeActivate = executeActivate;
			this._missionScoreboardSide = missionScoreboardSide;
			this._playersMap = new Dictionary<MissionPeer, MissionScoreboardPlayerVM>();
			this.Players = new MBBindingList<MissionScoreboardPlayerVM>();
			this.PlayerSortController = new MissionScoreboardPlayerSortControllerVM(ref this._players);
			this._avatarHeaderIndex = missionScoreboardSide.GetHeaderIds().IndexOf("avatar");
			int score = missionScoreboardSide.GetScore(null);
			string[] valuesOf = missionScoreboardSide.GetValuesOf(null);
			string[] headerIds = missionScoreboardSide.GetHeaderIds();
			this._bot = new MissionScoreboardPlayerVM(valuesOf, headerIds, score, this._executeActivate);
			foreach (MissionPeer missionPeer in missionScoreboardSide.Players)
			{
				this.AddPlayer(missionPeer);
			}
			this.UpdateBotAttributes();
			this.UpdateRoundAttributes();
			this.IsSingleSide = isSingleSide;
			this.IsSecondSide = isSecondSide;
			BasicCultureObject culture = cultureColorInfo.Culture;
			this.CultureId = ((culture != null) ? culture.StringId : null) ?? string.Empty;
			this.CultureColor1 = cultureColorInfo.Color1;
			this.CultureColor2 = cultureColorInfo.Color2;
			string text = "0x";
			BasicCultureObject culture2 = cultureColorInfo.Culture;
			this.TeamColor = text + ((culture2 != null) ? culture2.Color2.ToString("X") : null);
			this.ShowAttackerOrDefenderIcons = Mission.Current.HasMissionBehavior<MissionMultiplayerSiegeClient>();
			this.IsAttacker = missionScoreboardSide.Side == BattleSideEnum.Attacker;
			this.RefreshValues();
			NetworkCommunicator.OnPeerAveragePingUpdated += this.OnPeerPingUpdated;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00007E44 File Offset: 0x00006044
		public override void RefreshValues()
		{
			base.RefreshValues();
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>((this._missionScoreboardSide.Side == BattleSideEnum.Attacker) ? MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			if (this.IsSingleSide)
			{
				this.Name = MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
			else
			{
				this.Name = @object.Name.ToString();
			}
			this.EntryProperties = new MBBindingList<MissionScoreboardHeaderItemVM>();
			string[] headerIds = this._missionScoreboardSide.GetHeaderIds();
			string[] headerNames = this._missionScoreboardSide.GetHeaderNames();
			for (int i = 0; i < headerIds.Length; i++)
			{
				this.EntryProperties.Add(new MissionScoreboardHeaderItemVM(this, headerIds[i], headerNames[i], headerIds[i] == "avatar", this._irregularHeaderIDs.Contains(headerIds[i])));
			}
			this.UpdatePlayersText();
			MissionScoreboardPlayerSortControllerVM playerSortController = this.PlayerSortController;
			if (playerSortController == null)
			{
				return;
			}
			playerSortController.RefreshValues();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00007F24 File Offset: 0x00006124
		public void Tick(float dt)
		{
			foreach (MissionScoreboardPlayerVM missionScoreboardPlayerVM in this.Players)
			{
				missionScoreboardPlayerVM.Tick(dt);
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00007F70 File Offset: 0x00006170
		public override void OnFinalize()
		{
			base.OnFinalize();
			NetworkCommunicator.OnPeerAveragePingUpdated -= this.OnPeerPingUpdated;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007FA9 File Offset: 0x000061A9
		public void UpdateRoundAttributes()
		{
			this.RoundsWon = this._missionScoreboardSide.SideScore;
			this.SortPlayers();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00007FC4 File Offset: 0x000061C4
		public void UpdateBotAttributes()
		{
			int num = ((this._missionScoreboardSide.Side == BattleSideEnum.Attacker) ? MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.NumberOfBotsTeam2.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			if (num > 0)
			{
				int score = this._missionScoreboardSide.GetScore(null);
				string[] valuesOf = this._missionScoreboardSide.GetValuesOf(null);
				this._bot.UpdateAttributes(valuesOf, score);
				if (!this.Players.Contains(this._bot))
				{
					this.Players.Add(this._bot);
				}
			}
			else if (num == 0 && this.Players.Contains(this._bot))
			{
				this.Players.Remove(this._bot);
			}
			this.SortPlayers();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00008074 File Offset: 0x00006274
		public void UpdatePlayerAttributes(MissionPeer player)
		{
			if (this._playersMap.ContainsKey(player))
			{
				int score = this._missionScoreboardSide.GetScore(player);
				string[] valuesOf = this._missionScoreboardSide.GetValuesOf(player);
				this._playersMap[player].UpdateAttributes(valuesOf, score);
			}
			this.SortPlayers();
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000080C4 File Offset: 0x000062C4
		public void RemovePlayer(MissionPeer peer)
		{
			MissionScoreboardPlayerVM missionScoreboardPlayerVM;
			if (this._playersMap.TryGetValue(peer, out missionScoreboardPlayerVM))
			{
				this.Players.Remove(missionScoreboardPlayerVM);
				this._playersMap.Remove(peer);
			}
			else
			{
				Debug.FailedAssert("Trying to remove a player that is not registered", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Scoreboard\\MissionScoreboardSideVM.cs", "RemovePlayer", 167);
			}
			this.SortPlayers();
			this.UpdatePlayersText();
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008124 File Offset: 0x00006324
		public void AddPlayer(MissionPeer peer)
		{
			if (!this._playersMap.ContainsKey(peer))
			{
				int score = this._missionScoreboardSide.GetScore(peer);
				string[] valuesOf = this._missionScoreboardSide.GetValuesOf(peer);
				string[] headerIds = this._missionScoreboardSide.GetHeaderIds();
				MissionScoreboardPlayerVM missionScoreboardPlayerVM = new MissionScoreboardPlayerVM(peer, valuesOf, headerIds, score, this._executeActivate);
				this._playersMap.Add(peer, missionScoreboardPlayerVM);
				this.Players.Add(missionScoreboardPlayerVM);
			}
			this.SortPlayers();
			this.UpdatePlayersText();
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000819C File Offset: 0x0000639C
		private void UpdatePlayersText()
		{
			TextObject textObject = new TextObject("{=R28ac5ij}{NUMBER} Players", null);
			textObject.SetTextVariable("NUMBER", this.Players.Count);
			this.PlayersText = textObject.ToString();
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000081D8 File Offset: 0x000063D8
		private void SortPlayers()
		{
			this.PlayerSortController.SortByCurrentState();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000081E8 File Offset: 0x000063E8
		private void OnPeerPingUpdated(NetworkCommunicator peer)
		{
			MissionPeer component = peer.GetComponent<MissionPeer>();
			if (component != null)
			{
				this.UpdatePlayerAttributes(component);
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00008208 File Offset: 0x00006408
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.EnableGenericAvatars)
			{
				foreach (MissionScoreboardPlayerVM missionScoreboardPlayerVM in this.Players)
				{
					if (!missionScoreboardPlayerVM.IsBot)
					{
						missionScoreboardPlayerVM.RefreshAvatar();
					}
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00008264 File Offset: 0x00006464
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000826C File Offset: 0x0000646C
		[DataSourceProperty]
		public MBBindingList<MissionScoreboardPlayerVM> Players
		{
			get
			{
				return this._players;
			}
			set
			{
				if (this._players != value)
				{
					this._players = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionScoreboardPlayerVM>>(value, "Players");
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000828A File Offset: 0x0000648A
		// (set) Token: 0x060001FB RID: 507 RVA: 0x00008292 File Offset: 0x00006492
		[DataSourceProperty]
		public MBBindingList<MissionScoreboardHeaderItemVM> EntryProperties
		{
			get
			{
				return this._entryProperties;
			}
			set
			{
				if (value != this._entryProperties)
				{
					this._entryProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionScoreboardHeaderItemVM>>(value, "EntryProperties");
				}
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001FC RID: 508 RVA: 0x000082B0 File Offset: 0x000064B0
		// (set) Token: 0x060001FD RID: 509 RVA: 0x000082B8 File Offset: 0x000064B8
		[DataSourceProperty]
		public MissionScoreboardPlayerSortControllerVM PlayerSortController
		{
			get
			{
				return this._playerSortController;
			}
			set
			{
				if (value != this._playerSortController)
				{
					this._playerSortController = value;
					base.OnPropertyChangedWithValue<MissionScoreboardPlayerSortControllerVM>(value, "PlayerSortController");
				}
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001FE RID: 510 RVA: 0x000082D6 File Offset: 0x000064D6
		// (set) Token: 0x060001FF RID: 511 RVA: 0x000082DE File Offset: 0x000064DE
		[DataSourceProperty]
		public bool IsSingleSide
		{
			get
			{
				return this._isSingleSide;
			}
			set
			{
				if (value != this._isSingleSide)
				{
					this._isSingleSide = value;
					base.OnPropertyChangedWithValue(value, "IsSingleSide");
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000200 RID: 512 RVA: 0x000082FC File Offset: 0x000064FC
		// (set) Token: 0x06000201 RID: 513 RVA: 0x00008304 File Offset: 0x00006504
		[DataSourceProperty]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (value != this._cultureColor1)
				{
					this._cultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor1");
				}
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00008327 File Offset: 0x00006527
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000832F File Offset: 0x0000652F
		[DataSourceProperty]
		public Color CultureColor2
		{
			get
			{
				return this._cultureColor2;
			}
			set
			{
				if (value != this._cultureColor2)
				{
					this._cultureColor2 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor2");
				}
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00008352 File Offset: 0x00006552
		// (set) Token: 0x06000205 RID: 517 RVA: 0x0000835A File Offset: 0x0000655A
		[DataSourceProperty]
		public bool IsSecondSide
		{
			get
			{
				return this._isSecondSide;
			}
			set
			{
				if (value != this._isSecondSide)
				{
					this._isSecondSide = value;
					base.OnPropertyChangedWithValue(value, "IsSecondSide");
				}
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00008378 File Offset: 0x00006578
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00008380 File Offset: 0x00006580
		[DataSourceProperty]
		public bool ShowAttackerOrDefenderIcons
		{
			get
			{
				return this._showAttackerOrDefenderIcons;
			}
			set
			{
				if (value != this._showAttackerOrDefenderIcons)
				{
					this._showAttackerOrDefenderIcons = value;
					base.OnPropertyChangedWithValue(value, "ShowAttackerOrDefenderIcons");
				}
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000839E File Offset: 0x0000659E
		// (set) Token: 0x06000209 RID: 521 RVA: 0x000083A6 File Offset: 0x000065A6
		[DataSourceProperty]
		public bool IsAttacker
		{
			get
			{
				return this._isAttacker;
			}
			set
			{
				if (value != this._isAttacker)
				{
					this._isAttacker = value;
					base.OnPropertyChangedWithValue(value, "IsAttacker");
				}
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600020A RID: 522 RVA: 0x000083C4 File Offset: 0x000065C4
		// (set) Token: 0x0600020B RID: 523 RVA: 0x000083CC File Offset: 0x000065CC
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

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600020C RID: 524 RVA: 0x000083EF File Offset: 0x000065EF
		// (set) Token: 0x0600020D RID: 525 RVA: 0x000083F7 File Offset: 0x000065F7
		[DataSourceProperty]
		public string PlayersText
		{
			get
			{
				return this._playersText;
			}
			set
			{
				if (value != this._playersText)
				{
					this._playersText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayersText");
				}
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000841A File Offset: 0x0000661A
		// (set) Token: 0x0600020F RID: 527 RVA: 0x00008422 File Offset: 0x00006622
		[DataSourceProperty]
		public string CultureId
		{
			get
			{
				return this._cultureId;
			}
			set
			{
				if (value != this._cultureId)
				{
					this._cultureId = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureId");
				}
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00008445 File Offset: 0x00006645
		// (set) Token: 0x06000211 RID: 529 RVA: 0x0000844D File Offset: 0x0000664D
		[DataSourceProperty]
		public int RoundsWon
		{
			get
			{
				return this._roundsWon;
			}
			set
			{
				if (this._roundsWon != value)
				{
					this._roundsWon = value;
					base.OnPropertyChangedWithValue(value, "RoundsWon");
				}
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000846B File Offset: 0x0000666B
		// (set) Token: 0x06000213 RID: 531 RVA: 0x00008473 File Offset: 0x00006673
		[DataSourceProperty]
		public string TeamColor
		{
			get
			{
				return this._teamColor;
			}
			set
			{
				if (value != this._teamColor)
				{
					this._teamColor = value;
					base.OnPropertyChangedWithValue<string>(value, "TeamColor");
				}
			}
		}

		// Token: 0x04000109 RID: 265
		private readonly MissionScoreboardComponent.MissionScoreboardSide _missionScoreboardSide;

		// Token: 0x0400010A RID: 266
		private readonly Dictionary<MissionPeer, MissionScoreboardPlayerVM> _playersMap;

		// Token: 0x0400010B RID: 267
		private MissionScoreboardPlayerVM _bot;

		// Token: 0x0400010C RID: 268
		private Action<MissionScoreboardPlayerVM> _executeActivate;

		// Token: 0x0400010D RID: 269
		private const string _avatarHeaderId = "avatar";

		// Token: 0x0400010E RID: 270
		private readonly int _avatarHeaderIndex;

		// Token: 0x0400010F RID: 271
		private List<string> _irregularHeaderIDs = new List<string> { "name", "avatar", "score", "kill", "assist" };

		// Token: 0x04000110 RID: 272
		private MBBindingList<MissionScoreboardPlayerVM> _players;

		// Token: 0x04000111 RID: 273
		private MBBindingList<MissionScoreboardHeaderItemVM> _entryProperties;

		// Token: 0x04000112 RID: 274
		private MissionScoreboardPlayerSortControllerVM _playerSortController;

		// Token: 0x04000113 RID: 275
		private bool _isSingleSide;

		// Token: 0x04000114 RID: 276
		private bool _isSecondSide;

		// Token: 0x04000115 RID: 277
		private bool _showAttackerOrDefenderIcons;

		// Token: 0x04000116 RID: 278
		private bool _isAttacker;

		// Token: 0x04000117 RID: 279
		private int _roundsWon;

		// Token: 0x04000118 RID: 280
		private string _name;

		// Token: 0x04000119 RID: 281
		private string _cultureId;

		// Token: 0x0400011A RID: 282
		private string _teamColor;

		// Token: 0x0400011B RID: 283
		private string _playersText;

		// Token: 0x0400011C RID: 284
		private Color _cultureColor1;

		// Token: 0x0400011D RID: 285
		private Color _cultureColor2;
	}
}
