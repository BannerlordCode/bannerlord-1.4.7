using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002AF RID: 687
	public abstract class MissionMultiplayerGameModeBaseClient : MissionNetwork, ICameraModeLogic
	{
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002689 RID: 9865 RVA: 0x0008E973 File Offset: 0x0008CB73
		// (set) Token: 0x0600268A RID: 9866 RVA: 0x0008E97B File Offset: 0x0008CB7B
		public MissionLobbyComponent MissionLobbyComponent { get; private set; }

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x0600268B RID: 9867 RVA: 0x0008E984 File Offset: 0x0008CB84
		// (set) Token: 0x0600268C RID: 9868 RVA: 0x0008E98C File Offset: 0x0008CB8C
		public MissionNetworkComponent MissionNetworkComponent { get; private set; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x0600268D RID: 9869 RVA: 0x0008E995 File Offset: 0x0008CB95
		// (set) Token: 0x0600268E RID: 9870 RVA: 0x0008E99D File Offset: 0x0008CB9D
		public MissionScoreboardComponent ScoreboardComponent { get; private set; }

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x0600268F RID: 9871 RVA: 0x0008E9A6 File Offset: 0x0008CBA6
		// (set) Token: 0x06002690 RID: 9872 RVA: 0x0008E9AE File Offset: 0x0008CBAE
		public MultiplayerGameNotificationsComponent NotificationsComponent { get; private set; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002691 RID: 9873 RVA: 0x0008E9B7 File Offset: 0x0008CBB7
		// (set) Token: 0x06002692 RID: 9874 RVA: 0x0008E9BF File Offset: 0x0008CBBF
		public MultiplayerWarmupComponent WarmupComponent { get; private set; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002693 RID: 9875 RVA: 0x0008E9C8 File Offset: 0x0008CBC8
		// (set) Token: 0x06002694 RID: 9876 RVA: 0x0008E9D0 File Offset: 0x0008CBD0
		public IRoundComponent RoundComponent { get; private set; }

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002695 RID: 9877 RVA: 0x0008E9D9 File Offset: 0x0008CBD9
		// (set) Token: 0x06002696 RID: 9878 RVA: 0x0008E9E1 File Offset: 0x0008CBE1
		public MultiplayerTimerComponent TimerComponent { get; private set; }

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002697 RID: 9879
		public abstract bool IsGameModeUsingGold { get; }

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002698 RID: 9880
		public abstract bool IsGameModeTactical { get; }

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002699 RID: 9881 RVA: 0x0008E9EA File Offset: 0x0008CBEA
		public virtual bool IsGameModeUsingCasualGold
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x0600269A RID: 9882
		public abstract bool IsGameModeUsingRoundCountdown { get; }

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x0600269B RID: 9883 RVA: 0x0008E9ED File Offset: 0x0008CBED
		public virtual bool IsGameModeUsingAllowCultureChange
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x0600269C RID: 9884 RVA: 0x0008E9F0 File Offset: 0x0008CBF0
		public virtual bool IsGameModeUsingAllowTroopChange
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x0600269D RID: 9885
		public abstract MultiplayerGameType GameType { get; }

		// Token: 0x0600269E RID: 9886
		public abstract int GetGoldAmount();

		// Token: 0x0600269F RID: 9887 RVA: 0x0008E9F3 File Offset: 0x0008CBF3
		public virtual SpectatorCameraTypes GetMissionCameraLockMode(bool lockedToMainPlayer)
		{
			return SpectatorCameraTypes.Invalid;
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060026A0 RID: 9888 RVA: 0x0008E9F6 File Offset: 0x0008CBF6
		public bool IsRoundInProgress
		{
			get
			{
				IRoundComponent roundComponent = this.RoundComponent;
				return roundComponent != null && roundComponent.CurrentRoundState == MultiplayerRoundState.InProgress;
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060026A1 RID: 9889 RVA: 0x0008EA0C File Offset: 0x0008CC0C
		public bool IsInWarmup
		{
			get
			{
				return this.MissionLobbyComponent.IsInWarmup;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060026A2 RID: 9890 RVA: 0x0008EA19 File Offset: 0x0008CC19
		public float RemainingTime
		{
			get
			{
				return this.TimerComponent.GetRemainingTime(GameNetwork.IsClientOrReplay);
			}
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x0008EA2C File Offset: 0x0008CC2C
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this.MissionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this.MissionNetworkComponent = base.Mission.GetMissionBehavior<MissionNetworkComponent>();
			this.ScoreboardComponent = base.Mission.GetMissionBehavior<MissionScoreboardComponent>();
			this.NotificationsComponent = base.Mission.GetMissionBehavior<MultiplayerGameNotificationsComponent>();
			this.WarmupComponent = base.Mission.GetMissionBehavior<MultiplayerWarmupComponent>();
			this.RoundComponent = base.Mission.GetMissionBehavior<IRoundComponent>();
			this.TimerComponent = base.Mission.GetMissionBehavior<MultiplayerTimerComponent>();
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x0008EAB6 File Offset: 0x0008CCB6
		public override void EarlyStart()
		{
			this.MissionLobbyComponent.MissionType = this.GameType;
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x0008EACC File Offset: 0x0008CCCC
		public bool CheckTimer(out int remainingTime, out int remainingWarningTime, bool forceUpdate = false)
		{
			bool flag = false;
			float num = 0f;
			if (this.WarmupComponent != null && this.MissionLobbyComponent.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
			{
				flag = !this.WarmupComponent.IsInWarmup;
			}
			else if (this.RoundComponent != null)
			{
				flag = !this.RoundComponent.CurrentRoundState.StateHasVisualTimer();
				num = this.RoundComponent.LastRoundEndRemainingTime;
			}
			if (forceUpdate || !flag)
			{
				if (flag)
				{
					remainingTime = MathF.Ceiling(num);
				}
				else
				{
					remainingTime = MathF.Ceiling(this.RemainingTime);
				}
				remainingWarningTime = this.GetWarningTimer();
				return true;
			}
			remainingTime = 0;
			remainingWarningTime = 0;
			return false;
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x0008EB60 File Offset: 0x0008CD60
		protected virtual int GetWarningTimer()
		{
			return 0;
		}

		// Token: 0x060026A7 RID: 9895
		public abstract void OnGoldAmountChangedForRepresentative(MissionRepresentativeBase representative, int goldAmount);

		// Token: 0x060026A8 RID: 9896 RVA: 0x0008EB63 File Offset: 0x0008CD63
		public virtual bool CanRequestTroopChange()
		{
			return false;
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x0008EB66 File Offset: 0x0008CD66
		public virtual bool CanRequestCultureChange()
		{
			return false;
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x0008EB6C File Offset: 0x0008CD6C
		public bool IsClassAvailable(MultiplayerClassDivisions.MPHeroClass heroClass)
		{
			FormationClass formationClass;
			if (Enum.TryParse<FormationClass>(heroClass.ClassGroup.StringId, out formationClass))
			{
				return this.MissionLobbyComponent.IsClassAvailable(formationClass);
			}
			Debug.FailedAssert("\"" + heroClass.ClassGroup.StringId + "\" does not match with any FormationClass.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MultiplayerGameModeLogics\\ClientGameModeLogics\\MissionMultiplayerGameModeBaseClient.cs", "IsClassAvailable", 116);
			return false;
		}
	}
}
