using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x02000094 RID: 148
	public class MissionMultiplayerHUDExtensionVM : ViewModel
	{
		// Token: 0x06000E63 RID: 3683 RVA: 0x0002C388 File Offset: 0x0002A588
		public MissionMultiplayerHUDExtensionVM(Mission mission)
		{
			this._mission = mission;
			this._missionScoreboardComponent = mission.GetMissionBehavior<MissionScoreboardComponent>();
			this._gameMode = this._mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this.SpectatorControls = new MissionMultiplayerSpectatorHUDVM(this._mission);
			if (this._gameMode.RoundComponent != null)
			{
				this._gameMode.RoundComponent.OnCurrentRoundStateChanged += this.OnCurrentGameModeStateChanged;
			}
			if (this._gameMode.WarmupComponent != null)
			{
				this._gameMode.WarmupComponent.OnWarmupEnded += this.OnCurrentGameModeStateChanged;
			}
			this._missionScoreboardComponent.OnRoundPropertiesChanged += this.SetTeamScoresDirty;
			MissionPeer.OnTeamChanged += this.OnTeamChanged;
			NetworkCommunicator.OnPeerComponentAdded += this.OnPeerComponentAdded;
			Mission.Current.OnMissionReset += this.OnMissionReset;
			MissionLobbyComponent missionBehavior = mission.GetMissionBehavior<MissionLobbyComponent>();
			this._isTeamsEnabled = missionBehavior.MissionType != MultiplayerGameType.Duel;
			this._missionLobbyEquipmentNetworkComponent = mission.GetMissionBehavior<MissionLobbyEquipmentNetworkComponent>();
			this.IsRoundCountdownAvailable = this._gameMode.IsGameModeUsingRoundCountdown;
			this.IsRoundCountdownSuspended = false;
			this._isTeamScoresEnabled = this._isTeamsEnabled;
			this.UpdateShowTeamScores();
			this.Teammates = new MBBindingList<MPPlayerVM>();
			this.Enemies = new MBBindingList<MPPlayerVM>();
			this._teammateDictionary = new Dictionary<MissionPeer, MPPlayerVM>();
			this._enemyDictionary = new Dictionary<MissionPeer, MPPlayerVM>();
			this.ShowHud = true;
			this.RefreshValues();
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0002C508 File Offset: 0x0002A708
		public override void RefreshValues()
		{
			base.RefreshValues();
			string strValue = MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			TextObject textObject = new TextObject("{=XJTX8w8M}Warmup Phase - {GAME_MODE}{newline}Waiting for players to join", null);
			textObject.SetTextVariable("GAME_MODE", GameTexts.FindText("str_multiplayer_official_game_type_name", strValue));
			this.WarmupInfoText = textObject.ToString();
			this.SpectatorControls.RefreshValues();
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0002C55E File Offset: 0x0002A75E
		private void OnMissionReset(object sender, PropertyChangedEventArgs e)
		{
			this.IsGeneralWarningCountdownActive = false;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0002C568 File Offset: 0x0002A768
		private void OnPeerComponentAdded(PeerComponent component)
		{
			if (component.IsMine && component is MissionRepresentativeBase)
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				MissionRepresentativeBase missionRepresentativeBase = ((myPeer != null) ? myPeer.VirtualPlayer.GetComponent<MissionRepresentativeBase>() : null);
				this.AllyTeamScore = this._missionScoreboardComponent.GetRoundScore(BattleSideEnum.Attacker);
				this.EnemyTeamScore = this._missionScoreboardComponent.GetRoundScore(BattleSideEnum.Defender);
				this._isTeammateAndEnemiesRelevant = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>().IsGameModeTactical && !Mission.Current.HasMissionBehavior<MissionMultiplayerSiegeClient>() && this._gameMode.GameType != MultiplayerGameType.Battle;
				this.CommanderInfo = new CommanderInfoVM(missionRepresentativeBase);
				this.ShowCommanderInfo = true;
				if (this._isTeammateAndEnemiesRelevant)
				{
					this.OnRefreshTeamMembers();
					this.OnRefreshEnemyMembers();
				}
				this.ShowPowerLevels = this._gameMode.GameType == MultiplayerGameType.Battle;
			}
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0002C638 File Offset: 0x0002A838
		public override void OnFinalize()
		{
			MissionPeer.OnTeamChanged -= this.OnTeamChanged;
			if (this._gameMode.RoundComponent != null)
			{
				this._gameMode.RoundComponent.OnCurrentRoundStateChanged -= this.OnCurrentGameModeStateChanged;
			}
			if (this._gameMode.WarmupComponent != null)
			{
				this._gameMode.WarmupComponent.OnWarmupEnded -= this.OnCurrentGameModeStateChanged;
			}
			this._missionScoreboardComponent.OnRoundPropertiesChanged -= this.SetTeamScoresDirty;
			NetworkCommunicator.OnPeerComponentAdded -= this.OnPeerComponentAdded;
			CommanderInfoVM commanderInfo = this.CommanderInfo;
			if (commanderInfo != null)
			{
				commanderInfo.OnFinalize();
			}
			this.CommanderInfo = null;
			MissionMultiplayerSpectatorHUDVM spectatorControls = this.SpectatorControls;
			if (spectatorControls != null)
			{
				spectatorControls.OnFinalize();
			}
			this.SpectatorControls = null;
			base.OnFinalize();
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0002C708 File Offset: 0x0002A908
		public void Tick(float dt)
		{
			this.IsInWarmup = this._gameMode.IsInWarmup;
			this.CheckTimers(false);
			if (this._isTeammateAndEnemiesRelevant)
			{
				this.OnRefreshTeamMembers();
				this.OnRefreshEnemyMembers();
			}
			if (this._isTeamScoresDirty)
			{
				this.UpdateTeamScores();
				this._isTeamScoresDirty = false;
			}
			CommanderInfoVM commanderInfo = this._commanderInfo;
			if (commanderInfo != null)
			{
				commanderInfo.Tick(dt);
			}
			MissionMultiplayerSpectatorHUDVM spectatorControls = this._spectatorControls;
			if (spectatorControls == null)
			{
				return;
			}
			spectatorControls.Tick(dt);
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0002C77C File Offset: 0x0002A97C
		private void CheckTimers(bool forceUpdate = false)
		{
			int num;
			int num2;
			if (this._gameMode.CheckTimer(out num, out num2, forceUpdate))
			{
				this.RemainingRoundTime = TimeSpan.FromSeconds((double)num).ToString("mm':'ss");
				this.WarnRemainingTime = (float)num <= 5f;
				if (this.GeneralWarningCountdown != num2)
				{
					this.IsGeneralWarningCountdownActive = num2 > 0;
					this.GeneralWarningCountdown = num2;
				}
			}
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0002C7E1 File Offset: 0x0002A9E1
		private void OnToggleLoadout(bool isActive)
		{
			this.ShowHud = !isActive;
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0002C7ED File Offset: 0x0002A9ED
		public void OnSpectatedAgentFocusIn(Agent followedAgent)
		{
			MissionMultiplayerSpectatorHUDVM spectatorControls = this._spectatorControls;
			if (spectatorControls == null)
			{
				return;
			}
			spectatorControls.OnSpectatedAgentFocusIn(followedAgent);
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0002C800 File Offset: 0x0002AA00
		public void OnSpectatedAgentFocusOut(Agent followedPeer)
		{
			MissionMultiplayerSpectatorHUDVM spectatorControls = this._spectatorControls;
			if (spectatorControls == null)
			{
				return;
			}
			spectatorControls.OnSpectatedAgentFocusOut(followedPeer);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0002C813 File Offset: 0x0002AA13
		private void OnCurrentGameModeStateChanged()
		{
			this.CheckTimers(true);
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0002C81C File Offset: 0x0002AA1C
		private void SetTeamScoresDirty()
		{
			this._isTeamScoresDirty = true;
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0002C828 File Offset: 0x0002AA28
		private void UpdateTeamScores()
		{
			if (this._isTeamScoresEnabled)
			{
				int roundScore = this._missionScoreboardComponent.GetRoundScore(BattleSideEnum.Attacker);
				int roundScore2 = this._missionScoreboardComponent.GetRoundScore(BattleSideEnum.Defender);
				this.AllyTeamScore = (this._isAttackerTeamAlly ? roundScore : roundScore2);
				this.EnemyTeamScore = (this._isAttackerTeamAlly ? roundScore2 : roundScore);
			}
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x0002C87C File Offset: 0x0002AA7C
		private void UpdateTeamBanners()
		{
			Team attackerTeam = Mission.Current.AttackerTeam;
			BannerImageIdentifierVM bannerImageIdentifierVM = new BannerImageIdentifierVM((attackerTeam != null) ? attackerTeam.Banner : null, true);
			Team defenderTeam = Mission.Current.DefenderTeam;
			BannerImageIdentifierVM bannerImageIdentifierVM2 = new BannerImageIdentifierVM((defenderTeam != null) ? defenderTeam.Banner : null, true);
			this.AllyBanner = (this._isAttackerTeamAlly ? bannerImageIdentifierVM : bannerImageIdentifierVM2);
			this.EnemyBanner = (this._isAttackerTeamAlly ? bannerImageIdentifierVM2 : bannerImageIdentifierVM);
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0002C8E8 File Offset: 0x0002AAE8
		private void OnTeamChanged(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (peer.IsMine)
			{
				if (this._isTeamScoresEnabled || this._gameMode.GameType == MultiplayerGameType.Battle)
				{
					this._isAttackerTeamAlly = newTeam.Side == BattleSideEnum.Attacker;
					this.SetTeamScoresDirty();
				}
				CommanderInfoVM commanderInfo = this.CommanderInfo;
				if (commanderInfo != null)
				{
					commanderInfo.OnTeamChanged();
				}
			}
			if (this.CommanderInfo == null)
			{
				return;
			}
			MPPlayerVM mpplayerVM = this.Teammates.SingleOrDefault<MPPlayerVM>((MPPlayerVM x) => x.Peer.GetNetworkPeer() == peer);
			if (mpplayerVM != null)
			{
				mpplayerVM.RefreshTeam();
			}
			string text;
			string text2;
			this.GetTeamColors(Mission.Current.AttackerTeam, out text, out text2);
			if (this._isTeamScoresEnabled || this._gameMode.GameType == MultiplayerGameType.Battle)
			{
				string text3;
				string text4;
				this.GetTeamColors(Mission.Current.DefenderTeam, out text3, out text4);
				if (this._isAttackerTeamAlly)
				{
					this.AllyTeamColor = text;
					this.AllyTeamColor2 = text2;
					this.EnemyTeamColor = text3;
					this.EnemyTeamColor2 = text4;
				}
				else
				{
					this.AllyTeamColor = text3;
					this.AllyTeamColor2 = text4;
					this.EnemyTeamColor = text;
					this.EnemyTeamColor2 = text2;
				}
				this.CommanderInfo.RefreshColors(this.AllyTeamColor, this.AllyTeamColor2, this.EnemyTeamColor, this.EnemyTeamColor2);
			}
			else
			{
				this.AllyTeamColor = text;
				this.AllyTeamColor2 = text2;
				this.CommanderInfo.RefreshColors(this.AllyTeamColor, this.AllyTeamColor2, this.EnemyTeamColor, this.EnemyTeamColor2);
			}
			this.UpdateTeamBanners();
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0002CA54 File Offset: 0x0002AC54
		private void GetTeamColors(Team team, out string color, out string color2)
		{
			color = team.Color.ToString("X");
			color = color.Remove(0, 2);
			color = "#" + color + "FF";
			color2 = team.Color2.ToString("X");
			color2 = color2.Remove(0, 2);
			color2 = "#" + color2 + "FF";
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0002CAC8 File Offset: 0x0002ACC8
		private void OnRefreshTeamMembers()
		{
			List<MPPlayerVM> list = this.Teammates.ToList<MPPlayerVM>();
			foreach (MissionPeer missionPeer in VirtualPlayer.Peers<MissionPeer>())
			{
				if (missionPeer.GetNetworkPeer().GetComponent<MissionPeer>() != null && this._playerTeam != null && missionPeer.Team != null && missionPeer.Team == this._playerTeam && missionPeer.Team != Mission.Current.SpectatorTeam)
				{
					MPPlayerVM mpplayerVM;
					if (this._teammateDictionary.TryGetValue(missionPeer, out mpplayerVM))
					{
						list.Remove(mpplayerVM);
					}
					else
					{
						MPPlayerVM mpplayerVM2 = new MPPlayerVM(missionPeer);
						this.Teammates.Add(mpplayerVM2);
						this._teammateDictionary.Add(missionPeer, mpplayerVM2);
					}
				}
			}
			foreach (MPPlayerVM mpplayerVM3 in list)
			{
				this.Teammates.Remove(mpplayerVM3);
				this._teammateDictionary.Remove(mpplayerVM3.Peer);
			}
			foreach (MPPlayerVM mpplayerVM4 in this.Teammates)
			{
				mpplayerVM4.RefreshDivision(false);
				mpplayerVM4.RefreshGold();
				mpplayerVM4.RefreshProperties();
				mpplayerVM4.UpdateDisabled();
			}
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x0002CC4C File Offset: 0x0002AE4C
		private void OnRefreshEnemyMembers()
		{
			List<MPPlayerVM> list = this.Enemies.ToList<MPPlayerVM>();
			foreach (MissionPeer missionPeer in VirtualPlayer.Peers<MissionPeer>())
			{
				if (missionPeer.GetNetworkPeer().GetComponent<MissionPeer>() != null && this._playerTeam != null && missionPeer.Team != null && missionPeer.Team != this._playerTeam && missionPeer.Team != Mission.Current.SpectatorTeam)
				{
					MPPlayerVM mpplayerVM;
					if (this._enemyDictionary.TryGetValue(missionPeer, out mpplayerVM))
					{
						list.Remove(mpplayerVM);
					}
					else
					{
						MPPlayerVM mpplayerVM2 = new MPPlayerVM(missionPeer);
						this.Enemies.Add(mpplayerVM2);
						this._enemyDictionary.Add(missionPeer, mpplayerVM2);
					}
				}
			}
			foreach (MPPlayerVM mpplayerVM3 in list)
			{
				this.Enemies.Remove(mpplayerVM3);
				this._enemyDictionary.Remove(mpplayerVM3.Peer);
			}
			foreach (MPPlayerVM mpplayerVM4 in this.Enemies)
			{
				mpplayerVM4.RefreshDivision(false);
				mpplayerVM4.UpdateDisabled();
			}
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0002CDC4 File Offset: 0x0002AFC4
		private void UpdateShowTeamScores()
		{
			this.ShowTeamScores = !this._gameMode.IsInWarmup && this.ShowCommanderInfo && this._gameMode.GameType != MultiplayerGameType.Siege;
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0002CDF8 File Offset: 0x0002AFF8
		private Team _playerTeam
		{
			get
			{
				if (!GameNetwork.IsMyPeerReady)
				{
					return null;
				}
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (component == null)
				{
					return null;
				}
				if (component == null)
				{
					return null;
				}
				if (component.Team == null || component.Team.Side == BattleSideEnum.None)
				{
					return null;
				}
				return component.Team;
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0002CE41 File Offset: 0x0002B041
		// (set) Token: 0x06000E78 RID: 3704 RVA: 0x0002CE49 File Offset: 0x0002B049
		[DataSourceProperty]
		public bool IsOrderActive
		{
			get
			{
				return this._isOrderActive;
			}
			set
			{
				if (value != this._isOrderActive)
				{
					this._isOrderActive = value;
					base.OnPropertyChangedWithValue(value, "IsOrderActive");
				}
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06000E79 RID: 3705 RVA: 0x0002CE67 File Offset: 0x0002B067
		// (set) Token: 0x06000E7A RID: 3706 RVA: 0x0002CE6F File Offset: 0x0002B06F
		[DataSourceProperty]
		public CommanderInfoVM CommanderInfo
		{
			get
			{
				return this._commanderInfo;
			}
			set
			{
				if (value != this._commanderInfo)
				{
					this._commanderInfo = value;
					base.OnPropertyChangedWithValue<CommanderInfoVM>(value, "CommanderInfo");
				}
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x0002CE8D File Offset: 0x0002B08D
		// (set) Token: 0x06000E7C RID: 3708 RVA: 0x0002CE95 File Offset: 0x0002B095
		[DataSourceProperty]
		public MissionMultiplayerSpectatorHUDVM SpectatorControls
		{
			get
			{
				return this._spectatorControls;
			}
			set
			{
				if (value != this._spectatorControls)
				{
					this._spectatorControls = value;
					base.OnPropertyChangedWithValue<MissionMultiplayerSpectatorHUDVM>(value, "SpectatorControls");
				}
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x0002CEB3 File Offset: 0x0002B0B3
		// (set) Token: 0x06000E7E RID: 3710 RVA: 0x0002CEBB File Offset: 0x0002B0BB
		[DataSourceProperty]
		public MBBindingList<MPPlayerVM> Teammates
		{
			get
			{
				return this._teammatesList;
			}
			set
			{
				if (value != this._teammatesList)
				{
					this._teammatesList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPlayerVM>>(value, "Teammates");
				}
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06000E7F RID: 3711 RVA: 0x0002CED9 File Offset: 0x0002B0D9
		// (set) Token: 0x06000E80 RID: 3712 RVA: 0x0002CEE1 File Offset: 0x0002B0E1
		[DataSourceProperty]
		public MBBindingList<MPPlayerVM> Enemies
		{
			get
			{
				return this._enemiesList;
			}
			set
			{
				if (value != this._enemiesList)
				{
					this._enemiesList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPlayerVM>>(value, "Enemies");
				}
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x0002CEFF File Offset: 0x0002B0FF
		// (set) Token: 0x06000E82 RID: 3714 RVA: 0x0002CF07 File Offset: 0x0002B107
		[DataSourceProperty]
		public BannerImageIdentifierVM AllyBanner
		{
			get
			{
				return this._defenderBanner;
			}
			set
			{
				if (value != this._defenderBanner)
				{
					this._defenderBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "AllyBanner");
				}
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x0002CF25 File Offset: 0x0002B125
		// (set) Token: 0x06000E84 RID: 3716 RVA: 0x0002CF2D File Offset: 0x0002B12D
		[DataSourceProperty]
		public BannerImageIdentifierVM EnemyBanner
		{
			get
			{
				return this._attackerBanner;
			}
			set
			{
				if (value != this._attackerBanner)
				{
					this._attackerBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "EnemyBanner");
				}
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0002CF4B File Offset: 0x0002B14B
		// (set) Token: 0x06000E86 RID: 3718 RVA: 0x0002CF53 File Offset: 0x0002B153
		[DataSourceProperty]
		public bool IsRoundCountdownAvailable
		{
			get
			{
				return this._isRoundCountdownAvailable;
			}
			set
			{
				if (value != this._isRoundCountdownAvailable)
				{
					this._isRoundCountdownAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsRoundCountdownAvailable");
				}
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0002CF71 File Offset: 0x0002B171
		// (set) Token: 0x06000E88 RID: 3720 RVA: 0x0002CF79 File Offset: 0x0002B179
		[DataSourceProperty]
		public bool IsRoundCountdownSuspended
		{
			get
			{
				return this._isRoundCountdownSuspended;
			}
			set
			{
				if (value != this._isRoundCountdownSuspended)
				{
					this._isRoundCountdownSuspended = value;
					base.OnPropertyChangedWithValue(value, "IsRoundCountdownSuspended");
				}
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0002CF97 File Offset: 0x0002B197
		// (set) Token: 0x06000E8A RID: 3722 RVA: 0x0002CF9F File Offset: 0x0002B19F
		[DataSourceProperty]
		public bool ShowTeamScores
		{
			get
			{
				return this._showTeamScores;
			}
			set
			{
				if (value != this._showTeamScores)
				{
					this._showTeamScores = value;
					base.OnPropertyChangedWithValue(value, "ShowTeamScores");
				}
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0002CFBD File Offset: 0x0002B1BD
		// (set) Token: 0x06000E8C RID: 3724 RVA: 0x0002CFC5 File Offset: 0x0002B1C5
		[DataSourceProperty]
		public string RemainingRoundTime
		{
			get
			{
				return this._remainingRoundTime;
			}
			set
			{
				if (value != this._remainingRoundTime)
				{
					this._remainingRoundTime = value;
					base.OnPropertyChangedWithValue<string>(value, "RemainingRoundTime");
				}
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06000E8D RID: 3725 RVA: 0x0002CFE8 File Offset: 0x0002B1E8
		// (set) Token: 0x06000E8E RID: 3726 RVA: 0x0002CFF0 File Offset: 0x0002B1F0
		[DataSourceProperty]
		public bool WarnRemainingTime
		{
			get
			{
				return this._warnRemainingTime;
			}
			set
			{
				if (value != this._warnRemainingTime)
				{
					this._warnRemainingTime = value;
					base.OnPropertyChangedWithValue(value, "WarnRemainingTime");
				}
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06000E8F RID: 3727 RVA: 0x0002D00E File Offset: 0x0002B20E
		// (set) Token: 0x06000E90 RID: 3728 RVA: 0x0002D016 File Offset: 0x0002B216
		[DataSourceProperty]
		public int AllyTeamScore
		{
			get
			{
				return this._allyTeamScore;
			}
			set
			{
				if (value != this._allyTeamScore)
				{
					this._allyTeamScore = value;
					base.OnPropertyChangedWithValue(value, "AllyTeamScore");
				}
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x0002D034 File Offset: 0x0002B234
		// (set) Token: 0x06000E92 RID: 3730 RVA: 0x0002D03C File Offset: 0x0002B23C
		[DataSourceProperty]
		public int EnemyTeamScore
		{
			get
			{
				return this._enemyTeamScore;
			}
			set
			{
				if (value != this._enemyTeamScore)
				{
					this._enemyTeamScore = value;
					base.OnPropertyChangedWithValue(value, "EnemyTeamScore");
				}
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06000E93 RID: 3731 RVA: 0x0002D05A File Offset: 0x0002B25A
		// (set) Token: 0x06000E94 RID: 3732 RVA: 0x0002D062 File Offset: 0x0002B262
		[DataSourceProperty]
		public string AllyTeamColor
		{
			get
			{
				return this._allyTeamColor;
			}
			set
			{
				if (value != this._allyTeamColor)
				{
					this._allyTeamColor = value;
					base.OnPropertyChangedWithValue<string>(value, "AllyTeamColor");
				}
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06000E95 RID: 3733 RVA: 0x0002D085 File Offset: 0x0002B285
		// (set) Token: 0x06000E96 RID: 3734 RVA: 0x0002D08D File Offset: 0x0002B28D
		[DataSourceProperty]
		public string AllyTeamColor2
		{
			get
			{
				return this._allyTeamColor2;
			}
			set
			{
				if (value != this._allyTeamColor2)
				{
					this._allyTeamColor2 = value;
					base.OnPropertyChangedWithValue<string>(value, "AllyTeamColor2");
				}
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x0002D0B0 File Offset: 0x0002B2B0
		// (set) Token: 0x06000E98 RID: 3736 RVA: 0x0002D0B8 File Offset: 0x0002B2B8
		[DataSourceProperty]
		public string EnemyTeamColor
		{
			get
			{
				return this._enemyTeamColor;
			}
			set
			{
				if (value != this._enemyTeamColor)
				{
					this._enemyTeamColor = value;
					base.OnPropertyChangedWithValue<string>(value, "EnemyTeamColor");
				}
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06000E99 RID: 3737 RVA: 0x0002D0DB File Offset: 0x0002B2DB
		// (set) Token: 0x06000E9A RID: 3738 RVA: 0x0002D0E3 File Offset: 0x0002B2E3
		[DataSourceProperty]
		public string EnemyTeamColor2
		{
			get
			{
				return this._enemyTeamColor2;
			}
			set
			{
				if (value != this._enemyTeamColor2)
				{
					this._enemyTeamColor2 = value;
					base.OnPropertyChangedWithValue<string>(value, "EnemyTeamColor2");
				}
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x0002D106 File Offset: 0x0002B306
		// (set) Token: 0x06000E9C RID: 3740 RVA: 0x0002D10E File Offset: 0x0002B30E
		[DataSourceProperty]
		public bool ShowHud
		{
			get
			{
				return this._showHUD;
			}
			set
			{
				if (value != this._showHUD)
				{
					this._showHUD = value;
					base.OnPropertyChangedWithValue(value, "ShowHud");
				}
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06000E9D RID: 3741 RVA: 0x0002D12C File Offset: 0x0002B32C
		// (set) Token: 0x06000E9E RID: 3742 RVA: 0x0002D134 File Offset: 0x0002B334
		[DataSourceProperty]
		public bool ShowCommanderInfo
		{
			get
			{
				return this._showCommanderInfo;
			}
			set
			{
				if (value != this._showCommanderInfo)
				{
					this._showCommanderInfo = value;
					base.OnPropertyChangedWithValue(value, "ShowCommanderInfo");
					this.UpdateShowTeamScores();
				}
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06000E9F RID: 3743 RVA: 0x0002D158 File Offset: 0x0002B358
		// (set) Token: 0x06000EA0 RID: 3744 RVA: 0x0002D160 File Offset: 0x0002B360
		[DataSourceProperty]
		public bool ShowPowerLevels
		{
			get
			{
				return this._showPowerLevels;
			}
			set
			{
				if (value != this._showPowerLevels)
				{
					this._showPowerLevels = value;
					base.OnPropertyChangedWithValue(value, "ShowPowerLevels");
				}
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x0002D17E File Offset: 0x0002B37E
		// (set) Token: 0x06000EA2 RID: 3746 RVA: 0x0002D186 File Offset: 0x0002B386
		[DataSourceProperty]
		public bool IsInWarmup
		{
			get
			{
				return this._isInWarmup;
			}
			set
			{
				if (value != this._isInWarmup)
				{
					this._isInWarmup = value;
					base.OnPropertyChangedWithValue(value, "IsInWarmup");
					this.UpdateShowTeamScores();
					CommanderInfoVM commanderInfo = this.CommanderInfo;
					if (commanderInfo == null)
					{
						return;
					}
					commanderInfo.UpdateWarmupDependentFlags(this._isInWarmup);
				}
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x0002D1C0 File Offset: 0x0002B3C0
		// (set) Token: 0x06000EA4 RID: 3748 RVA: 0x0002D1C8 File Offset: 0x0002B3C8
		[DataSourceProperty]
		public string WarmupInfoText
		{
			get
			{
				return this._warmupInfoText;
			}
			set
			{
				if (value != this._warmupInfoText)
				{
					this._warmupInfoText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarmupInfoText");
				}
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x0002D1EB File Offset: 0x0002B3EB
		// (set) Token: 0x06000EA6 RID: 3750 RVA: 0x0002D1F3 File Offset: 0x0002B3F3
		[DataSourceProperty]
		public int GeneralWarningCountdown
		{
			get
			{
				return this._generalWarningCountdown;
			}
			set
			{
				if (value != this._generalWarningCountdown)
				{
					this._generalWarningCountdown = value;
					base.OnPropertyChangedWithValue(value, "GeneralWarningCountdown");
				}
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x0002D211 File Offset: 0x0002B411
		// (set) Token: 0x06000EA8 RID: 3752 RVA: 0x0002D219 File Offset: 0x0002B419
		[DataSourceProperty]
		public bool IsGeneralWarningCountdownActive
		{
			get
			{
				return this._isGeneralWarningCountdownActive;
			}
			set
			{
				if (value != this._isGeneralWarningCountdownActive)
				{
					this._isGeneralWarningCountdownActive = value;
					base.OnPropertyChangedWithValue(value, "IsGeneralWarningCountdownActive");
				}
			}
		}

		// Token: 0x04000699 RID: 1689
		private const float RemainingTimeWarningThreshold = 5f;

		// Token: 0x0400069A RID: 1690
		private readonly Mission _mission;

		// Token: 0x0400069B RID: 1691
		private readonly Dictionary<MissionPeer, MPPlayerVM> _teammateDictionary;

		// Token: 0x0400069C RID: 1692
		private readonly Dictionary<MissionPeer, MPPlayerVM> _enemyDictionary;

		// Token: 0x0400069D RID: 1693
		private readonly MissionScoreboardComponent _missionScoreboardComponent;

		// Token: 0x0400069E RID: 1694
		private readonly MissionLobbyEquipmentNetworkComponent _missionLobbyEquipmentNetworkComponent;

		// Token: 0x0400069F RID: 1695
		private readonly MissionMultiplayerGameModeBaseClient _gameMode;

		// Token: 0x040006A0 RID: 1696
		private readonly bool _isTeamsEnabled;

		// Token: 0x040006A1 RID: 1697
		private bool _isAttackerTeamAlly;

		// Token: 0x040006A2 RID: 1698
		private bool _isTeammateAndEnemiesRelevant;

		// Token: 0x040006A3 RID: 1699
		private bool _isTeamScoresEnabled;

		// Token: 0x040006A4 RID: 1700
		private bool _isTeamScoresDirty;

		// Token: 0x040006A5 RID: 1701
		private bool _isOrderActive;

		// Token: 0x040006A6 RID: 1702
		private CommanderInfoVM _commanderInfo;

		// Token: 0x040006A7 RID: 1703
		private MissionMultiplayerSpectatorHUDVM _spectatorControls;

		// Token: 0x040006A8 RID: 1704
		private bool _warnRemainingTime;

		// Token: 0x040006A9 RID: 1705
		private bool _isRoundCountdownAvailable;

		// Token: 0x040006AA RID: 1706
		private bool _isRoundCountdownSuspended;

		// Token: 0x040006AB RID: 1707
		private bool _showTeamScores;

		// Token: 0x040006AC RID: 1708
		private string _remainingRoundTime;

		// Token: 0x040006AD RID: 1709
		private string _allyTeamColor;

		// Token: 0x040006AE RID: 1710
		private string _allyTeamColor2;

		// Token: 0x040006AF RID: 1711
		private string _enemyTeamColor;

		// Token: 0x040006B0 RID: 1712
		private string _enemyTeamColor2;

		// Token: 0x040006B1 RID: 1713
		private string _warmupInfoText;

		// Token: 0x040006B2 RID: 1714
		private int _allyTeamScore = -1;

		// Token: 0x040006B3 RID: 1715
		private int _enemyTeamScore = -1;

		// Token: 0x040006B4 RID: 1716
		private MBBindingList<MPPlayerVM> _teammatesList;

		// Token: 0x040006B5 RID: 1717
		private MBBindingList<MPPlayerVM> _enemiesList;

		// Token: 0x040006B6 RID: 1718
		private bool _showHUD;

		// Token: 0x040006B7 RID: 1719
		private bool _showCommanderInfo;

		// Token: 0x040006B8 RID: 1720
		private bool _showPowerLevels;

		// Token: 0x040006B9 RID: 1721
		private bool _isInWarmup;

		// Token: 0x040006BA RID: 1722
		private int _generalWarningCountdown;

		// Token: 0x040006BB RID: 1723
		private bool _isGeneralWarningCountdownActive;

		// Token: 0x040006BC RID: 1724
		private BannerImageIdentifierVM _defenderBanner;

		// Token: 0x040006BD RID: 1725
		private BannerImageIdentifierVM _attackerBanner;
	}
}
