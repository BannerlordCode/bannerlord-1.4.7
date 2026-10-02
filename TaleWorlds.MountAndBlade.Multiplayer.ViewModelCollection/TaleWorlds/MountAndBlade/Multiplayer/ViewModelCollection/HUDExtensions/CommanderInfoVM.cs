using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x02000092 RID: 146
	public class CommanderInfoVM : ViewModel
	{
		// Token: 0x06000E0A RID: 3594 RVA: 0x0002B208 File Offset: 0x00029408
		public CommanderInfoVM(MissionRepresentativeBase missionRepresentative)
		{
			this._missionRepresentative = missionRepresentative;
			this.AllyControlPoints = new MBBindingList<CapturePointVM>();
			this.NeutralControlPoints = new MBBindingList<CapturePointVM>();
			this.EnemyControlPoints = new MBBindingList<CapturePointVM>();
			this._gameMode = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this._missionScoreboardComponent = Mission.Current.GetMissionBehavior<MissionScoreboardComponent>();
			this._commanderInfo = Mission.Current.GetMissionBehavior<ICommanderInfo>();
			this.ShowTacticalInfo = true;
			if (this._gameMode != null)
			{
				this.UpdateWarmupDependentFlags(this._gameMode.IsInWarmup);
				this.UsePowerComparer = this._gameMode.GameType == MultiplayerGameType.Battle && this._gameMode.ScoreboardComponent != null;
				if (this.UsePowerComparer)
				{
					this.PowerLevelComparer = new PowerLevelComparer(1.0, 1.0);
				}
				if (this.UseMoraleComparer)
				{
					this.RegisterMoraleEvents();
				}
			}
			this._siegeClient = Mission.Current.GetMissionBehavior<MissionMultiplayerSiegeClient>();
			if (this._siegeClient != null)
			{
				this._siegeClient.OnCapturePointRemainingMoraleGainsChangedEvent += this.OnCapturePointRemainingMoraleGainsChanged;
			}
			Mission.Current.OnMissionReset += this.OnMissionReset;
			MultiplayerMissionAgentVisualSpawnComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
			missionBehavior.OnMyAgentSpawnedFromVisual += this.OnPreparationEnded;
			missionBehavior.OnMyAgentVisualSpawned += this.OnRoundStarted;
			this.OnTeamChanged();
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0002B364 File Offset: 0x00029564
		private void OnRoundStarted()
		{
			this.OnTeamChanged();
			if (this.UsePowerComparer)
			{
				this._attackerTeamInitialMemberCount = this._missionScoreboardComponent.Sides[1].Players.Count<MissionPeer>();
				this._defenderTeamInitialMemberCount = this._missionScoreboardComponent.Sides[0].Players.Count<MissionPeer>();
			}
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0002B3BC File Offset: 0x000295BC
		private void RegisterMoraleEvents()
		{
			if (!this._areMoraleEventsRegistered)
			{
				this._commanderInfo.OnMoraleChangedEvent += this.OnUpdateMorale;
				this._commanderInfo.OnFlagNumberChangedEvent += this.OnNumberOfCapturePointsChanged;
				this._commanderInfo.OnCapturePointOwnerChangedEvent += this.OnCapturePointOwnerChanged;
				this.AreMoralesIndependent = this._commanderInfo.AreMoralesIndependent;
				this.ResetCapturePointLists();
				this.InitCapturePoints();
				this._areMoraleEventsRegistered = true;
			}
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0002B43A File Offset: 0x0002963A
		private void OnPreparationEnded()
		{
			this.ShowTacticalInfo = true;
			this.OnTeamChanged();
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0002B44C File Offset: 0x0002964C
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this._commanderInfo != null)
			{
				this._commanderInfo.OnMoraleChangedEvent -= this.OnUpdateMorale;
				this._commanderInfo.OnFlagNumberChangedEvent -= this.OnNumberOfCapturePointsChanged;
				this._commanderInfo.OnCapturePointOwnerChangedEvent -= this.OnCapturePointOwnerChanged;
			}
			Mission.Current.OnMissionReset -= this.OnMissionReset;
			MultiplayerMissionAgentVisualSpawnComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
			missionBehavior.OnMyAgentSpawnedFromVisual -= this.OnPreparationEnded;
			missionBehavior.OnMyAgentVisualSpawned -= this.OnRoundStarted;
			if (this._siegeClient != null)
			{
				this._siegeClient.OnCapturePointRemainingMoraleGainsChangedEvent -= this.OnCapturePointRemainingMoraleGainsChanged;
			}
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0002B50E File Offset: 0x0002970E
		public void UpdateWarmupDependentFlags(bool isInWarmup)
		{
			this.UseMoraleComparer = !isInWarmup && this._gameMode.IsGameModeTactical && this._commanderInfo != null;
			this.ShowControlPointStatus = !isInWarmup;
			if (!isInWarmup && this.UseMoraleComparer)
			{
				this.RegisterMoraleEvents();
			}
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0002B550 File Offset: 0x00029750
		public void OnUpdateMorale(BattleSideEnum side, float morale)
		{
			if (this._allyTeam != null && this._allyTeam.Side == side)
			{
				this.AllyMoralePercentage = MathF.Round(MathF.Abs(morale * 100f));
				return;
			}
			if (this._enemyTeam != null && this._enemyTeam.Side == side)
			{
				this.EnemyMoralePercentage = MathF.Round(MathF.Abs(morale * 100f));
			}
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0002B5B8 File Offset: 0x000297B8
		private void OnMissionReset(object sender, PropertyChangedEventArgs e)
		{
			if (this.UseMoraleComparer)
			{
				this.AllyMoralePercentage = 50;
				this.EnemyMoralePercentage = 50;
			}
			if (this.UsePowerComparer)
			{
				this.PowerLevelComparer.Update(1.0, 1.0, 1.0, 1.0);
			}
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0002B614 File Offset: 0x00029814
		internal void Tick(float dt)
		{
			foreach (CapturePointVM capturePointVM in this.AllyControlPoints)
			{
				capturePointVM.Refresh(0f, 0f, 0f);
			}
			foreach (CapturePointVM capturePointVM2 in this.EnemyControlPoints)
			{
				capturePointVM2.Refresh(0f, 0f, 0f);
			}
			foreach (CapturePointVM capturePointVM3 in this.NeutralControlPoints)
			{
				capturePointVM3.Refresh(0f, 0f, 0f);
			}
			if (this._allyTeam != null && this.UsePowerComparer)
			{
				int count = Mission.Current.AttackerTeam.ActiveAgents.Count;
				int count2 = Mission.Current.DefenderTeam.ActiveAgents.Count;
				this.AllyMemberCount = ((this._allyTeam.Side == BattleSideEnum.Attacker) ? count : count2);
				this.EnemyMemberCount = ((this._allyTeam.Side == BattleSideEnum.Attacker) ? count2 : count);
				int num = ((this._allyTeam.Side == BattleSideEnum.Attacker) ? this._attackerTeamInitialMemberCount : this._defenderTeamInitialMemberCount);
				Team allyTeam = this._allyTeam;
				int num2 = ((allyTeam != null && allyTeam.Side == BattleSideEnum.Attacker) ? this._defenderTeamInitialMemberCount : this._attackerTeamInitialMemberCount);
				if (num2 == 0 && num == 0)
				{
					this.PowerLevelComparer.Update(1.0, 1.0, 1.0, 1.0);
					return;
				}
				this.PowerLevelComparer.Update((double)this.EnemyMemberCount, (double)this.AllyMemberCount, (double)num2, (double)num);
			}
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0002B808 File Offset: 0x00029A08
		private void OnCapturePointOwnerChanged(FlagCapturePoint target, Team newOwnerTeam)
		{
			CapturePointVM capturePointVM = this.FindCapturePointInLists(target);
			if (capturePointVM != null)
			{
				this.RemoveFlagFromLists(capturePointVM);
				this.HandleAddNewCapturePoint(capturePointVM);
				capturePointVM.OnOwnerChanged(newOwnerTeam);
			}
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0002B838 File Offset: 0x00029A38
		private void OnCapturePointRemainingMoraleGainsChanged(int[] remainingMoraleArr)
		{
			foreach (CapturePointVM capturePointVM in this.AllyControlPoints)
			{
				int flagIndex = capturePointVM.Target.FlagIndex;
				if (flagIndex >= 0 && remainingMoraleArr.Length > flagIndex)
				{
					capturePointVM.OnRemainingMoraleChanged(remainingMoraleArr[flagIndex]);
				}
			}
			foreach (CapturePointVM capturePointVM2 in this.EnemyControlPoints)
			{
				int flagIndex2 = capturePointVM2.Target.FlagIndex;
				if (flagIndex2 >= 0 && remainingMoraleArr.Length > flagIndex2)
				{
					capturePointVM2.OnRemainingMoraleChanged(remainingMoraleArr[flagIndex2]);
				}
			}
			foreach (CapturePointVM capturePointVM3 in this.NeutralControlPoints)
			{
				int flagIndex3 = capturePointVM3.Target.FlagIndex;
				if (flagIndex3 >= 0 && remainingMoraleArr.Length > flagIndex3)
				{
					capturePointVM3.OnRemainingMoraleChanged(remainingMoraleArr[flagIndex3]);
				}
			}
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0002B950 File Offset: 0x00029B50
		private void OnNumberOfCapturePointsChanged()
		{
			this.ResetCapturePointLists();
			this.InitCapturePoints();
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0002B960 File Offset: 0x00029B60
		private void InitCapturePoints()
		{
			if (this._commanderInfo != null)
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				bool flag;
				if (myPeer == null)
				{
					flag = null != null;
				}
				else
				{
					MissionPeer component = myPeer.GetComponent<MissionPeer>();
					flag = ((component != null) ? component.Team : null) != null;
				}
				if (flag)
				{
					foreach (FlagCapturePoint flagCapturePoint in this._commanderInfo.AllCapturePoints.Where<FlagCapturePoint>((FlagCapturePoint c) => !c.IsDeactivated).ToArray<FlagCapturePoint>())
					{
						CapturePointVM capturePointVM = new CapturePointVM(flagCapturePoint, TargetIconType.Flag_A + flagCapturePoint.FlagIndex);
						this.HandleAddNewCapturePoint(capturePointVM);
					}
					this.RefreshMoraleIncreaseLevels();
				}
			}
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0002B9FC File Offset: 0x00029BFC
		private void HandleAddNewCapturePoint(CapturePointVM capturePointVM)
		{
			this.RemoveFlagFromLists(capturePointVM);
			if (this._allyTeam == null)
			{
				return;
			}
			Team team = this._commanderInfo.GetFlagOwner(capturePointVM.Target);
			if (team != null && (team.Side == BattleSideEnum.None || team.Side == BattleSideEnum.NumSides))
			{
				team = null;
			}
			capturePointVM.OnOwnerChanged(team);
			bool isDeactivated = capturePointVM.Target.IsDeactivated;
			if ((team == null || team.TeamIndex == -1) && !isDeactivated)
			{
				int num = MathF.Min(this.NeutralControlPoints.Count, capturePointVM.Target.FlagIndex);
				this.NeutralControlPoints.Insert(num, capturePointVM);
			}
			else if (this._allyTeam == team)
			{
				int num2 = MathF.Min(this.AllyControlPoints.Count, capturePointVM.Target.FlagIndex);
				this.AllyControlPoints.Insert(num2, capturePointVM);
			}
			else if (this._allyTeam != team)
			{
				int num3 = MathF.Min(this.EnemyControlPoints.Count, capturePointVM.Target.FlagIndex);
				this.EnemyControlPoints.Insert(num3, capturePointVM);
			}
			else if (team.Side != BattleSideEnum.None)
			{
				Debug.FailedAssert("Incorrect flag team state", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\HUDExtensions\\CommanderInfoVM.cs", "HandleAddNewCapturePoint", 321);
			}
			this.RefreshMoraleIncreaseLevels();
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0002BB24 File Offset: 0x00029D24
		private void RefreshMoraleIncreaseLevels()
		{
			this.AllyMoraleIncreaseLevel = MathF.Max(0, this.AllyControlPoints.Count - this.EnemyControlPoints.Count);
			this.EnemyMoraleIncreaseLevel = MathF.Max(0, this.EnemyControlPoints.Count - this.AllyControlPoints.Count);
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0002BB78 File Offset: 0x00029D78
		private void RemoveFlagFromLists(CapturePointVM capturePoint)
		{
			if (this.AllyControlPoints.Contains(capturePoint))
			{
				this.AllyControlPoints.Remove(capturePoint);
				return;
			}
			if (this.NeutralControlPoints.Contains(capturePoint))
			{
				this.NeutralControlPoints.Remove(capturePoint);
				return;
			}
			if (this.EnemyControlPoints.Contains(capturePoint))
			{
				this.EnemyControlPoints.Remove(capturePoint);
			}
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0002BBD8 File Offset: 0x00029DD8
		public void OnTeamChanged()
		{
			if (!GameNetwork.IsMyPeerReady || !this.ShowTacticalInfo)
			{
				return;
			}
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			this._allyTeam = component.Team;
			if (this._allyTeam == null)
			{
				return;
			}
			IEnumerable<Team> enumerable = Mission.Current.Teams.Where<Team>((Team t) => t.IsEnemyOf(this._allyTeam));
			this._enemyTeam = enumerable.FirstOrDefault<Team>();
			if (this._allyTeam.Side == BattleSideEnum.None)
			{
				this._allyTeam = Mission.Current.AttackerTeam;
				return;
			}
			this.ResetCapturePointLists();
			this.InitCapturePoints();
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0002BC68 File Offset: 0x00029E68
		private void ResetCapturePointLists()
		{
			this.AllyControlPoints.Clear();
			this.NeutralControlPoints.Clear();
			this.EnemyControlPoints.Clear();
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0002BC8C File Offset: 0x00029E8C
		private CapturePointVM FindCapturePointInLists(FlagCapturePoint target)
		{
			CapturePointVM capturePointVM = this.AllyControlPoints.SingleOrDefault<CapturePointVM>((CapturePointVM c) => c.Target == target);
			if (capturePointVM != null)
			{
				return capturePointVM;
			}
			CapturePointVM capturePointVM2 = this.EnemyControlPoints.SingleOrDefault<CapturePointVM>((CapturePointVM c) => c.Target == target);
			if (capturePointVM2 != null)
			{
				return capturePointVM2;
			}
			CapturePointVM capturePointVM3 = this.NeutralControlPoints.SingleOrDefault<CapturePointVM>((CapturePointVM c) => c.Target == target);
			if (capturePointVM3 != null)
			{
				return capturePointVM3;
			}
			return null;
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0002BCFE File Offset: 0x00029EFE
		public void RefreshColors(string allyTeamColor, string allyTeamColorSecondary, string enemyTeamColor, string enemyTeamColorSecondary)
		{
			this.AllyTeamColor = allyTeamColor;
			this.AllyTeamColorSecondary = allyTeamColorSecondary;
			this.EnemyTeamColor = enemyTeamColor;
			this.EnemyTeamColorSecondary = enemyTeamColorSecondary;
			if (this.UsePowerComparer)
			{
				this.PowerLevelComparer.SetColors(this.EnemyTeamColor, this.AllyTeamColor);
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x0002BD3C File Offset: 0x00029F3C
		// (set) Token: 0x06000E1F RID: 3615 RVA: 0x0002BD44 File Offset: 0x00029F44
		[DataSourceProperty]
		public MBBindingList<CapturePointVM> AllyControlPoints
		{
			get
			{
				return this._allyControlPoints;
			}
			set
			{
				if (value != this._allyControlPoints)
				{
					this._allyControlPoints = value;
					base.OnPropertyChangedWithValue<MBBindingList<CapturePointVM>>(value, "AllyControlPoints");
				}
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x0002BD62 File Offset: 0x00029F62
		// (set) Token: 0x06000E21 RID: 3617 RVA: 0x0002BD6A File Offset: 0x00029F6A
		[DataSourceProperty]
		public MBBindingList<CapturePointVM> NeutralControlPoints
		{
			get
			{
				return this._neutralControlPoints;
			}
			set
			{
				if (value != this._neutralControlPoints)
				{
					this._neutralControlPoints = value;
					base.OnPropertyChangedWithValue<MBBindingList<CapturePointVM>>(value, "NeutralControlPoints");
				}
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x0002BD88 File Offset: 0x00029F88
		// (set) Token: 0x06000E23 RID: 3619 RVA: 0x0002BD90 File Offset: 0x00029F90
		[DataSourceProperty]
		public MBBindingList<CapturePointVM> EnemyControlPoints
		{
			get
			{
				return this._enemyControlPoints;
			}
			set
			{
				if (value != this._enemyControlPoints)
				{
					this._enemyControlPoints = value;
					base.OnPropertyChangedWithValue<MBBindingList<CapturePointVM>>(value, "EnemyControlPoints");
				}
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x0002BDAE File Offset: 0x00029FAE
		// (set) Token: 0x06000E25 RID: 3621 RVA: 0x0002BDB6 File Offset: 0x00029FB6
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

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x0002BDD9 File Offset: 0x00029FD9
		// (set) Token: 0x06000E27 RID: 3623 RVA: 0x0002BDE1 File Offset: 0x00029FE1
		[DataSourceProperty]
		public string AllyTeamColorSecondary
		{
			get
			{
				return this._allyTeamColorSecondary;
			}
			set
			{
				if (value != this._allyTeamColorSecondary)
				{
					this._allyTeamColorSecondary = value;
					base.OnPropertyChangedWithValue<string>(value, "AllyTeamColorSecondary");
				}
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06000E28 RID: 3624 RVA: 0x0002BE04 File Offset: 0x0002A004
		// (set) Token: 0x06000E29 RID: 3625 RVA: 0x0002BE0C File Offset: 0x0002A00C
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

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x0002BE2F File Offset: 0x0002A02F
		// (set) Token: 0x06000E2B RID: 3627 RVA: 0x0002BE37 File Offset: 0x0002A037
		[DataSourceProperty]
		public string EnemyTeamColorSecondary
		{
			get
			{
				return this._enemyTeamColorSecondary;
			}
			set
			{
				if (value != this._enemyTeamColorSecondary)
				{
					this._enemyTeamColorSecondary = value;
					base.OnPropertyChangedWithValue<string>(value, "EnemyTeamColorSecondary");
				}
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x0002BE5A File Offset: 0x0002A05A
		// (set) Token: 0x06000E2D RID: 3629 RVA: 0x0002BE62 File Offset: 0x0002A062
		[DataSourceProperty]
		public int AllyMoraleIncreaseLevel
		{
			get
			{
				return this._allyMoraleIncreaseLevel;
			}
			set
			{
				if (value != this._allyMoraleIncreaseLevel)
				{
					this._allyMoraleIncreaseLevel = value;
					base.OnPropertyChangedWithValue(value, "AllyMoraleIncreaseLevel");
				}
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x0002BE80 File Offset: 0x0002A080
		// (set) Token: 0x06000E2F RID: 3631 RVA: 0x0002BE88 File Offset: 0x0002A088
		[DataSourceProperty]
		public int EnemyMoraleIncreaseLevel
		{
			get
			{
				return this._enemyMoraleIncreaseLevel;
			}
			set
			{
				if (value != this._enemyMoraleIncreaseLevel)
				{
					this._enemyMoraleIncreaseLevel = value;
					base.OnPropertyChangedWithValue(value, "EnemyMoraleIncreaseLevel");
				}
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x0002BEA6 File Offset: 0x0002A0A6
		// (set) Token: 0x06000E31 RID: 3633 RVA: 0x0002BEAE File Offset: 0x0002A0AE
		[DataSourceProperty]
		public int AllyMoralePercentage
		{
			get
			{
				return this._allyMoralePercentage;
			}
			set
			{
				if (value != this._allyMoralePercentage)
				{
					this._allyMoralePercentage = value;
					base.OnPropertyChangedWithValue(value, "AllyMoralePercentage");
				}
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x0002BECC File Offset: 0x0002A0CC
		// (set) Token: 0x06000E33 RID: 3635 RVA: 0x0002BED4 File Offset: 0x0002A0D4
		[DataSourceProperty]
		public int EnemyMoralePercentage
		{
			get
			{
				return this._enemyMoralePercentage;
			}
			set
			{
				if (value != this._enemyMoralePercentage)
				{
					this._enemyMoralePercentage = value;
					base.OnPropertyChangedWithValue(value, "EnemyMoralePercentage");
				}
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x0002BEF2 File Offset: 0x0002A0F2
		// (set) Token: 0x06000E35 RID: 3637 RVA: 0x0002BEFA File Offset: 0x0002A0FA
		[DataSourceProperty]
		public int AllyMemberCount
		{
			get
			{
				return this._allyMemberCount;
			}
			set
			{
				if (value != this._allyMemberCount)
				{
					this._allyMemberCount = value;
					base.OnPropertyChangedWithValue(value, "AllyMemberCount");
				}
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x0002BF18 File Offset: 0x0002A118
		// (set) Token: 0x06000E37 RID: 3639 RVA: 0x0002BF20 File Offset: 0x0002A120
		[DataSourceProperty]
		public int EnemyMemberCount
		{
			get
			{
				return this._enemyMemberCount;
			}
			set
			{
				if (value != this._enemyMemberCount)
				{
					this._enemyMemberCount = value;
					base.OnPropertyChangedWithValue(value, "EnemyMemberCount");
				}
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x0002BF3E File Offset: 0x0002A13E
		// (set) Token: 0x06000E39 RID: 3641 RVA: 0x0002BF46 File Offset: 0x0002A146
		[DataSourceProperty]
		public PowerLevelComparer PowerLevelComparer
		{
			get
			{
				return this._powerLevelComparer;
			}
			set
			{
				if (value != this._powerLevelComparer)
				{
					this._powerLevelComparer = value;
					base.OnPropertyChangedWithValue<PowerLevelComparer>(value, "PowerLevelComparer");
				}
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x0002BF64 File Offset: 0x0002A164
		// (set) Token: 0x06000E3B RID: 3643 RVA: 0x0002BF6C File Offset: 0x0002A16C
		[DataSourceProperty]
		public bool UsePowerComparer
		{
			get
			{
				return this._usePowerComparer;
			}
			set
			{
				if (value != this._usePowerComparer)
				{
					this._usePowerComparer = value;
					base.OnPropertyChangedWithValue(value, "UsePowerComparer");
				}
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x0002BF8A File Offset: 0x0002A18A
		// (set) Token: 0x06000E3D RID: 3645 RVA: 0x0002BF92 File Offset: 0x0002A192
		[DataSourceProperty]
		public bool UseMoraleComparer
		{
			get
			{
				return this._useMoraleComparer;
			}
			set
			{
				if (value != this._useMoraleComparer)
				{
					this._useMoraleComparer = value;
					base.OnPropertyChangedWithValue(value, "UseMoraleComparer");
				}
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x0002BFB0 File Offset: 0x0002A1B0
		// (set) Token: 0x06000E3F RID: 3647 RVA: 0x0002BFB8 File Offset: 0x0002A1B8
		[DataSourceProperty]
		public bool ShowTacticalInfo
		{
			get
			{
				return this._showTacticalInfo;
			}
			set
			{
				if (value != this._showTacticalInfo)
				{
					this._showTacticalInfo = value;
					base.OnPropertyChangedWithValue(value, "ShowTacticalInfo");
				}
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x0002BFD6 File Offset: 0x0002A1D6
		// (set) Token: 0x06000E41 RID: 3649 RVA: 0x0002BFDE File Offset: 0x0002A1DE
		[DataSourceProperty]
		public bool AreMoralesIndependent
		{
			get
			{
				return this._areMoralesIndependent;
			}
			set
			{
				if (value != this._areMoralesIndependent)
				{
					this._areMoralesIndependent = value;
					base.OnPropertyChangedWithValue(value, "AreMoralesIndependent");
				}
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x0002BFFC File Offset: 0x0002A1FC
		// (set) Token: 0x06000E43 RID: 3651 RVA: 0x0002C004 File Offset: 0x0002A204
		[DataSourceProperty]
		public bool ShowControlPointStatus
		{
			get
			{
				return this._showControlPointStatus;
			}
			set
			{
				if (value != this._showControlPointStatus)
				{
					this._showControlPointStatus = value;
					base.OnPropertyChangedWithValue(value, "ShowControlPointStatus");
				}
			}
		}

		// Token: 0x0400066E RID: 1646
		private readonly MissionRepresentativeBase _missionRepresentative;

		// Token: 0x0400066F RID: 1647
		private readonly MissionMultiplayerGameModeBaseClient _gameMode;

		// Token: 0x04000670 RID: 1648
		private readonly MissionMultiplayerSiegeClient _siegeClient;

		// Token: 0x04000671 RID: 1649
		private readonly MissionScoreboardComponent _missionScoreboardComponent;

		// Token: 0x04000672 RID: 1650
		private const float InitialArmyStrength = 1f;

		// Token: 0x04000673 RID: 1651
		private int _attackerTeamInitialMemberCount;

		// Token: 0x04000674 RID: 1652
		private int _defenderTeamInitialMemberCount;

		// Token: 0x04000675 RID: 1653
		private Team _allyTeam;

		// Token: 0x04000676 RID: 1654
		private Team _enemyTeam;

		// Token: 0x04000677 RID: 1655
		private ICommanderInfo _commanderInfo;

		// Token: 0x04000678 RID: 1656
		private bool _areMoraleEventsRegistered;

		// Token: 0x04000679 RID: 1657
		private MBBindingList<CapturePointVM> _allyControlPoints;

		// Token: 0x0400067A RID: 1658
		private MBBindingList<CapturePointVM> _neutralControlPoints;

		// Token: 0x0400067B RID: 1659
		private MBBindingList<CapturePointVM> _enemyControlPoints;

		// Token: 0x0400067C RID: 1660
		private int _allyMoraleIncreaseLevel;

		// Token: 0x0400067D RID: 1661
		private int _enemyMoraleIncreaseLevel;

		// Token: 0x0400067E RID: 1662
		private int _allyMoralePercentage;

		// Token: 0x0400067F RID: 1663
		private int _enemyMoralePercentage;

		// Token: 0x04000680 RID: 1664
		private int _allyMemberCount;

		// Token: 0x04000681 RID: 1665
		private int _enemyMemberCount;

		// Token: 0x04000682 RID: 1666
		private PowerLevelComparer _powerLevelComparer;

		// Token: 0x04000683 RID: 1667
		private bool _showTacticalInfo;

		// Token: 0x04000684 RID: 1668
		private bool _usePowerComparer;

		// Token: 0x04000685 RID: 1669
		private bool _useMoraleComparer;

		// Token: 0x04000686 RID: 1670
		private bool _areMoralesIndependent;

		// Token: 0x04000687 RID: 1671
		private bool _showControlPointStatus;

		// Token: 0x04000688 RID: 1672
		private string _allyTeamColor;

		// Token: 0x04000689 RID: 1673
		private string _allyTeamColorSecondary;

		// Token: 0x0400068A RID: 1674
		private string _enemyTeamColor;

		// Token: 0x0400068B RID: 1675
		private string _enemyTeamColorSecondary;
	}
}
