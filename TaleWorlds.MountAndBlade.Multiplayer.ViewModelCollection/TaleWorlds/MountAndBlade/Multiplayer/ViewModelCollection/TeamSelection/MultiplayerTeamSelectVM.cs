using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.TeamSelection
{
	// Token: 0x02000019 RID: 25
	public class MultiplayerTeamSelectVM : ViewModel
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000166 RID: 358 RVA: 0x000065F4 File Offset: 0x000047F4
		private MissionRepresentativeBase missionRep
		{
			get
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				if (myPeer == null)
				{
					return null;
				}
				VirtualPlayer virtualPlayer = myPeer.VirtualPlayer;
				if (virtualPlayer == null)
				{
					return null;
				}
				return virtualPlayer.GetComponent<MissionRepresentativeBase>();
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006614 File Offset: 0x00004814
		public MultiplayerTeamSelectVM(Mission mission, Action<Team> onChangeTeamTo, Action onAutoAssign, Action onClose, IEnumerable<Team> teams, string gamemode)
		{
			this._onClose = onClose;
			this._onAutoAssign = onAutoAssign;
			this._gamemodeStr = gamemode;
			Debug.Print("MultiplayerTeamSelectVM 1", 0, Debug.DebugColor.White, 17179869184UL);
			this._gameMode = mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			MissionScoreboardComponent missionBehavior = mission.GetMissionBehavior<MissionScoreboardComponent>();
			Debug.Print("MultiplayerTeamSelectVM 2", 0, Debug.DebugColor.White, 17179869184UL);
			this.IsRoundCountdownAvailable = this._gameMode.IsGameModeUsingRoundCountdown;
			Debug.Print("MultiplayerTeamSelectVM 3", 0, Debug.DebugColor.White, 17179869184UL);
			Team team = teams.FirstOrDefault<Team>((Team t) => t.Side == BattleSideEnum.None);
			this.TeamSpectators = new TeamSelectTeamInstanceVM(missionBehavior, team, null, null, onChangeTeamTo, new MultiplayerBattleColors.MultiplayerCultureColorInfo(null, false));
			Debug.Print("MultiplayerTeamSelectVM 4", 0, Debug.DebugColor.White, 17179869184UL);
			Team team2 = teams.FirstOrDefault<Team>((Team t) => t.Side == BattleSideEnum.Attacker);
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			Debug.Print("MultiplayerTeamSelectVM 5", 0, Debug.DebugColor.White, 17179869184UL);
			Team team3 = teams.FirstOrDefault<Team>((Team t) => t.Side == BattleSideEnum.Defender);
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			this.Team1 = new TeamSelectTeamInstanceVM(missionBehavior, team2, @object, team2.Banner, onChangeTeamTo, multiplayerBattleColors.AttackerColors);
			this.Team2 = new TeamSelectTeamInstanceVM(missionBehavior, team3, object2, team3.Banner, onChangeTeamTo, multiplayerBattleColors.DefenderColors);
			Debug.Print("MultiplayerTeamSelectVM 6", 0, Debug.DebugColor.White, 17179869184UL);
			if (GameNetwork.IsMyPeerReady)
			{
				this._missionPeer = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				this.IsCancelDisabled = this._missionPeer.Team == null;
			}
			Debug.Print("MultiplayerTeamSelectVM 7", 0, Debug.DebugColor.White, 17179869184UL);
			this.RefreshValues();
			Debug.Print("MultiplayerTeamSelectVM 8", 0, Debug.DebugColor.White, 17179869184UL);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000683C File Offset: 0x00004A3C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.AutoassignLbl = new TextObject("{=bON4Kn6B}Auto Assign", null).ToString();
			this.TeamSelectTitle = new TextObject("{=aVixswW5}Team Selection", null).ToString();
			this.GamemodeLbl = GameTexts.FindText("str_multiplayer_official_game_type_name", this._gamemodeStr).ToString();
			this.Team1.RefreshValues();
			this.Team2.RefreshValues();
			this.TeamSpectators.RefreshValues();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000068B8 File Offset: 0x00004AB8
		public void Tick(float dt)
		{
			this.RemainingRoundTime = TimeSpan.FromSeconds((double)MathF.Ceiling(this._gameMode.RemainingTime)).ToString("mm':'ss");
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000068F0 File Offset: 0x00004AF0
		public void RefreshDisabledTeams(List<Team> disabledTeams)
		{
			if (disabledTeams == null)
			{
				TeamSelectTeamInstanceVM teamSpectators = this.TeamSpectators;
				if (teamSpectators != null)
				{
					teamSpectators.SetIsDisabled(false, false);
				}
				TeamSelectTeamInstanceVM team = this.Team1;
				if (team != null)
				{
					team.SetIsDisabled(false, false);
				}
				TeamSelectTeamInstanceVM team2 = this.Team2;
				if (team2 == null)
				{
					return;
				}
				team2.SetIsDisabled(false, false);
				return;
			}
			else
			{
				TeamSelectTeamInstanceVM teamSpectators2 = this.TeamSpectators;
				if (teamSpectators2 != null)
				{
					bool flag = false;
					bool flag2;
					if (disabledTeams == null)
					{
						flag2 = false;
					}
					else
					{
						TeamSelectTeamInstanceVM teamSpectators3 = this.TeamSpectators;
						flag2 = disabledTeams.Contains((teamSpectators3 != null) ? teamSpectators3.Team : null);
					}
					teamSpectators2.SetIsDisabled(flag, flag2);
				}
				TeamSelectTeamInstanceVM team3 = this.Team1;
				if (team3 != null)
				{
					TeamSelectTeamInstanceVM team4 = this.Team1;
					Team team5 = ((team4 != null) ? team4.Team : null);
					MissionPeer missionPeer = this._missionPeer;
					bool flag3 = team5 == ((missionPeer != null) ? missionPeer.Team : null);
					bool flag4;
					if (disabledTeams == null)
					{
						flag4 = false;
					}
					else
					{
						TeamSelectTeamInstanceVM team6 = this.Team1;
						flag4 = disabledTeams.Contains((team6 != null) ? team6.Team : null);
					}
					team3.SetIsDisabled(flag3, flag4);
				}
				TeamSelectTeamInstanceVM team7 = this.Team2;
				if (team7 == null)
				{
					return;
				}
				TeamSelectTeamInstanceVM team8 = this.Team2;
				Team team9 = ((team8 != null) ? team8.Team : null);
				MissionPeer missionPeer2 = this._missionPeer;
				bool flag5 = team9 == ((missionPeer2 != null) ? missionPeer2.Team : null);
				bool flag6;
				if (disabledTeams == null)
				{
					flag6 = false;
				}
				else
				{
					TeamSelectTeamInstanceVM team10 = this.Team2;
					flag6 = disabledTeams.Contains((team10 != null) ? team10.Team : null);
				}
				team7.SetIsDisabled(flag5, flag6);
				return;
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006A14 File Offset: 0x00004C14
		public void RefreshPlayerAndBotCount(int playersCountOne, int playersCountTwo, int botsCountOne, int botsCountTwo)
		{
			MBTextManager.SetTextVariable("PLAYER_COUNT", playersCountOne.ToString(), false);
			this.Team1.DisplayedSecondary = new TextObject("{=Etjqamlh}{PLAYER_COUNT} Players", null).ToString();
			MBTextManager.SetTextVariable("BOT_COUNT", botsCountOne.ToString(), false);
			this.Team1.DisplayedSecondarySub = new TextObject("{=eCOJSSUH}({BOT_COUNT} Bots)", null).ToString();
			MBTextManager.SetTextVariable("PLAYER_COUNT", playersCountTwo.ToString(), false);
			this.Team2.DisplayedSecondary = new TextObject("{=Etjqamlh}{PLAYER_COUNT} Players", null).ToString();
			MBTextManager.SetTextVariable("BOT_COUNT", botsCountTwo.ToString(), false);
			this.Team2.DisplayedSecondarySub = new TextObject("{=eCOJSSUH}({BOT_COUNT} Bots)", null).ToString();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006AD5 File Offset: 0x00004CD5
		public void RefreshFriendsPerTeam(IEnumerable<MissionPeer> friendsTeamOne, IEnumerable<MissionPeer> friendsTeamTwo)
		{
			this.Team1.RefreshFriends(friendsTeamOne);
			this.Team2.RefreshFriends(friendsTeamTwo);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00006AEF File Offset: 0x00004CEF
		[UsedImplicitly]
		public void ExecuteCancel()
		{
			this._onClose();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00006AFC File Offset: 0x00004CFC
		[UsedImplicitly]
		public void ExecuteAutoAssign()
		{
			this._onAutoAssign();
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00006B09 File Offset: 0x00004D09
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00006B11 File Offset: 0x00004D11
		[DataSourceProperty]
		public TeamSelectTeamInstanceVM Team1
		{
			get
			{
				return this._team1;
			}
			set
			{
				if (value != this._team1)
				{
					this._team1 = value;
					base.OnPropertyChangedWithValue<TeamSelectTeamInstanceVM>(value, "Team1");
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00006B2F File Offset: 0x00004D2F
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00006B37 File Offset: 0x00004D37
		[DataSourceProperty]
		public TeamSelectTeamInstanceVM Team2
		{
			get
			{
				return this._team2;
			}
			set
			{
				if (value != this._team2)
				{
					this._team2 = value;
					base.OnPropertyChangedWithValue<TeamSelectTeamInstanceVM>(value, "Team2");
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00006B55 File Offset: 0x00004D55
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00006B5D File Offset: 0x00004D5D
		[DataSourceProperty]
		public TeamSelectTeamInstanceVM TeamSpectators
		{
			get
			{
				return this._teamSpectators;
			}
			set
			{
				if (value != this._teamSpectators)
				{
					this._teamSpectators = value;
					base.OnPropertyChangedWithValue<TeamSelectTeamInstanceVM>(value, "TeamSpectators");
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00006B7B File Offset: 0x00004D7B
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00006B83 File Offset: 0x00004D83
		[DataSourceProperty]
		public string TeamSelectTitle
		{
			get
			{
				return this._teamSelectTitle;
			}
			set
			{
				this._teamSelectTitle = value;
				base.OnPropertyChangedWithValue<string>(value, "TeamSelectTitle");
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00006B98 File Offset: 0x00004D98
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00006BA0 File Offset: 0x00004DA0
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

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00006BBE File Offset: 0x00004DBE
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00006BC6 File Offset: 0x00004DC6
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

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00006BE9 File Offset: 0x00004DE9
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00006BF1 File Offset: 0x00004DF1
		[DataSourceProperty]
		public string GamemodeLbl
		{
			get
			{
				return this._gamemodeLbl;
			}
			set
			{
				this._gamemodeLbl = value;
				base.OnPropertyChangedWithValue<string>(value, "GamemodeLbl");
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00006C06 File Offset: 0x00004E06
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00006C0E File Offset: 0x00004E0E
		[DataSourceProperty]
		public string AutoassignLbl
		{
			get
			{
				return this._autoassignLbl;
			}
			set
			{
				this._autoassignLbl = value;
				base.OnPropertyChangedWithValue<string>(value, "AutoassignLbl");
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00006C23 File Offset: 0x00004E23
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00006C2B File Offset: 0x00004E2B
		[DataSourceProperty]
		public bool IsCancelDisabled
		{
			get
			{
				return this._isCancelDisabled;
			}
			set
			{
				this._isCancelDisabled = value;
				base.OnPropertyChangedWithValue(value, "IsCancelDisabled");
			}
		}

		// Token: 0x040000BF RID: 191
		private readonly Action _onClose;

		// Token: 0x040000C0 RID: 192
		private readonly Action _onAutoAssign;

		// Token: 0x040000C1 RID: 193
		private readonly MissionMultiplayerGameModeBaseClient _gameMode;

		// Token: 0x040000C2 RID: 194
		private readonly MissionPeer _missionPeer;

		// Token: 0x040000C3 RID: 195
		private readonly string _gamemodeStr;

		// Token: 0x040000C4 RID: 196
		private string _teamSelectTitle;

		// Token: 0x040000C5 RID: 197
		private bool _isRoundCountdownAvailable;

		// Token: 0x040000C6 RID: 198
		private string _remainingRoundTime;

		// Token: 0x040000C7 RID: 199
		private string _gamemodeLbl;

		// Token: 0x040000C8 RID: 200
		private string _autoassignLbl;

		// Token: 0x040000C9 RID: 201
		private bool _isCancelDisabled;

		// Token: 0x040000CA RID: 202
		private TeamSelectTeamInstanceVM _team1;

		// Token: 0x040000CB RID: 203
		private TeamSelectTeamInstanceVM _team2;

		// Token: 0x040000CC RID: 204
		private TeamSelectTeamInstanceVM _teamSpectators;
	}
}
